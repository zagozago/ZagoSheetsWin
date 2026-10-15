using System.Text;
using System.Xml.Linq;
using SheetsWindows.Infrastructure;
using Xunit;
namespace SheetsWindows.Tests;
public sealed class SecurityReviewTests
{
    [Theory]
    [InlineData("C:\\Dados\\Mendel-m4-assembly-data-sheet-DK.ods", "ZSW-diagn-mendelm4assemblydatasheetdkods.jsonl")]
    [InlineData("/dados/Relatorio 2026.CSV", "ZSW-diagn-relatorio2026csv.jsonl")]
    [InlineData(null, "ZSW-diagn.jsonl")]
    public void SuggestedDiagnosisNameUsesOnlyBoundedBasename(string? path, string expected)
    { Assert.Equal(expected, DiagnosticLog.SuggestedFileName(path)); }
    [Fact]
    public void HtmlDisguisedAsOdsIsIdentifiedWithoutInterpretingScripts()
    {
        var bytes = Encoding.UTF8.GetBytes("<!doctype html><html><script>throw new Error('never execute')</script><p>not a spreadsheet</p></html>");
        var failure = Assert.Throws<InvalidDataException>(() => SpreadsheetFormats.Prepare("ods", bytes));
        Assert.Equal(DiagnosticCode.OdsPackage, DiagnosticLog.CreateEntry(DiagnosticEvent.Failed, failure: failure).Code);
        Assert.Equal(bytes, Encoding.UTF8.GetBytes("<!doctype html><html><script>throw new Error('never execute')</script><p>not a spreadsheet</p></html>"));
    }
    [Fact]
    public void OdsCancellationIsObservedBeforeParsing()
    {
        using var cancellation = new CancellationTokenSource(); cancellation.Cancel();
        Assert.ThrowsAny<OperationCanceledException>(() => SpreadsheetFormats.ReadOds([1,2,3], cancellation.Token));
    }
    [Fact]
    public void VersionsAgreeBetweenCatalogApplicationAndInstaller()
    {
        var root = new DirectoryInfo(AppContext.BaseDirectory);
        while (root is not null && !File.Exists(Path.Combine(root.FullName, "Directory.Build.props"))) root = root.Parent;
        Assert.NotNull(root);
        var project = XDocument.Load(Path.Combine(root.FullName, "src/SheetsWindows.Windows/SheetsWindows.Windows.csproj"));
        var version = project.Root!.Element("PropertyGroup")!.Element("Version")!.Value;
        Assert.Equal(version, UiText.ProductVersion);
        Assert.Contains("#define PilotVersion \"" + version + "\"", File.ReadAllText(Path.Combine(root.FullName, "installer/SheetsWindows.iss")));
    }
    [Theory]
    [InlineData("{\"installed\":{\"client_id\":\"test.apps.googleusercontent.com\",\"refresh_token\":\"personal\"}}")]
    [InlineData("{\"type\":\"service_account\",\"installed\":{\"client_id\":\"test.apps.googleusercontent.com\"}}")]
    public void PersonalTokensAndServiceAccountsCannotBeStoredAsDesktopConfiguration(string json)
    { Assert.Throws<InvalidDataException>(() => OAuthClient.FromJson(json)); }
}
