using System.Text.Json;
namespace SheetsWindows.Infrastructure;

public sealed record OpeningPreferences(bool RestrictToFolder, string? Folder = null);
public static class OpeningPolicy
{
    public static string PathFor(LocalStorage storage) => Path.Combine(storage.Root, "opening-policy.json");
    public static bool IsConfigured(LocalStorage storage) => File.Exists(PathFor(storage)) || File.Exists(PilotSetup.PolicyPath(storage));
    public static OpeningPreferences Load(LocalStorage storage)
    {
        var path = PathFor(storage);
        if (!File.Exists(path))
        {
            var legacy = PilotSetup.PolicyPath(storage);
            if (!File.Exists(legacy)) return new(false);
            if ((File.GetAttributes(legacy) & FileAttributes.ReparsePoint) != 0 || new FileInfo(legacy).Length > 32768) throw new InvalidDataException("Invalid folder policy.");
            var folder = File.ReadAllText(legacy);
            if (!Path.IsPathFullyQualified(folder)) throw new InvalidDataException("Invalid folder policy.");
            return new(true, folder);
        }
        if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0 || new FileInfo(path).Length > 32768) throw new InvalidDataException("Invalid opening policy.");
        try
        {
            var value = JsonSerializer.Deserialize<OpeningPreferences>(File.ReadAllText(path)) ?? throw new InvalidDataException("Invalid opening policy.");
            if (value.RestrictToFolder && (string.IsNullOrWhiteSpace(value.Folder) || !Path.IsPathFullyQualified(value.Folder))) throw new InvalidDataException("Invalid folder policy.");
            return value;
        }
        catch (JsonException ex) { throw new InvalidDataException("Invalid opening policy.", ex); }
    }
    public static async Task SaveAsync(LocalStorage storage, bool restrictToFolder, string? folder, bool acknowledged, CancellationToken ct = default)
    {
        if (!acknowledged) throw new ArgumentException("Conversion and backup acknowledgement required.");
        var value = new OpeningPreferences(restrictToFolder, restrictToFolder ? PilotSetup.ValidateFolder(folder ?? "") : null);
        PrivateDirectory.Create(storage.Root);
        await using var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("opening-policy", ct);
        var path = PathFor(storage); if (IsConfigured(storage)) _ = Load(storage);
        var temp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var file = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None)) { JsonSerializer.Serialize(file, value); file.Flush(true); }
            File.Move(temp, path, true);
        }
        finally { if (File.Exists(temp)) File.Delete(temp); }
    }
}
