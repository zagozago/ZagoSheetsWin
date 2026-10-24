using System.Drawing.Drawing2D;
using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal enum UiIcon { Folder, Settings, Backup, Help, External, Restore, File, Check, Warning, SelectAll, ClearSelection, Trash, More, Bulb }

// Drawn locally: consistent strokes, no icon font or external asset dependency.
internal static class VisualStyle
{
    internal static Color Border => SystemInformation.HighContrast ? SystemColors.WindowText : Branding.Current == ApplicationTheme.Dark ? Color.FromArgb(49, 73, 60) : Color.FromArgb(205, 226, 215);
    internal static void Icon(Button button, UiIcon icon)
    {
        void Refresh()
        {
            if (button.IsDisposed) return;
            var size = Math.Max(16, 16 * button.DeviceDpi / 96);
            var old = button.Image; var bitmap = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias; g.ScaleTransform(size / 20f, size / 20f);
                using var p = new Pen(button.ForeColor, 1.5f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
                switch (icon)
                {
                    case UiIcon.Folder: using (var fill = new SolidBrush(button.ForeColor)) g.FillPolygon(fill, [new(2, 6), new(8, 6), new(10, 8), new(18, 8), new(16, 17), new(2, 17)]); g.DrawPolygon(p, [new(2, 6), new(8, 6), new(10, 8), new(18, 8), new(16, 17), new(2, 17)]); g.DrawLines(p, [new(2, 14), new(2, 4), new(7, 4), new(9, 6), new(17, 6)]); break;
                    case UiIcon.SelectAll: g.DrawRectangle(p, 2, 2, 12, 12); g.DrawRectangle(p, 6, 6, 12, 12); g.DrawLines(p, [new(8, 11), new(11, 14), new(16, 8)]); break;
                    case UiIcon.ClearSelection: g.DrawRectangle(p, 3, 3, 14, 14); break;
                    case UiIcon.Trash: g.DrawLine(p, 3, 5, 17, 5); g.DrawLines(p, [new(7, 5), new(7, 2), new(13, 2), new(13, 5)]); g.DrawPolygon(p, [new(5,5),new(6,18),new(14,18),new(15,5)]); g.DrawLine(p, 8, 8, 8, 15); g.DrawLine(p, 12, 8, 12, 15); break;
                    case UiIcon.More: for(var y=4;y<=16;y+=6) { using var fill = new SolidBrush(button.ForeColor); g.FillEllipse(fill, 9, y-1, 2.5f, 2.5f); } break;
                    case UiIcon.Bulb: g.DrawArc(p, 5, 2, 10, 12, 145, 250); g.DrawLines(p,[new(6,12),new(8,15),new(12,15),new(14,12)]); g.DrawLine(p, 8, 18, 12, 18); break;
                    case UiIcon.Settings:
                        var gear = new List<PointF>();
                        for (var i = 0; i < 32; i++) { var a = (i + .5) * Math.PI / 16; var r = i % 4 is 0 or 3 ? 6f : 8.5f; gear.Add(new(10 + r * (float)Math.Cos(a), 10 + r * (float)Math.Sin(a))); }
                        g.DrawPolygon(p, gear.ToArray()); g.DrawEllipse(p, 7, 7, 6, 6); break;
                    case UiIcon.Backup: g.DrawEllipse(p, 3, 2, 14, 5); g.DrawLine(p, 3, 4, 3, 16); g.DrawLine(p, 17, 4, 17, 16); g.DrawArc(p, 3, 8, 14, 5, 0, 180); g.DrawArc(p, 3, 13, 14, 5, 0, 180); break;
                    case UiIcon.Help: g.DrawEllipse(p, 2, 2, 16, 16); g.DrawArc(p, 7, 5, 6, 6, 190, 250); g.DrawLine(p, 10, 11, 10, 12); g.DrawEllipse(p, 9.6f, 14, .8f, .8f); break;
                    case UiIcon.External: g.DrawLines(p, [new(11, 3), new(17, 3), new(17, 9)]); g.DrawLine(p, 17, 3, 8, 12); g.DrawLines(p, [new(8, 4), new(3, 4), new(3, 17), new(16, 17), new(16, 12)]); break;
                    case UiIcon.Restore: g.DrawLine(p, 10, 2, 10, 12); g.DrawLines(p, [new(6, 8), new(10, 12), new(14, 8)]); g.DrawLines(p, [new(3, 12), new(3, 17), new(17, 17), new(17, 12)]); break;
                    case UiIcon.Check: g.DrawEllipse(p, 2, 2, 16, 16); g.DrawLines(p, [new(6, 10), new(9, 13), new(14, 7)]); break;
                    case UiIcon.Warning: g.DrawEllipse(p, 2, 2, 16, 16); g.DrawLine(p, 10, 6, 10, 11); g.DrawEllipse(p, 9.6f, 14, .8f, .8f); break;
                    default: g.DrawPolygon(p, [new(4, 2), new(12, 2), new(16, 6), new(16, 18), new(4, 18)]); g.DrawLine(p, 7, 10, 13, 10); g.DrawLine(p, 7, 13, 13, 13); break;
                }
            }
            button.Image = bitmap; old?.Dispose();
        }
        button.TextImageRelation = TextImageRelation.ImageBeforeText;
        button.ImageAlign = ContentAlignment.MiddleCenter;
        button.ForeColorChanged += (_, _) => Refresh(); button.DpiChangedAfterParent += (_, _) => Refresh();
        button.Disposed += (_, _) => button.Image?.Dispose(); Refresh();
    }
    internal static void FitWindow(Form form, FlowLayoutPanel content)
    {
        void Fit()
        {
            if (!form.IsHandleCreated || form.WindowState != FormWindowState.Normal) return;
            content.PerformLayout();
            var needed = content.Controls.Cast<Control>().Where(c => c.Visible).Select(c => c.Bottom + c.Margin.Bottom).DefaultIfEmpty(0).Max() + content.Padding.Bottom;
            var maximum = Screen.FromControl(form).WorkingArea.Height - (form.Height - form.ClientSize.Height) - 24;
            if (needed > content.ClientSize.Height) form.ClientSize = new Size(form.ClientSize.Width, Math.Min(maximum, form.ClientSize.Height + needed - content.ClientSize.Height));
        }
        form.Shown += (_, _) => Fit(); form.DpiChanged += (_, _) => form.BeginInvoke((Action)Fit);
    }
    internal static SurfaceCard Card(params Control[] controls)
    {
        var card = new SurfaceCard(); card.Controls.AddRange(controls); Ui.Adapt(card); return card;
    }
}

