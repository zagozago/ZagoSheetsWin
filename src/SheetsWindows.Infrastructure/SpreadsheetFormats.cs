using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using ExcelDataReader;

namespace SheetsWindows.Infrastructure;

public sealed class SpreadsheetCapacityException : NotSupportedException
{
    private readonly LocalizedMessage? descriptor;
    public SpreadsheetCapacityException(string message) : base(message) { }
    public SpreadsheetCapacityException(LocalizedMessage message) : base(message.SourceText) { descriptor = message; }
    public string UserMessage => descriptor?.Text ?? Message;
    public DiagnosticCode Code => descriptor?.Key switch
    {
        "capacity.tableRows" => DiagnosticCode.TextRows, "capacity.tableCells" => DiagnosticCode.TextCells,
        "capacity.tableColumns" => DiagnosticCode.TextColumns, "capacity.cellCharacters" => DiagnosticCode.CellCharacters,
        "capacity.importSize" => DiagnosticCode.ImportBytes, "capacity.normalizedSize" => DiagnosticCode.NormalizedBytes,
        _ => DiagnosticCode.Other
    };
}
public class ConversionMismatchException() : IOException("Converted cell values differ; source preserved.");
public sealed class FormulaVerificationException : ConversionMismatchException;
public sealed class CopyRequiredException() : NotSupportedException("This workbook requires copy-only import.");
public sealed record TextImportOptions(string Encoding = "auto", string Delimiter = "auto", int InterpretationVersion = 4);
public sealed record SheetValues(string Name, IReadOnlyList<IReadOnlyList<object?>> Rows);
public sealed record SpreadsheetPayload(byte[] Bytes, string MimeType, IReadOnlyList<SheetValues>? Expected, TextImportOptions? TextInterpretation = null);

