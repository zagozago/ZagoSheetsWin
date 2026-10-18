using System.Security.Cryptography;
using SheetsWindows.Core;

namespace SheetsWindows.Infrastructure;

public interface IConversionVerifier
{
    Task VerifyAsync(ImportOperation operation, RemoteAttempt mapping, CancellationToken ct);
}
public sealed class ConversionVerifier(GoogleDriveClient drive, TextImportOptions? options = null, ProcessingTelemetry? telemetry = null, ImportInterpretations? interpretations = null) : IConversionVerifier
{
    public bool HasWarnings { get; private set; }
    private void Warn() { HasWarnings = true; telemetry?.Mark(ProcessingStep.ConversionWarning); }
    public async Task VerifyAsync(ImportOperation operation, RemoteAttempt mapping, CancellationToken ct)
    {
        HasWarnings = false;
        using var timing = telemetry?.Begin(ProcessingPhase.Verification);
        var snapshot = operation.Snapshot ?? throw new InvalidDataException("Backup required.");
        if (snapshot.Length > GoogleImport.MaxBytes || mapping.FileId is null || !mapping.Verified || mapping.AccountId != operation.AccountId || mapping.Hash != snapshot.Sha256) throw new InvalidDataException("Fidelity binding invalid.");
        await using var file = new FileStream(snapshot.BackupPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (file.Length != snapshot.Length) throw new InvalidDataException("Backup changed.");
        var bytes = new byte[checked((int)file.Length)]; await file.ReadExactlyAsync(bytes, ct);
        if (Convert.ToHexString(SHA256.HashData(bytes)) != snapshot.Sha256) throw new InvalidDataException("Backup changed.");
        var interpretation = operation.Format is "csv" or "tsv" ? interpretations?.Load(operation, options ?? new()) ?? options : options;
        var expected = await Task.Run(() => SpreadsheetFormats.Prepare(operation.Format, bytes, interpretation, ct).Expected, ct);
        if (expected is null) { Warn(); return; }
        var exported = await drive.ExportXlsxAsync(operation.AccountId, mapping.FileId, ct);
        try { await Task.Run(() => SpreadsheetFormats.VerifyValues(expected, exported, ct, textImport: operation.Format is "csv" or "tsv"), ct); }
        catch (ConversionMismatchException) { Warn(); }
    }
}
