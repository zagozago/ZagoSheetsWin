namespace SheetsWindows.Windows;

// Ordinary labels retain native rendering. The opt-in prefix uses RichEdit for
// mixed fonts with native shaping, wrapping and bidirectional script support.
internal sealed class EmphasisLabel : Label
{
    private RichTextBox? formatted;
    private bool updating;
    public EmphasisLabel() { UseMnemonic = false; AutoEllipsis = false; }
    internal bool HasEmphasis => Font.Bold || BoldPrefix;
    internal bool BoldPrefix
    {
        get => formatted is not null;
        set
        {
            if (!value || formatted is not null) return;
            formatted = new RichTextBox { BorderStyle = BorderStyle.None, ReadOnly = true,
                DetectUrls = false, ScrollBars = RichTextBoxScrollBars.None,
                WordWrap = true, TabStop = false, Dock = DockStyle.Fill };
            formatted.FontChanged += (_, _) => UpdateFormatting();
            Controls.Add(formatted);
            UpdateFormatting();
        }
    }
    private void UpdateFormatting()
    {
        if (formatted is null || updating) return;
        updating = true;
        try
        {
            formatted.BackColor = BackColor; formatted.ForeColor = ForeColor;
            formatted.RightToLeft = RightToLeft; formatted.Font = Font;
            formatted.Text = Text;
            formatted.SelectAll(); formatted.SelectionFont = Font;
            var colon = Text.IndexOfAny([':', '：', '﹕', '꞉']);
            if (colon >= 0)
            {
                formatted.Select(0, colon + 1);
                using var bold = new Font(Font, FontStyle.Bold);
                formatted.SelectionFont = bold;
            }
            formatted.Select(0, 0);

        }
        finally { updating = false; }
    }
    public override Size GetPreferredSize(Size proposedSize)
    {
        if (!BoldPrefix) return base.GetPreferredSize(proposedSize);
        var width = MaximumSize.Width > 0 ? MaximumSize.Width : proposedSize.Width;
        if (width <= 0) width = 480;
        using var bold = new Font(Font, FontStyle.Bold);
        var size = TextRenderer.MeasureText(Text, bold, new Size(Math.Max(20, width - 4), int.MaxValue), TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
        return new Size(Math.Min(width, size.Width + 4), size.Height + 6 * DeviceDpi / 96);
    }
    protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); UpdateFormatting(); }
    protected override void OnFontChanged(EventArgs e) { base.OnFontChanged(e); UpdateFormatting(); }
    protected override void OnBackColorChanged(EventArgs e) { base.OnBackColorChanged(e); UpdateFormatting(); }
    protected override void OnForeColorChanged(EventArgs e) { base.OnForeColorChanged(e); UpdateFormatting(); }
    protected override void OnRightToLeftChanged(EventArgs e) { base.OnRightToLeftChanged(e); UpdateFormatting(); }
    protected override void OnSizeChanged(EventArgs e) { base.OnSizeChanged(e); UpdateFormatting(); }
}
