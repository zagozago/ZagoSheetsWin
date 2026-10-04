using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using HtmlAgilityPack;

namespace SheetsWindows.Infrastructure;

// Some exporters label an HTML table as .xls. Parse locally; never execute scripts or fetch links.
internal static class HtmlWorkbookReader
{
    public static bool IsHtml(byte[] bytes)
    {
        var prefix = Encoding.UTF8.GetString(bytes, 0, Math.Min(bytes.Length, 4096)).TrimStart('\uFEFF', ' ', '\t', '\r', '\n');
        if (bytes.AsSpan().StartsWith(new byte[] { 255, 254 })) prefix = Encoding.Unicode.GetString(bytes, 2, Math.Min(bytes.Length - 2, 4096));
        if (bytes.AsSpan().StartsWith(new byte[] { 254, 255 })) prefix = Encoding.BigEndianUnicode.GetString(bytes, 2, Math.Min(bytes.Length - 2, 4096));
        return Regex.IsMatch(prefix, @"^\s*(?:<!doctype\s+html\b|<html\b|<head\b|<body\b|<table\b|<!--)", RegexOptions.IgnoreCase);
    }
    private static string Decode(byte[] bytes)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        if (bytes.AsSpan().StartsWith(new byte[] { 255, 254 })) return new UnicodeEncoding(false, true, true).GetString(bytes, 2, bytes.Length - 2);
        if (bytes.AsSpan().StartsWith(new byte[] { 254, 255 })) return new UnicodeEncoding(true, true, true).GetString(bytes, 2, bytes.Length - 2);
        var head = Encoding.ASCII.GetString(bytes, 0, Math.Min(bytes.Length, 4096));
        var charset = Regex.Match(head, "charset\\s*=\\s*[\"']?\\s*([\\w-]+)", RegexOptions.IgnoreCase).Groups[1].Value.ToLowerInvariant();
        var offset = bytes.AsSpan().StartsWith(new byte[] { 239, 187, 191 }) ? 3 : 0;
        if (offset != 0 && charset is not ("" or "utf-8" or "utf8")) throw new InvalidDataException("HTML encoding conflicts with BOM.");
        Encoding encoding = charset switch
        {
            "" or "utf-8" or "utf8" => new UTF8Encoding(false, true),
            "windows-1252" or "iso-8859-1" => Encoding.GetEncoding(1252, EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback),
            _ => throw new NotSupportedException("Unsupported HTML spreadsheet encoding.")
        };
        try { return encoding.GetString(bytes, offset, bytes.Length - offset); }
        catch (DecoderFallbackException ex) { throw new InvalidDataException("Invalid HTML spreadsheet encoding.", ex); }
    }
    private static string Text(HtmlNode node)
    {
        var text = new StringBuilder();
        void Visit(HtmlNode current)
        {
            if (current.Name is "script" or "style") return;
            if (current.NodeType == HtmlNodeType.Text) text.Append(HtmlEntity.DeEntitize(current.InnerText));
            else if (current.Name == "br") text.Append('\n');
            else foreach (var child in current.ChildNodes) Visit(child);
        }
        Visit(node);
        var value = string.Join("\n", text.ToString().Replace('\u00A0', ' ').Split('\n').Select(s => Regex.Replace(s, @"[\t\r ]+", " ").Trim())).Trim();
        if (value.Length > 32767) throw new SpreadsheetCapacityException("Uma célula excede o limite de 32.767 caracteres.");
        XmlConvert.VerifyXmlChars(value); return value;
    }
    private static object Value(HtmlNode node)
    {
        var text = Text(node);
        // Do not turn identifiers with leading zeros or formula-like text into numbers/formulas.
        if (!node.GetAttributeValue("style", "").Contains("\\@", StringComparison.Ordinal)
            && Regex.IsMatch(text, @"^-?(?:0|[1-9][0-9]*)(?:\.[0-9]+)?$")
            && text.Count(char.IsDigit) <= 15 && double.TryParse(text, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var number)) return number;
        return text;
    }
    public static IReadOnlyList<SheetValues> Read(byte[] bytes, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        var document = new HtmlDocument { OptionMaxNestedChildNodes = 128 };
        try { document.LoadHtml(Decode(bytes)); }
        catch (Exception ex) when (ex is not OperationCanceledException and not InvalidDataException and not NotSupportedException and not OutOfMemoryException)
        { throw new InvalidDataException("Invalid HTML spreadsheet structure.", ex); }
        var tables = document.DocumentNode.Descendants("table").ToArray();
        if (tables.Length is 0 or > 99 || tables.Any(t => t.Ancestors("table").Any())) throw new InvalidDataException("HTML spreadsheet requires up to 99 non-nested tables.");
        var sheets = new List<SheetValues>(); var cells = 0;
        foreach (var table in tables)
        {
            var grid = new Dictionary<(int Row, int Column), object?>(); var r = 0; var height = 0; var width = 0;
            foreach (var row in table.Descendants("tr"))
            {
                ct.ThrowIfCancellationRequested();
                if (r >= SpreadsheetFormats.MaxRows) throw new SpreadsheetCapacityException("A planilha HTML excede o limite de linhas.");
                var c = 0;
                foreach (var cell in row.ChildNodes.Where(n => n.Name is "td" or "th"))
                {
                    ct.ThrowIfCancellationRequested();
                    while (grid.ContainsKey((r, c))) c++;
                    int Span(string name, int max)
                    {
                        var attr = cell.GetAttributeValue(name, "1");
                        if (!int.TryParse(attr, out var size) || size < 1 || size > max) throw new SpreadsheetCapacityException("Uma célula mesclada da planilha HTML excede os limites.");
                        return size;
                    }
                    var rows = Span("rowspan", SpreadsheetFormats.MaxRows); var columns = Span("colspan", SpreadsheetFormats.MaxColumns);
                    if (r + rows > SpreadsheetFormats.MaxRows || c + columns > SpreadsheetFormats.MaxColumns || (long)cells + (long)rows * columns > SpreadsheetFormats.MaxCells) throw new SpreadsheetCapacityException("A planilha HTML excede o limite de células.");
                    var value = Value(cell);
                    for (var dr = 0; dr < rows; dr++) for (var dc = 0; dc < columns; dc++)
                    {
                        if (!grid.TryAdd((r + dr, c + dc), dr == 0 && dc == 0 ? value : "")) throw new InvalidDataException("Overlapping HTML cells.");
                        cells++;
                    }
                    width = Math.Max(width, c + columns); height = Math.Max(height, r + rows); c += columns;
                }
                r++;
            }
            if (width == 0 || height == 0) throw new InvalidDataException("Empty HTML table.");
            if ((long)height * width > SpreadsheetFormats.MaxCells || (long)sheets.Sum(s => s.Rows.Count * (s.Rows.FirstOrDefault()?.Count ?? 0)) + (long)height * width > SpreadsheetFormats.MaxCells) throw new SpreadsheetCapacityException("A planilha HTML excede o limite de células.");
            var result = new List<IReadOnlyList<object?>>();
            for (var y = 0; y < height; y++)
            {
                ct.ThrowIfCancellationRequested();
                result.Add(Enumerable.Range(0, width).Select(x => grid.GetValueOrDefault((y, x), "")).ToArray());
            }
            sheets.Add(new(tables.Length == 1 ? "Dados" : "Dados " + (sheets.Count + 1), result));
        }
        // Keep report filters/headings outside the table in a separate information tab.
        var information = document.DocumentNode.Descendants().Where(n => n.Name is "h1" or "h2" or "h3" or "p" or "li")
            .Where(n => !n.Ancestors().Any(a => a.Name is "table" or "script" or "style" or "p" or "li"))
            .Select(n => (IReadOnlyList<object?>)new object?[] { Text(n) }).Where(r => ((string)r[0]!).Length != 0).ToArray();
        if (information.Length > SpreadsheetFormats.MaxRows || (long)sheets.Sum(s => s.Rows.Count * s.Rows[0].Count) + information.Length > SpreadsheetFormats.MaxCells) throw new SpreadsheetCapacityException("As informações do relatório HTML excedem os limites.");
        if (information.Length != 0) sheets.Add(new("Informações", information));
        return sheets;
    }
}
