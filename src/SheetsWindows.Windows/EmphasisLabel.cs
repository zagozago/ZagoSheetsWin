using System.Text.RegularExpressions;

namespace SheetsWindows.Windows;

// Plain accessible text with selective emphasis. Measurement and painting share
// the same wrapping algorithm, including when the text or Windows DPI changes.
internal sealed class EmphasisLabel : Label
{
    private static readonly string[] Phrases =
    [
        "ZagoSheetsWin", "Zagotools", "Google Sheets", "Google Drive",
        "backup do original", "backup local", "original é mantido", "original foi preservado",
        "original e o backup foram preservados", "original é substituído por um atalho",
        "antes da substituição", "após a importação confirmada", "importação confirmada",
        "Restaurar em…", "Selecionar todos", "Autorizar Google", "Verificar conexão Google",
        "Aplicativos padrão", "Abrir com", "importar como cópia", "Importe como cópia",
        "operações pendentes são protegidas", "Operações pendentes são protegidas",
        "Limpeza automática só quando ativada", "não passam por servidor do Zagotools",
        "diretamente ao Google", "drive.file", "MIT", "Apache-2.0", "LGPL-3.0-only",
        "30 dias", "200 MB", "1 GB", "20 MiB", "Macros não funcionam",
        "Não será possível restaurar", "Usa sincronização?", "pasta local",
        "Escolha sua conta", "autorize o acesso", "Google conectado", "não precisa instalar"
    ];

    public EmphasisLabel()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        UseMnemonic = false;
    }

    internal bool HasEmphasis => Font.Bold || Phrases.Any(p => Text.Contains(p, StringComparison.OrdinalIgnoreCase));

    public override Size GetPreferredSize(Size proposedSize)
    {
        using var graphics = CreateGraphics();
        var width = MaximumSize.Width > 0 ? MaximumSize.Width : proposedSize.Width > 0 ? proposedSize.Width : 480;
        var size = LayoutText(graphics, Math.Max(1, width - Padding.Horizontal), false);
        return new Size(Math.Min(width, size.Width + Padding.Horizontal), size.Height + Padding.Vertical);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        LayoutText(e.Graphics, Math.Max(1, ClientSize.Width - Padding.Horizontal), true);
    }

    private Size LayoutText(Graphics graphics, int width, bool paint)
    {
        using var bold = new Font(Font, FontStyle.Bold);
        using var ink = new SolidBrush(Enabled ? ForeColor : SystemColors.GrayText);
        using var format = (StringFormat)StringFormat.GenericTypographic.Clone();
        format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
        var ranges = new List<(int Start, int End)>();
        foreach (var phrase in Phrases)
        {
            var start = 0;
            while ((start = Text.IndexOf(phrase, start, StringComparison.OrdinalIgnoreCase)) >= 0)
            { ranges.Add((start, start + phrase.Length)); start += phrase.Length; }
        }
        float x = 0, y = 0, widest = 0;
        var line = Math.Max(Font.GetHeight(graphics), bold.GetHeight(graphics));
        foreach (Match token in Regex.Matches(Text, @"\r\n|\n|[^\S\r\n]+|[^\s]+"))
        {
            if (token.Value is "\n" or "\r\n") { widest = Math.Max(widest, x); x = 0; y += line; continue; }
            var font = Font.Bold || ranges.Any(r => token.Index < r.End && token.Index + token.Length > r.Start) ? bold : Font;
            var measured = graphics.MeasureString(token.Value, font, int.MaxValue, format).Width;
            var whitespace = string.IsNullOrWhiteSpace(token.Value);
            if (x > 0 && x + measured > width) { widest = Math.Max(widest, x); x = 0; y += line; if (whitespace) continue; }
            if (measured <= width)
            {
                if (paint) graphics.DrawString(token.Value, font, ink, Padding.Left + x, Padding.Top + y, format);
                x += measured;
            }
            else
            {
                // Break long paths/identifiers without splitting Unicode text elements.
                var elements = System.Globalization.StringInfo.GetTextElementEnumerator(token.Value);
                while (elements.MoveNext())
                {
                    var part = elements.GetTextElement();
                    var partWidth = graphics.MeasureString(part, font, int.MaxValue, format).Width;
                    if (x > 0 && x + partWidth > width) { widest = Math.Max(widest, x); x = 0; y += line; }
                    if (paint) graphics.DrawString(part, font, ink, Padding.Left + x, Padding.Top + y, format);
                    x += partWidth;
                }
            }
        }
        return new Size((int)Math.Ceiling(Math.Max(widest, x)), (int)Math.Ceiling(y + line));
    }
}