internal class SurfaceCard : FlowLayoutPanel
{
    internal SurfaceCard()
    {
        DoubleBuffered = true; AutoSize = true; AutoSizeMode = AutoSizeMode.GrowAndShrink;
        FlowDirection = FlowDirection.TopDown; WrapContents = false;
        Padding = new Padding(14, 10, 14, 10); Margin = new Padding(0, 0, 0, 12);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e); e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var pen = new Pen(VisualStyle.Border);
        e.Graphics.DrawRoundedRectangle(pen, new RectangleF(.5f, .5f, Math.Max(1, Width - 1.5f), Math.Max(1, Height - 1.5f)), new SizeF(6, 6));
    }
}

internal sealed class NumberBadge(int number) : Control
{
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using var fill = new SolidBrush(SystemInformation.HighContrast ? SystemColors.Highlight : Color.FromArgb(25, 134, 74));
        var circle = new Rectangle(0, 0, Math.Min(Width, Height) - 1, Math.Min(Width, Height) - 1);
        e.Graphics.FillEllipse(fill, circle);
        TextRenderer.DrawText(e.Graphics, number.ToString(), Font, circle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }
}

internal sealed class DisclosureCard : SurfaceCard
{
    internal DisclosureButton Header { get; }
    internal SurfaceCard Body { get; }
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    internal bool Expanded { get; private set; }
    internal DisclosureCard(string key, UiIcon icon, params Control[] content)
    {
        Padding = new Padding(8); Body = VisualStyle.Card(content); Body.Padding = new Padding(6, 8, 6, 6); Body.Margin = Padding.Empty; Body.Visible = false;
        Header = new DisclosureButton { Text = UiText.Get(key), AutoSize = true, Padding = new Padding(4, 4, 20, 4), Font = new Font(LanguageVisuals.Family, 10, FontStyle.Bold), AccessibleName = UiText.Get(key) };
        VisualStyle.Icon(Header, icon); Header.Click += (_, _) => SetExpanded(!Expanded);
        Controls.Add(Header); Controls.Add(Body); Ui.Adapt(this);
    }
    internal void SetExpanded(bool value) { Expanded = value; Body.Visible = value; Header.Expanded = value; Header.Invalidate(); PerformLayout(); }
}

