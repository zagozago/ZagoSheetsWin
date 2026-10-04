using SheetsWindows.Core;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class OpeningPolicyTests
{
    private const string Client = "{\"installed\":{\"client_id\":\"pilot.apps.googleusercontent.com\"}}";
    [Fact]
    public async Task FreshSetupDoesNotRequireFolderButStillRequiresBackupConsent()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "policy"));
        Assert.Equal(new OpeningPreferences(false), OpeningPolicy.Load(storage));
        Assert.False(OpeningPolicy.IsConfigured(storage)); Assert.False(Directory.Exists(storage.Root));
        await Assert.ThrowsAsync<ArgumentException>(() => OpeningPolicy.SaveAsync(storage, false, null, false));
        Assert.False(Directory.Exists(storage.Root));
        LauncherConfiguration.SaveClient(storage, Client); Assert.True(FirstUseState.NeedsSetup(storage));
        await OpeningPolicy.SaveAsync(storage, false, null, true);
        Assert.False(FirstUseState.NeedsSetup(storage)); Assert.Equal(new OpeningPreferences(false), OpeningPolicy.Load(storage));
        Assert.False(File.Exists(PilotSetup.PolicyPath(storage))); Assert.Empty(Directory.GetFiles(storage.Root, "*.tmp"));
    }
    [Fact]
    public async Task UpgradePreservesLegacyFolderUntilUserExplicitlyChangesPreference()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "policy"));
        PilotSetup.Configure(storage, Client, w.Root, true);
        Assert.Equal(new OpeningPreferences(true, w.Root), OpeningPolicy.Load(storage));
        await OpeningPolicy.SaveAsync(storage, false, null, true);
        Assert.Equal(new OpeningPreferences(false), OpeningPolicy.Load(storage)); Assert.Equal(w.Root, File.ReadAllText(PilotSetup.PolicyPath(storage)));
        await OpeningPolicy.SaveAsync(storage, true, w.Root, true);
        Assert.Equal(new OpeningPreferences(true, w.Root), OpeningPolicy.Load(storage));
    }
    [Fact]
    public async Task InvalidScopeCannotReplaceLastSavedPolicy()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "policy"));
        await OpeningPolicy.SaveAsync(storage, false, null, true); var original = File.ReadAllText(OpeningPolicy.PathFor(storage));
        await Assert.ThrowsAsync<ArgumentException>(() => OpeningPolicy.SaveAsync(storage, true, Path.GetPathRoot(w.Root), true));
        Assert.Equal(original, File.ReadAllText(OpeningPolicy.PathFor(storage)));
        File.WriteAllText(OpeningPolicy.PathFor(storage), "{\"RestrictToFolder\":true,\"Folder\":\"relative\"}");
        Assert.Throws<InvalidDataException>(() => OpeningPolicy.Load(storage));
        await Assert.ThrowsAsync<InvalidDataException>(() => OpeningPolicy.SaveAsync(storage, false, null, true));
        Assert.Contains("relative", File.ReadAllText(OpeningPolicy.PathFor(storage))); Assert.Empty(Directory.GetFiles(storage.Root, "*.tmp"));
    }
    [Fact]
    public void AuthorizationStateDoesNotDependOnFolderSetupOrTokenExpiry()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.Combine(w.Root, "policy"));
        LauncherConfiguration.SaveClient(storage, Client);
        Assert.True(FirstUseState.NeedsSetup(storage));
        const string id = "pilot.apps.googleusercontent.com";
        Assert.False(FirstUseState.NeedsAuthorization(storage, new Vault(new(id, id + ":user", "expired", "refresh", DateTimeOffset.UtcNow.AddDays(-1)))));
        Assert.True(FirstUseState.NeedsAuthorization(storage, new Vault(null)));
    }
    private sealed class Vault(GoogleTokens? tokens) : ITokenVault
    {
        public GoogleTokens? Load() => tokens;
        public void Save(GoogleTokens value) => throw new NotSupportedException();
    }
}
