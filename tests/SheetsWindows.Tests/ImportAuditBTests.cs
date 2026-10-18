using System.Diagnostics;
using System.Text;
using System.Text.Json;
using SheetsWindows.Core;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class ImportAuditBTests
{
    [Theory]
    [InlineData("csv", '\t', 21, 5)]
    [InlineData("csv", '\t', 51, 8)]
    [InlineData("csv", '|', 51, 8)]
    [InlineData("tsv", '|', 6, 5)]
    public void DetectionUsesContentAndPreservesEachColumn(string format, char delimiter, int rows, int columns)
    {
        var input = string.Join("\r\n", Enumerable.Range(0, rows).Select(r => string.Join(delimiter, Enumerable.Range(0, columns).Select(c => $"{r}:{c}"))));
        var payload = SpreadsheetFormats.Prepare(format, Encoding.UTF8.GetBytes(input));
        Assert.Equal(rows, payload.Expected![0].Rows.Count);
        for (var r = 0; r < rows; r++)
        {
            Assert.Equal(columns, payload.Expected[0].Rows[r].Count);
            for (var c = 0; c < columns; c++) Assert.Equal($"{r}:{c}", payload.Expected[0].Rows[r][c]);
        }
        SpreadsheetFormats.VerifyValues(payload.Expected, payload.Bytes, textImport: true);
    }
    [Fact]
    public void PipeQuotedNewlinesEscapedQuotesAndLiteralTabHaveIndependentExpectedValues()
    {
        var payload = SpreadsheetFormats.Prepare("csv", Encoding.UTF8.GetBytes("id|nome|descricao|texto\r\n1|Ana|\"Contém vírgula, ponto e vírgula; e barra |\"|\"Primeira linha\nSegunda linha\"\r\n2|João|\"Valor com \"\"aspas\"\" dentro\"|Célula com\ttabulação\r\n3|Paula|açúcar|Normal\r\n"));
        var rows = payload.Expected![0].Rows;
        Assert.Equal(4, rows.Count); Assert.All(rows, row => Assert.Equal(4, row.Count));
        Assert.Equal("Primeira linha\nSegunda linha", rows[1][3]); Assert.Equal("Valor com \"aspas\" dentro", rows[2][2]);
        Assert.Equal("Célula com\ttabulação", rows[2][3]);
    }
    [Fact]
    public void LegacyPortugueseTextAndDecimalCommaArePreservedAsText()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var bytes = Encoding.GetEncoding(1252).GetBytes("produto;preco;nota\r\nCafé moído;19,90;\"São Paulo; ação\"\r\n");
        var rows = SpreadsheetFormats.Prepare("csv", bytes).Expected![0].Rows;
        Assert.Equal(2, rows.Count); Assert.Equal("Café moído", rows[1][0]); Assert.Equal("19,90", rows[1][1]); Assert.Equal("São Paulo; ação", rows[1][2]);
        foreach (var invalid in new byte[][] { [0x81], [0xE9, 0], [0xEF, 0xBB, 0xBF, 0xE9] })
            Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", invalid));
    }
    [Fact]
    public void FormulaLookingTextNeverBecomesAFormula()
    {
        var payload = SpreadsheetFormats.Prepare("csv", Encoding.UTF8.GetBytes("=1+1|+SUM(A1)|-123|@name\n001|19,90|true|2026-10-17"));
        Assert.Equal("=1+1", payload.Expected![0].Rows[0][0]);
        SpreadsheetFormats.VerifyValues(payload.Expected, payload.Bytes);
    }
    [Fact]
    public void AmbiguityRequiresChoiceAndCapacityCannotBeBypassed()
    {
        var bytes = Encoding.UTF8.GetBytes("a,b|c\n1,2|3");
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", bytes));
        Assert.Equal("a,b", SpreadsheetFormats.Prepare("csv", bytes, new(Delimiter: "pipe")).Expected![0].Rows[0][0]);
        Assert.Throws<SpreadsheetCapacityException>(() => SpreadsheetFormats.Prepare("csv", Encoding.UTF8.GetBytes(new string('\n', 100001))));
    }
    [Theory]
    [InlineData(53941, 10)]
    [InlineData(90001, 5)]
    [InlineData(60001, 9)]
    [InlineData(60002, 9)]
    [InlineData(60001, 1)]
    [InlineData(60002, 1)]
    [InlineData(60001, 10)]
    [InlineData(30001, 20)]
    [InlineData(60000, 10)]
    public void ExpandedTextCapacityRoundTripsWithinBoundedTimeAndAllocation(int rows, int columns)
    {
        var input = string.Join('\n', Enumerable.Range(0, rows).Select(r => string.Join(',', Enumerable.Range(0, columns).Select(c => $"{r:D5}-{c}"))));
        var started = Stopwatch.StartNew(); var allocated = GC.GetAllocatedBytesForCurrentThread();
        var payload = SpreadsheetFormats.Prepare("csv", Encoding.UTF8.GetBytes(input));
        SpreadsheetFormats.VerifyValues(payload.Expected!, payload.Bytes, textImport: true);
        Assert.Equal(rows, payload.Expected![0].Rows.Count); Assert.Equal($"{rows - 1:D5}-{columns - 1}", payload.Expected[0].Rows[^1][^1]);
        Assert.True(started.Elapsed < TimeSpan.FromMinutes(2));
        Assert.True(GC.GetAllocatedBytesForCurrentThread() - allocated < 2L * 1024 * 1024 * 1024);
        // The elevated allowance cannot accidentally enable other workbook formats.
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.ReadExcel(payload.Bytes));
    }
    [Fact]
    public void OdsBlankTailsAreSparseButInternalCoordinatesRemainExact()
    {
        var rows = "<table:table-row><table:table-cell table:number-columns-repeated='2'/><table:table-cell office:value-type='string'><text:p>valor</text:p></table:table-cell><table:table-cell table:number-columns-repeated='16381'/></table:table-row>"
            + "<table:table-row table:number-rows-repeated='2'><table:table-cell table:number-columns-repeated='16384'/></table:table-row>"
            + "<table:table-row><table:table-cell office:value-type='float' office:value='12.5'/></table:table-row>"
            + "<table:table-row table:number-rows-repeated='1048572'><table:table-cell table:number-columns-repeated='16384'/></table:table-row>";
        var bytes = FormatTests.Ods(rows); var payload = SpreadsheetFormats.Prepare("ods", bytes);
        Assert.Equal(bytes, payload.Bytes); Assert.Equal(4, payload.Expected![0].Rows.Count);
        Assert.Null(payload.Expected[0].Rows[0][0]); Assert.Equal("valor", payload.Expected[0].Rows[0][2]); Assert.Empty(payload.Expected[0].Rows[1]); Assert.Equal(12.5, payload.Expected[0].Rows[3][0]);
        SpreadsheetFormats.VerifyValues(payload.Expected, SpreadsheetFormats.WriteXlsx(payload.Expected));
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("ods", FormatTests.Ods("<table:table-row table:number-rows-repeated='1048577'/>")));
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("ods", FormatTests.Ods("<table:table-row><table:table-cell table:number-columns-repeated='16384' office:value-type='float' office:value='1'/></table:table-row>")));
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("ods", FormatTests.Ods("<table:table-row table:number-rows-repeated='50001'/><table:table-row><table:table-cell office:value-type='float' office:value='1'/></table:table-row>")));
    }
    [Fact]
    public async Task InterpretationIsImmutableAndRepairNeverReusesTheOldIdentity()
    {
        using var w = new Workspace(); var path = Path.ChangeExtension(w.Source, ".csv"); File.WriteAllText(path, "a|b\n1|2");
        var original = await w.Coordinator().PrepareAsync("A", path);
        var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!); var interpretations = new ImportInterpretations(storage);
        Assert.Equal(1, interpretations.Bind(original, new(), hasRemoteIntent: true).InterpretationVersion);
        Assert.Equal(1, interpretations.Bind(original, new(Delimiter: "pipe"), hasRemoteIntent: false).InterpretationVersion);
        var reader = new BackupRepairSource(original, Guid.NewGuid());
        var repair = await new LocalPreparation(w.Registry(), reader, new FileOperationLock(storage.LocksPath), new BackupStore(storage.BackupsPath)).PrepareAsync("A", path);
        Assert.NotEqual(original.Id, repair.Id); Assert.NotEqual(original.SourceKey, repair.SourceKey);
        Assert.Equal(original, w.Registry().Get(original.Id)); Assert.Equal(File.ReadAllBytes(path), File.ReadAllBytes(repair.Snapshot!.BackupPath));
        Assert.Equal(3, interpretations.Bind(repair, new(Delimiter: "pipe"), hasRemoteIntent: false).InterpretationVersion);
        File.WriteAllText(original.Snapshot!.BackupPath, "tampered");
        Assert.Throws<InvalidDataException>(() => reader.Open(path));
    }
    [Theory]
    [InlineData("capacity.tableRows", DiagnosticCode.TextRows)]
    [InlineData("capacity.tableCells", DiagnosticCode.TextCells)]
    [InlineData("capacity.tableColumns", DiagnosticCode.TextColumns)]
    [InlineData("capacity.cellCharacters", DiagnosticCode.CellCharacters)]
    [InlineData("capacity.importSize", DiagnosticCode.ImportBytes)]
    [InlineData("capacity.normalizedSize", DiagnosticCode.NormalizedBytes)]
    public void CapacityHasItsOwnCategoryAndSpecificReason(string key, DiagnosticCode expected)
    {
        var message = key.StartsWith("capacity.table", StringComparison.Ordinal) ? UiText.Message(key, ("limit", 60000)) : UiText.Message(key);
        var entry = DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, failure: new SpreadsheetCapacityException(message));
        Assert.Equal(DiagnosticFailure.Capacity, entry.Failure); Assert.Equal(expected, entry.Code);
    }
    [Fact]
    public async Task CurrentAttemptExportDoesNotMixOtherFilesOrLegacyHistory()
    {
        using var w = new Workspace(); var log = new DiagnosticLog(new(Path.Combine(w.Root, "state"))); var id = Guid.NewGuid();
        await log.RecordAsync(DiagnosticEvent.Failed, attemptId: Guid.NewGuid(), failure: new InvalidDataException("private"));
        await log.RecordAsync(DiagnosticEvent.Started);
        await log.RecordAsync(DiagnosticEvent.Started, attemptId: id);
        var current = DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, attemptId: id, failure: new InvalidDataException("Irregular delimited table."));
        var destination = Path.Combine(w.Root, "attempt.jsonl"); await log.ExportAsync(destination, currentAttempt: current);
        var entries = File.ReadLines(destination).Select(line => JsonSerializer.Deserialize<DiagnosticEntry>(line)!).ToArray();
        Assert.Equal(3, entries.Length); Assert.All(entries, entry => Assert.Equal(id, entry.AttemptId));
        Assert.Equal(DiagnosticCode.TextRowWidth, entries[1].Code); Assert.DoesNotContain("private", File.ReadAllText(destination));
    }
}
