using SheetsWindows.Core;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed partial class GoogleTests
{
    private static GoogleImport ReopeningImporter(Workspace w, HttpClient http, IGoogleAuth auth,
        GoogleRemoteRegistry remote, IReadOnlySet<Guid> candidates) => new(w.Coordinator(), w.Registry(), remote,
        new SourceReader(), new FileOperationLock(w.Locks), auth, new GoogleDriveClient(http, auth), reopenCandidates: candidates);

    [Fact]
    public async Task NewOpeningOfMissingPendingUploadPreservesOldBackupAndHistory()
    {
        using var w = new Workspace(); File.WriteAllBytes(w.Source, Workbook());
        var old = await w.Coordinator().PrepareAsync("A", w.Source);
        var remote = new GoogleRemoteRegistry(Path.Combine(w.Root, "state", "google.db"));
        var mapping = remote.Begin("sheet:" + old.Id.ToString("N"), "A", "sheet", old.Snapshot!.Sha256);
        var backup = File.ReadAllBytes(old.Snapshot.BackupPath);
        using var server = new DriveServer(); using var http = new HttpClient(server, false); var auth = new FixedAuth();
        await using var opening = await SpreadsheetOpening.AcquireAsync(new(Path.Combine(w.Root, "state")), w.Source);
        var importer = ReopeningImporter(w, http, auth, remote, opening.Candidates);
        var receipt = await importer.ImportReceiptAsync(w.Source);
        Assert.NotEqual(old.Id, receipt.Operation.Id); Assert.Equal(2, server.Posts);
        Assert.Equal(mapping, remote.Get(mapping.Key)); Assert.Equal(backup, File.ReadAllBytes(old.Snapshot.BackupPath));
        Assert.Equal(old.Snapshot, w.Registry().Get(old.Id)!.Snapshot);
        Assert.Contains(w.Registry().Events(old.Id), e => e.Kind == "PendingReopened");
        Assert.Equal(receipt.Url, await importer.ImportAsync(w.Source)); Assert.Equal(2, server.Posts);
    }

    [Fact]
    public async Task FailureCreatedInThisOpeningCannotTriggerFreshUploadOnRetry()
    {
        using var w = new Workspace(); File.WriteAllBytes(w.Source, Workbook());
        using var server = new DriveServer { LoseSheetResponse = true, ListLag = true };
        using var http = new HttpClient(server, false); var auth = new FixedAuth();
        var remote = new GoogleRemoteRegistry(Path.Combine(w.Root, "state", "google.db"));
        await using var opening = await SpreadsheetOpening.AcquireAsync(new(Path.Combine(w.Root, "state")), w.Source);
        var importer = ReopeningImporter(w, http, auth, remote, opening.Candidates);
        await Assert.ThrowsAsync<HttpRequestException>(() => importer.ImportAsync(w.Source));
        await Assert.ThrowsAsync<ReconciliationRequiredException>(() => importer.ImportAsync(w.Source));
        Assert.Equal(2, server.Posts); Assert.Single(w.Registry().Pending());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ExistingOrAmbiguousRemoteNeverStartsAnotherUpload(bool ambiguous)
    {
        using var w = new Workspace(); File.WriteAllBytes(w.Source, Workbook());
        using var server = new DriveServer(); var initial = Importer(w, server);
        await initial.ImportAsync(w.Source); var old = Assert.Single(w.Registry().Pending());
        // Simulate lost local acknowledgement while keeping the real remote object.
        var otherRemote = new GoogleRemoteRegistry(Path.Combine(w.Root, "pending.db"));
        var pending = otherRemote.Begin("sheet:" + old.Id.ToString("N"), "A", "sheet", old.Snapshot!.Sha256);
        server.Files["file_2"] = new { id = "file_2", mimeType = GoogleDriveClient.SheetMime, trashed = false,
            capabilities = new { canEdit = true }, appProperties = new Dictionary<string, string> { ["sw_operation"] = pending.Marker, ["sw_hash"] = pending.Hash } };
        server.DuplicateList = ambiguous;
        using var http = new HttpClient(server, false);
        await using var opening = await SpreadsheetOpening.AcquireAsync(new(Path.Combine(w.Root, "state")), w.Source);
        var importer = ReopeningImporter(w, http, new FixedAuth(), otherRemote, opening.Candidates);
        if (ambiguous) await Assert.ThrowsAsync<ReconciliationRequiredException>(() => importer.ImportAsync(w.Source));
        else Assert.Equal(GoogleDriveClient.Editor("file_2"), await importer.ImportAsync(w.Source));
        Assert.Equal(2, server.Posts); Assert.Equal(old, w.Registry().Get(old.Id));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RemoteValidationFailureNeverRetiresPendingAssociation(bool wrongMime)
    {
        using var w = new Workspace(); File.WriteAllBytes(w.Source, Workbook());
        var old = await w.Coordinator().PrepareAsync("A", w.Source);
        var remote = new GoogleRemoteRegistry(Path.Combine(w.Root, "state", "google.db"));
        var pending = remote.Begin("sheet:" + old.Id.ToString("N"), "A", "sheet", old.Snapshot!.Sha256);
        using var server = new DriveServer(); using var http = new HttpClient(server, false);
        server.Files["existing"] = new { id = "existing", mimeType = wrongMime ? "application/octet-stream" : GoogleDriveClient.SheetMime,
            trashed = false, capabilities = new { canEdit = wrongMime },
            appProperties = new Dictionary<string, string> { ["sw_operation"] = pending.Marker, ["sw_hash"] = pending.Hash } };
        await using var opening = await SpreadsheetOpening.AcquireAsync(new(Path.Combine(w.Root, "state")), w.Source);
        await Assert.ThrowsAsync<InvalidDataException>(() => ReopeningImporter(w, http, new FixedAuth(), remote, opening.Candidates).ImportAsync(w.Source));
        Assert.Equal(0, server.Posts); Assert.Equal(old, w.Registry().Get(old.Id));
    }

    [Fact]
    public async Task WaiterDoesNotReclassifyOperationThatWasActiveAtArrival()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "state"));
        var first = await SpreadsheetOpening.AcquireAsync(storage, w.Source);
        var old = await w.Coordinator().PrepareAsync("A", w.Source);
        var waiting = SpreadsheetOpening.AcquireAsync(storage, w.Source);
        Assert.False(waiting.IsCompleted);
        await first.DisposeAsync(); await using var second = await waiting;
        Assert.DoesNotContain(old.Id, second.Candidates);
    }

    [Fact]
    public async Task DifferentFilesHaveIndependentOpeningLocks()
    {
        using var w = new Workspace(); var other = Path.Combine(w.Root, "other.xlsx"); File.Copy(w.Source, other);
        var storage = new LocalStorage(Path.Combine(w.Root, "state"));
        await using var first = await SpreadsheetOpening.AcquireAsync(storage, w.Source);
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(2));
        await using var second = await SpreadsheetOpening.AcquireAsync(storage, other, ct: timeout.Token);
    }

    [Fact]
    public async Task ManualRetryInSameWindowDoesNotQualifyAsAnotherOpening()
    {
        using var w = new Workspace(); var old = await w.Coordinator().PrepareAsync("A", w.Source);
        await using var opening = await SpreadsheetOpening.AcquireAsync(new(Path.Combine(w.Root, "state")), w.Source, allowFreshAttempt: false);
        Assert.DoesNotContain(old.Id, opening.Candidates);
    }
}
