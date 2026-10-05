using SheetsWindows.Infrastructure;
using Xunit;
namespace SheetsWindows.Tests;
public sealed class LanguageSettingsTests
{
    [Fact]
    public void ExplicitLanguageOverridesWindowsAndAutomaticUsesPreferredList()
    {
        Assert.Equal("pt",LanguageSettings.Resolve("pt",["en-US"]));
        Assert.Equal("en",LanguageSettings.Resolve("auto",["de-DE","en-GB","pt-BR"]));
        Assert.Equal("pt",LanguageSettings.Resolve("auto",["es-ES","pt-PT"]));
        Assert.Equal("en",LanguageSettings.Resolve("auto",["zh-TW","yue-Hant-HK"]));
        Assert.Throws<ArgumentException>(()=>LanguageSettings.Resolve("zh",["en"]));
        Assert.Equal(new[]{"en","pt"},LanguageSettings.Available.Select(l=>l.Code));
    }
    [Fact]
    public async Task InstallerInitializesOnceAndUpdatesPreserveUserChoiceAndOtherState()
    {
        using var w=new Workspace();var storage=new LocalStorage(Path.Combine(w.Root,"lang"));
        Assert.Equal("auto",LanguageSettings.Load(storage));Assert.False(Directory.Exists(storage.Root));
        await LanguageSettings.InitializeFromInstallerAsync(storage,"pt");Assert.Equal("pt",LanguageSettings.Load(storage));
        await ThemeSettings.SaveAsync(storage,ApplicationTheme.Dark);
        await LanguageSettings.SaveAsync(storage,"en");await LanguageSettings.InitializeFromInstallerAsync(storage,"pt");
        Assert.Equal("en",LanguageSettings.Load(storage));Assert.Equal(ApplicationTheme.Dark,ThemeSettings.Load(storage));
        await LanguageSettings.SaveAsync(storage,"auto");Assert.Equal("auto",LanguageSettings.Load(storage));
        await LanguageSettings.InitializeFromInstallerAsync(storage,"en");Assert.Equal("auto",LanguageSettings.Load(storage));
        Assert.Empty(Directory.GetFiles(storage.Root,"*.tmp"));
    }
    [Fact]
    public async Task InvalidLanguageIsNotSilentlyOverwritten()
    {
        using var w=new Workspace();var storage=new LocalStorage(Path.Combine(w.Root,"lang"));Directory.CreateDirectory(storage.Root);
        var path=Path.Combine(storage.Root,"language.json");File.WriteAllText(path,"\"xx\"");
        Assert.Throws<InvalidDataException>(()=>LanguageSettings.Load(storage));
        await Assert.ThrowsAsync<InvalidDataException>(()=>LanguageSettings.SaveAsync(storage,"pt"));
        await Assert.ThrowsAsync<InvalidDataException>(()=>LanguageSettings.InitializeFromInstallerAsync(storage,"en"));
        Assert.Equal("\"xx\"",File.ReadAllText(path));
        await Assert.ThrowsAsync<ArgumentException>(()=>LanguageSettings.SaveAsync(storage,"zh"));
    }
}
