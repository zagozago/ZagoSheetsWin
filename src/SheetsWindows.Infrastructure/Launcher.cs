using System.ComponentModel;
using System.Security;
using System.Text;
using SheetsWindows.Core;

namespace SheetsWindows.Infrastructure;

public enum LauncherAction { Home, Open, Login, Defaults, Version, Setup, Recovery, Register, Unregister, Copy, FirstUse }
public sealed record LauncherRequest(LauncherAction Action, string? Path = null)
{
    public static LauncherRequest Parse(string[] args)
    {
        if (args.Length == 1 && args[0] == "--first-use") return new(LauncherAction.FirstUse);
        if (args.Length == 1 && args[0] == "--setup") return new(LauncherAction.Setup);
        if (args.Length == 1 && args[0] == "--recovery") return new(LauncherAction.Recovery);
        if (args.Length == 1 && args[0] == "--register") return new(LauncherAction.Register);
        if (args.Length == 1 && args[0] == "--unregister") return new(LauncherAction.Unregister);
        if (args.Length == 0) return new(LauncherAction.Home);
        if (args.Length == 1 && args[0] == "--version") return new(LauncherAction.Version);
        if (args.Length == 1 && args[0] == "--login") return new(LauncherAction.Login);
        if (args.Length == 1 && args[0] == "--defaults") return new(LauncherAction.Defaults);
        var path = args.Length == 1 && !args[0].StartsWith("--", StringComparison.Ordinal) ? args[0] :
            args.Length == 2 && args[0] is "--open" or "--copy" ? args[1] : throw new ArgumentException("One spreadsheet path required.");
        if (!System.IO.Path.IsPathFullyQualified(path) || path.Any(char.IsControl) || path.Contains('"')
            || !SpreadsheetFormats.Extensions.Contains(System.IO.Path.GetExtension(path).ToLowerInvariant())) throw new ArgumentException("Absolute supported spreadsheet path required.");
        return new(args.Length == 2 && args[0] == "--copy" ? LauncherAction.Copy : LauncherAction.Open, System.IO.Path.GetFullPath(path));
    }
}
public sealed class LauncherNotConfiguredException() : InvalidOperationException("Launcher setup required.");
public static class LauncherConfiguration
{
    public static string ClientPath(LocalStorage storage) => System.IO.Path.Combine(storage.Root, "launcher-client.json");
    public static void SaveClient(LocalStorage storage, string json)
    {
        if (Encoding.UTF8.GetByteCount(json) > 65536) throw new InvalidDataException("Client configuration too large.");
        var client = OAuthClient.FromJson(json); PrivateDirectory.Create(storage.Root);
        var path = ClientPath(storage);
        if (File.Exists(path))
        {
            if (OAuthClient.FromJson(File.ReadAllText(path)).Id != client.Id) throw new LocalConflictException("Changing OAuth client requires a separate migration.");
            return; // Preserve configured client; never change account namespace through registration.
        }
        var tmp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var file = new FileStream(tmp, FileMode.CreateNew, FileAccess.Write, FileShare.None)) { file.Write(Encoding.UTF8.GetBytes(json)); file.Flush(true); }
            File.Move(tmp, path, overwrite: false);
        }
        finally { if (File.Exists(tmp)) File.Delete(tmp); }
    }
    // Resolve only during setup; persist the selected identity before authorization.
    // Existing invalid data is an error, never a reason to switch clients.
    public static async Task<string> SetupClientJsonAsync(LocalStorage storage, string? customPath = null, CancellationToken ct = default)
    {
        if (File.Exists(ClientPath(storage)))
        {
            _ = await LoadClientAsync(storage, ct);
            return await File.ReadAllTextAsync(ClientPath(storage), ct);
        }
        string json;
        if (!string.IsNullOrWhiteSpace(customPath))
        {
            await using var file = new FileStream(customPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            if (file.Length > 65536 || (File.GetAttributes(customPath) & FileAttributes.ReparsePoint) != 0)
                throw new InvalidDataException("Invalid OAuth client.");
            using var reader = new StreamReader(file);
            json = await reader.ReadToEndAsync(ct);
        }
        else
        {
            using var resource = typeof(LauncherConfiguration).Assembly.GetManifestResourceStream("OAuth.official.desktop.json")
                ?? throw new LauncherNotConfiguredException();
            using var reader = new StreamReader(resource);
            json = await reader.ReadToEndAsync(ct);
        }
        _ = OAuthClient.FromJson(json);
        return json;
    }
    public static async Task<OAuthClient> LoadClientAsync(LocalStorage storage, CancellationToken ct = default)
    {
        var path = ClientPath(storage);
        if (!File.Exists(path)) throw new LauncherNotConfiguredException();
        await using var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (file.Length > 65536 || (File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0) throw new InvalidDataException("Invalid launcher configuration.");
        using var reader = new StreamReader(file); return OAuthClient.FromJson(await reader.ReadToEndAsync(ct).ConfigureAwait(false));
    }
}
public static class LauncherErrors
{
    public static bool Expected(Exception ex) => ex is IOException or InvalidDataException or UnauthorizedAccessException or InvalidOperationException
        or TimeoutException or NotSupportedException or ArgumentException or Win32Exception or HttpRequestException or OperationCanceledException
        or System.Security.Cryptography.CryptographicException or Microsoft.Data.Sqlite.SqliteException or System.Text.Json.JsonException or System.Xml.XmlException or KeyNotFoundException or SecurityException or ExcelDataReader.Exceptions.ExcelReaderException;
    public static string Message(Exception ex) => ex switch
    {
        BackupQuotaException => UiText.Get("error.backupQuota"),
        BackupRemovedException => UiText.Get("error.backupRemoved"),
        LauncherNotConfiguredException => UiText.Get("error.setupRequired"),
        AuthorizationRequiredException => UiText.Get("error.authorizationRequired"),
        ReconciliationRequiredException => UiText.Get("error.reconciliationRequired"),
        FormulaVerificationException => UiText.Get("error.formulaVerification"),
        ConversionMismatchException => UiText.Get("error.conversionMismatch"),
        SpreadsheetCapacityException capacity => UiText.Format("error.capacity", ("reason", capacity.UserMessage)),
        InvalidDataException data when data.Message is "Unsupported workbook content type." or "Ambiguous workbook content type." or "Invalid content types XML." => UiText.Get("error.xlsxContentType"),
        InvalidDataException data when data.Message is "Invalid text encoding or characters." or "Encoding conflicts with BOM." => UiText.Get("error.textEncoding"),
        InvalidDataException data when data.Message == "Ambiguous CSV delimiter; configure it explicitly." => UiText.Get("error.csvDelimiter"),
        InvalidDataException data when data.Message is "Irregular delimited table." or "Invalid quoted field." or "Unclosed quoted field." or "Empty text spreadsheet." or "Empty table." => UiText.Get("error.textStructure"),
        InvalidDataException data when data.Message == "Export too large." => UiText.Get("error.exportSize"),
        GoogleApiException api when api.Status == 429 || api.Status >= 500 => UiText.Get("error.googleUnavailable"),
        GoogleApiException api when api.Status is 401 or 403 => UiText.Get("error.googleAccess"),
        GoogleApiException => UiText.Get("error.googleConversion"),
        HttpRequestException => UiText.Get("error.network"),
        CopyRequiredException => UiText.Get("error.copyRequired"),
        LocalConflictException => UiText.Get("error.localConflict"),
        TimeoutException => UiText.Get("error.busy"),
        OperationCanceledException => UiText.Get("error.cancelled"),
        _ => UiText.Get("error.generic")
    };
}

public sealed class WindowsLauncher(LocalStorage storage, HttpClient http, IBrowserLauncher browser, ProcessingTelemetry? telemetry = null)
{
    public string? ImportNotice { get; private set; }
    private GoogleOAuth Auth(OAuthClient client, FileOperationLock locks) => new(http, client,
        new DpapiTokenVault(System.IO.Path.Combine(storage.Root, "auth"), client.Id), new LoopbackAuthorizationReceiver(browser.Open), locks);
    public async Task CheckConnectionAsync(CancellationToken ct = default)
    {
        var client = await LauncherConfiguration.LoadClientAsync(storage, ct);
        try { _ = await Auth(client, new FileOperationLock(storage.LocksPath)).AccessAsync(refresh: true, cancellationToken: ct); }
        catch (GoogleApiException ex) when (ex.Status == 401) { throw new AuthorizationRequiredException(); }
    }
    public async Task LoginAsync(CancellationToken ct = default)
    {
        var client = await LauncherConfiguration.LoadClientAsync(storage, ct);
        await Auth(client, new FileOperationLock(storage.LocksPath)).ConnectAsync(ct);
    }
    public async Task<string> OpenAsync(string path, IProgress<string>? progress = null, CancellationToken ct = default)
    {
        ImportNotice = null;
        // Optional folder scope; fixed-local-file and sync protections still apply.
        var request = LauncherRequest.Parse(["--open", path]);
        var client = await LauncherConfiguration.LoadClientAsync(storage, ct);
        if (!OpeningPolicy.IsConfigured(storage)) throw new LauncherNotConfiguredException();
        var opening = OpeningPolicy.Load(storage);
        if (!ShortcutSettings.Load(storage)) return await CopyAsync(path, progress, ct);
        if (!opening.RestrictToFolder && SourceEnvironment.RequiresCopy(path))
        {
            var copied = await CopyAsync(path, progress, ct);
            ImportNotice = UiText.Get("import.notice.syncedCopy");
            return copied;
        }
        if (SpreadsheetFormats.Format(path) == "xls" && !XlsReplacementSettings.Load(storage)) return await CopyAsync(path, progress, ct);
        var sources = new WindowsRetirementReader(opening.RestrictToFolder ? opening.Folder : null);
        await using (var eligibility = sources.Open(request.Path!)) { }
        progress?.Report(UiText.Get("progress.fileAndBackup"));
        var textOptions = SpreadsheetFormats.Format(path) == "xlsx" ? null : ExtendedConfiguration.Load(storage);
        var preparation = storage.CreatePreparation(); var local = new SqliteOperationRegistry(storage.DatabasePath);
        var remote = new GoogleRemoteRegistry(System.IO.Path.Combine(storage.Root, "google.db")); var locks = new FileOperationLock(storage.LocksPath);
        var auth = Auth(client, locks);
        var importer = new GoogleImport(preparation, local, remote, new SourceReader(), locks, auth, new GoogleDriveClient(http, auth, new UploadSessionStore(Path.Combine(storage.Root, "uploads"))), textOptions, telemetry, new BackupManagement(storage));
        progress?.Report(UiText.Get("progress.openingSheets"));
        var receipt = await importer.ImportReceiptAsync(request.Path!, ct);
        if (!receipt.CanReplace) return await PublishCopyAsync(receipt, progress, ct);
        progress?.Report(UiText.Get("progress.publishingShortcut"));
        var coordinator = new ReplacementCoordinator(local, remote, new ManagedBackupStore(storage), locks,
            new ReplacementJournal(System.IO.Path.Combine(storage.Root, "replacement.db")), sources, browser, new ConversionVerifier(new GoogleDriveClient(http, auth), textOptions, telemetry), ShortcutIcon.Ensure(storage));
        try { return await coordinator.ReplaceAsync(receipt, ct); }
        catch (FormulaVerificationException) when (receipt.Operation.Format == "xls")
        {
            var shortcut = await PublishCopyAsync(receipt, progress, ct);
            ImportNotice = UiText.Get("import.notice.formulaCopy");
            return shortcut;
        }
    }
    public async Task<string> CopyAsync(string path, IProgress<string>? progress = null, CancellationToken ct = default)
    {
        _ = LauncherRequest.Parse(["--copy", path]);
        var client = await LauncherConfiguration.LoadClientAsync(storage, ct);
        var options = SpreadsheetFormats.Format(path) == "xlsx" ? null : ExtendedConfiguration.Load(storage);
        var sources = new SourceReader(copyEnvironments: true);
        // Preflight is read-only, does not hydrate online-only placeholders and precedes OAuth/network.
        await using (var preflight = sources.Open(path)) { }
        var locks = new FileOperationLock(storage.LocksPath); var auth = Auth(client, locks);
        var importer = new GoogleImport(storage.CreatePreparation(sources), new SqliteOperationRegistry(storage.DatabasePath),
            new GoogleRemoteRegistry(Path.Combine(storage.Root, "google.db")), sources, locks, auth, new GoogleDriveClient(http, auth, new UploadSessionStore(Path.Combine(storage.Root, "uploads"))), options, telemetry, new BackupManagement(storage));
        progress?.Report(UiText.Get("progress.importingCopy"));
        return await PublishCopyAsync(await importer.ImportReceiptAsync(path, ct), progress, ct);
    }
    public async Task<string> ResumeAsync(Guid id, bool replace, IProgress<string>? progress = null, CancellationToken ct = default)
    {
        var local = new SqliteOperationRegistry(storage.DatabasePath);
        var operation = local.Get(id) ?? throw new InvalidOperationException("Unknown operation.");
        new BackupManagement(storage).RequireAvailable(id);
        var journal = new ReplacementJournal(Path.Combine(storage.Root, "replacement.db"));
        var recorded = journal.Get(id); var path = recorded?.SourcePath ?? operation.SourcePath;
        if (replace && !ShortcutSettings.Load(storage))
        {
            if (!File.Exists(path) || recorded is { Step: >= 3 }) throw new CopyRequiredException();
            replace = false;
        }
        var remote = new GoogleRemoteRegistry(Path.Combine(storage.Root, "google.db"));
        var mapping = remote.Get("sheet:" + id.ToString("N")) ?? throw new ReconciliationRequiredException();
        var client = await LauncherConfiguration.LoadClientAsync(storage, ct); var locks = new FileOperationLock(storage.LocksPath); var auth = Auth(client, locks);
        var access = await auth.AccessAsync(cancellationToken: ct);
        if (access.AccountId != operation.AccountId || mapping.AccountId != operation.AccountId) throw new LocalConflictException("Account changed.");
        var drive = new GoogleDriveClient(http, auth, new UploadSessionStore(Path.Combine(storage.Root, "uploads")));
        var options = operation.Format == "xlsx" ? null : ExtendedConfiguration.Load(storage);
        ImportReceipt receipt;
        if (File.Exists(path))
        {
            var sources = new SourceReader(copyEnvironments: !replace);
            await using (var source = sources.Open(path))
            {
                if (source.Source.IdentityKey != operation.SourceKey || source.Source.Format != operation.Format) throw new LocalConflictException("Source changed.");
                var importer = new GoogleImport(storage.CreatePreparation(sources), local, remote, sources, locks, auth, drive, options, telemetry, new BackupManagement(storage));
                receipt = await importer.ImportReceiptAsync(path, ct);
                if (receipt.Operation.Id != id) throw new LocalConflictException("Operation changed.");
            }
        }
        else
        {
            // Only a recorded retirement can reconcile an absent original; no new upload is allowed.
            if (!replace || recorded is null || recorded.Step < 3 || mapping.FileId is null || !mapping.Verified) throw new ReconciliationRequiredException();
            var file = await drive.GetAsync(access.AccountId, mapping.FileId, ct);
            if (file.Trashed || !file.CanEdit || file.MimeType != GoogleDriveClient.SheetMime
                || !file.Properties.TryGetValue("sw_operation", out var marker) || marker != mapping.Marker
                || !file.Properties.TryGetValue("sw_hash", out var hash) || hash != mapping.Hash) throw new ReconciliationRequiredException();
            receipt = new(operation, GoogleDriveClient.Editor(mapping.FileId), path);
        }
        if (!replace) return await PublishCopyAsync(receipt, progress, ct);
        if (!receipt.CanReplace || operation.Format == "xls" && !XlsReplacementSettings.Load(storage)) throw new CopyRequiredException();
        if (!OpeningPolicy.IsConfigured(storage)) throw new LauncherNotConfiguredException();
        var opening = OpeningPolicy.Load(storage);
        return await new ReplacementCoordinator(local, remote, new ManagedBackupStore(storage), locks, journal,
            new WindowsRetirementReader(opening.RestrictToFolder ? opening.Folder : null), browser, new ConversionVerifier(drive, options, telemetry), ShortcutIcon.Ensure(storage)).ReplaceAsync(receipt, ct);
    }

    private async Task<string> PublishCopyAsync(ImportReceipt receipt, IProgress<string>? progress, CancellationToken ct)
    {
        await using var held = await new FileOperationLock(storage.LocksPath).AcquireAsync(receipt.Operation.SourceKey, ct);
        await new ManagedBackupStore(storage).VerifyAsync(receipt.Operation.Id, receipt.Operation.Snapshot ?? throw new InvalidDataException("No backup."), ct);
        if (!ShortcutSettings.Load(storage))
        {
            ct.ThrowIfCancellationRequested(); browser.Open(receipt.Url);
            await new BackupManagement(storage).RegisterCopyCompletionAsync(receipt, ct);
            progress?.Report(UiText.Get("emphasis.originalPreserved"));
            return receipt.Url.AbsoluteUri;
        }
        var folder = Path.Combine(storage.Root, "shortcuts"); PrivateDirectory.Create(folder);
        var shortcut = Path.Combine(folder, receipt.Operation.Id.ToString("N") + ".url"); var bytes = InternetShortcut.ForExisting(shortcut, receipt.Url, ShortcutIcon.Ensure(storage));
        if (!File.Exists(shortcut)) InternetShortcut.Publish(shortcut, bytes);
        using var checkedShortcut = InternetShortcut.Hold(shortcut, bytes);
        ct.ThrowIfCancellationRequested(); browser.Open(receipt.Url);
        await new BackupManagement(storage).RegisterCopyCompletionAsync(receipt, ct);
        progress?.Report(UiText.Get("import.notice.copyComplete"));
        return shortcut;
    }

}