internal sealed class ConnectionStatusLabel : Label
{
    protected override void OnPaint(PaintEventArgs e)
    {
        if (!Text.StartsWith("✓", StringComparison.Ordinal)) { base.OnPaint(e); return; }
        e.Graphics.Clear(BackColor); e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var message = Text.TrimStart('✓', ' '); var diameter = 20 * DeviceDpi / 96; var gap = 8 * DeviceDpi / 96;
        var size = TextRenderer.MeasureText(message, Font, new Size(Math.Max(80, Width - Padding.Horizontal - diameter - gap), int.MaxValue), TextFormatFlags.WordBreak);
        var x = Math.Max(Padding.Left, (Width - size.Width - diameter - gap) / 2); var rtl = RightToLeft == RightToLeft.Yes;
        var circleX = rtl ? x + size.Width + gap : x;
        using var fill = new SolidBrush(SystemInformation.HighContrast ? SystemColors.Highlight : Color.FromArgb(16, 108, 60));
        var circle = new Rectangle(circleX, (Height - diameter) / 2, diameter, diameter); e.Graphics.FillEllipse(fill, circle);
        TextRenderer.DrawText(e.Graphics, "✓", Font, circle, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        TextRenderer.DrawText(e.Graphics, message, Font, new Rectangle(rtl ? x : x + diameter + gap, (Height - size.Height) / 2, size.Width, size.Height), ForeColor,
            TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | (rtl ? TextFormatFlags.RightToLeft : 0));
    }
}

internal sealed class ExternalLinkLabel : LinkLabel
{
    protected override void OnRightToLeftChanged(EventArgs e) { base.OnRightToLeftChanged(e); Padding = RightToLeft == RightToLeft.Yes ? new Padding(0,0,20,0) : new Padding(20,0,0,0); }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using var pen = new Pen(LinkColor, 1.2f); var scale = DeviceDpi / 96f; var x = RightToLeft == RightToLeft.Yes ? Width - 14 * scale : 1; var y = (Height - 12 * scale) / 2;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.DrawLines(pen, [new PointF(x + 6 * scale,y),new PointF(x + 12 * scale,y),new PointF(x + 12 * scale,y + 6 * scale)]);
        e.Graphics.DrawLine(pen,x + 12 * scale,y,x + 4 * scale,y + 8 * scale);
        e.Graphics.DrawLines(pen,[new PointF(x + 4 * scale,y + 2 * scale),new PointF(x,y + 2 * scale),new PointF(x,y + 12 * scale),new PointF(x + 10 * scale,y + 12 * scale),new PointF(x + 10 * scale,y + 8 * scale)]);
    }
}

internal sealed class DisclosureButton : AdaptiveButton
{
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    internal bool Expanded { get; set; }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e); var x = RightToLeft == RightToLeft.Yes ? 7 : Width - 13; var y = Height / 2;
        using var p = new Pen(ForeColor, 1.5f);
        e.Graphics.DrawLines(p, (Point[])(Expanded ? [new Point(x-3,y-2),new Point(x,y+1),new Point(x+3,y-2)] : [new Point(x-2,y-3),new Point(x+1,y),new Point(x-2,y+3)]));
    }
}
