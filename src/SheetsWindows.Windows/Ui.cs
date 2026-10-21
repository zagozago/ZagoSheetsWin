using SheetsWindows.Infrastructure;
namespace SheetsWindows.Windows;

// Shared native controls. Text remains selectable/readable at Windows DPI and keyboard focus is native.
internal static class Ui
{
    internal const float ButtonFontSize = 9f;
    internal static Label Text(string text, bool heading = false) => new EmphasisLabel()
    {
        Text = text, AutoSize = true, MaximumSize = new Size(480, 0),
        Margin = new Padding(0, heading ? 12 : 4, 0, 8),
        Font = new Font(LanguageVisuals.Family, heading ? 11 : 10, heading ? FontStyle.Bold : FontStyle.Regular)
    };
    internal static Label Center(Label label) { label.Tag = "center"; label.TextAlign = ContentAlignment.MiddleCenter; return label; }
    internal static Label Hero(string text) => new EmphasisLabel { Tag = "center", Text = text, AutoSize = true, TextAlign = ContentAlignment.MiddleCenter,
        Font = new Font(LanguageVisuals.Family, 16, FontStyle.Bold), Margin = new Padding(0, 16, 0, 10), MaximumSize = new Size(700, 0) };
    internal static TableLayoutPanel Pair(Control left, Control right) => Columns(left, right);
    internal static TableLayoutPanel Columns(params Control[] children)
    {
        var row = new TableLayoutPanel { AutoSize = false, ColumnCount = children.Length, RowCount = 1, Margin = new Padding(0, 2, 0, 6) };
        foreach (var child in children) row.ColumnStyles.Add(new(SizeType.Percent, 100f / children.Length)); row.RowStyles.Add(new(SizeType.Percent, 100));
        for (var i = 0; i < children.Length; i++) { children[i].Dock = DockStyle.Fill; children[i].Margin = new Padding(0, 2, i == children.Length - 1 ? 0 : 8, 2); }
        for (var i = 0; i < children.Length; i++) row.Controls.Add(children[i], i, 0);
        void Fit()
        {
            var width = Math.Max(50, (row.ClientSize.Width - row.Padding.Horizontal) / children.Length - 8);
            var height = 32;
            foreach (Control child in row.Controls)
            {
                var needed = child.PreferredSize.Height;
                if (child is CheckBox) { child.AutoSize = false; needed = Math.Max(32, TextRenderer.MeasureText(child.Text, child.Font, new Size(Math.Max(30, width - 54), int.MaxValue), TextFormatFlags.WordBreak).Height + 12); }
                else if (child is Button) { child.AutoSize = false; needed = TextRenderer.MeasureText(child.Text, child.Font, new Size(Math.Max(30,width-child.Padding.Horizontal-12-((child as Button)?.Image?.Width+6 ?? 0)),int.MaxValue),TextFormatFlags.WordBreak).Height + child.Padding.Vertical + 10; }
                height = Math.Max(height, needed + child.Margin.Vertical);
            }
            row.Height = height + row.Padding.Vertical;
        }
        row.SizeChanged += (_, _) => Fit(); row.PaddingChanged += (_, _) => Fit(); foreach (Control child in row.Controls) { child.TextChanged += (_, _) => Fit(); child.FontChanged += (_, _) => Fit(); } Fit(); return row;
    }
    internal static Panel Separator() => new() { Height = 1, Width = 480, Margin = new Padding(0, 10, 0, 10), AccessibleName = UiText.Get("accessibility.separator"), Tag = "separator" };
    internal static void Primary(Button button) { button.Tag = "primary"; button.MinimumSize = new Size(0, 38); }
    internal static void Adapt(FlowLayoutPanel layout)
    {
        void ResizeChildren()
        {
            var available = Math.Max(120, layout.ClientSize.Width - layout.Padding.Horizontal - (layout.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0));
            foreach (Control child in layout.Controls)
            {
                if (child is FlowLayoutPanel nested) { nested.MaximumSize = new Size(available, 0); nested.MinimumSize = new Size(available, 0); nested.Width = available; }
                else if (child is Label label)
                {
                    label.MaximumSize = new Size(available - label.Margin.Horizontal, 0);
                    if (label.Tag as string == "center") { label.AutoSize = false; label.Width = available - label.Margin.Horizontal; label.Height = TextRenderer.MeasureText(label.Text, label.Font, new Size(label.Width, int.MaxValue), TextFormatFlags.WordBreak).Height + 12; }
                }
                else if (child is CheckBox check) { check.AutoSize = false; check.Width = available; check.Height = TextRenderer.MeasureText(check.Text, check.Font, new Size(available - 54, 0), TextFormatFlags.WordBreak).Height + 12; }
                else if (child.Tag as string == "centerFields")
                {
                    var width = Math.Min(360, available); child.MinimumSize = new Size(width, 0); child.Width = width;
                    child.Margin = new Padding((available - width) / 2, child.Margin.Top, 0, child.Margin.Bottom);
                }
                else if (child is Button or TextBox or ComboBox or TableLayoutPanel || child.Tag as string == "separator") child.Width = available - child.Margin.Horizontal;
            }
        }
        void Watch(Control child) { child.TextChanged += (_,_) => ResizeChildren(); child.FontChanged += (_,_) => ResizeChildren(); }
        foreach (Control child in layout.Controls) Watch(child);
        layout.ControlAdded += (_,e) => { if (e.Control is {} child) Watch(child); ResizeChildren(); };
        layout.Resize += (_, _) => ResizeChildren();
        layout.FontChanged += (_, _) => ResizeChildren();
        layout.HandleCreated += (_, _) => ResizeChildren();
        ResizeChildren();
    }
}


