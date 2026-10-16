using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal sealed class InformationButton : Button
{
    private readonly Func<FlowLayoutPanel> build;
    private readonly bool hover;
    private ToolStripDropDown? popup;
    private FlowLayoutPanel? body;
    private readonly System.Windows.Forms.Timer delay = new() { Interval = 250 };
    internal bool PopupVisible => popup?.Visible == true;
    internal FlowLayoutPanel? PopupContent => body;
    internal InformationButton(Func<FlowLayoutPanel> build, bool hover = true)
    {
        this.build = build; this.hover = hover;
        Text = ""; AccessibleName = UiText.Get("home.help"); Size = new Size(22, 22); Margin = new Padding(2, 2, 0, 0); Padding = Padding.Empty;
        FlatStyle = FlatStyle.Flat; FlatAppearance.BorderSize = 0; Cursor = Cursors.Hand;
        Click += (_, _) => { if (PopupVisible) popup!.Close(); else OpenInformation(); };
        MouseEnter += (_, _) => { if (this.hover) delay.Start(); }; MouseLeave += (_, _) => delay.Stop();
        delay.Tick += (_, _) => { delay.Stop(); if (Enabled) OpenInformation(); };
    }
    internal void OpenInformation()
    {
        if (PopupVisible) return;
        if (popup is null)
        {
            body = build(); body.AutoScroll = true; body.Padding = new Padding(10); body.Width = 360;
            body.FlowDirection = FlowDirection.TopDown; body.WrapContents = false;
            var close = new AdaptiveButton { AutoSize = true, Text = UiText.Get("action.close") }; close.Click += (_, _) => popup!.Close(); body.Controls.Add(close);
            popup = new ToolStripDropDown { AutoClose = true, Padding = Padding.Empty };
            popup.Items.Add(new ToolStripControlHost(body) { AutoSize = false, Padding = Padding.Empty, Margin = Padding.Empty });
            LocalizedControls.Attach(body); Ui.Adapt(body);
        }
        var screen = Screen.FromControl(this).WorkingArea;
        body!.Width = Math.Min(380 * DeviceDpi / 96, screen.Width - 24); body.Font = new Font(Font.FontFamily, Math.Max(8, Font.SizeInPoints - 2)); body.RightToLeft = RightToLeft;
        void Style(Control control)
        {
            control.BackColor = BackColor; control.ForeColor = ForeColor; control.Font = control is AdaptiveButton ? new Font(body.Font.FontFamily, Ui.ButtonFontSize, control.Font.Style) : body.Font; control.RightToLeft = RightToLeft;
            if (control.Tag as string == "separator") control.BackColor = ForeColor;
            if (control is LinkLabel link) link.LinkColor = ForeColor;
            foreach (Control child in control.Controls) Style(child);
        }
        Style(body); body.PerformLayout();
        body.Height = Math.Min(Math.Min(screen.Height - 40, 420 * DeviceDpi / 96), body.Controls.Cast<Control>().Sum(c => c.Height + c.Margin.Vertical) + body.Padding.Vertical + 8);
        popup!.Items[0].Size = body.Size;
        var position = PointToScreen(new Point(0, Height));
        position.X = Math.Clamp(position.X, screen.Left + 4, Math.Max(screen.Left + 4, screen.Right - body.Width - 8));
        position.Y = Math.Clamp(position.Y, screen.Top + 4, Math.Max(screen.Top + 4, screen.Bottom - body.Height - 8));
        popup.Show(this, PointToClient(position)); body.SelectNextControl(null, true, true, true, false);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent?.BackColor ?? BackColor);
        // Use the single circled-information glyph, independent of the UI language.
        using var symbolFont = new Font("Segoe UI Symbol", 9, FontStyle.Regular);
        TextRenderer.DrawText(e.Graphics, "ⓘ", symbolFont, ClientRectangle, ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
        if (Focused && ShowFocusCues) ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle, ForeColor, BackColor);
    }
    protected override void Dispose(bool disposing) { if (disposing) { delay.Dispose(); popup?.Dispose(); } base.Dispose(disposing); }
    internal static FlowLayoutPanel TextPanel(params string[] keys)
    {
        var panel = new FlowLayoutPanel { AutoSize = false };
        foreach (var key in keys) panel.Controls.Add(Ui.Text(UiText.Get(key)));
        return panel;
    }
    internal static void AddLegalLinks(FlowLayoutPanel panel)
    {
        foreach (var (key, url) in new[] { ("legal.privacy", "https://zagotools.top/legal.html#privacidade"), ("legal.terms", "https://zagotools.top/legal.html#termos") })
        {
            var link = new LinkLabel { AutoSize = true, Text = UiText.Get(key) };
            link.LinkClicked += (_, _) => { try { new BrowserLauncher().Open(new Uri(url)); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { MessageBox.Show(url, link.Text); } };
            panel.Controls.Add(link);
        }
    }
}
