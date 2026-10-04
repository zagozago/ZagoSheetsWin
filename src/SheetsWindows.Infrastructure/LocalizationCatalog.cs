using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SheetsWindows.Infrastructure;

// Locale selection and persistence are intentionally separate from message lookup.
// Source strings remain available for stable diagnostic exception details.
public sealed class LocalizationCatalog
{
    private static readonly Regex Slot = new(@"\{([A-Za-z][A-Za-z0-9_]*)(?::([^{}]+))?\}", RegexOptions.CultureInvariant);
    private readonly IReadOnlyDictionary<string, string> source;
    private readonly IReadOnlyDictionary<string, string> fallback;
    private readonly IReadOnlyDictionary<string, string> translation;

    public LocalizationCatalog(IReadOnlyDictionary<string, string> source,
        IReadOnlyDictionary<string, string>? fallback = null,
        IReadOnlyDictionary<string, string>? translation = null)
    {
        this.source = Copy(source);
        this.fallback = Copy(fallback ?? new Dictionary<string, string>());
        this.translation = Copy(translation ?? new Dictionary<string, string>());
        Validate(this.fallback); Validate(this.translation);
    }

    private static IReadOnlyDictionary<string, string> Copy(IReadOnlyDictionary<string, string> values) =>
        new ReadOnlyDictionary<string, string>(new Dictionary<string, string>(values, StringComparer.Ordinal));

    private void Validate(IReadOnlyDictionary<string, string> pack)
    {
        foreach (var (key, text) in pack)
        {
            if (!source.TryGetValue(key, out var original) || string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Unknown or empty localization entry.");
            var expected = Slots(original); var actual = Slots(text);
            if (!expected.Order(StringComparer.Ordinal).SequenceEqual(actual.Order(StringComparer.Ordinal)))
                throw new ArgumentException("Localization placeholders do not match the source.");
        }
    }

    private static string[] Slots(string text) => Slot.Matches(text).Select(m => m.Groups[1].Value + ":" + m.Groups[2].Value).ToArray();
    public string Source(string key) => source.TryGetValue(key, out var text) ? text : throw new KeyNotFoundException("Unknown localization key.");
    public string Get(string key)
    {
        // English fallback is used when present; while preparing the source-only
        // release, missing fallback entries safely retain Portuguese.
        _ = Source(key);
        return translation.TryGetValue(key, out var text) ? text : fallback.TryGetValue(key, out text) ? text : source[key];
    }

    public string Format(string key, CultureInfo culture, params (string Name, object? Value)[] arguments) => Interpolate(Get(key), culture, arguments);
    public string FormatSource(string key, CultureInfo culture, params (string Name, object? Value)[] arguments) => Interpolate(Source(key), culture, arguments);

    private static string Interpolate(string text, CultureInfo culture, (string Name, object? Value)[] arguments)
    {
        var values = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var (name, value) in arguments)
            if (!values.TryAdd(name, value)) throw new ArgumentException("Duplicate localization argument.");
        var expected = Slot.Matches(text).Select(m => m.Groups[1].Value).ToHashSet(StringComparer.Ordinal);
        if (!expected.SetEquals(values.Keys)) throw new ArgumentException("Localization arguments do not match the message.");
        // Replacement is a single pass: values containing braces are literal data.
        return Slot.Replace(text, match => values[match.Groups[1].Value] is IFormattable formatted
            ? formatted.ToString(match.Groups[2].Success ? match.Groups[2].Value : null, culture)
            : values[match.Groups[1].Value]?.ToString() ?? "");
    }
}
