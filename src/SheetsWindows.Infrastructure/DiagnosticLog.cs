using SheetsWindows.Core;
using System.Text;
using System.Text.Json;

namespace SheetsWindows.Infrastructure;

public enum DiagnosticEvent { Started, Completed, Cancelled, AuthorizationRequired, ReconciliationRequired, LocalConflict, ConversionMismatch, Failed, Exported }
public enum DiagnosticFailure { WorkbookContentType, FormulaVerification, ConversionDifference, GoogleApi, Network, FileAccess, InvalidData, UnsupportedFormat, Other, Capacity }
public enum DiagnosticCode { SnapshotIntegrity, RemoteAssociation, SourceIdentity, SourceChanged, AssociationBinding, TextEncoding, TextStructure, CsvDelimiter, WorkbookContentType, FileAccess, Database, GoogleApi, Network, Other, FileMissing, FileBusy, OdsPackage, TextRows, TextCells, TextColumns, CellCharacters, ImportBytes, NormalizedBytes, OdsContentBounds, OdsRepeat, RemoteExportBytes, TextRowWidth }
public sealed record RemoteValidationContext(bool IsFolder, bool IsTrashed, bool CanEdit, bool MimeMatches,
    bool MarkerPresent, bool MarkerMatches, bool HashPresent, bool HashMatches, bool PreviouslyVerified);
