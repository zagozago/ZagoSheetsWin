using Xunit;
using System.Globalization;
using SheetsWindows.Infrastructure;

namespace SheetsWindows.Tests;

public sealed class LocalizationTests
{
    [Fact]
    public void FallbackUsesSelectedPackThenEnglishThenSourceWithoutChangingSource()
    {
        var source = new Dictionary<string,string> { ["a"]="Fonte A", ["b"]="Fonte B", ["c"]="Fonte C" };
        var fallback = new Dictionary<string,string> { ["a"]="English A", ["b"]="English B" };
        var selected = new Dictionary<string,string> { ["a"]="Selecionado A" };
        var catalog = new LocalizationCatalog(source, fallback, selected);
        source["a"]="External mutation"; selected["a"]="External mutation";
        Assert.Equal("Selecionado A",catalog.Get("a")); Assert.Equal("English B",catalog.Get("b"));
        Assert.Equal("Fonte C",catalog.Get("c")); Assert.Equal("Fonte A",catalog.Source("a"));
        Assert.Throws<KeyNotFoundException>(()=>catalog.Get("missing"));
    }

    [Fact]
    public void NamedSlotsCanReorderAndValuesWithBracesRemainLiteral()
    {
        var catalog = new LocalizationCatalog(new Dictionary<string,string> { ["result"]="{count:N0}: {name}" },
            translation:new Dictionary<string,string> { ["result"]="{name} ({count:N0})" });
        Assert.Equal("{count} (1.234)",catalog.Format("result",CultureInfo.GetCultureInfo("pt-BR"),("count",1234),("name","{count}")));
        Assert.Equal("{count} (1,234)",catalog.Format("result",CultureInfo.GetCultureInfo("en-US"),("count",1234),("name","{count}")));
    }

    [Fact]
    public void InvalidPlaceholdersAndFormatsAreRejectedBeforeDisplayingTranslation()
    {
        var source = new Dictionary<string,string> { ["value"]="{count:N2}" };
        foreach (var invalid in new[] {"{other:N2}","{count}","{count:N2} {count:N2}",""})
            Assert.Throws<ArgumentException>(()=>new LocalizationCatalog(source,translation:new Dictionary<string,string>{["value"]=invalid}));
        Assert.Throws<ArgumentException>(()=>new LocalizationCatalog(source,translation:new Dictionary<string,string>{["unknown"]="Unknown"}));
    }

    [Fact]
    public void MissingExtraOrDuplicateArgumentsAreRejected()
    {
        var catalog = new LocalizationCatalog(new Dictionary<string,string>{["value"]="{count}"});
        Assert.Throws<ArgumentException>(()=>catalog.Format("value",CultureInfo.InvariantCulture));
        Assert.Throws<ArgumentException>(()=>catalog.Format("value",CultureInfo.InvariantCulture,("count",1),("other",2)));
        Assert.Throws<ArgumentException>(()=>catalog.Format("value",CultureInfo.InvariantCulture,("count",1),("count",2)));
    }

    [Fact]
    public void SourceResourceIncludesAccessibleHelpAndInstallerIndependentMessages()
    {
        Assert.Equal("&Ajuda",UiText.Get("home.help"));
        Assert.Contains("opcional",UiText.Get("tutorial.defaultsExplanation"));
        Assert.Contains("sol",UiText.Get("accessibility.themeDescription"));
        Assert.Contains("drive.file",UiText.Get("setup.privacyExplanation"));
        Assert.Contains("MIT",UiText.Get("about.description"));
        Assert.Contains("ZagoSheetsWin",UiText.EmphasisTerms);
        Assert.DoesNotContain("{version}",UiText.Format("about.description",("version",UiText.ProductVersion)));
    }

    [Fact]
    public void CapacityExceptionKeepsSourceTextAndProducesCompleteUserMessage()
    {
        var exception = new SpreadsheetCapacityException(UiText.Message("capacity.tableRows",("limit",50000)));
        Assert.Equal(exception.Message,exception.UserMessage);
        Assert.Contains("linhas",exception.Message);
        Assert.Contains("O original foi preservado",LauncherErrors.Message(exception));
        Assert.Equal("Technical reason",new SpreadsheetCapacityException("Technical reason").Message);
    }
}
