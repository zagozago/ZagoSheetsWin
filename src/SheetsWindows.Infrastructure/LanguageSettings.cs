using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace SheetsWindows.Infrastructure;

public sealed record ApplicationLanguage(string Code, string DisplayCode, string NativeName, string EnglishName);
public static class LanguageSettings
{
    public const string Automatic = "auto";
    public static IReadOnlyList<ApplicationLanguage> Available { get; } = Array.AsReadOnly(new[] {
        new ApplicationLanguage("en", "EN", "English", "English"),
        new ApplicationLanguage("pt", "PT", "Português", "Portuguese") });
    public static bool Supported(string code) => Available.Any(l => l.Code == code);
    public static string Resolve(string preference, IEnumerable<string> preferredLanguages)
    {
        if (Supported(preference)) return preference;
        if (preference != Automatic) throw new ArgumentException("Unsupported language preference.");
        foreach (var locale in preferredLanguages)
        {
            var code = locale.Split('-')[0].ToLowerInvariant();
            if (Supported(code)) return code;
        }
        return "en";
    }
    private static string PathFor(LocalStorage storage) => Path.Combine(storage.Root, "language.json");
    public static string Load(LocalStorage storage)
    {
        var path = PathFor(storage);
        if (!File.Exists(path)) return Automatic;
        if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0) throw new InvalidDataException("Invalid language settings.");
        using var file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        if (file.Length > 1024) throw new InvalidDataException("Invalid language settings.");
        var value = JsonSerializer.Deserialize<string>(file);
        return value is not null && (value == Automatic || Supported(value)) ? value : throw new InvalidDataException("Invalid language settings.");
    }
    public static async Task SaveAsync(LocalStorage storage, string preference, CancellationToken ct = default)
    {
        if (preference != Automatic && !Supported(preference)) throw new ArgumentException("Unsupported language preference.");
        PrivateDirectory.Create(storage.Root);
        await using var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("language-settings", ct);
        var path = PathFor(storage); if (File.Exists(path)) _ = Load(storage);
        var temp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            using (var file = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None)) { JsonSerializer.Serialize(file, preference); file.Flush(true); }
            File.Move(temp, path, true);
        }
        finally { if (File.Exists(temp)) File.Delete(temp); }
    }
    // Installer initializes only an absent preference. Updates never overwrite an app choice.
    public static async Task InitializeFromInstallerAsync(LocalStorage storage, string code, CancellationToken ct = default)
    {
        if (!Supported(code)) throw new ArgumentException("Unsupported installer language.");
        PrivateDirectory.Create(storage.Root);
        await using var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("language-settings", ct);
        var path = PathFor(storage); if (File.Exists(path)) return;
        using var file = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
        JsonSerializer.Serialize(file, code); file.Flush(true);
    }
    public static IEnumerable<string> WindowsPreferredLanguages()
    {
        if (OperatingSystem.IsWindows())
        {
            uint size = 0;
            if (GetUserPreferredUILanguages(8, out _, null, ref size) && size is > 0 and < 65536)
            {
                var buffer = new char[size];
                if (GetUserPreferredUILanguages(8, out _, buffer, ref size))
                    return new string(buffer).Split('\0', StringSplitOptions.RemoveEmptyEntries);
            }
        }
        return [CultureInfo.CurrentUICulture.Name];
    }
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetUserPreferredUILanguages(uint flags, out uint count, [Out] char[]? buffer, ref uint size);
}
