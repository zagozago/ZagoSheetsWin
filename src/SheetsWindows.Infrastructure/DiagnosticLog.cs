using SheetsWindows.Core;
using System.Text;
using System.Text.Json;

namespace SheetsWindows.Infrastructure;

public enum DiagnosticEvent { Started, Completed, Cancelled, AuthorizationRequired, ReconciliationRequired, LocalConflict, ConversionMismatch, Failed, Exported }
public enum DiagnosticFailure { WorkbookContentType, FormulaVerification, ConversionDifference, GoogleApi, Network, FileAccess, InvalidData, UnsupportedFormat, Other }
public enum DiagnosticCode { SnapshotIntegrity, RemoteAssociation, SourceIdentity, SourceChanged, AssociationBinding, TextEncoding, TextStructure, CsvDelimiter, WorkbookContentType, FileAccess, Database, GoogleApi, Network, Other }
public sealed record DiagnosticEntry(DateTimeOffset Time, DiagnosticEvent Event, Guid? Operation, ProcessingMetrics? Metrics = null, DiagnosticFailure? Failure = null, string? Version = null, Guid? AttemptId = null, ProcessingPhase? Stage = null, DiagnosticCode? Code = null, int? NativeError = null, int? GoogleStatus = null, int? SqliteCode = null);
// Diagnostic files are disposable; durable operation journals and backups are never pruned.
public sealed class DiagnosticLog(LocalStorage storage)
{
    public const int MaxFileBytes = 64 * 1024;
    public const int FileCount = 4;
    public static DiagnosticEvent Failure(Exception ex) => ex switch
    {
        OperationCanceledException => DiagnosticEvent.Cancelled,
        AuthorizationRequiredException => DiagnosticEvent.AuthorizationRequired,
        ReconciliationRequiredException => DiagnosticEvent.ReconciliationRequired,
        SheetsWindows.Core.LocalConflictException => DiagnosticEvent.LocalConflict,
        ConversionMismatchException => DiagnosticEvent.ConversionMismatch,
        _ => DiagnosticEvent.Failed
    };
    public async Task RecordAsync(DiagnosticEvent kind, Guid? operation = null, ProcessingMetrics? metrics = null, Exception? failure = null, Guid? attemptId = null, ProcessingPhase? stage = null)
    {
        try
        {
            if (metrics is { IsValid: false }) throw new InvalidDataException("Invalid processing metrics.");
            if (!Enum.IsDefined(kind)) throw new ArgumentException("Unknown diagnostic event.");
            var root = Path.Combine(storage.Root, "logs"); PrivateDirectory.Create(root);
            await using var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("diagnostics");
            var path = Path.Combine(root, "events.jsonl");
            for (var i = 0; i < FileCount; i++)
            {
                var candidate = i == 0 ? path : path + "." + i;
                if (File.Exists(candidate) && (File.GetAttributes(candidate) & FileAttributes.ReparsePoint) != 0) throw new InvalidDataException("Invalid diagnostic file.");
            }
            DiagnosticFailure? reason = failure switch
            {
                null => null,
                FormulaVerificationException => DiagnosticFailure.FormulaVerification,
                ConversionMismatchException => DiagnosticFailure.ConversionDifference,
                InvalidDataException data when data.Message is "Unsupported workbook content type." or "Ambiguous workbook content type." or "Invalid content types XML." => DiagnosticFailure.WorkbookContentType,
                GoogleApiException => DiagnosticFailure.GoogleApi,
                HttpRequestException => DiagnosticFailure.Network,
                UnauthorizedAccessException or System.ComponentModel.Win32Exception => DiagnosticFailure.FileAccess,
                InvalidDataException => DiagnosticFailure.InvalidData,
                NotSupportedException => DiagnosticFailure.UnsupportedFormat,
                _ => DiagnosticFailure.Other
            };
            DiagnosticCode? code = failure is null ? null : failure switch
            {
                InvalidDataException data when data.Message == "Snapshot integrity failure." => DiagnosticCode.SnapshotIntegrity,
                InvalidDataException data when data.Message == "Remote conversion or association validation failed." => DiagnosticCode.RemoteAssociation,
                LocalConflictException conflict when conflict.Message == "Source identity changed; upload blocked." => DiagnosticCode.SourceIdentity,
                LocalConflictException conflict when conflict.Message == "Source changed; upload blocked." => DiagnosticCode.SourceChanged,
                InvalidOperationException binding when binding.Message == "Association binding changed." => DiagnosticCode.AssociationBinding,
                InvalidDataException data when data.Message is "Invalid text encoding or characters." or "Encoding conflicts with BOM." => DiagnosticCode.TextEncoding,
                InvalidDataException data when data.Message == "Ambiguous CSV delimiter; configure it explicitly." => DiagnosticCode.CsvDelimiter,
                InvalidDataException data when data.Message is "Irregular delimited table." or "Invalid quoted field." or "Unclosed quoted field." => DiagnosticCode.TextStructure,
                InvalidDataException data when data.Message is "Unsupported workbook content type." or "Ambiguous workbook content type." or "Invalid content types XML." => DiagnosticCode.WorkbookContentType,
                Microsoft.Data.Sqlite.SqliteException => DiagnosticCode.Database,
                GoogleApiException => DiagnosticCode.GoogleApi,
                HttpRequestException => DiagnosticCode.Network,
                IOException or UnauthorizedAccessException or System.ComponentModel.Win32Exception => DiagnosticCode.FileAccess,
                _ => DiagnosticCode.Other
            };
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new DiagnosticEntry(DateTimeOffset.UtcNow, kind, operation, metrics, reason, UiText.ProductVersion, attemptId, stage, code, failure?.HResult, (failure as GoogleApiException)?.Status, (failure as Microsoft.Data.Sqlite.SqliteException)?.SqliteErrorCode)) + "\n");
            if (File.Exists(path) && new FileInfo(path).Length + bytes.Length > MaxFileBytes)
            {
                var last = path + "." + (FileCount - 1); if (File.Exists(last)) File.Delete(last);
                for (var i = FileCount - 2; i >= 0; i--)
                {
                    var source = i == 0 ? path : path + "." + i;
                    if (File.Exists(source)) File.Move(source, path + "." + (i + 1), false);
                }
            }
            using var file = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read); file.Write(bytes); file.Flush(true);
        }
        catch (Exception ex) when (LauncherErrors.Expected(ex)) { /* Diagnostics cannot change operation outcome. */ }
    }
    public async Task ExportAsync(string destination, bool overwrite = false)
    {
        await using var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("diagnostics");
        var root = Path.Combine(storage.Root, "logs"); var output = new StringBuilder();
        for (var i = FileCount - 1; i >= 0; i--)
        {
            var path = Path.Combine(root, "events.jsonl" + (i == 0 ? "" : "." + i));
            if (!File.Exists(path)) continue;
            if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0 || new FileInfo(path).Length > MaxFileBytes) throw new InvalidDataException("Invalid diagnostic file.");
            foreach (var line in await File.ReadAllLinesAsync(path))
            {
                var entry = JsonSerializer.Deserialize<DiagnosticEntry>(line) ?? throw new InvalidDataException("Invalid diagnostic entry.");
                if (entry.Metrics is { IsValid: false }) throw new InvalidDataException("Invalid processing metrics.");
                if (entry.Failure is { } reason && !Enum.IsDefined(reason)) throw new InvalidDataException("Invalid diagnostic failure category.");
                if (!Enum.IsDefined(entry.Event)) throw new InvalidDataException("Invalid diagnostic event.");
                if (entry.Stage is { } stage && !Enum.IsDefined(stage) || entry.Code is { } code && !Enum.IsDefined(code))
                    throw new InvalidDataException("Invalid diagnostic context.");
                if (entry.Version is { } version && !System.Text.RegularExpressions.Regex.IsMatch(version, @"^\d{1,4}\.\d{1,4}\.\d{1,4}(?:-[A-Za-z0-9.-]{1,32})?$"))
                    throw new InvalidDataException("Invalid diagnostic version.");
                output.AppendLine(JsonSerializer.Serialize(entry)); // Re-serialize only allowlisted fields.
            }
        }
        output.AppendLine(JsonSerializer.Serialize(new DiagnosticEntry(DateTimeOffset.UtcNow, DiagnosticEvent.Exported, null, Version: UiText.ProductVersion)));
        var target = Path.GetFullPath(destination);
        if (File.Exists(target) && (File.GetAttributes(target) & FileAttributes.ReparsePoint) != 0) throw new InvalidDataException("Invalid diagnostic destination.");
        var temporary = target + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var file = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            { file.Write(Encoding.UTF8.GetBytes(output.ToString())); file.Flush(true); }
            File.Move(temporary, target, overwrite);
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }
}