public static class SpreadsheetFormats
{
    public const string XlsxMime = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public const int MaxCells = 500000, MaxRows = 50000, MaxColumns = 1000;
    public const int MaxTextCells = 2000000, MaxTextRows = 100000;
    public static readonly string[] Extensions = [".xlsx", ".ods", ".xls", ".csv", ".tsv"];
    public static string Format(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();
        if (!Extensions.Contains(extension)) throw new NotSupportedException("Unsupported spreadsheet extension.");
        return extension[1..];
    }
    public static SpreadsheetPayload Prepare(string format, byte[] bytes, TextImportOptions? options = null, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        if (bytes.Length > GoogleImport.MaxBytes) throw new SpreadsheetCapacityException(UiText.Message("capacity.importSize"));
        switch (format)
        {
            case "xlsx": GoogleImport.ValidateXlsx(bytes); return new(bytes, XlsxMime, null);
            case "csv":
            case "tsv":
                var effective = options ?? new(); string? delimiter = null;
                var table = new SheetValues("Dados", ReadTextCore(bytes, format, effective, ct, value => delimiter = value));
                return new(WriteXlsx([table], ct, textImport: effective.InterpretationVersion != 1, compactText: effective.InterpretationVersion >= 3, preserveLineEndings: effective.InterpretationVersion >= 4), XlsxMime, [table], effective with { Delimiter = delimiter ?? effective.Delimiter });
            case "ods":
                IReadOnlyList<SheetValues>? expected;
                try { expected = ReadOds(bytes, ct); }
                catch (XmlException) { throw new InvalidDataException("Malformed ODS XML."); }
                catch (InvalidDataException ex) when (ex.Message is "Package part missing." or "Oversized or ambiguous ZIP package." or "XML part too large.") { throw new InvalidDataException("Invalid ODS package structure."); }
                return new(bytes, "application/vnd.oasis.opendocument.spreadsheet", expected);
            case "xls":
                if (HtmlWorkbookReader.IsHtml(bytes))
                {
                    var html = HtmlWorkbookReader.Read(bytes, ct);
                    return new(WriteXlsx(html, ct), XlsxMime, html);
                }
                // Validate independently with ExcelDataReader before converting the upload payload.
                var legacy = ReadExcel(bytes, binary: true, ct: ct);
                var converted = LegacyWorkbookConverter.Convert(bytes, ct, out var copyOnly);
                return new(converted, XlsxMime, copyOnly ? null : legacy);
            default: throw new NotSupportedException("Unsupported spreadsheet format.");
        }
    }
    private static string Decode(byte[] bytes, string mode, int version)
    {
        Encoding encoding; var offset = 0;
        if (mode == "auto")
        {
            if (bytes.AsSpan().StartsWith(new byte[] { 0xEF, 0xBB, 0xBF })) { encoding = new UTF8Encoding(false, true); offset = 3; }
            else if (bytes.AsSpan().StartsWith(new byte[] { 0xFF, 0xFE })) { encoding = new UnicodeEncoding(false, false, true); offset = 2; }
            else if (bytes.AsSpan().StartsWith(new byte[] { 0xFE, 0xFF })) { encoding = new UnicodeEncoding(true, false, true); offset = 2; }
            else encoding = new UTF8Encoding(false, true);
        }
        else if (mode == "windows-1252")
        {
            if (bytes.Any(b => b is 0x81 or 0x8D or 0x8F or 0x90 or 0x9D)) throw new InvalidDataException("Invalid text encoding or characters.");
            if (bytes.AsSpan().StartsWith(new byte[] { 0xEF, 0xBB, 0xBF }) || bytes.AsSpan().StartsWith(new byte[] { 0xFF, 0xFE }) || bytes.AsSpan().StartsWith(new byte[] { 0xFE, 0xFF })) throw new InvalidDataException("Encoding conflicts with BOM.");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            encoding = Encoding.GetEncoding(1252, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback);
        }
        else throw new ArgumentException("Unsupported text encoding.");
        try { return encoding.GetString(bytes, offset, bytes.Length - offset); }
        catch (DecoderFallbackException) when (mode == "auto" && version >= 2 && offset == 0)
        {
            // No BOM: strict UTF-8 first, then the explicitly bounded legacy fallback.
            // Reject undefined CP1252 bytes and binary-like control streams.
            if (bytes.Any(b => b is 0 or 0x81 or 0x8D or 0x8F or 0x90 or 0x9D)) throw new InvalidDataException("Invalid text encoding or characters.");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var decoded = Encoding.GetEncoding(1252, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback).GetString(bytes);
            if (decoded.Any(c => char.IsControl(c) && c is not ('\t' or '\r' or '\n'))) throw new InvalidDataException("Invalid text encoding or characters.");
            return decoded;
        }
        catch (DecoderFallbackException) { throw new InvalidDataException("Invalid text encoding or characters."); }
    }
    public static IReadOnlyList<IReadOnlyList<object?>> ReadText(byte[] bytes, string format, TextImportOptions options, CancellationToken ct = default) => ReadTextCore(bytes, format, options, ct);
    private static IReadOnlyList<IReadOnlyList<object?>> ReadTextCore(byte[] bytes, string format, TextImportOptions options, CancellationToken ct, Action<string>? picked = null)
    {
        ct.ThrowIfCancellationRequested();
        if (bytes.Length > GoogleImport.MaxBytes) throw new SpreadsheetCapacityException(UiText.Message("capacity.importSize"));
        var text = Decode(bytes, options.Encoding, options.InterpretationVersion);
        if (options.InterpretationVersion is not (1 or 2 or 3 or 4)) throw new InvalidDataException("Unsupported interpretation version.");
        if (text.Length == 0 || text.All(c => c < ' ' && c is not ('\t' or '\n' or '\r'))) throw new InvalidDataException("Empty text spreadsheet.");
        if (options.InterpretationVersion >= 3) return ReadTextV3(text, format, options, ct, picked);
        if (format == "tsv" && options.InterpretationVersion == 1) return ParseDelimited(text, '\t', ct, 1);
        if (options.Delimiter is "comma" or "semicolon" or "tab" or "pipe")
            return ParseDelimited(text, options.Delimiter switch { "comma" => ',', "semicolon" => ';', "tab" => '\t', _ => '|' }, ct, options.InterpretationVersion);
        if (options.Delimiter != "auto") throw new ArgumentException("Unsupported delimiter.");
        var candidates = new List<IReadOnlyList<IReadOnlyList<object?>>>();
        SpreadsheetCapacityException? capacity = null;
        foreach (var separator in options.InterpretationVersion == 1 ? new[] { ',', ';' } : new[] { ',', ';', '\t', '|' })
        {
            try { var rows = ParseDelimited(text, separator, ct, options.InterpretationVersion); if (rows[0].Count > 1) candidates.Add(rows); }
            catch (SpreadsheetCapacityException ex) { capacity ??= ex; }
            catch (InvalidDataException) { }
        }
        // Never use another separator to bypass a resource boundary.
        if (capacity is not null) throw capacity;
        if (candidates.Count > 1) throw new InvalidDataException("Ambiguous CSV delimiter; configure it explicitly.");
        if (candidates.Count == 1) return candidates[0];
        var quoted = false;
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] == '"') { if (quoted && i + 1 < text.Length && text[i + 1] == '"') i++; else quoted = !quoted; }
            else if (!quoted && (text[i] is ',' or ';' || options.InterpretationVersion >= 2 && text[i] is '\t' or '|')) throw new InvalidDataException("Irregular delimited table.");
        }
        return ParseDelimited(text, ',', ct, options.InterpretationVersion);
    }
    private static IReadOnlyList<IReadOnlyList<object?>> ParseDelimited(string text, char separator, CancellationToken ct, int version)
    {
        var rows = new List<IReadOnlyList<object?>>(); var row = new List<object?>(); var field = new StringBuilder();
        var quoted = false; var closed = false; var cells = 0;
        var maxCells = version >= 3 ? MaxTextCells : version == 2 ? 600000 : MaxCells; var maxRows = version >= 3 ? MaxTextRows : version == 2 ? 60000 : MaxRows;
        void Field()
        {
            if (++cells > maxCells) throw new SpreadsheetCapacityException(UiText.Message("capacity.tableCells", ("limit", maxCells)));
            if (row.Count >= MaxColumns) throw new SpreadsheetCapacityException(UiText.Message("capacity.tableColumns", ("limit", MaxColumns)));
            if (field.Length > 32767) throw new SpreadsheetCapacityException(UiText.Message("capacity.cellCharacters"));
            row.Add(field.ToString()); field.Clear(); closed = false;
        }
        void Row()
        {
            Field(); if (rows.Count >= maxRows) throw new SpreadsheetCapacityException(UiText.Message("capacity.tableRows", ("limit", maxRows)));
            if (rows.Count > 0 && row.Count != rows[0].Count) throw new InvalidDataException("Irregular delimited table.");
            rows.Add(row); row = [];
        }
        for (var i = 0; i < text.Length; i++)
        {
            if ((i & 4095) == 0) ct.ThrowIfCancellationRequested();
            if (field.Length > 32767) throw new SpreadsheetCapacityException(UiText.Message("capacity.cellCharacters"));
            var c = text[i];
            if (quoted)
            {
                if (c == '"') { if (i + 1 < text.Length && text[i + 1] == '"') { field.Append('"'); i++; } else { quoted = false; closed = true; } }
                else field.Append(c);
            }
            else if (c == separator) Field();
            else if (c is '\r' or '\n') { Row(); if (c == '\r' && i + 1 < text.Length && text[i + 1] == '\n') i++; }
            else if (c == '"' && field.Length == 0 && !closed) quoted = true;
            else { if (closed || c == '"') throw new InvalidDataException("Invalid quoted field."); field.Append(c); }
        }
        if (quoted) throw new InvalidDataException("Unclosed quoted field.");
        if (field.Length > 0 || closed || row.Count > 0 || text[^1] == separator) Row();
        if (rows.Count == 0) throw new InvalidDataException("Empty table.");
        return rows;
    }
    private static char Separator(string name) => name switch { "comma" => ',', "semicolon" => ';', "tab" => '\t', "pipe" => '|', _ => throw new ArgumentException("Unsupported delimiter.") };
    private sealed record TextShape(int Rows, int Columns, long Cells, int Longest);
    // First pass validates the whole grammar/width using counters only. A failed alternative
    // cannot invent a capacity error or allocate a second million-cell table.
    private static TextShape ScanText(string text, char separator, CancellationToken ct, Action<string>? fieldValue = null)
    {
        var quoted = false; var closed = false; var length = 0; var columns = 0; var width = 0; var rows = 0; long cells = 0; var longest = 0;
        StringBuilder? field = fieldValue is null ? null : new();
        InvalidDataException Invalid(string message, int logicalRow) { var error = new InvalidDataException(message); error.Data["LogicalRow"] = logicalRow; return error; }
        void Character(char c) { length++; field?.Append(c); }
        void Field() { columns++; cells++; longest = Math.Max(longest, length); fieldValue?.Invoke(field!.ToString()); field?.Clear(); length = 0; closed = false; }
        void Row() { Field(); rows++; if (rows == 1) width = columns; else if (columns != width) throw Invalid("Irregular delimited table.", rows); columns = 0; }
        for (var i = 0; i < text.Length; i++)
        {
            if ((i & 4095) == 0) ct.ThrowIfCancellationRequested();
            var c = text[i];
            if (quoted)
            {
                if (c == '"') { if (i + 1 < text.Length && text[i + 1] == '"') { Character('"'); i++; } else { quoted = false; closed = true; } }
                else Character(c);
            }
            else if (c == separator) Field();
            else if (c is '\r' or '\n') { Row(); if (c == '\r' && i + 1 < text.Length && text[i + 1] == '\n') i++; }
            else if (c == '"' && length == 0 && !closed) quoted = true;
            else { if (closed || c == '"') throw Invalid("Invalid quoted field.", rows + 1); Character(c); }
        }
        if (quoted) throw Invalid("Unclosed quoted field.", rows + 1);
        if (length > 0 || closed || columns > 0 || text.Length > 0 && text[^1] == separator) Row();
        if (rows == 0) throw new InvalidDataException("Empty text spreadsheet.");
        return new(rows, width, cells, longest);
    }
    private static IReadOnlyList<IReadOnlyList<object?>> ReadTextV3(string text, string format, TextImportOptions options, CancellationToken ct, Action<string>? picked)
    {
        if (text.Length == 0) throw new InvalidDataException("Empty text spreadsheet.");
        char? hint = null;
        var end = text.IndexOfAny(['\r', '\n']);
        var first = end < 0 ? text : text[..end];
        if (first.Length == 5 && first.StartsWith("sep=", StringComparison.Ordinal) && first[4] is ',' or ';' or '|' or '\t')
        {
            hint = first[4]; var skip = end < 0 ? text.Length : end + 1;
            if (end >= 0 && text[end] == '\r' && skip < text.Length && text[skip] == '\n') skip++;
            text = text[skip..];
        }
        var explicitSeparator = options.Delimiter == "auto" ? (char?)null : Separator(options.Delimiter);
        if (hint is not null && explicitSeparator is not null && hint != explicitSeparator) throw new InvalidDataException("Conflicting Excel separator hint.");
        var selected = hint ?? explicitSeparator;
        var candidates = new List<(char Separator, TextShape Shape)>();
        InvalidDataException? syntax = null;
        foreach (var separator in selected is { } one ? new[] { one } : new[] { ',', ';', '\t', '|' })
        {
            try { var shape = ScanText(text, separator, ct); if (selected is not null || shape.Columns > 1) candidates.Add((separator, shape)); }
            catch (InvalidDataException ex) { if (syntax is null || ex.Message == "Unclosed quoted field.") syntax = ex; }
        }
        if (hint is not null && (candidates.Count != 1 || candidates[0].Shape.Columns < 2)) throw new InvalidDataException("Conflicting Excel separator hint.");
        // Decimal commas inside an otherwise regular semicolon table are value evidence,
        // independent of whether the first row is a header. Genuine 13/14 ties remain ties.
        if (candidates.Count == 2 && candidates.Any(c => c.Separator == ';') && candidates.Any(c => c.Separator == ','))
        {
            var commas = 0; var numeric = true;
            ScanText(text, ';', ct, value => { if (value.Contains(',')) { commas++; numeric &= Regex.IsMatch(value, @"^[+-]?[0-9]+,[0-9]+$", RegexOptions.CultureInvariant); } });
            if (commas > 0 && numeric) candidates.RemoveAll(c => c.Separator == ',');
        }
        if (candidates.Count > 1) throw new InvalidDataException("Ambiguous CSV delimiter; configure it explicitly.");
        if (candidates.Count == 0)
        {
            // TSV without any tab can be a single text column containing punctuation.
            // CSV remains conservative: irregular delimiters must never become false success.
            if (format == "tsv" && !text.Contains('\t') || options.InterpretationVersion >= 4 && !HasUnquotedSeparator(text) || !text.Any(c => c is ',' or ';' or '\t' or '|'))
                candidates.Add(('\t', ScanText(text, '\t', ct)));
            else throw syntax ?? new InvalidDataException("Irregular delimited table.");
        }
        var chosen = candidates[0]; var shapeChosen = chosen.Shape;
        if (shapeChosen.Rows > MaxTextRows) throw new SpreadsheetCapacityException(UiText.Message("capacity.tableRows", ("limit", MaxTextRows)));
        if (shapeChosen.Columns > MaxColumns) throw new SpreadsheetCapacityException(UiText.Message("capacity.tableColumns", ("limit", MaxColumns)));
        if (shapeChosen.Cells > MaxTextCells) throw new SpreadsheetCapacityException(UiText.Message("capacity.tableCells", ("limit", MaxTextCells)));
        if (shapeChosen.Longest > 32767) throw new SpreadsheetCapacityException(UiText.Message("capacity.cellCharacters"));
        picked?.Invoke(chosen.Separator switch { ',' => "comma", ';' => "semicolon", '\t' => "tab", _ => "pipe" });
        var result = new List<IReadOnlyList<object?>>(shapeChosen.Rows); var row = new List<object?>(shapeChosen.Columns);
        ScanText(text, chosen.Separator, ct, value => { row.Add(value); if (row.Count == shapeChosen.Columns) { result.Add(row); row = new(shapeChosen.Columns); } });
        return result;
    }
    private static bool HasUnquotedSeparator(string text)
    {
        var quoted = false;
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] == '"') { if (quoted && i + 1 < text.Length && text[i + 1] == '"') i++; else quoted = !quoted; }
            else if (!quoted && text[i] is ',' or ';' or '\t' or '|') return true;
        }
        return false;
    }
    private static ZipArchive OpenPackage(byte[] bytes, bool textImport = false)
    {
        var zip = new ZipArchive(new MemoryStream(bytes), ZipArchiveMode.Read);
        var expandedLimit = (textImport ? 160L : 96L) * 1024 * 1024;
        long total = 0; var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (zip.Entries.Count > 10000) { zip.Dispose(); throw new InvalidDataException("Oversized or ambiguous ZIP package."); }
        foreach (var entry in zip.Entries)
        {
            if (entry.Length > expandedLimit - total || !names.Add(entry.FullName))
            { zip.Dispose(); throw new InvalidDataException("Oversized or ambiguous ZIP package."); }
            total += entry.Length;
        }
        return zip;
    }
    private static XDocument Xml(ZipArchive zip, string path)
    {
        var entry = zip.GetEntry(path) ?? throw new InvalidDataException("Package part missing.");
        if (entry.Length > 64 * 1024 * 1024) throw new InvalidDataException("XML part too large.");
        using var stream = entry.Open(); using var reader = XmlReader.Create(stream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null, MaxCharactersInDocument = 64 * 1024 * 1024 });
        return XDocument.Load(reader);
    }
    public static IReadOnlyList<SheetValues>? ReadOds(byte[] bytes, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();
        if (bytes.Length < 4 || bytes[0] != 0x50 || bytes[1] != 0x4b || bytes[2] != 3 || bytes[3] != 4)
            throw new InvalidDataException("Not an ODS package.");
        using var zip = OpenPackage(bytes);
        var mime = zip.GetEntry("mimetype") ?? throw new InvalidDataException("ODS mimetype missing.");
        if (mime.Length > 128) throw new InvalidDataException("Invalid ODS mimetype.");
        using (var reader = new StreamReader(mime.Open())) if (reader.ReadToEnd() != "application/vnd.oasis.opendocument.spreadsheet") throw new InvalidDataException("Not an ODS package.");
        if (zip.Entries.Any(e => e.FullName.StartsWith("Basic/", StringComparison.OrdinalIgnoreCase) || e.FullName.StartsWith("Scripts/", StringComparison.OrdinalIgnoreCase))) throw new NotSupportedException("ODS scripts excluded.");
        var doc = Xml(zip, "content.xml");
        XNamespace office = "urn:oasis:names:tc:opendocument:xmlns:office:1.0", table = "urn:oasis:names:tc:opendocument:xmlns:table:1.0", text = "urn:oasis:names:tc:opendocument:xmlns:text:1.0";
        if (doc.Root?.Name != office + "document-content" || doc.Root.Element(office + "body")?.Element(office + "spreadsheet") is not XElement book) throw new InvalidDataException("Invalid ODS workbook.");
        if (doc.Descendants(office + "scripts").Any(e => e.Elements().Any())) throw new NotSupportedException("ODS scripts excluded.");
        var simple = !doc.Descendants().Any(e => e.Name.LocalName is "scripts" or "script" or "object" or "image" or "frame" or "covered-table-cell"
            || e.Attributes().Any(a => a.Name.LocalName is "formula" or "number-columns-spanned" or "number-rows-spanned" or "href"));
        var sheets = new List<SheetValues>(); var cells = 0;
        foreach (var sheet in book.Elements(table + "table"))
        {
            ct.ThrowIfCancellationRequested();
            if (sheets.Count >= 100) throw new InvalidDataException("Too many sheets.");
            var name = (string?)sheet.Attribute(table + "name") ?? throw new InvalidDataException("ODS sheet name missing.");
            var rows = new List<IReadOnlyList<object?>>(); long rowPosition = 0;
            foreach (var r in sheet.Descendants(table + "table-row"))
            {
                ct.ThrowIfCancellationRequested();
                var row = new List<object?>(); long columnPosition = 0;
                foreach (var c in r.Elements().Where(e => e.Name == table + "table-cell" || e.Name == table + "covered-table-cell"))
                {
                    var repeated = Repeat(c, table + "number-columns-repeated", 16384);
                    object? value = null; var type = (string?)c.Attribute(office + "value-type");
                    switch (type)
                    {
                        case null: if (c.HasElements) simple = false; break;
                        case "string":
                            value = (string?)c.Attribute(office + "string-value") ?? string.Join("\n", c.Elements(text + "p").Select(p => p.Value));
                            if (c.Descendants().Any(e => e.Name != text + "p" && e.Name != text + "span")) simple = false;
                            break;
                        case "float":
                        case "percentage":
                        case "currency":
                            if (!double.TryParse((string?)c.Attribute(office + "value"), NumberStyles.Float, CultureInfo.InvariantCulture, out var number) || !double.IsFinite(number)) throw new InvalidDataException("Invalid ODS number."); value = number; break;
                        case "boolean": value = (string?)c.Attribute(office + "boolean-value") switch { "true" => true, "false" => false, _ => throw new InvalidDataException("Invalid ODS boolean.") }; break;
                        default: simple = false; break;
                    }
                    var content = type is not null || value is not null || c.Attribute(table + "formula") is not null
                        || c.Name == table + "covered-table-cell" || c.HasElements || c.Attributes().Any(a => a.Name.LocalName == "href")
                        || c.Attribute(table + "number-columns-spanned") is not null || c.Attribute(table + "number-rows-spanned") is not null;
                    columnPosition = checked(columnPosition + repeated);
                    if (columnPosition > 16384) throw new InvalidDataException("ODS columns too large.");
                    if (!content) continue;
                    if (columnPosition > MaxColumns) throw new InvalidDataException("ODS content dimensions exceed limits.");
                    while (row.Count < columnPosition - repeated) row.Add(null);
                    for (var i = 0; i < repeated; i++) row.Add(value);
                }
                var copies = Repeat(r, table + "number-rows-repeated", 1048576);
                rowPosition = checked(rowPosition + copies);
                if (rowPosition > 1048576) throw new InvalidDataException("ODS rows too large.");
                if (row.Count == 0) continue;
                if (rowPosition > MaxRows || (long)cells + (long)row.Count * copies > MaxCells) throw new InvalidDataException("ODS content dimensions exceed limits.");
                while (rows.Count < rowPosition - copies) rows.Add(Array.Empty<object?>());
                cells += row.Count * copies; var values = row.ToArray();
                for (var i = 0; i < copies; i++) rows.Add(values);
            }
            sheets.Add(new(name, rows));
        }
        if (sheets.Count == 0 || sheets.Select(s => s.Name).Distinct(StringComparer.Ordinal).Count() != sheets.Count) throw new InvalidDataException("Invalid ODS sheets.");
        return simple ? sheets : null;
    }
    private static int Repeat(XElement element, XName name, int max)
    {
        var attr = element.Attribute(name); if (attr is null) return 1;
        if (!int.TryParse(attr.Value, out var value) || value < 1 || value > max) throw new InvalidDataException("Unbounded repetition."); return value;
    }
    public static IReadOnlyList<SheetValues> ReadExcel(byte[] bytes, bool binary = false, CancellationToken ct = default, bool textImport = false)
    {
        if (!binary)
        {
            GoogleImport.ValidateXlsx(bytes);
            using var zip = OpenPackage(bytes, textImport); _ = Xml(zip, "xl/workbook.xml");
            foreach (var entry in zip.Entries.Where(e => e.FullName.StartsWith("xl/worksheets/", StringComparison.Ordinal) && e.FullName.EndsWith(".xml", StringComparison.Ordinal)))
            {
                var xmlLimit = (textImport ? 128L : 64L) * 1024 * 1024;
                if (entry.Length > xmlLimit) throw new SpreadsheetCapacityException(UiText.Message("capacity.uncompressedSheet", ("limit", xmlLimit / (1024 * 1024))));
                using var xmlStream = entry.Open();
                using var xmlReader = XmlReader.Create(xmlStream, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null, MaxCharactersInDocument = xmlLimit });
                while (xmlReader.Read())
                {
                    ct.ThrowIfCancellationRequested();
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.LocalName == "f" && xmlReader.NamespaceURI == "http://schemas.openxmlformats.org/spreadsheetml/2006/main") throw new FormulaVerificationException();
                }
            }
        }
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var stream = new MemoryStream(bytes);
        using var reader = binary ? ExcelReaderFactory.CreateBinaryReader(stream) : ExcelReaderFactory.CreateOpenXmlReader(stream);
        var sheets = new List<SheetValues>(); var cells = 0;
        var maxRows = textImport ? MaxTextRows : MaxRows; var maxCells = textImport ? MaxTextCells : MaxCells;
        do
        {
            if (sheets.Count >= 100 || reader.FieldCount > MaxColumns || reader.RowCount > maxRows) throw new InvalidDataException("Workbook dimensions exceed limits.");
            var rows = new List<IReadOnlyList<object?>>();
            while (reader.Read())
            {
                ct.ThrowIfCancellationRequested();
                if (rows.Count >= maxRows || (long)cells + reader.FieldCount > maxCells) throw new InvalidDataException("Workbook cell limit exceeded.");
                var row = new object?[reader.FieldCount]; for (var i = 0; i < row.Length; i++) row[i] = reader.GetValue(i); cells += row.Length; rows.Add(row);
            }
            sheets.Add(new(reader.Name, rows));
        } while (reader.NextResult());
        return sheets;
    }
    public static void VerifyValues(IReadOnlyList<SheetValues> expected, byte[] exported, CancellationToken ct = default, bool textImport = false)
    {
        var actual = ReadExcel(exported, ct: ct, textImport: textImport);
        if (actual.Count != expected.Count) throw new ConversionMismatchException();
        for (var s = 0; s < expected.Count; s++)
        {
            if (expected[s].Name != actual[s].Name) throw new ConversionMismatchException();
            var left = expected[s].Rows; var right = actual[s].Rows;
            for (var r = 0; r < Math.Max(left.Count, right.Count); r++)
            {
                ct.ThrowIfCancellationRequested();
                var x = r < left.Count ? left[r] : Array.Empty<object?>();
                var y = r < right.Count ? right[r] : Array.Empty<object?>();
                for (var c = 0; c < Math.Max(x.Count, y.Count); c++)
                {
                    object? Normalize(object? value) => value switch { null or "" => null, int n => (double)n, string text when textImport => text.Replace("\r\n", "\n", StringComparison.Ordinal), _ => value };
                    if (!Equals(Normalize(c < x.Count ? x[c] : null), Normalize(c < y.Count ? y[c] : null))) throw new ConversionMismatchException();
                }
            }
        }
    }
    public static byte[] WriteXlsx(IReadOnlyList<SheetValues> sheets, CancellationToken ct = default, bool textImport = false, bool compactText = false, bool preserveLineEndings = false)
    {
        long total = 0;
        foreach (var sheet in sheets)
        {
            ct.ThrowIfCancellationRequested();
            if (sheet.Rows.Count > (textImport ? MaxTextRows : MaxRows)) throw new SpreadsheetCapacityException(UiText.Message("capacity.tableRows", ("limit", textImport ? MaxTextRows : MaxRows)));
            foreach (var row in sheet.Rows)
            {
                ct.ThrowIfCancellationRequested(); total += row.Count;
                if (row.Count > MaxColumns) throw new SpreadsheetCapacityException(UiText.Message("capacity.tableColumns", ("limit", MaxColumns)));
                if (total > (textImport ? MaxTextCells : MaxCells)) throw new SpreadsheetCapacityException(UiText.Message("capacity.tableCells", ("limit", textImport ? MaxTextCells : MaxCells)));
                if (row.Any(v => v is string value && value.Length > 32767)) throw new SpreadsheetCapacityException(UiText.Message("capacity.cellCharacters"));
            }
        }
        XNamespace main = "http://schemas.openxmlformats.org/spreadsheetml/2006/main", rel = "http://schemas.openxmlformats.org/officeDocument/2006/relationships", package = "http://schemas.openxmlformats.org/package/2006/relationships", types = "http://schemas.openxmlformats.org/package/2006/content-types";
        using var buffer = new MemoryStream();
        using (var zip = new ZipArchive(buffer, ZipArchiveMode.Create, true))
        {
            void Part(string name, XElement xml) { var entry = zip.CreateEntry(name); entry.LastWriteTime = new DateTimeOffset(1980, 1, 1, 0, 0, 0, TimeSpan.Zero); using var stream = entry.Open(); xml.Save(stream); }
            Part("[Content_Types].xml", new XElement(types + "Types", new XElement(types + "Default", new XAttribute("Extension", "rels"), new XAttribute("ContentType", "application/vnd.openxmlformats-package.relationships+xml")), new XElement(types + "Default", new XAttribute("Extension", "xml"), new XAttribute("ContentType", "application/xml")), new XElement(types + "Override", new XAttribute("PartName", "/xl/workbook.xml"), new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml")), sheets.Select((_, i) => new XElement(types + "Override", new XAttribute("PartName", $"/xl/worksheets/sheet{i + 1}.xml"), new XAttribute("ContentType", "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml")))));
            Part("_rels/.rels", new XElement(package + "Relationships", new XElement(package + "Relationship", new XAttribute("Id", "rId1"), new XAttribute("Type", rel.NamespaceName + "/officeDocument"), new XAttribute("Target", "xl/workbook.xml"))));
            Part("xl/workbook.xml", new XElement(main + "workbook", new XElement(main + "sheets", sheets.Select((s, i) => new XElement(main + "sheet", new XAttribute("name", s.Name), new XAttribute("sheetId", i + 1), new XAttribute(rel + "id", "rId" + (i + 1)))))));
            Part("xl/_rels/workbook.xml.rels", new XElement(package + "Relationships", sheets.Select((_, i) => new XElement(package + "Relationship", new XAttribute("Id", "rId" + (i + 1)), new XAttribute("Type", rel.NamespaceName + "/worksheet"), new XAttribute("Target", $"worksheets/sheet{i + 1}.xml")))));
            for (var s = 0; s < sheets.Count; s++)
            {
                var entry = zip.CreateEntry($"xl/worksheets/sheet{s + 1}.xml");
                entry.LastWriteTime = new DateTimeOffset(1980, 1, 1, 0, 0, 0, TimeSpan.Zero);
                using var stream = entry.Open();
                using var writer = XmlWriter.Create(stream, new XmlWriterSettings { Encoding = new UTF8Encoding(false), CloseOutput = false, NewLineHandling = preserveLineEndings ? NewLineHandling.Entitize : NewLineHandling.Replace });
                writer.WriteStartElement("worksheet", main.NamespaceName); writer.WriteStartElement("sheetData", main.NamespaceName);
                var rows = sheets[s].Rows;
                for (var r = 0; r < rows.Count; r++)
                {
                    ct.ThrowIfCancellationRequested();
                    writer.WriteStartElement("row", main.NamespaceName); writer.WriteAttributeString("r", (r + 1).ToString(CultureInfo.InvariantCulture));
                    for (var c = 0; c < rows[r].Count; c++)
                    {
                        var value = rows[r][c]; if (value is null) continue;
                        writer.WriteStartElement("c", main.NamespaceName); writer.WriteAttributeString("r", Column(c) + (r + 1));
                        if (value is bool flag)
                        { writer.WriteAttributeString("t", "b"); writer.WriteElementString("v", main.NamespaceName, flag ? "1" : "0"); }
                        else if (value is double or int) writer.WriteElementString("v", main.NamespaceName, Convert.ToString(value, CultureInfo.InvariantCulture));
                        else
                        {
                            writer.WriteAttributeString("t", "inlineStr"); writer.WriteStartElement("is", main.NamespaceName); writer.WriteStartElement("t", main.NamespaceName);
                            if (!compactText || value.ToString()!.Any(char.IsWhiteSpace)) writer.WriteAttributeString("xml", "space", XNamespace.Xml.NamespaceName, "preserve");
                            writer.WriteString(EscapeSpreadsheetText(value.ToString()!));
                            writer.WriteEndElement(); writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); writer.WriteEndElement();
            }
        }
        var bytes = buffer.ToArray();
        if (textImport)
        {
            // Reject a generated package beyond the same verification budget before uploading.
            using var packageCheck = OpenPackage(bytes, textImport: true);
            if (packageCheck.Entries.Any(e => e.FullName.StartsWith("xl/worksheets/", StringComparison.Ordinal) && e.Length > 128L * 1024 * 1024))
                throw new SpreadsheetCapacityException(UiText.Message("capacity.uncompressedSheet", ("limit", 128)));
        }
        if (bytes.Length > GoogleImport.MaxBytes) throw new SpreadsheetCapacityException(UiText.Message("capacity.normalizedSize")); return bytes;
    }
    private static string EscapeSpreadsheetText(string value)
    {
        // ST_Xstring represents XML-forbidden controls without deleting cell data.
        var literal = Regex.Replace(value, @"_x[0-9a-fA-F]{4}_", m => "_x005F_" + m.Value[1..]);
        return Regex.Replace(literal, @"[\x00-\x08\x0B\x0C\x0E-\x1F\uFFFE\uFFFF]", m => $"_x{(int)m.Value[0]:X4}_");
    }
    private static string Column(int index)
    {
        var result = ""; for (var i = index + 1; i > 0; i = (i - 1) / 26) result = (char)('A' + (i - 1) % 26) + result; return result;
    }
}
