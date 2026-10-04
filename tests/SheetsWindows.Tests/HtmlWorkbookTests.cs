using System.Text;
using SheetsWindows.Infrastructure;
using Xunit;
namespace SheetsWindows.Tests;
public sealed class HtmlWorkbookTests
{
    private static SpreadsheetPayload Prepare(string html) => SpreadsheetFormats.Prepare("xls", Encoding.UTF8.GetBytes(html));
    [Fact]
    public void HtmlDisguisedAsXlsPreservesEntitiesNumbersLiteralIdentifiersAndFilters()
    {
        var payload = Prepare("<!DOCTYPE html><html><head><meta charset='UTF-8'></head><body><h1>Filtros</h1><ul><li>País: Brasil</li></ul><table><tr><th>Nome</th><th>Valor</th><th>ID</th></tr><tr><td><b>Ação &amp; café</b><br>segunda linha</td><td>2442</td><td>00123</td></tr><tr><td>=SUM(A1:A2)</td><td>12.5</td><td>&lt;ok&gt;</td></tr></table><script>throw 'never execute';</script></body></html>");
        Assert.Equal(SpreadsheetFormats.XlsxMime, payload.MimeType);
        var data = payload.Expected![0].Rows;
        Assert.Equal("Ação & café\nsegunda linha", data[1][0]); Assert.Equal(2442d, data[1][1]); Assert.Equal("00123", data[1][2]);
        Assert.Equal("=SUM(A1:A2)", data[2][0]); Assert.Equal(12.5d, data[2][1]);
        Assert.Equal("Informações", payload.Expected[1].Name); Assert.Equal("País: Brasil", payload.Expected[1].Rows[1][0]);
        SpreadsheetFormats.VerifyValues(payload.Expected, payload.Bytes);
    }
    [Fact]
    public void HtmlGridHandlesMergedCellsMultipleTablesAndUnevenRows()
    {
        var payload = Prepare("<html><table><tr><td rowspan='2'>A<td colspan='2'>B</tr><tr><td>C<td>D</tr></table><table><tr><td>E<td>F</tr><tr><td>G</tr></table></html>");
        Assert.Equal(2, payload.Expected!.Count); Assert.Equal(new object?[] { "", "C", "D" }, payload.Expected[0].Rows[1]);
        Assert.Equal(new object?[] { "G", "" }, payload.Expected[1].Rows[1]); SpreadsheetFormats.VerifyValues(payload.Expected, payload.Bytes);
    }
    [Theory]
    [InlineData("<html><body>No table</body></html>")]
    [InlineData("<html><table><tr><td><table><tr><td>nested</td></tr></table></td></tr></table></html>")]
    public void UnsupportedHtmlStructureFailsBeforeUpload(string html) => Assert.Throws<InvalidDataException>(() => Prepare(html));
    [Theory]
    [InlineData("<table><tr><td colspan='1001'>oversized</td></tr></table>")]
    [InlineData("<table><tr><td rowspan='50001'>oversized</td></tr></table>")]
    public void HtmlSpansCannotBypassResourceLimits(string html) => Assert.Throws<SpreadsheetCapacityException>(() => Prepare(html));
    [Fact]
    public void DeclaredLegacyEncodingAndUtf16BomPreserveAccents()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var html = "<html><meta charset=windows-1252><table><tr><td>Ação</td></tr></table></html>";
        var legacy = SpreadsheetFormats.Prepare("xls", Encoding.GetEncoding(1252).GetBytes(html)); Assert.Equal("Ação", legacy.Expected![0].Rows[0][0]);
        var utf16 = Encoding.Unicode.GetPreamble().Concat(Encoding.Unicode.GetBytes("<table><tr><td>Ação</td></tr></table>")).ToArray();
        var payload = SpreadsheetFormats.Prepare("xls", utf16); Assert.Equal("Ação", payload.Expected![0].Rows[0][0]);
        SpreadsheetFormats.VerifyValues(payload.Expected, payload.Bytes);
    }
}
