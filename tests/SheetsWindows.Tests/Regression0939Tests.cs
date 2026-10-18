using System.IO.Compression;
using System.Text;
using System.Xml.Linq;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class Regression0939Tests
{
    [Fact]
    public void ConversionDifferenceIsNotMisreportedAsFileAccess()
    {
        var entry = DiagnosticLog.CreateEntry(DiagnosticEvent.ConversionMismatch, failure: new ConversionMismatchException());
        Assert.Equal(DiagnosticCode.ConversionValues, entry.Code); Assert.Equal(DiagnosticFailure.ConversionDifference, entry.Failure);
    }

    [Fact]
    public void QuotedSingleColumnTreatsAllPunctuationAsContent()
    {
        var bytes = Encoding.UTF8.GetBytes("descricao\r\n\"vírgula, ponto; pipe| aqui\"\r\n\"outro, ; | todos aqui\"\r\n");
        var payload = SpreadsheetFormats.Prepare("csv", bytes);
        Assert.Equal(3, payload.Expected![0].Rows.Count); Assert.All(payload.Expected[0].Rows, row => Assert.Single(row));
        Assert.Equal("vírgula, ponto; pipe| aqui", payload.Expected[0].Rows[1][0]);
        Assert.Equal("outro, ; | todos aqui", payload.Expected[0].Rows[2][0]);
        SpreadsheetFormats.VerifyValues(payload.Expected, payload.Bytes, textImport: true);
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", bytes, new(InterpretationVersion: 3)));
    }
    [Fact]
    public void ActualExcelHintFixtureIsNotSilentlyReparsedInsideAQuotedValue()
    {
        var bytes = Encoding.UTF8.GetBytes("sep=;\r\nid;nome;valor\r\n\"1;Ana;1,20\"\r\n");
        var error = Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("csv", bytes));
        Assert.Equal(DiagnosticCode.TextSeparatorHint, DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, failure: error).Code);
        var corrected = SpreadsheetFormats.Prepare("csv", Encoding.UTF8.GetBytes("sep=;\r\nid;nome;valor\r\n1;Ana;1,20\r\n"));
        Assert.Equal(2, corrected.Expected![0].Rows.Count); Assert.Equal("1,20", corrected.Expected[0].Rows[1][2]);
    }
    [Fact]
    public void CrLfInsideQuotedFieldIsPreservedInNewPayloadAndComparedAsLogicalNewline()
    {
        var bytes = Encoding.UTF8.GetBytes("id;texto;nota\r\n1;\"primeira\r\nsegunda\";\"x;y\"\r\n2;\"palavra \"\"citada\"\"\";fim\r\n");
        var current = SpreadsheetFormats.Prepare("csv", bytes);
        Assert.Equal("primeira\r\nsegunda", SpreadsheetFormats.ReadExcel(current.Bytes)[0].Rows[1][1]);
        var legacy = SpreadsheetFormats.Prepare("csv", bytes, new(InterpretationVersion: 3));
        Assert.Equal("primeira\nsegunda", SpreadsheetFormats.ReadExcel(legacy.Bytes)[0].Rows[1][1]);
        SpreadsheetFormats.VerifyValues(current.Expected!, current.Bytes, textImport: true);
        SpreadsheetFormats.VerifyValues(current.Expected!, legacy.Bytes, textImport: true);
        Assert.Throws<ConversionMismatchException>(() => SpreadsheetFormats.VerifyValues(current.Expected!, legacy.Bytes));
        var changed = current.Expected!.Select(s => new SheetValues(s.Name, s.Rows.Select((row, index) => index == 1 ? (IReadOnlyList<object?>)new object?[] { "1", "primeira\nDIFERENTE", "x;y" } : row).ToArray())).ToArray();
        Assert.Throws<ConversionMismatchException>(() => SpreadsheetFormats.VerifyValues(changed, current.Bytes, textImport: true));
        Assert.Equal("palavra \"citada\"", current.Expected![0].Rows[2][1]);
    }
    [Fact]
    public void LoneCarriageReturnIsNotTreatedAsEquivalentToLineFeed()
    {
        var expected = new[] { new SheetValues("Dados", new[] { (IReadOnlyList<object?>)new object?[] { "a\rb" } }) };
        var different = SpreadsheetFormats.WriteXlsx([new("Dados", [new object?[] { "a\nb" }])]);
        Assert.Throws<ConversionMismatchException>(() => SpreadsheetFormats.VerifyValues(expected, different, textImport: true));
    }
}
