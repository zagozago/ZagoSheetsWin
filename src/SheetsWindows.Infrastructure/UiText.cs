using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;

namespace SheetsWindows.Infrastructure;

public static class UiText
{
    private static readonly JsonDocument Resource = Load("pt");
    private static readonly IReadOnlyDictionary<string, string> SourceStrings = Strings(Resource);
    private static readonly IReadOnlyDictionary<string, string> EnglishStrings = Strings(Load("en"));
    private static LocalizationCatalog catalog = new(SourceStrings, EnglishStrings, SourceStrings);
    private static readonly ConcurrentDictionary<string, LocalizedMessage> rendered = new(StringComparer.Ordinal);
    public static string Language { get; private set; } = "pt";
    public static event Action? Changed;
    private static JsonDocument Load(string code)
    {
        using var stream = typeof(UiText).Assembly.GetManifestResourceStream($"Localization.{code}.json")
            ?? throw new InvalidOperationException("Localization resource is missing.");
        return JsonDocument.Parse(stream);
    }
    private static IReadOnlyDictionary<string,string> Strings(JsonDocument resource) => resource.RootElement.GetProperty("strings").EnumerateObject()
        .ToDictionary(p => p.Name, p => p.Value.GetString()!, StringComparer.Ordinal);
    public static void Select(string code)
    {
        if (!LanguageSettings.Supported(code)) throw new ArgumentException("Unsupported language.");
        var next = new LocalizationCatalog(SourceStrings, EnglishStrings, code == "pt" ? SourceStrings : EnglishStrings);
        catalog = next; Language = code; Changed?.Invoke();
    }
    public static string ProductVersion => Resource.RootElement.GetProperty("productVersion").GetString()!;
    public static IReadOnlyList<string> EmphasisTerms => Resource.RootElement.GetProperty("emphasisKeys").EnumerateArray()
        .Select(value => Get(value.GetString()!)).ToList().AsReadOnly();
    public static string Get(string key) => Remember(new(key, [], raw: true));
    public static string GetFromDescriptor(LocalizedMessage message) => Remember(message);
    public static string Source(string key) => catalog.Source(key);
    public static string Format(string key, params (string Name, object? Value)[] arguments) => Remember(new(key, arguments));
    private static string Remember(LocalizedMessage message)
    {
        var text = message.Text;
        // Descriptors, not parsing/replacing words, refresh existing control bindings.
        if (rendered.Count > 4096) rendered.Clear();
        rendered[text] = message;
        return text;
    }
    public static LocalizedMessage? Descriptor(string? text) => text is not null && rendered.TryGetValue(text, out var value) ? value : null;
    internal static string Raw(string key) => catalog.Get(key);
    internal static string Render(string key, (string Name, object? Value)[] arguments) => catalog.Format(key, CultureInfo.CurrentCulture, arguments);
    internal static string FormatSource(string key, params (string Name, object? Value)[] arguments) => catalog.FormatSource(key, CultureInfo.CurrentCulture, arguments);
    public static LocalizedMessage Message(string key, params (string Name, object? Value)[] arguments) => new(key, arguments);
}

public sealed class LocalizedMessage
{
    private readonly string key;
    private readonly (string Name, object? Value)[] arguments;
    private readonly LocalizedMessage?[] nested;
    private readonly bool raw;
    internal LocalizedMessage(string key, (string Name, object? Value)[] arguments, bool raw = false)
    {
        this.raw = raw; this.key = key; this.arguments = arguments.ToArray();
        nested = arguments.Select(a => a.Value is string text ? UiText.Descriptor(text) : null).ToArray();
    }
    public string SourceText => raw ? UiText.Source(key) : UiText.FormatSource(key, arguments);
    public string Text => raw ? UiText.Raw(key) : UiText.Render(key, arguments.Select((a, i) => (a.Name, nested[i] is { } message ? (object?)message.Text : a.Value)).ToArray());
}