public sealed record DiagnosticEntry(DateTimeOffset Time, DiagnosticEvent Event, Guid? Operation, ProcessingMetrics? Metrics = null, DiagnosticFailure? Failure = null, string? Version = null, Guid? AttemptId = null, ProcessingPhase? Stage = null, DiagnosticCode? Code = null, int? NativeError = null, int? GoogleStatus = null, int? SqliteCode = null, RemoteValidationContext? RemoteValidation = null, IReadOnlyList<ProcessingCheckpoint>? Trace = null, string? Revision = null, int? Win32Code = null);
// Diagnostic files are disposable; durable operation journals and backups are never pruned.
public sealed class DiagnosticLog(LocalStorage storage)
{
    public static string SuggestedFileName(string? sourcePath = null)
    {
        if (string.IsNullOrEmpty(sourcePath)) return "ZSW-diagn.jsonl";
        // Deliberate filename metadata only. Source paths and names never enter the exported JSONL.
        var name = sourcePath.Replace('\\', '/').Split('/')[^1].ToLowerInvariant();
        var suffix = string.Concat(name.Where(char.IsLetterOrDigit).Take(100));
        return suffix.Length == 0 ? "ZSW-diagn.jsonl" : "ZSW-diagn-" + suffix + ".jsonl";
    }
    public static string? BuildRevision
    {
        get
        {
            var version = System.Reflection.CustomAttributeExtensions.GetCustomAttribute<System.Reflection.AssemblyInformationalVersionAttribute>(typeof(DiagnosticLog).Assembly)?.InformationalVersion;
            var match = System.Text.RegularExpressions.Regex.Match(version ?? "", @"[+]([a-fA-F0-9]{40})$");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
    private static void ValidateRevision(string? revision)
    {
        if (revision is not null && !System.Text.RegularExpressions.Regex.IsMatch(revision, @"^[a-fA-F0-9]{40}$"))
            throw new InvalidDataException("Invalid diagnostic revision.");
    }
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
    public async Task RecordAsync(DiagnosticEvent kind, Guid? operation = null, ProcessingMetrics? metrics = null, Exception? failure = null, Guid? attemptId = null, ProcessingPhase? stage = null, IReadOnlyList<ProcessingCheckpoint>? trace = null)
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
            ValidateTrace(trace);
            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(CreateEntry(kind, operation, metrics, failure, attemptId, stage, trace)) + "\n");
            if (bytes.Length > MaxFileBytes) throw new InvalidDataException("Diagnostic entry too large.");
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
    public static DiagnosticEntry CreateEntry(DiagnosticEvent kind, Guid? operation = null, ProcessingMetrics? metrics = null,
        Exception? failure = null, Guid? attemptId = null, ProcessingPhase? stage = null, IReadOnlyList<ProcessingCheckpoint>? trace = null)
    {
        DiagnosticFailure? reason = failure switch
        {
            null => null,
            SpreadsheetCapacityException => DiagnosticFailure.Capacity,
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
            SpreadsheetCapacityException capacity => capacity.Code,
            InvalidDataException data when data.Message == "Export too large." => DiagnosticCode.RemoteExportBytes,
            InvalidDataException data when data.Message == "Irregular delimited table." => DiagnosticCode.TextRowWidth,
            InvalidDataException data when data.Message is "ODS content dimensions exceed limits." or "ODS columns too large." or "ODS rows too large." => DiagnosticCode.OdsContentBounds,
            InvalidDataException data when data.Message == "Unbounded repetition." => DiagnosticCode.OdsRepeat,
            InvalidDataException data when data.Message is "Not an ODS package." or "ODS mimetype missing." or "Invalid ODS mimetype." or "Invalid ODS workbook." => DiagnosticCode.OdsPackage,
            FileNotFoundException or DirectoryNotFoundException => DiagnosticCode.FileMissing,
            System.ComponentModel.Win32Exception win32 when win32.NativeErrorCode is 32 or 33 => DiagnosticCode.FileBusy,
            IOException io when (io.HResult & 0xffff) is 32 or 33 => DiagnosticCode.FileBusy,
            InvalidDataException data when data.Message == "Snapshot integrity failure." => DiagnosticCode.SnapshotIntegrity,
            InvalidDataException data when data.Message == "Remote conversion or association validation failed." => DiagnosticCode.RemoteAssociation,
            LocalConflictException conflict when conflict.Message == "Source identity changed; upload blocked." => DiagnosticCode.SourceIdentity,
            LocalConflictException conflict when conflict.Message is "Source changed; upload blocked." or "Source changed after snapshot; original is preserved." => DiagnosticCode.SourceChanged,
            InvalidOperationException binding when binding.Message == "Association binding changed." => DiagnosticCode.AssociationBinding,
            InvalidDataException data when data.Message is "Invalid text encoding or characters." or "Encoding conflicts with BOM." => DiagnosticCode.TextEncoding,
            InvalidDataException data when data.Message == "Ambiguous CSV delimiter; configure it explicitly." => DiagnosticCode.CsvDelimiter,
            InvalidDataException data when data.Message is "Irregular delimited table." or "Invalid quoted field." or "Unclosed quoted field." => DiagnosticCode.TextStructure,
            InvalidDataException data when data.Message is "Unsupported workbook content type." or "Ambiguous workbook content type." or "Invalid content types XML." => DiagnosticCode.WorkbookContentType,
            InvalidDataException => DiagnosticCode.Other,
            Microsoft.Data.Sqlite.SqliteException => DiagnosticCode.Database,
            GoogleApiException => DiagnosticCode.GoogleApi,
            HttpRequestException => DiagnosticCode.Network,
            IOException or UnauthorizedAccessException or System.ComponentModel.Win32Exception => DiagnosticCode.FileAccess,
            _ => DiagnosticCode.Other
        };
        return new DiagnosticEntry(DateTimeOffset.UtcNow, kind, operation, metrics, reason, UiText.ProductVersion, attemptId, stage, code,
            failure?.HResult, (failure as GoogleApiException)?.Status, (failure as Microsoft.Data.Sqlite.SqliteException)?.SqliteErrorCode, failure?.Data["RemoteValidation"] as RemoteValidationContext, trace, BuildRevision, (failure as System.ComponentModel.Win32Exception)?.NativeErrorCode);
    }
    private static void ValidateTrace(IReadOnlyList<ProcessingCheckpoint>? trace)
    {
        if (trace is null) return;
        if (trace.Count > ProcessingTelemetry.MaxCheckpoints || trace.Any(p => p is null || !Enum.IsDefined(p.Step) || !Enum.IsDefined(p.Outcome) || p.ElapsedMs < 0))
            throw new InvalidDataException("Invalid diagnostic trace.");
    }
    public async Task ExportAsync(string destination, bool overwrite = false, DiagnosticEntry? currentAttempt = null)
    {
        await using var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("diagnostics");
        var root = Path.Combine(storage.Root, "logs"); var output = new StringBuilder();
        var included = new HashSet<(Guid, DiagnosticEvent)>();
        for (var i = FileCount - 1; i >= 0; i--)
        {
            var path = Path.Combine(root, "events.jsonl" + (i == 0 ? "" : "." + i));
            if (!File.Exists(path)) continue;
            if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0 || new FileInfo(path).Length > MaxFileBytes) throw new InvalidDataException("Invalid diagnostic file.");
            foreach (var line in await File.ReadAllLinesAsync(path))
            {
                DiagnosticEntry entry;
                try { entry = JsonSerializer.Deserialize<DiagnosticEntry>(line) ?? throw new InvalidDataException("Invalid diagnostic entry."); }
                catch (JsonException) when (currentAttempt?.AttemptId is not null) { continue; } // The window snapshot remains exportable even if disposable history is damaged.
                if (currentAttempt?.AttemptId is { } requested && entry.AttemptId != requested) continue;
                ValidateTrace(entry.Trace); ValidateRevision(entry.Revision);
                if (entry.Metrics is { IsValid: false }) throw new InvalidDataException("Invalid processing metrics.");
                if (entry.Failure is { } reason && !Enum.IsDefined(reason)) throw new InvalidDataException("Invalid diagnostic failure category.");
                if (!Enum.IsDefined(entry.Event)) throw new InvalidDataException("Invalid diagnostic event.");
                if (entry.Stage is { } stage && !Enum.IsDefined(stage) || entry.Code is { } code && !Enum.IsDefined(code))
                    throw new InvalidDataException("Invalid diagnostic context.");
                if (entry.Version is { } version && !System.Text.RegularExpressions.Regex.IsMatch(version, @"^\d{1,4}\.\d{1,4}\.\d{1,4}(?:-[A-Za-z0-9.-]{1,32})?$"))
                    throw new InvalidDataException("Invalid diagnostic version.");
                if (entry.AttemptId is { } attempt) included.Add((attempt, entry.Event));
                output.AppendLine(JsonSerializer.Serialize(entry)); // Re-serialize only allowlisted fields.
            }
        }
        if (currentAttempt is { AttemptId: { } currentId } && !included.Contains((currentId, currentAttempt.Event)))
        {
            ValidateTrace(currentAttempt.Trace); ValidateRevision(currentAttempt.Revision);
            if (currentAttempt.Version != UiText.ProductVersion || currentAttempt.Metrics is { IsValid: false }
                || !Enum.IsDefined(currentAttempt.Event) || currentAttempt.Stage is { } currentStage && !Enum.IsDefined(currentStage)
                || currentAttempt.Code is { } currentCode && !Enum.IsDefined(currentCode)
                || currentAttempt.Failure is { } currentFailure && !Enum.IsDefined(currentFailure))
                throw new InvalidDataException("Invalid current diagnostic context.");
            output.AppendLine(JsonSerializer.Serialize(currentAttempt));
        }
        output.AppendLine(JsonSerializer.Serialize(new DiagnosticEntry(DateTimeOffset.UtcNow, DiagnosticEvent.Exported, null, Version: UiText.ProductVersion, AttemptId: currentAttempt?.AttemptId, Revision: BuildRevision)));
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


