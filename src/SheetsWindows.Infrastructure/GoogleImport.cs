using System.IO.Compression;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml;
using SheetsWindows.Core;

namespace SheetsWindows.Infrastructure;

public sealed class GoogleImport(LocalPreparation preparation, IOperationRegistry local, IRemoteRegistry remote,
    ISourceReader sources, IOperationLock locks, IGoogleAuth auth, GoogleDriveClient drive, TextImportOptions? textOptions = null, ProcessingTelemetry? telemetry = null, BackupManagement? backupManagement = null)
{
    public const int MaxBytes = 20 * 1024 * 1024;
    public async Task<Uri> ImportAsync(string path, CancellationToken ct = default) => (await ImportReceiptAsync(path, ct)).Url;
    public async Task<ImportReceipt> ImportReceiptAsync(string path, CancellationToken ct = default, bool renewTrashed = true)
    {
        telemetry?.Mark(ProcessingStep.Preflight, CheckpointOutcome.Started);
        await using (var preflight = sources.Open(path))
            if (preflight.Content.Length > MaxBytes) throw new SpreadsheetCapacityException(UiText.Message("capacity.importSize"));
        telemetry?.Mark(ProcessingStep.Preflight);
        telemetry?.Mark(ProcessingStep.Authorization, CheckpointOutcome.Started);
        var access = await auth.AccessAsync(cancellationToken: ct);
        telemetry?.Mark(ProcessingStep.Authorization);
        if (renewTrashed) await RetireTrashedSourceAssociationAsync(access.AccountId, path, ct);
        if (backupManagement is not null)
        {
            Guid? existingId; long incoming;
            await using (var preflight = sources.Open(path))
            {
                var existing = local.Pending().FirstOrDefault(o => o.AccountId == access.AccountId && o.SourceKey == preflight.Source.IdentityKey);
                existingId = existing?.Id;
                if (existingId is { } existingOperationId) backupManagement.RequireAvailable(existingOperationId);
                incoming = existing?.Snapshot is { } saved && File.Exists(saved.BackupPath) ? 0 : preflight.Content.Length;
            }
            await backupManagement.MaintainAsync(ct, existingId);
            await backupManagement.MakeRoomAsync(incoming, ct, existingId);
        }
        telemetry?.Mark(ProcessingStep.SnapshotPreparation, CheckpointOutcome.Started);
        var op = await preparation.PrepareAsync(access.AccountId, path, ct);
        telemetry?.Mark(ProcessingStep.SnapshotPreparation);
        await using var source = sources.Open(path);
        if (source.Source.IdentityKey != op.SourceKey) throw new LocalConflictException("Source identity changed; upload blocked.");
        await using var held = await locks.AcquireAsync(source.Source.IdentityKey, ct);
        op = local.Get(op.Id)!;
        var snapshot = op.Snapshot ?? throw new InvalidOperationException("Snapshot required.");
        source.Content.Position = 0;
        if (Convert.ToHexString(await SHA256.HashDataAsync(source.Content, ct)) != snapshot.Sha256) throw new LocalConflictException("Source changed; upload blocked.");
        telemetry?.Mark(ProcessingStep.SourceValidation);
        // Hold the backup handle while checking and freezing the bytes that will be sent.
        await using var backup = new FileStream(snapshot.BackupPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (backup.Length > MaxBytes) throw new SpreadsheetCapacityException(UiText.Message("capacity.importSize"));
        using var buffer = new MemoryStream(); await backup.CopyToAsync(buffer, ct); var bytes = buffer.ToArray();
        if (bytes.LongLength != snapshot.Length || Convert.ToHexString(SHA256.HashData(bytes)) != snapshot.Sha256) throw new InvalidDataException("Snapshot integrity failure.");
        telemetry?.Mark(ProcessingStep.SnapshotValidation);
        SpreadsheetPayload payload;
        telemetry?.Mark(ProcessingStep.Conversion, CheckpointOutcome.Started);
        using (telemetry?.Begin(ProcessingPhase.Conversion)) payload = await Task.Run(() => SpreadsheetFormats.Prepare(op.Format, bytes, textOptions, ct), ct);
        telemetry?.Mark(ProcessingStep.Conversion);
        using var uploadTiming = telemetry?.Begin(ProcessingPhase.Upload);
        // Fidelity limitations are warnings after a verified Google import; replacement follows user settings.
        var canReplace = true;
        var sheetKey = "sheet:" + op.Id.ToString("N");
        if (remote.Get(sheetKey) is { } pending)
        {
            if (pending.AccountId != access.AccountId || pending.Hash != snapshot.Sha256) throw new LocalConflictException("Upload binding changed.");
            // Existing sessions must finish with the exact payload used by their originating version.
            if (pending.FileId is null && op.Format == "xls")
            {
                try { _ = drive.CanResume(pending, payload.Bytes, payload.MimeType); }
                catch (LocalConflictException)
                {
                    if (!drive.CanResume(pending, bytes, "application/vnd.ms-excel")) throw;
                    payload = new(bytes, "application/vnd.ms-excel", SpreadsheetFormats.ReadExcel(bytes, binary: true, ct: ct));
                }
            }
            if (pending.FileId is null && drive.CanResume(pending, payload.Bytes, payload.MimeType))
            {
                telemetry?.Mark(ProcessingStep.UploadResume, CheckpointOutcome.Started);
                try { remote.Candidate(sheetKey, await drive.ResumeAsync(pending, payload.Bytes, payload.MimeType, ct)); }
                catch (ReconciliationRequiredException) { /* Reconcile the marker; never repeat initiation. */ }
            }
            var known = await EnsureAsync(sheetKey, access.AccountId, "sheet", snapshot.Sha256,
                _ => throw new ReconciliationRequiredException(), ct);
            telemetry?.Mark(ProcessingStep.ImportReady);
            return new ImportReceipt(op, GoogleDriveClient.Editor(known), source.Source.Path, canReplace);
        }
        var folder = await EnsureFolderForAccountAsync(access.AccountId, ct);
        var id = await EnsureAsync(sheetKey, access.AccountId, "sheet", snapshot.Sha256,
            a => drive.CreateAsync(a, Path.GetFileNameWithoutExtension(path), folder, payload.Bytes, ct, payload.MimeType), ct);
        telemetry?.Mark(ProcessingStep.ImportReady);
        return new ImportReceipt(op, GoogleDriveClient.Editor(id), source.Source.Path, canReplace);
    }
    public async Task<string> EnsureFolderAsync(CancellationToken ct = default)
    {
        var access = await auth.AccessAsync(cancellationToken: ct);
        return await EnsureFolderForAccountAsync(access.AccountId, ct);
    }
    private async Task<string> EnsureFolderForAccountAsync(string accountId, CancellationToken ct)
    {
        var key = "folder:" + accountId;
        await using var held = await locks.AcquireAsync(key, ct);
        telemetry?.Mark(ProcessingStep.FolderAssociation, CheckpointOutcome.Started);
        if (remote.Get(key) is { FileId: not null } old)
        {
            if (old.AccountId != accountId || old.Kind != "folder" || old.Hash != "") throw new InvalidOperationException("Association binding changed.");
            var file = await drive.GetAsync(accountId, old.FileId, ct);
            if (file.Trashed)
            {
                ValidateRemote(file, old, allowTrashed: true);
                remote.RetireTrashedAssociation(key, old.FileId);
                telemetry?.Mark(ProcessingStep.TrashedAssociationRetired);
            }
        }
        return await EnsureAsync(key, accountId, "folder", "", a => drive.CreateAsync(a, "ZagoSheetsWin", "root", null, ct), ct);
    }
    private async Task RetireTrashedSourceAssociationAsync(string accountId, string path, CancellationToken ct)
    {
        await using var source = sources.Open(path);
        await using var held = await locks.AcquireAsync(source.Source.IdentityKey, ct);
        var old = local.Pending().FirstOrDefault(o => o.AccountId == accountId && o.SourceKey == source.Source.IdentityKey);
        if (old?.Snapshot is not { } snapshot || remote.Get("sheet:" + old.Id.ToString("N")) is not { FileId: not null } mapping) return;
        if (mapping.AccountId != accountId || mapping.Kind != "sheet" || mapping.Hash != snapshot.Sha256) throw new InvalidOperationException("Association binding changed.");
        telemetry?.Mark(ProcessingStep.PreviousAssociation, CheckpointOutcome.Started);
        var file = await drive.GetAsync(accountId, mapping.FileId, ct);
        if (file.Trashed)
        {
            var validation = ValidateRemote(file, mapping, allowTrashed: true);
            ct.ThrowIfCancellationRequested();
            local.RetireTrashedAssociation(old.Id, old.Version);
            telemetry?.Mark(ProcessingStep.TrashedAssociationRetired, validation: validation);
        }
        telemetry?.Mark(ProcessingStep.PreviousAssociation);
    }
    private async Task<string> EnsureAsync(string key, string account, string kind, string hash, Func<RemoteAttempt, Task<string>> create, CancellationToken ct)
    {
        var attempt = remote.Get(key);
        if (attempt is null)
        {
            attempt = remote.Begin(key, account, kind, hash); // commit intent BEFORE touching Google
            telemetry?.Mark(ProcessingStep.RemoteIntent);
            telemetry?.Mark(ProcessingStep.RemoteCandidate, CheckpointOutcome.Started);
            var candidate = await create(attempt); remote.Candidate(key, candidate);
            telemetry?.Mark(ProcessingStep.RemoteCandidate);
            attempt = remote.Get(key)!;
        }
        if (attempt.AccountId != account || attempt.Kind != kind || attempt.Hash != hash) throw new InvalidOperationException("Association binding changed.");
        GoogleFile file;
        telemetry?.Mark(ProcessingStep.RemoteLookup, CheckpointOutcome.Started);
        if (attempt.FileId is not null) file = await drive.GetAsync(account, attempt.FileId, ct);
        else
        {
            var found = await drive.FindAsync(attempt, ct);
            if (found.Count != 1) throw new ReconciliationRequiredException();
            file = found[0]; remote.Candidate(key, file.Id);
        }
        telemetry?.Mark(ProcessingStep.RemoteLookup);
        ValidateRemote(file, attempt);
        remote.Verify(key, file.Id);
        telemetry?.Mark(ProcessingStep.RemoteVerified);
        return file.Id;
    }
    private RemoteValidationContext ValidateRemote(GoogleFile file, RemoteAttempt attempt, bool allowTrashed = false)
    {
        var hasMarker = file.Properties.TryGetValue("sw_operation", out var marker);
        var hasHash = file.Properties.TryGetValue("sw_hash", out var remoteHash);
        var validation = new RemoteValidationContext(attempt.Kind == "folder", file.Trashed, file.CanEdit,
            file.MimeType == (attempt.Kind == "sheet" ? GoogleDriveClient.SheetMime : GoogleDriveClient.FolderMime),
            hasMarker, hasMarker && marker == attempt.Marker, hasHash, hasHash && remoteHash == attempt.Hash, attempt.Verified);
        if (validation.IsTrashed && !allowTrashed || !validation.CanEdit || !validation.MimeMatches || !validation.MarkerMatches || !validation.HashMatches)
        {
            telemetry?.Mark(ProcessingStep.RemoteValidation, CheckpointOutcome.Failed, validation);
            var failure = new InvalidDataException("Remote conversion or association validation failed.");
            failure.Data["RemoteValidation"] = validation;
            throw failure;
        }
        telemetry?.Mark(ProcessingStep.RemoteValidation, validation: validation);
        return validation;
    }
    public static void ValidateXlsx(byte[] bytes)
    {
        using var zip = new ZipArchive(new MemoryStream(bytes), ZipArchiveMode.Read);
        if (zip.Entries.Count > 10000 || zip.Entries.Select(e => e.FullName).Distinct(StringComparer.OrdinalIgnoreCase).Count() != zip.Entries.Count) throw new InvalidDataException("Ambiguous or oversized ZIP index.");
        if (zip.Entries.Any(e => e.FullName.EndsWith("vbaProject.bin", StringComparison.OrdinalIgnoreCase))) throw new NotSupportedException("Macros are not supported.");
        var types = zip.GetEntry("[Content_Types].xml") ?? throw new InvalidDataException("Not an XLSX package.");
        var workbook = zip.GetEntry("xl/workbook.xml") ?? throw new InvalidDataException("Workbook missing.");
        if (types.Length > 1024 * 1024 || workbook.Length > 1024 * 1024) throw new InvalidDataException("Workbook metadata too large.");
        using var t = types.Open(); using var tr = XmlReader.Create(t, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null, MaxCharactersInDocument = 1024 * 1024 }); var xml = XDocument.Load(tr);
        XNamespace typesNamespace = "http://schemas.openxmlformats.org/package/2006/content-types";
        if (xml.Root?.Name != typesNamespace + "Types") throw new InvalidDataException("Invalid content types XML.");
        var overrides = xml.Root.Elements(typesNamespace + "Override").Where(e => (string?)e.Attribute("PartName") == "/xl/workbook.xml").ToArray();
        var defaults = xml.Root.Elements(typesNamespace + "Default").Where(e => string.Equals((string?)e.Attribute("Extension"), "xml", StringComparison.OrdinalIgnoreCase)).ToArray();
        if (overrides.Length > 1 || defaults.Length > 1) throw new InvalidDataException("Ambiguous workbook content type.");
        var workbookType = (string?)(overrides.Length == 1 ? overrides[0] : defaults.SingleOrDefault())?.Attribute("ContentType");
        if (workbookType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml") throw new InvalidDataException("Unsupported workbook content type.");
        using var w = workbook.Open(); using var wr = XmlReader.Create(w, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null, MaxCharactersInDocument = 1024 * 1024 }); var book = XDocument.Load(wr);
        if (book.Root?.Name != XName.Get("workbook", "http://schemas.openxmlformats.org/spreadsheetml/2006/main")) throw new InvalidDataException("Invalid workbook XML.");
    }
}

