using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal static class Branding
{
    public const string Name = "ZagoSheetsWin";
    public static string Credit => UiText.Get("about.credit");
    private static ApplicationTheme? selected;
    private static event Action? ThemeChanged;
    internal static ApplicationTheme Current
    {
        get
        {
            if (selected is null) { try { selected = ThemeSettings.Load(LocalStorage.ForCurrentUser()); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { selected = ApplicationTheme.Light; } }
            return selected.Value;
        }
    }
    internal static void PreviewTheme(ApplicationTheme theme) { selected = theme; ThemeChanged?.Invoke(); }
    internal static void Refresh(Form form) => ThemeChanged?.Invoke();
    public static void Apply(Form form, bool aboutButton = true, bool compact = false, bool languageButton = true)
    {
        using (var stream = typeof(Branding).Assembly.GetManifestResourceStream("Brand.icon.ico")!) form.Icon = new Icon(stream);
        form.Font = new Font("Segoe UI", 10);
        void Theme(Control control)
        {
            if (control is AdaptiveButton action && action.Font.SizeInPoints != Ui.ButtonFontSize)
                action.Font = new Font(action.Font.FontFamily, Ui.ButtonFontSize, action.Font.Style);
            var highContrast = SystemInformation.HighContrast;
            var dark = Current == ApplicationTheme.Dark;
            var background = highContrast ? SystemColors.Window : dark ? Color.FromArgb(7, 16, 11) : Color.FromArgb(243, 249, 246);
            var panel = highContrast ? SystemColors.Window : dark ? Color.FromArgb(13, 28, 19) : Color.White;
            var foreground = highContrast ? SystemColors.WindowText : dark ? Color.FromArgb(237, 248, 240) : Color.FromArgb(23, 49, 38);
            var green = highContrast ? SystemColors.Highlight : dark ? Color.FromArgb(98, 217, 139) : Color.FromArgb(24, 128, 75);
            var inherited = control.Parent?.BackColor ?? background;
            control.BackColor = control is Form ? background : control is SurfaceCard ? panel
                : control is ThemeToggle or InformationButton ? inherited
                : control is Button or TextBox or ComboBox or ListBox ? panel : inherited;
            if (control.Tag as string == "status") control.BackColor = highContrast ? panel : dark ? Color.FromArgb(22, 49, 35) : Color.FromArgb(230, 244, 236);
            if (control.Tag as string == "errorSurface") control.BackColor = highContrast ? panel : dark ? Color.FromArgb(49, 27, 29) : Color.FromArgb(255, 245, 245);
            control.ForeColor = foreground;
            if (control is Button button) { button.FlatStyle = highContrast ? FlatStyle.System : FlatStyle.Flat; button.FlatAppearance.BorderColor = green; button.UseVisualStyleBackColor = false;
                button.FlatAppearance.MouseOverBackColor = highContrast ? SystemColors.Highlight : dark ? Color.FromArgb(18, 37, 26) : Color.FromArgb(244, 250, 246);
                button.FlatAppearance.MouseDownBackColor = button.FlatAppearance.MouseOverBackColor; }
            if (control is LinkLabel link) { link.LinkColor = green; link.ActiveLinkColor = foreground; link.VisitedLinkColor = green; }
            foreach (Control child in control.Controls) Theme(child);
            if (control.Tag as string == "separator") control.BackColor = highContrast ? SystemColors.WindowText : dark ? Color.FromArgb(61, 79, 68) : Color.FromArgb(211, 225, 216);
            if (control is Button primary && primary.Tag as string == "primary" && !highContrast)
            {
                primary.BackColor = Color.FromArgb(18, 126, 69); primary.ForeColor = Color.White;
                primary.FlatAppearance.MouseOverBackColor = Color.FromArgb(16, 108, 60);
                if (!primary.Font.Bold || primary.Font.SizeInPoints != Ui.ButtonFontSize) primary.Font = new Font(primary.Font.FontFamily, Ui.ButtonFontSize, FontStyle.Bold);
            }
            if (control is Button disabled && !disabled.Enabled)
            {
                if (dark && !highContrast) disabled.BackColor = Color.FromArgb(26, 43, 33);
                // Blend with the actual surface, including primary buttons. High contrast follows Windows.
                var surface = disabled.BackColor; var text = disabled.ForeColor;
                disabled.ForeColor = highContrast ? SystemColors.GrayText : dark ? Color.FromArgb(178, 195, 184) : Color.FromArgb(
                    (text.R + surface.R) / 2, (text.G + surface.G) / 2, (text.B + surface.B) / 2);
                disabled.FlatAppearance.BorderColor = highContrast ? SystemColors.GrayText : Color.FromArgb(
                    (green.R + surface.R) / 2, (green.G + surface.G) / 2, (green.B + surface.B) / 2);
            }
            if (control is ListView list)
            {
                list.BackColor = panel; list.ForeColor = foreground;
                for (var row = 0; row < list.Items.Count; row++)
                {
                    var item = list.Items[row];
                    item.BackColor = row % 2 == 0 || highContrast ? panel : dark ? Color.FromArgb(17, 34, 24) : Color.FromArgb(247, 251, 249); item.ForeColor = foreground;
                    if (item.SubItems.Count > 3) { item.UseItemStyleForSubItems = false; item.SubItems[3].ForeColor = item.Tag is RecoveryEntry { CanClean: true } ? green : foreground; }
                }
            }
            control.Invalidate();
        }
        var header = new Panel { Dock = DockStyle.Top, Height = 64, Padding = new Padding(18, 8, 18, 8) };
        using var source = typeof(Branding).Assembly.GetManifestResourceStream("Brand.z.png")!;
        using var original = Image.FromStream(source); var logo = new Bitmap(original);
        var picture = new PictureBox { Image = logo, SizeMode = PictureBoxSizeMode.Zoom, Dock = DockStyle.Left, Width = 32 };
        form.Disposed += (_, _) => logo.Dispose();
        var title = new Label { Text = Name, AutoSize = true, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 0, 0) };
        title.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        var right = new FlowLayoutPanel { Dock = DockStyle.Right, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 8, 0, 0) };
        var toggle = new ThemeToggle { Width = 32, Height = 28, Margin = new Padding(3,0,3,0) };
        var tooltip = new ToolTip();
        void UpdateToggle() { toggle.Dark = Current == ApplicationTheme.Dark; toggle.AccessibleName = toggle.Dark ? UiText.Get("theme.activateLight") : UiText.Get("theme.activateDark"); tooltip.SetToolTip(toggle, toggle.AccessibleName); toggle.Invalidate(); }
        toggle.Click += async (_, _) =>
        {
            toggle.Enabled = false;
            try { var next = Current == ApplicationTheme.Dark ? ApplicationTheme.Light : ApplicationTheme.Dark; await ThemeSettings.SaveAsync(LocalStorage.ForCurrentUser(), next); selected = next; ThemeChanged?.Invoke(); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { MessageBox.Show(form, UiText.Get("theme.saveFailed"), Name, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            finally { if (!toggle.IsDisposed) toggle.Enabled = true; }
        };
        if (aboutButton)
        {
            var about = new LinkLabel { Text = UiText.Get("about.open"), AutoSize = true, Height = 32, Margin = new Padding(4, 5, 8, 0), TextAlign = ContentAlignment.MiddleCenter, TabStop = true, AccessibleName = UiText.Get("accessibility.about") };
            about.LinkClicked += (_, _) => { using var info = new AboutForm(); info.ShowDialog(form); }; right.Controls.Add(about);
        }
        if (languageButton)
        {
            var language = new AdaptiveButton { Text = LanguageSettings.Details(UiText.Language).DisplayCode, AutoSize = false, Width = 48, Height = 28, Padding = new Padding(6,1,6,1), AccessibleName = UiText.Get("language.button"), Margin = new Padding(3,0,3,0) };
            language.Click += (_,_) => { using var picker = new LanguagePicker(); if (picker.ShowDialog(form) == DialogResult.OK && !form.IsDisposed)
                { LanguageVisuals.RestoreAfterSelection(form); } };
            void UpdateLanguage() { language.Text = LanguageSettings.Details(UiText.Language).DisplayCode; tooltip.SetToolTip(language, UiText.Get("language.button")); UpdateToggle(); }
            UiText.Changed += UpdateLanguage; form.Disposed += (_,_) => UiText.Changed -= UpdateLanguage;
            tooltip.SetToolTip(language, UiText.Get("language.button"));
            right.Controls.Add(language);
        }
        var brand = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 8, 0, 0) };
        title.Margin = new Padding(0, 2, 0, 0);
        brand.Controls.Add(title); brand.Controls.Add(new InformationButton(() => InformationButton.TextPanel("home.description"), hover: false) { Margin = new Padding(5, 4, 0, 0), Size = new Size(20, 20) });
        right.Controls.Add(toggle); header.Controls.Add(brand); header.Controls.Add(picture); header.Controls.Add(right);
        form.Controls.Add(header); header.SendToBack();
        void FitHeader()
        {
            // Minimized client bounds must never become the restored window bounds.
            if (form.WindowState != FormWindowState.Normal) return;
            var required = header.Padding.Horizontal + picture.Width + title.PreferredSize.Width + 26 + right.PreferredSize.Width;
            if (form.ClientSize.Width < required) form.ClientSize = new Size(required, form.ClientSize.Height);
        }
        header.Layout += (_, _) => FitHeader(); form.Shown += (_, _) => FitHeader();
        void RefreshTheme() { if (!form.IsDisposed) { Theme(form); UpdateToggle(); } }
        Action changed = RefreshTheme; ThemeChanged += changed;
        Microsoft.Win32.UserPreferenceChangedEventHandler preference = (_, _) => { if (form.IsHandleCreated && !form.IsDisposed) { try { form.BeginInvoke((Action)RefreshTheme); } catch (InvalidOperationException) { } } };
        Microsoft.Win32.SystemEvents.UserPreferenceChanged += preference;
        form.Disposed += (_, _) => { ThemeChanged -= changed; Microsoft.Win32.SystemEvents.UserPreferenceChanged -= preference; tooltip.Dispose(); };
        void TrackEnabled(Control control)
        {
            if (control is Button) control.EnabledChanged += (_, _) => { if (!control.IsDisposed) Theme(control); };
            control.ControlAdded += (_, e) => { if (e.Control is { } added) { TrackEnabled(added); Theme(added); } };
            foreach (Control child in control.Controls) TrackEnabled(child);
        }
        TrackEnabled(form);
        LanguageVisuals.Attach(form);
        LocalizedControls.Attach(form);
        RefreshTheme();
    }
}
internal sealed class AboutForm : Form
{
    public AboutForm()
    {
        Text = UiText.Get("about.title"); ClientSize = new Size(880, 580); MinimumSize = new Size(740, 540); AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterParent;
        var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2, Padding = new Padding(18, 2, 18, 18) };
        body.ColumnStyles.Add(new(SizeType.Percent, 42)); body.ColumnStyles.Add(new(SizeType.Percent, 58));
        body.RowStyles.Add(new(SizeType.Percent, 57)); body.RowStyles.Add(new(SizeType.Percent, 43));
        var about = VisualStyle.Card(Ui.Text(UiText.Get("about.open"), true), Ui.Text(UiText.Format("about.description", ("version", UiText.ProductVersion))));
        var links = VisualStyle.Card();
        var licenseCard = VisualStyle.Card(Ui.Text("MIT License", true));
        var dependencies = VisualStyle.Card(Ui.Text(UiText.Get("about.dependencies")));
        foreach (var card in new[] { about, links, licenseCard, dependencies }) { card.Dock = DockStyle.Fill; card.AutoSize = false; card.AutoScroll = true; card.Margin = new Padding(0, 0, 10, 10); }
        body.Controls.Add(about, 0, 0); body.Controls.Add(links, 0, 1); body.Controls.Add(licenseCard, 1, 0); body.Controls.Add(dependencies, 1, 1);
        foreach (var item in new[] { (UiText.Get("about.website"), "https://zagotools.top/"), (UiText.Get("about.licensesWebsite"), "https://zagotools.top/legal.html#licencas"), (UiText.Get("about.upstream"), "https://github.com/SwatiK425/open-in-google/"), (UiText.Get("about.upstreamAuthor"), "https://github.com/SwatiK425"), (UiText.Get("about.sourceCode"), "https://github.com/zagozago/ZagoSheetsWin"), (UiText.Get("legal.privacy"), "https://zagotools.top/legal.html#privacidade"), (UiText.Get("legal.terms"), "https://zagotools.top/legal.html#termos") })
        {
            var link = new LinkLabel { Text = item.Item1, AutoSize = true, Margin = new Padding(0, 4, 0, 4) };
            link.LinkClicked += (_, _) => { try { new BrowserLauncher().Open(new Uri(item.Item2)); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { MessageBox.Show(item.Item2, UiText.Get("about.linkDialog")); } };
            links.Controls.Add(link);
        }
        var license = Path.Combine(AppContext.BaseDirectory, "LICENSE");
        var licenseText = new TextBox { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, BorderStyle = BorderStyle.None, Text = File.ReadAllText(license), Width = 400 };
        licenseCard.Controls.Add(licenseText);
        void FitLicense() => licenseText.Height = Math.Max(80, licenseCard.ClientSize.Height - licenseCard.Padding.Vertical - licenseCard.Controls[0].Height - 24);
        licenseCard.SizeChanged += (_, _) => FitLicense(); FitLicense();
        Controls.Add(body); Branding.Apply(this, aboutButton: false);
    }
}

