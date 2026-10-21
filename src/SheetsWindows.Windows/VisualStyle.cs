using System.Drawing.Drawing2D;
using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal enum UiIcon { Folder, Settings, Backup, Help, External, Restore, File, Check, Warning }

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
                    case UiIcon.Folder: g.DrawPolygon(p, [new(2, 6), new(8, 6), new(10, 8), new(18, 8), new(16, 17), new(2, 17)]); g.DrawLines(p, [new(2, 14), new(2, 4), new(7, 4), new(9, 6), new(17, 6)]); break;
                    case UiIcon.Settings:
                        g.DrawEllipse(p, 5, 5, 10, 10); g.DrawEllipse(p, 8, 8, 4, 4);
                        for (var i = 0; i < 8; i++) { var a = i * Math.PI / 4; g.DrawLine(p, 10 + 5 * (float)Math.Cos(a), 10 + 5 * (float)Math.Sin(a), 10 + 8 * (float)Math.Cos(a), 10 + 8 * (float)Math.Sin(a)); } break;
                    case UiIcon.Backup: g.DrawArc(p, 3, 3, 14, 14, 215, 300); g.DrawLines(p, [new(2, 3), new(2, 8), new(7, 8)]); g.DrawLines(p, [new(10, 6), new(10, 10), new(13, 12)]); break;
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
    internal static SurfaceCard Card(params Control[] controls)
    {
        var card = new SurfaceCard(); card.Controls.AddRange(controls); Ui.Adapt(card); return card;
    }
}

internal sealed class SurfaceCard : FlowLayoutPanel
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
