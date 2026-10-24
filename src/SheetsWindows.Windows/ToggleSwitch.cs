using System.Drawing.Drawing2D;

namespace SheetsWindows.Windows;

// Retains native checkbox keyboard/UIA state while drawing a switch in both themes.
internal sealed class ToggleSwitch : CheckBox
{
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    internal bool CenterGroup { get; init; }
    public ToggleSwitch()
    {
        SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        Cursor = Cursors.Hand; AutoSize = false; MinimumSize = new Size(48, 28);
    }
    public override Size GetPreferredSize(Size proposedSize)
    {
        var width = proposedSize.Width > 60 ? proposedSize.Width : Math.Max(180, Width);
        var text = TextRenderer.MeasureText(Text, Font, new Size(Math.Max(40, width - 54), int.MaxValue), TextFormatFlags.WordBreak);
        return new Size(width, Math.Max(28, text.Height + 10));
    }
    internal static void Draw(Graphics g, Rectangle bounds, bool on, Color background, Color foreground, bool enabled = true)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var color = enabled ? on ? Color.FromArgb(18, 126, 69) : Color.FromArgb(114, 131, 121) : SystemColors.GrayText;
        using var track = new SolidBrush(color); using var outline = new Pen(foreground, 1);
        using var shape = new GraphicsPath(); var h = bounds.Height;
        shape.AddArc(bounds.X, bounds.Y, h, h, 90, 180); shape.AddArc(bounds.Right - h, bounds.Y, h, h, 270, 180); shape.CloseFigure();
        g.FillPath(track, shape); if (SystemInformation.HighContrast) g.DrawPath(outline, shape);
        using var knob = new SolidBrush(Color.White);
        var diameter = h - 6; g.FillEllipse(knob, on ? bounds.Right - diameter - 3 : bounds.Left + 3, bounds.Top + 3, diameter, diameter);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(BackColor); var scale = DeviceDpi / 96f;
        var trackWidth = (int)(38 * scale); var trackHeight = (int)(20 * scale); var gap = (int)(10 * scale);
        var rtl = RightToLeft == RightToLeft.Yes;
        var textWidth = Math.Min(Math.Max(1, Width - trackWidth - gap - 4), TextRenderer.MeasureText(Text, Font, new Size(Math.Max(1, Width - trackWidth - gap - 4), int.MaxValue), TextFormatFlags.WordBreak).Width);
        var inset = CenterGroup ? Math.Max(2, (Width - textWidth - trackWidth - gap) / 2) : 2;
        Draw(e.Graphics, new Rectangle(rtl ? Width - trackWidth - inset : inset, (Height - trackHeight) / 2, trackWidth, trackHeight), Checked, BackColor, ForeColor, Enabled);
        var text = new Rectangle(rtl ? inset : inset + trackWidth + gap, 2, CenterGroup ? textWidth : Math.Max(1, Width - trackWidth - gap - 2), Height - 4);
        TextRenderer.DrawText(e.Graphics, Text, Font, text, Enabled ? ForeColor : SystemColors.GrayText,
            TextFormatFlags.WordBreak | TextFormatFlags.VerticalCenter | (rtl ? TextFormatFlags.RightToLeft | TextFormatFlags.Right : 0));
        if (Focused && ShowFocusCues) ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle, ForeColor, BackColor);
    }
}

internal sealed class ToggleListView : ListView
{
    private ImageList? switches;
    private List<Bitmap> switchImages = [];
    protected override void OnHandleCreated(EventArgs e) { base.OnHandleCreated(e); RefreshSwitches(); }
    protected override void OnBackColorChanged(EventArgs e) { base.OnBackColorChanged(e); if (IsHandleCreated) RefreshSwitches(); }
    private void RefreshSwitches()
    {
        var old = switches; var oldImages = switchImages; switchImages = [];
        switches = new ImageList { ImageSize = new Size(38, 22), ColorDepth = ColorDepth.Depth32Bit };
        foreach (var on in new[] { false, true })
        {
            var image = new Bitmap(38, 22); switchImages.Add(image); using var graphics = Graphics.FromImage(image); graphics.Clear(Color.Transparent);
            ToggleSwitch.Draw(graphics, new Rectangle(1, 2, 36, 18), on, BackColor, ForeColor); switches.Images.Add(image);
        }
        StateImageList = switches; old?.Dispose(); foreach (var image in oldImages) image.Dispose();
    }
    protected override void Dispose(bool disposing) { base.Dispose(disposing); if (disposing) { switches?.Dispose(); foreach (var image in switchImages) image.Dispose(); } }
}
