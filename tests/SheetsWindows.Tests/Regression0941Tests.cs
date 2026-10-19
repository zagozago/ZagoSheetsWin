using System.IO.Compression;
using System.Text;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class Regression0941Tests
{
    [Fact]
    public void TwoMillionCellsRoundTripAcrossTheExpandedTextVerificationBudget()
    {
        var input = new StringBuilder();
        for (var row = 0; row < 100000; row++)
        {
            for (var col = 0; col < 20; col++) { if (col > 0) input.Append('|'); input.Append(row).Append('_').Append(col); }
            input.Append('\n');
        }
        var payload = SpreadsheetFormats.Prepare("csv", Encoding.UTF8.GetBytes(input.ToString()));
        var sheet = Assert.Single(payload.Expected!);
        Assert.Equal(2_000_000, sheet.Rows.Sum(r => r.Count)); Assert.Equal("99999_19", sheet.Rows[^1][^1]);
        using (var zip = new ZipArchive(new MemoryStream(payload.Bytes)))
        {
            var expanded = zip.GetEntry("xl/worksheets/sheet1.xml")!.Length;
            Assert.True(expanded > 64L * 1024 * 1024); Assert.True(expanded < 128L * 1024 * 1024);
        }
        SpreadsheetFormats.VerifyValues(payload.Expected!, payload.Bytes, textImport: true);
        Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.ReadExcel(payload.Bytes));
        var extra = new SheetValues("Extra", new IReadOnlyList<object?>[] { new object?[] { "one cell beyond the limit" } });
        Assert.Throws<SpreadsheetCapacityException>(() => SpreadsheetFormats.WriteXlsx([sheet, extra], textImport: true));
    }
    [Fact]
    public void CellBudgetIsSharedByCsvAndTsvAndDoesNotRaiseRowOrColumnLimits()
    {
        var row = string.Join('\t', Enumerable.Repeat("v", 201));
        var input = Encoding.UTF8.GetBytes(string.Join('\n', Enumerable.Repeat(row, 9951)));
        Assert.Equal(DiagnosticCode.TextCells, Assert.Throws<SpreadsheetCapacityException>(() => SpreadsheetFormats.Prepare("tsv", input)).Code);
        Assert.Equal(100000, SpreadsheetFormats.MaxTextRows); Assert.Equal(1000, SpreadsheetFormats.MaxColumns);
        Assert.Equal(500000, SpreadsheetFormats.MaxCells); Assert.Equal(50000, SpreadsheetFormats.MaxRows);
    }
}
