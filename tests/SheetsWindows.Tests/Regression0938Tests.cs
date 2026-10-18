using System.Diagnostics;
using System.Text;
using System.Text.Json;
using SheetsWindows.Core;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class Regression0938Tests
{
    [Fact]
    public void HeaderlessDecimalCommaIsValueEvidenceAndLeadingZerosRemainLiteral()
    {
        var bytes = Encoding.UTF8.GetBytes("001;Ana;0,1\r\n002;Leo;0,2\r\n003;Bia;0,3\r\n");
        var payload = SpreadsheetFormats.Prepare("csv", bytes);
        Assert.Equal(3, payload.Expected![0].Rows.Count); Assert.Equal("001", payload.Expected[0].Rows[0][0]); Assert.Equal("0,1", payload.Expected[0].Rows[0][2]);
        SpreadsheetFormats.VerifyValues(payload.Expected, payload.Bytes, textImport: true);
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", bytes, new(InterpretationVersion: 2)));
    }
    [Fact]
    public void SingleColumnTsvPreservesPunctuationButBrokenCsvNeverBecomesOneColumn()
    {
        var rows = SpreadsheetFormats.ReadText(Encoding.UTF8.GetBytes("mensagem\ncontém vírgula, e ; ponto e pipe |\nfim"), "tsv", new());
        Assert.Equal(3, rows.Count); Assert.All(rows, r => Assert.Single(r)); Assert.Equal("contém vírgula, e ; ponto e pipe |", rows[1][0]);
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.ReadText(Encoding.UTF8.GetBytes("a;b\n1;2;3"), "csv", new()));
    }
    [Theory]
    [InlineData("a,b;c\n1,2;3\n4,5;6")]
    [InlineData("nome,local|tipo\nAna,PR|A\nLeo,SC|B")]
    public void RealTiesRemainBlockedUntilExplicitPerFileChoice(string text)
    {
        var bytes = Encoding.UTF8.GetBytes(text);
        Assert.Equal(DiagnosticCode.CsvDelimiter, DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, failure: Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", bytes))).Code);
        Assert.Equal(2, SpreadsheetFormats.Prepare("csv", bytes, new(Delimiter: "comma")).Expected![0].Rows[0].Count);
    }
    [Theory]
    [InlineData(';', "semicolon")]
    [InlineData(',', "comma")]
    [InlineData('|', "pipe")]
    [InlineData('\t', "tab")]
    public void ExactExcelHintIsOmittedAndConflictsNeverIgnored(char sep, string mode)
    {
        var bytes = Encoding.UTF8.GetBytes($"sep={sep}\r\nid{sep}texto\r\n001{sep}\"a\nb\"\r\n");
        var payload = SpreadsheetFormats.Prepare("csv", bytes);
        Assert.Equal(2, payload.Expected![0].Rows.Count); Assert.Equal("id", payload.Expected[0].Rows[0][0]); Assert.Equal("a\nb", payload.Expected[0].Rows[1][1]);
        SpreadsheetFormats.VerifyValues(payload.Expected, payload.Bytes, textImport: true);
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", bytes, new(Delimiter: mode == "comma" ? "pipe" : "comma")));
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", Encoding.UTF8.GetBytes($"sep={sep}\na{sep}b\n1")));
    }
    [Theory]
    [InlineData("", DiagnosticCode.TextEmpty)]
    [InlineData("id;texto\n1;\"ab", DiagnosticCode.TextUnclosedQuote)]
    [InlineData("id;texto\n1;ab\"cd", DiagnosticCode.TextMisplacedQuote)]
    [InlineData("id;texto\n1;2;3", DiagnosticCode.TextRowWidth)]
    public void InvalidInputsHaveSpecificNonSensitiveDiagnostics(string text, DiagnosticCode code)
    {
        var ex = Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", Encoding.UTF8.GetBytes(text)));
        var diagnostic = DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, failure: ex);
        Assert.Equal(code, diagnostic.Code); Assert.True(ImportRetry.RequiresInputChange(ex));
        Assert.DoesNotContain("id;texto", JsonSerializer.Serialize(diagnostic));
    }
    [Fact]
    public void ExactBoundariesRoundTripAndOverflowsFailBeforePayload()
    {
        var input = Encoding.UTF8.GetBytes(string.Join('\n', Enumerable.Range(0, 100000).Select(r => string.Join('|', Enumerable.Range(0, 10).Select(c => $"{r}_{c}")))));
        var allocated = GC.GetAllocatedBytesForCurrentThread(); var sw = Stopwatch.StartNew();
        var payload = SpreadsheetFormats.Prepare("csv", input);
        SpreadsheetFormats.VerifyValues(payload.Expected!, payload.Bytes, textImport: true);
        Assert.Equal(100000, payload.Expected![0].Rows.Count); Assert.Equal("99999_9", payload.Expected[0].Rows[^1][^1]);
        Assert.True(sw.Elapsed < TimeSpan.FromMinutes(2)); Assert.True(GC.GetAllocatedBytesForCurrentThread() - allocated < 2L * 1024 * 1024 * 1024);
        Assert.Throws<SpreadsheetCapacityException>(() => SpreadsheetFormats.Prepare("csv", Encoding.UTF8.GetBytes(new string('\n', 100001))));
        var overCells = Encoding.UTF8.GetBytes(string.Join('\n', Enumerable.Repeat(string.Join('|', Enumerable.Repeat("v", 101)), 9901)));
        Assert.Equal(DiagnosticCode.TextCells, Assert.Throws<SpreadsheetCapacityException>(() => SpreadsheetFormats.Prepare("csv", overCells)).Code);
        var row = Enumerable.Repeat<object?>("v", 11).ToArray();
        var tooMany = new SheetValues("Dados", Enumerable.Repeat<IReadOnlyList<object?>>(row, 90910).ToArray());
        Assert.Throws<SpreadsheetCapacityException>(() => SpreadsheetFormats.WriteXlsx([tooMany], textImport: true));
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.ReadExcel(payload.Bytes));
    }
    [Fact]
    public void ExplicitLegacyEncodingCannotBypassUndefinedByteRejection()
    {
        byte[] bytes = [105, 100, 59, 116, 10, 49, 59, 0x81];
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", bytes, new(Encoding: "windows-1252")));
    }
    [Fact]
    public void OdsOutsideDeclaredGridIsAnExplicitRejectionWithUnchangedPayload()
    {
        foreach (var body in new[] { "<table:table-row><table:table-cell table:number-columns-repeated='1000000'/></table:table-row>", "<table:table-row table:number-rows-repeated='2000000'/>" })
        {
            var bytes = FormatTests.Ods(body); var before = bytes.ToArray();
            var ex = Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("ods", bytes));
            Assert.Equal(DiagnosticCode.OdsRepeat, DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, failure: ex).Code); Assert.Equal(before, bytes);
        }
    }
    [Fact]
    public void ReconciliationIsNotClassifiedAsFileAccessAndOpaqueOperationIsExportable()
    {
        var id = Guid.NewGuid(); var telemetry = new ProcessingTelemetry(); telemetry.BindOperation(id);
        var entry = DiagnosticLog.CreateEntry(DiagnosticEvent.ReconciliationRequired, operation: telemetry.OperationId, failure: new ReconciliationRequiredException());
        Assert.Equal(id, entry.Operation); Assert.Equal(DiagnosticCode.RemoteReconciliation, entry.Code);
        Assert.False(ImportRetry.IsTransient(new ReconciliationRequiredException()));
    }
}
