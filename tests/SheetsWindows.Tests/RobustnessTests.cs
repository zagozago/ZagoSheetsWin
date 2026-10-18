using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SheetsWindows.Core;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class RobustnessTests
{
    private sealed class Auth : IGoogleAuth
    {
        public string Account = "A"; public int Refreshes;
        public Task<GoogleAccess> AccessAsync(bool refresh = false, CancellationToken cancellationToken = default)
        { cancellationToken.ThrowIfCancellationRequested(); if (refresh) Refreshes++; return Task.FromResult(new GoogleAccess(Account, "private-token")); }
    }
    private sealed class Protector : IUploadSecretProtector
    {
        // Test-only authenticated encryption, never used in production.
        private readonly byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes("fixture"));
        public byte[] Protect(byte[] bytes)
        {
            var result = new byte[28 + bytes.Length]; RandomNumberGenerator.Fill(result.AsSpan(0, 12));
            using var aes = new AesGcm(key, 16); aes.Encrypt(result.AsSpan(0, 12), bytes, result.AsSpan(28), result.AsSpan(12, 16)); return result;
        }
        public byte[] Unprotect(byte[] bytes)
        { var result = new byte[bytes.Length - 28]; using var aes = new AesGcm(key, 16); aes.Decrypt(bytes.AsSpan(0, 12), bytes.AsSpan(28), bytes.AsSpan(12, 16), result); return result; }
    }
    private sealed class Server : HttpMessageHandler
    {
        public int Posts, Queries, Chunks; public bool LoseChunk, LoseFinal, Offline, Expired, NoProgress, Unauthorized; public string? BadRange;
        public string Location = "https://www.googleapis.com/upload/drive/v3/files?uploadType=resumable&upload_id=secret-session";
        public readonly MemoryStream Received = new(); public readonly List<long> Starts = [];
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            Assert.Equal("private-token", request.Headers.Authorization!.Parameter);
            if (request.Method == HttpMethod.Post)
            { Posts++; var result = new HttpResponseMessage(HttpStatusCode.OK); result.Headers.Location = new Uri(Location); return result; }
            Assert.Equal(HttpMethod.Put, request.Method); Assert.Equal("/upload/drive/v3/files", request.RequestUri!.AbsolutePath);
            if (Offline) throw new HttpRequestException("private detail");
            if (Expired) return new(HttpStatusCode.NotFound);
            if (Unauthorized) { Unauthorized = false; return new(HttpStatusCode.Unauthorized); }
            var range = request.Content!.Headers.ContentRange!;
            if (range.From is null) Queries++;
            else
            {
                Chunks++; Starts.Add(range.From.Value);
                if (!NoProgress)
                {
                    Assert.Equal(Received.Length, range.From); var bytes = await request.Content.ReadAsByteArrayAsync(ct); Received.Write(bytes);
                    if (LoseChunk) { LoseChunk = false; Offline = true; throw new HttpRequestException("lost chunk acknowledgment"); }
                }
            }
            if (Received.Length == range.Length)
            {
                if (LoseFinal) { LoseFinal = false; Offline = true; throw new HttpRequestException("lost completion"); }
                return Json(new { id = "sheet_1" });
            }
            var response = new HttpResponseMessage((HttpStatusCode)308);
            if (BadRange is not null) response.Headers.TryAddWithoutValidation("Range", BadRange);
            else if (Received.Length > 0) response.Headers.TryAddWithoutValidation("Range", "bytes=0-" + (Received.Length - 1));
            return response;
        }
    }
    private static HttpResponseMessage Json(object obj) => new(HttpStatusCode.OK) { Content = new StringContent(JsonSerializer.Serialize(obj)) };
    private static RemoteAttempt Attempt() => new("sheet:test", "A", "sheet", "source-hash", "marker", null, false);
    private static UploadSessionStore Store(Workspace w) => new(Path.Combine(w.Root, "state", "uploads"), new Protector());
    [Fact]
    public async Task RestartAfterLostChunkUsesServerOffsetAndEncryptedDurableSession()
    {
        using var w = new Workspace(); using var server = new Server { LoseChunk = true }; using var http = new HttpClient(server); var auth = new Auth(); var bytes = new byte[700000]; RandomNumberGenerator.Fill(bytes);
        var uploader = new ResumableUpload(http, auth, Store(w));
        await Assert.ThrowsAsync<HttpRequestException>(() => uploader.StartAsync(Attempt(), "{}", bytes, "application/test", default));
        Assert.Equal(256 * 1024, server.Received.Length); Assert.Equal(1, server.Posts);
        var disk = File.ReadAllBytes(Assert.Single(Directory.GetFiles(Path.Combine(w.Root, "state", "uploads")))); Assert.DoesNotContain("secret-session", Encoding.UTF8.GetString(disk));
        server.Offline = false;
        Assert.Equal("sheet_1", await new ResumableUpload(http, auth, Store(w)).ResumeAsync(Attempt(), bytes, "application/test", default));
        Assert.Equal(bytes, server.Received.ToArray()); Assert.Equal(new long[] { 0, 262144, 524288 }, server.Starts); Assert.Equal(1, server.Posts); Assert.True(server.Queries >= 2);
        Assert.Equal("UploadSession [protected]", Store(w).Get(Attempt(), bytes, "application/test")!.ToString());
    }
    [Fact]
    public async Task LostCompletionResumesWithoutSendingMoreContent()
    {
        using var w = new Workspace(); using var server = new Server { LoseFinal = true }; using var http = new HttpClient(server); var bytes = new byte[100];
        var upload = new ResumableUpload(http, new Auth(), Store(w));
        await Assert.ThrowsAsync<HttpRequestException>(() => upload.StartAsync(Attempt(), "{}", bytes, "application/test", default));
        server.Offline = false; Assert.Equal("sheet_1", await upload.ResumeAsync(Attempt(), bytes, "application/test", default)); Assert.Equal(1, server.Chunks); Assert.Equal(1, server.Posts);
    }
    [Fact]
    public async Task ExpiredSessionRequiresReconciliationWithoutAnotherPost()
    {
        using var w = new Workspace(); using var server = new Server { Expired = true }; using var http = new HttpClient(server); var bytes = new byte[100];
        var upload = new ResumableUpload(http, new Auth(), Store(w));
        await Assert.ThrowsAsync<ReconciliationRequiredException>(() => upload.StartAsync(Attempt(), "{}", bytes, "application/test", default));
        await Assert.ThrowsAsync<ReconciliationRequiredException>(() => upload.ResumeAsync(Attempt(), bytes, "application/test", default)); Assert.Equal(1, server.Posts); Assert.Equal(0, server.Chunks);
    }
    [Theory]
    [InlineData("bytes=1-42")]
    [InlineData("bytes=0-999999999999999999")]
    [InlineData("bytes=0-100")]
    [InlineData("bytes=0-99")]
    public async Task MalformedOrComplete308AcknowledgmentBlocksContent(string range)
    {
        using var w = new Workspace(); using var server = new Server { BadRange = range }; using var http = new HttpClient(server);
        await Assert.ThrowsAsync<InvalidDataException>(() => new ResumableUpload(http, new Auth(), Store(w)).StartAsync(Attempt(), "{}", new byte[100], "application/test", default)); Assert.Equal(0, server.Chunks);
    }
    [Fact]
    public async Task NoProgressAndCancellationAreBounded()
    {
        using var w = new Workspace(); using var server = new Server { NoProgress = true }; using var http = new HttpClient(server);
        var upload = new ResumableUpload(http, new Auth(), Store(w)); var bytes = new byte[100];
        await Assert.ThrowsAsync<ReconciliationRequiredException>(() => upload.StartAsync(Attempt(), "{}", bytes, "application/test", default)); Assert.Equal(3, server.Chunks);
        using var cancel = new CancellationTokenSource(); cancel.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => upload.ResumeAsync(Attempt(), bytes, "application/test", cancel.Token)); Assert.Equal(1, server.Posts);
    }
    [Fact]
    public async Task ChangedPayloadAccountAndMimeCannotResume()
    {
        using var w = new Workspace(); var store = Store(w); var bytes = new byte[100]; store.Save(Attempt(), bytes, "application/test", new Uri(new Server().Location));
        Assert.Throws<LocalConflictException>(() => store.Get(Attempt() with { AccountId = "B" }, bytes, "application/test"));
        Assert.Throws<LocalConflictException>(() => store.Get(Attempt(), bytes, "application/other")); bytes[0] = 1;
        Assert.Throws<LocalConflictException>(() => store.Get(Attempt(), bytes, "application/test"));
        using var server = new Server(); using var http = new HttpClient(server); var auth = new Auth { Account = "B" };
        await Assert.ThrowsAsync<InvalidOperationException>(() => new ResumableUpload(http, auth, store).ResumeAsync(Attempt(), new byte[100], "application/test", default)); Assert.Equal(0, server.Queries);
    }
    [Theory]
    [InlineData("http://www.googleapis.com/upload/drive/v3/files?upload_id=secret")]
    [InlineData("https://evil.test/upload/drive/v3/files?upload_id=secret")]
    [InlineData("https://www.googleapis.com/drive/v3/files/other?upload_id=secret")]
    [InlineData("https://user@www.googleapis.com/upload/drive/v3/files?upload_id=secret")]
    public async Task UntrustedSessionIsRejectedBeforeContent(string location)
    {
        using var w = new Workspace(); using var server = new Server { Location = location }; using var http = new HttpClient(server);
        await Assert.ThrowsAsync<InvalidDataException>(() => new ResumableUpload(http, new Auth(), Store(w)).StartAsync(Attempt(), "{}", new byte[100], "application/test", default)); Assert.Equal(0, server.Chunks);
    }
    [Fact]
    public async Task UnauthorizedStatusRefreshesAccountBeforeRetry()
    {
        using var w = new Workspace(); using var server = new Server { Unauthorized = true }; using var http = new HttpClient(server); var auth = new Auth();
        Assert.Equal("sheet_1", await new ResumableUpload(http, auth, Store(w)).StartAsync(Attempt(), "{}", new byte[100], "application/test", default)); Assert.Equal(1, auth.Refreshes); Assert.Equal(1, server.Posts);
    }
    [WindowsFact]
    public void ProductionSessionUsesCurrentUserDpapiAndRejectsTampering()
    {
        using var w = new Workspace(); var store = new UploadSessionStore(Path.Combine(w.Root, "uploads")); var bytes = new byte[100];
        store.Save(Attempt(), bytes, "application/test", new Uri(new Server().Location)); Assert.NotNull(store.Get(Attempt(), bytes, "application/test"));
        var path = Assert.Single(Directory.GetFiles(Path.Combine(w.Root, "uploads"))); var encrypted = File.ReadAllBytes(path);
        Assert.DoesNotContain("secret-session", Encoding.UTF8.GetString(encrypted)); encrypted[^1] ^= 1; File.WriteAllBytes(path, encrypted);
        Assert.Throws<CryptographicException>(() => store.Get(Attempt(), bytes, "application/test"));
    }
    [Fact]
    public async Task DiagnosticsRotateRemainBoundedAndExportOnlyKnownFields()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "state")); var log = new DiagnosticLog(storage); var id = Guid.NewGuid();
        await log.RecordAsync(DiagnosticEvent.Started, id);
        var current = Path.Combine(storage.Root, "logs", "events.jsonl");
        var line = JsonSerializer.Serialize(new DiagnosticEntry(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero), DiagnosticEvent.Failed, id)) + "\n";
        var fullFile = string.Concat(Enumerable.Repeat(line, DiagnosticLog.MaxFileBytes / Encoding.UTF8.GetByteCount(line)));
        // Represent accumulated history without thousands of redundant fsyncs/ACL writes on CI.
        for (var i = 0; i < 5; i++) { File.WriteAllText(current, fullFile, new UTF8Encoding(false)); await log.RecordAsync(DiagnosticEvent.Failed, id); }
        var files = Directory.GetFiles(Path.Combine(storage.Root, "logs")); Assert.Equal(4, files.Length); Assert.All(files, file => Assert.InRange(new FileInfo(file).Length, 1, DiagnosticLog.MaxFileBytes));
        File.AppendAllText(current, "{\"Time\":\"2026-01-01T00:00:00Z\",\"Event\":0,\"Operation\":null,\"Token\":\"secret\",\"Path\":\"private.xlsx\"}\n");
        var destination = Path.Combine(w.Root, "diagnostic.jsonl"); await log.ExportAsync(destination); var content = File.ReadAllText(destination);
        Assert.DoesNotContain("secret", content); Assert.DoesNotContain("private.xlsx", content); Assert.Contains(id.ToString(), content);
        await Assert.ThrowsAsync<IOException>(() => log.ExportAsync(destination));
    }
    [Fact]
    public async Task DiagnosticsIdentifyWorkbookFailureWithoutExportingExceptionText()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "state"));
        var log = new DiagnosticLog(storage);
        await log.RecordAsync(DiagnosticEvent.Failed, failure: new InvalidDataException("Unsupported workbook content type."));
        await log.RecordAsync(DiagnosticEvent.Failed, failure: new IOException("private account / private file"));
        var destination = Path.Combine(w.Root, "diagnostic-reasons.jsonl"); await log.ExportAsync(destination);
        var entries = File.ReadAllLines(destination).Select(line => JsonSerializer.Deserialize<DiagnosticEntry>(line)!).ToArray();
        Assert.Equal(DiagnosticFailure.WorkbookContentType, entries[0].Failure);
        Assert.Equal(DiagnosticFailure.Other, entries[1].Failure);
        Assert.DoesNotContain("private account", File.ReadAllText(destination));
    }

    [Fact]
    public async Task ConfirmedDiagnosticOverwritePublishesLatestAttemptAtomically()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "state"));
        var log = new DiagnosticLog(storage); var destination = Path.Combine(w.Root, "diagnostic.jsonl");
        File.WriteAllText(destination, "old export");
        var attempt = Guid.NewGuid();
        await log.RecordAsync(DiagnosticEvent.Failed, failure: new InvalidDataException("Remote conversion or association validation failed."),
            attemptId: attempt, stage: ProcessingPhase.Upload);
        await log.ExportAsync(destination, overwrite: true);
        var entries = File.ReadAllLines(destination).Select(line => JsonSerializer.Deserialize<DiagnosticEntry>(line)!).ToArray();
        Assert.DoesNotContain("old export", File.ReadAllText(destination));
        var failure = Assert.Single(entries, e => e.Event == DiagnosticEvent.Failed);
        Assert.Equal(UiText.ProductVersion, failure.Version); Assert.Equal(attempt, failure.AttemptId);
        Assert.Equal(ProcessingPhase.Upload, failure.Stage); Assert.Equal(DiagnosticCode.RemoteAssociation, failure.Code);
        Assert.Equal(DiagnosticEvent.Exported, entries[^1].Event);
        Assert.Empty(Directory.GetFiles(w.Root, "diagnostic.jsonl.*.tmp"));
        var previous = File.ReadAllBytes(destination);
        File.AppendAllText(Path.Combine(storage.Root, "logs", "events.jsonl"), "{invalid}\n");
        await Assert.ThrowsAsync<JsonException>(() => log.ExportAsync(destination, overwrite: true));
        Assert.Equal(previous, File.ReadAllBytes(destination));
    }

    [Fact]
    public async Task DiagnosticContextDoesNotExportExceptionMessagesOrForgedVersionData()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "state")); var log = new DiagnosticLog(storage);
        await log.RecordAsync(DiagnosticEvent.Failed, failure: new IOException("secret-token C:/private/account.csv"));
        var path = Path.Combine(w.Root, "safe.jsonl"); await log.ExportAsync(path);
        Assert.DoesNotContain("secret-token", File.ReadAllText(path)); Assert.DoesNotContain("private", File.ReadAllText(path));
        File.AppendAllText(Path.Combine(storage.Root, "logs", "events.jsonl"), "{\"Time\":\"2026-01-01T00:00:00Z\",\"Event\":0,\"Operation\":null,\"Version\":\"secret-token\"}\n");
        await Assert.ThrowsAsync<InvalidDataException>(() => log.ExportAsync(path, overwrite: true));
        Assert.DoesNotContain("secret-token", File.ReadAllText(path));
    }

    [Fact]
    public async Task CurrentWindowFailureExportsEvenWhenHistoryCouldNotBeWritten()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "state")); Directory.CreateDirectory(storage.Root);
        File.WriteAllText(Path.Combine(storage.Root, "logs"), "blocked directory");
        var attempt = Guid.NewGuid();
        var current = DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, failure: new IOException("private path"), attemptId: attempt, stage: ProcessingPhase.Upload);
        var log = new DiagnosticLog(storage); await log.RecordAsync(DiagnosticEvent.Failed, attemptId: attempt);
        var target = Path.Combine(w.Root, "fresh.jsonl"); await log.ExportAsync(target, currentAttempt: current);
        var entries = File.ReadAllLines(target).Select(line => JsonSerializer.Deserialize<DiagnosticEntry>(line)!).ToArray();
        Assert.Equal(attempt, entries[0].AttemptId); Assert.Equal(DiagnosticCode.FileAccess, entries[0].Code);
        Assert.Equal(UiText.ProductVersion, entries[0].Version); Assert.DoesNotContain("private path", File.ReadAllText(target));
    }

    [Fact]
    public async Task RemoteValidationDiagnosticsContainOnlyBooleanChecks()
    {
        using var w = new Workspace(); var log = new DiagnosticLog(new LocalStorage(Path.Combine(w.Root, "state")));
        var failure = new InvalidDataException("Remote conversion or association validation failed.");
        var context = new RemoteValidationContext(false, false, true, true, false, false, false, false, true);
        failure.Data["RemoteValidation"] = context; failure.Data["Token"] = "secret"; failure.Data["GoogleId"] = "private-id";
        await log.RecordAsync(DiagnosticEvent.Failed, failure: failure);
        var path = Path.Combine(w.Root, "remote.jsonl"); await log.ExportAsync(path);
        var entry = JsonSerializer.Deserialize<DiagnosticEntry>(File.ReadLines(path).First())!;
        Assert.Equal(context, entry.RemoteValidation);
        Assert.DoesNotContain("secret", File.ReadAllText(path)); Assert.DoesNotContain("private-id", File.ReadAllText(path));
    }

    [Fact]
    public async Task TraceExportIsBoundedReadableAndRetainsChecksWithoutPrivateExceptionText()
    {
        using var w = new Workspace(); var telemetry = new ProcessingTelemetry();
        for (var i = 0; i < ProcessingTelemetry.MaxCheckpoints + 10; i++) telemetry.Mark(ProcessingStep.Preflight);
        var validation = new RemoteValidationContext(false, true, true, true, true, true, true, true, true);
        telemetry.Mark(ProcessingStep.RemoteValidation, CheckpointOutcome.Failed, validation);
        var trace = telemetry.CaptureTrace(); Assert.Equal(ProcessingTelemetry.MaxCheckpoints, trace.Count);
        var log = new DiagnosticLog(new LocalStorage(Path.Combine(w.Root, "state")));
        await log.RecordAsync(DiagnosticEvent.Failed, failure: new IOException("private-token /private/file.csv"), attemptId: Guid.NewGuid(), trace: trace);
        var path = Path.Combine(w.Root, "diagnostic.jsonl"); await log.ExportAsync(path);
        var entry = JsonSerializer.Deserialize<DiagnosticEntry>(File.ReadLines(path).First())!;
        Assert.Equal(trace, entry.Trace); Assert.Equal(validation, entry.Trace![^1].RemoteValidation);
        var text = File.ReadAllText(path); Assert.Contains("RemoteValidation", text); Assert.Contains("Failed", text);
        Assert.DoesNotContain("private-token", text); Assert.DoesNotContain("/private/file.csv", text);
        var forged = entry with { Trace = [new((ProcessingStep)999, CheckpointOutcome.Completed, 0)] };
        var current = Path.Combine(w.Root, "state", "logs", "events.jsonl");
        File.WriteAllText(current, JsonSerializer.Serialize(forged) + "\n");
        await Assert.ThrowsAsync<InvalidDataException>(() => log.ExportAsync(path, overwrite: true));
        Assert.Equal(text, File.ReadAllText(path));
    }

    [Fact]
    public async Task BrokenDiagnosticDirectoryDoesNotChangeOperationOutcome()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "state")); Directory.CreateDirectory(storage.Root); File.WriteAllText(Path.Combine(storage.Root, "logs"), "blocked");
        await new DiagnosticLog(storage).RecordAsync(DiagnosticEvent.Failed);
    }
    private sealed class Responses(Func<HttpRequestMessage, int, HttpResponseMessage> respond) : HttpMessageHandler
    {
        public int Calls;
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) => Task.FromResult(respond(request, ++Calls));
    }
    [Fact]
    public async Task ReadOnlyMetadataRetriesTransientFailureWithoutPosting()
    {
        using var server = new Responses((request, count) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            return count <= 2 ? new(HttpStatusCode.ServiceUnavailable) : Json(new { id = "sheet_1", mimeType = GoogleDriveClient.SheetMime, trashed = false, capabilities = new { canEdit = true } });
        });
        using var http = new HttpClient(server); Assert.Equal("sheet_1", (await new GoogleDriveClient(http, new Auth()).GetAsync("A", "sheet_1", default)).Id); Assert.Equal(3, server.Calls);
    }
    [Theory]
    [InlineData(429)]
    [InlineData(503)]
    public async Task ExportRetriesTransientResponsesWithoutCreatingAnotherFile(int status)
    {
        using var server = new Responses((request, count) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Contains("/export?", request.RequestUri!.AbsoluteUri);
            if (count <= 2)
            {
                var response = new HttpResponseMessage((HttpStatusCode)status);
                response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.Zero);
                return response;
            }
            return new(HttpStatusCode.OK) { Content = new ByteArrayContent([1, 2, 3]) };
        });
        using var http = new HttpClient(server);
        Assert.Equal(new byte[] { 1, 2, 3 }, await new GoogleDriveClient(http, new Auth()).ExportXlsxAsync("A", "sheet_1", default));
        Assert.Equal(3, server.Calls);
    }
    [Theory]
    [InlineData(400)]
    [InlineData(403)]
    [InlineData(404)]
    public async Task ExportDoesNotRetryPermanentFailures(int status)
    {
        using var server = new Responses((_, _) => new((HttpStatusCode)status)); using var http = new HttpClient(server);
        await Assert.ThrowsAsync<GoogleApiException>(() => new GoogleDriveClient(http, new Auth()).ExportXlsxAsync("A", "sheet_1", default));
        Assert.Equal(1, server.Calls);
    }
    [Theory]
    [InlineData("rateLimitExceeded", true)]
    [InlineData("userRateLimitExceeded", true)]
    [InlineData("insufficientFilePermissions", false)]
    [InlineData("dailyLimitExceeded", false)]
    [InlineData("storageQuotaExceeded", false)]
    [InlineData("private-server-content", false)]
    public async Task Export403RetriesOnlyAnExplicitTemporaryRateLimit(string reason, bool retry)
    {
        var telemetry = new ProcessingTelemetry();
        using var server = new Responses((request, count) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            if (count > 1) return new(HttpStatusCode.OK) { Content = new ByteArrayContent([1,2,3]) };
            var response = Json(new { error = new { message = "private-server-content", errors = new[] { new { reason } } } });
            response.StatusCode = HttpStatusCode.Forbidden;
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.Zero); return response;
        });
        using var http = new HttpClient(server); var drive = new GoogleDriveClient(http, new Auth(), telemetry: telemetry);
        if (retry)
        {
            Assert.Equal(new byte[]{1,2,3}, await drive.ExportXlsxAsync("A", "sheet_1", default));
            var context = Assert.Single(telemetry.CaptureTrace()).GoogleRequest!;
            Assert.Equal(reason, context.Reason); Assert.Equal(1, context.Retry); Assert.Equal(0, context.DelayMs); Assert.Equal("drive.export", context.Endpoint);
        }
        else
        {
            var error = await Assert.ThrowsAsync<GoogleApiException>(() => drive.ExportXlsxAsync("A", "sheet_1", default));
            var diagnostic = DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, failure: error);
            Assert.Equal("drive.export", diagnostic.GoogleEndpoint);
            Assert.DoesNotContain("private-server-content", System.Text.Json.JsonSerializer.Serialize(diagnostic));
            Assert.Empty(telemetry.CaptureTrace());
        }
        Assert.Equal(retry ? 2 : 1, server.Calls);
    }
    [Theory]
    [InlineData("not-json")]
    [InlineData("{\"error\":{\"errors\":[{\"reason\":\"rateLimitExceeded\"},{\"reason\":\"insufficientFilePermissions\"}]}}")]
    public async Task MalformedOrMixed403DoesNotAuthorizeAutomaticRetry(string body)
    {
        using var server = new Responses((_, _) => new(HttpStatusCode.Forbidden) { Content = new StringContent(body) });
        using var http = new HttpClient(server);
        await Assert.ThrowsAsync<GoogleApiException>(() => new GoogleDriveClient(http, new Auth()).ExportXlsxAsync("A", "sheet_1", default)); Assert.Equal(1, server.Calls);
    }
    [Fact]
    public async Task Temporary403RetryExhaustionKeepsReasonAndNeverRequestsLogin()
    {
        using var server = new Responses((request, _) =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            var response = Json(new { error = new { errors = new[] { new { reason = "userRateLimitExceeded" } } } });
            response.StatusCode = HttpStatusCode.Forbidden;
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.Zero); return response;
        });
        using var http = new HttpClient(server);
        var error = await Assert.ThrowsAsync<GoogleApiException>(() => new GoogleDriveClient(http, new Auth()).ExportXlsxAsync("A", "sheet_1", default));
        Assert.Equal(4, server.Calls); Assert.Equal("userRateLimitExceeded", error.Reason);
        Assert.Equal(UiText.Get("error.googleUnavailable"), LauncherErrors.Message(error));
        Assert.Equal(DiagnosticCode.BackupQuota, DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, failure: new BackupQuotaException()).Code);
    }
    [Fact]
    public async Task RetryAfterBeyondTheBoundDoesNotRetryEarlierThanRequested()
    {
        using var server = new Responses((_, _) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.TooManyRequests);
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.FromMinutes(1)); return response;
        });
        using var http = new HttpClient(server);
        var error = await Assert.ThrowsAsync<GoogleApiException>(() => new GoogleDriveClient(http, new Auth()).ExportXlsxAsync("A", "sheet_1", default)); Assert.Equal(1, server.Calls);
        Assert.True(error.RetryDeferred); Assert.False(ImportRetry.IsTransient(error));
    }
    [Fact]
    public async Task ExportTransientRetryIsBounded()
    {
        using var server = new Responses((_, _) =>
        {
            var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            response.Headers.RetryAfter = new System.Net.Http.Headers.RetryConditionHeaderValue(TimeSpan.Zero);
            return response;
        });
        using var http = new HttpClient(server);
        await Assert.ThrowsAsync<GoogleApiException>(() => new GoogleDriveClient(http, new Auth()).ExportXlsxAsync("A", "sheet_1", default));
        Assert.Equal(4, server.Calls);
    }
    [Fact]
    public async Task CreatePostIsNeverRetriedAfterTransientFailure()
    {
        using var server = new Responses((_, _) => new(HttpStatusCode.ServiceUnavailable)); using var http = new HttpClient(server);
        await Assert.ThrowsAsync<GoogleApiException>(() => new GoogleDriveClient(http, new Auth()).CreateAsync(Attempt(), "name", null, null, default)); Assert.Equal(1, server.Calls);
    }
    [Fact]
    public async Task RepeatedPaginationTokenAndOversizedMetadataAreRejected()
    {
        using var repeated = new Responses((_, _) => Json(new { nextPageToken = "same", files = Array.Empty<object>() })); using var http = new HttpClient(repeated);
        await Assert.ThrowsAsync<ReconciliationRequiredException>(() => new GoogleDriveClient(http, new Auth()).FindAsync(Attempt(), default)); Assert.Equal(2, repeated.Calls);
        using var oversized = new Responses((_, _) => new(HttpStatusCode.OK) { Content = new ByteArrayContent(new byte[1024 * 1024 + 1]) }); using var httpLarge = new HttpClient(oversized);
        await Assert.ThrowsAsync<InvalidDataException>(() => new GoogleDriveClient(httpLarge, new Auth()).GetAsync("A", "sheet_1", default));
    }
    private sealed class StalledStream : Stream
    {
        public override bool CanRead => true; public override bool CanSeek => false; public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException(); public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() => throw new NotSupportedException(); public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException(); public override void SetLength(long value) => throw new NotSupportedException(); public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        { await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken); return 0; }
    }
    [Fact]
    public async Task ResponseBodyAfterHeadersIsCoveredByOperationDeadline()
    {
        using var server = new Responses((_, _) => new(HttpStatusCode.OK) { Content = new StreamContent(new StalledStream()) }); using var http = new HttpClient(server);
        var drive = new GoogleDriveClient(http, new Auth(), requestTimeout: TimeSpan.FromMilliseconds(50));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => drive.GetAsync("A", "sheet_1", default));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => drive.ExportXlsxAsync("A", "sheet_1", default));
        using var w = new Workspace(); var store = Store(w); var bytes = new byte[100]; store.Save(Attempt(), bytes, "application/test", new Uri("https://www.googleapis.com/upload/drive/v3/files?upload_id=secret"));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => new ResumableUpload(http, new Auth(), store, TimeSpan.FromMilliseconds(50)).ResumeAsync(Attempt(), bytes, "application/test", default));
        Assert.NotNull(store.Get(Attempt(), bytes, "application/test"));
    }
    [Fact]
    public void NormalizedTextPayloadIsDeterministicForResumeBinding()
    {
        var bytes = Encoding.UTF8.GetBytes("id,value\n001,=1+1"); Assert.Equal(SpreadsheetFormats.Prepare("csv", bytes).Bytes, SpreadsheetFormats.Prepare("csv", bytes).Bytes);
    }
}

