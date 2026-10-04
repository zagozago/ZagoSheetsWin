using System.Globalization;
using System.Text.Json;

namespace SheetsWindows.Infrastructure;

public static class UiText
{
    private static readonly JsonDocument Resource = Load();
    private static readonly LocalizationCatalog Catalog = new(Resource.RootElement.GetProperty("strings").EnumerateObject()
        .ToDictionary(p => p.Name, p => p.Value.GetString()!, StringComparer.Ordinal));
    private static JsonDocument Load()
    {
        using var stream = typeof(UiText).Assembly.GetManifestResourceStream("Localization.pt.json")
            ?? throw new InvalidOperationException("Source localization resource is missing.");
        return JsonDocument.Parse(stream);
    }
    public static string ProductVersion => Resource.RootElement.GetProperty("productVersion").GetString()!;
    public static IReadOnlyList<string> EmphasisTerms { get; } = Resource.RootElement.GetProperty("emphasisKeys").EnumerateArray()
        .Select(value => Get(value.GetString()!)).ToList().AsReadOnly();
    public static string Get(string key) => Catalog.Get(key);
    public static string Source(string key) => Catalog.Source(key);
    public static string Format(string key, params (string Name, object? Value)[] arguments) => Catalog.Format(key, CultureInfo.CurrentCulture, arguments);
    internal static string FormatSource(string key, params (string Name, object? Value)[] arguments) => Catalog.FormatSource(key, CultureInfo.CurrentCulture, arguments);
    public static LocalizedMessage Message(string key, params (string Name, object? Value)[] arguments) => new(key, arguments);
}

public sealed class LocalizedMessage
{
    private readonly string key;
    private readonly (string Name, object? Value)[] arguments;
    internal LocalizedMessage(string key, (string Name, object? Value)[] arguments) { this.key = key; this.arguments = arguments.ToArray(); }
    public string SourceText => UiText.FormatSource(key, arguments);
    public string Text => UiText.Format(key, arguments);
}
