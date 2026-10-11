using SheetsWindows.Infrastructure;
namespace SheetsWindows.Windows;

// Shared native controls. Text remains selectable/readable at Windows DPI and keyboard focus is native.
internal static class Ui
{
    internal static Label Text(string text, bool heading = false) => new EmphasisLabel()
    {
        Text = text, AutoSize = true, MaximumSize = new Size(480, 0),
        Margin = new Padding(0, heading ? 12 : 4, 0, 8),
        Font = new Font(LanguageVisuals.Family, heading ? 11 : 10, heading ? FontStyle.Bold : FontStyle.Regular)
    };
    internal static Label Center(Label label) { label.Tag = "center"; label.TextAlign = ContentAlignment.MiddleCenter; return label; }
    internal static Label Hero(string text) => new EmphasisLabel { Tag = "center", Text = text, AutoSize = true, TextAlign = ContentAlignment.MiddleCenter,
        Font = new Font(LanguageVisuals.Family, 16, FontStyle.Bold), Margin = new Padding(0, 16, 0, 10), MaximumSize = new Size(700, 0) };
    internal static TableLayoutPanel Pair(Control left, Control right)
    {
        var row = new TableLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 2, RowCount = 1, Margin = new Padding(0, 2, 0, 6) };
        row.ColumnStyles.Add(new(SizeType.Percent, 50)); row.ColumnStyles.Add(new(SizeType.Percent, 50)); row.RowStyles.Add(new(SizeType.AutoSize));
        foreach (var child in new[] { left, right }) { child.Dock = DockStyle.Fill; child.Margin = new Padding(0, 2, 8, 2); }
        row.Controls.Add(left, 0, 0); row.Controls.Add(right, 1, 0);
        void Fit()
        {
            var width = Math.Max(50, row.ClientSize.Width / 2 - 8);
            foreach (Control child in row.Controls)
            {
                if (child is CheckBox) { child.AutoSize = false; child.Height = Math.Max(32, TextRenderer.MeasureText(child.Text, child.Font, new Size(Math.Max(30, width - 54), int.MaxValue), TextFormatFlags.WordBreak).Height + 12); }
                else if (child is Button) { child.AutoSize = false; child.Height = TextRenderer.MeasureText(child.Text, child.Font, new Size(Math.Max(30,width-child.Padding.Horizontal-12),int.MaxValue),TextFormatFlags.WordBreak).Height + child.Padding.Vertical + 10; }
            }
        }
        row.SizeChanged += (_, _) => Fit(); foreach (Control child in row.Controls) { child.TextChanged += (_, _) => Fit(); child.FontChanged += (_, _) => Fit(); } Fit(); return row;
    }
    internal static Panel Separator() => new() { Height = 1, Width = 480, Margin = new Padding(0, 10, 0, 10), AccessibleName = UiText.Get("accessibility.separator"), Tag = "separator" };
    internal static void Primary(Button button) { button.Tag = "primary"; button.MinimumSize = new Size(0, 38); }
    internal static void Adapt(FlowLayoutPanel layout)
    {
        void ResizeChildren()
        {
            var available = Math.Max(120, layout.ClientSize.Width - layout.Padding.Horizontal - (layout.AutoScroll ? SystemInformation.VerticalScrollBarWidth : 0) - 6);
            foreach (Control child in layout.Controls)
            {
                if (child is FlowLayoutPanel nested) { nested.MaximumSize = new Size(available, 0); nested.MinimumSize = new Size(available, 0); nested.Width = available; }
                else if (child is Label label)
                {
                    label.MaximumSize = new Size(available - label.Margin.Horizontal, 0);
                    if (label.Tag as string == "center") { label.AutoSize = false; label.Width = available - label.Margin.Horizontal; label.Height = TextRenderer.MeasureText(label.Text, label.Font, new Size(label.Width, int.MaxValue), TextFormatFlags.WordBreak).Height + 12; }
                }
                else if (child is CheckBox check) { check.AutoSize = false; check.Width = available; check.Height = TextRenderer.MeasureText(check.Text, check.Font, new Size(available - 54, 0), TextFormatFlags.WordBreak).Height + 12; }
                else if (child is Button or TextBox or ComboBox or TableLayoutPanel || child.Tag as string == "separator") child.Width = available - child.Margin.Horizontal;
            }
        }
        foreach (Control child in layout.Controls) child.TextChanged += (_,_) => ResizeChildren();
        layout.ControlAdded += (_,e) => { if (e.Control is {} child) child.TextChanged += (_,_) => ResizeChildren(); ResizeChildren(); };
        layout.Resize += (_, _) => ResizeChildren();
        layout.FontChanged += (_, _) => ResizeChildren();
        layout.HandleCreated += (_, _) => ResizeChildren();
        ResizeChildren();
    }
}

