using System.Drawing.Drawing2D;

namespace SheetsWindows.Windows;

// Single sun/moon action button; Space/Enter, Tab and native keyboard focus remain available.
internal sealed class ThemeToggle : Button
{
    [System.ComponentModel.DefaultValue(false)]
    public bool Dark { get; set; }
    public ThemeToggle()
    {
        Text = ""; Cursor = Cursors.Hand; AccessibleRole = AccessibleRole.PushButton;
        AccessibleDescription = "Mostra o tema atual: sol para claro, lua para escuro. Clique para mudar; preferência salva neste usuário do Windows.";
        FlatStyle = FlatStyle.Flat; FlatAppearance.BorderSize = 0;
        SetStyle(ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias; g.Clear(BackColor);
        var unit = Math.Min(DeviceDpi / 96f, Math.Min(Width, Height) / 30f);
        var cx = Width / 2f; var cy = Height / 2f;
        using var symbol = new Pen(Enabled ? ForeColor : SystemColors.GrayText, 1.5f * unit) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
        if (Dark)
        {
            // Two intersecting circular arcs outline a crescent, without a filled toggle track.
            using var moon = new GraphicsPath();
            moon.AddArc(cx - 9 * unit, cy - 9 * unit, 18 * unit, 18 * unit, 30.5f, 221.7f);
            moon.AddArc(cx - 4 * unit, cy - 13 * unit, 18 * unit, 18 * unit, 210.5f, -138.3f);
            moon.CloseFigure(); g.DrawPath(symbol, moon);
        }
        else
        {
            var r = 4 * unit; g.DrawEllipse(symbol, cx - r, cy - r, 2 * r, 2 * r);
            for (var i = 0; i < 8; i++)
            {
                var angle = i * Math.PI / 4;
                g.DrawLine(symbol, cx + (float)Math.Cos(angle) * 7 * unit, cy + (float)Math.Sin(angle) * 7 * unit,
                    cx + (float)Math.Cos(angle) * 10 * unit, cy + (float)Math.Sin(angle) * 10 * unit);
            }
        }
        if (Focused && ShowFocusCues) ControlPaint.DrawFocusRectangle(g, new Rectangle(0, 0, Width - 1, Height - 1), ForeColor, BackColor);
    }
    protected override AccessibleObject CreateAccessibilityInstance() => new ThemeAccessibility(this);
    private sealed class ThemeAccessibility(ThemeToggle owner) : ControlAccessibleObject(owner)
    {
        public override string DefaultAction => "Alternar tema";
        public override void DoDefaultAction() => owner.PerformClick();
        public override string? Value { get => owner.Dark ? "Escuro" : "Claro"; set { } }
    }
}
