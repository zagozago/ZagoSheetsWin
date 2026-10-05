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
            var highContrast = SystemInformation.HighContrast;
            var dark = Current == ApplicationTheme.Dark;
            var background = highContrast ? SystemColors.Window : dark ? Color.FromArgb(7, 16, 11) : Color.FromArgb(238, 247, 240);
            var panel = highContrast ? SystemColors.Window : dark ? Color.FromArgb(13, 28, 19) : Color.White;
            var foreground = highContrast ? SystemColors.WindowText : dark ? Color.FromArgb(237, 248, 240) : Color.FromArgb(23, 49, 38);
            var green = highContrast ? SystemColors.Highlight : dark ? Color.FromArgb(98, 217, 139) : Color.FromArgb(25, 134, 74);
            control.BackColor = control is ThemeToggle ? background : control is Button or TextBox or ComboBox or ListBox ? panel : background;
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
                if (!primary.Font.Bold) primary.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            }
            if (control is ListView list)
            {
                list.BackColor = panel; list.ForeColor = foreground;
                foreach (ListViewItem item in list.Items) { item.BackColor = panel; item.ForeColor = foreground; }
            }
            control.Invalidate();
        }
        var header = new Panel { Dock = DockStyle.Top, Height = compact ? 60 : 70, Padding = new Padding(12, 8, 12, 8) };
        using var source = typeof(Branding).Assembly.GetManifestResourceStream("Brand.z.png")!;
        using var original = Image.FromStream(source); var logo = new Bitmap(original);
        var picture = new PictureBox { Image = logo, SizeMode = PictureBoxSizeMode.Zoom, Dock = DockStyle.Left, Width = 36 };
        form.Disposed += (_, _) => logo.Dispose();
        var title = new Label { Text = Name, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 0, 0) };
        title.Font = new Font("Segoe UI", 13, FontStyle.Bold);
        var right = new FlowLayoutPanel { Dock = DockStyle.Right, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 5, 0, 0) };
        var toggle = new ThemeToggle { Width = 36, Height = 34 };
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
            var about = new LinkLabel { Text = UiText.Get("about.open"), AutoSize = true, Height = 32, TextAlign = ContentAlignment.MiddleCenter, TabStop = true, AccessibleName = UiText.Get("accessibility.about") };
            about.LinkClicked += (_, _) => { using var info = new AboutForm(); info.ShowDialog(form); }; right.Controls.Add(about);
        }
        if (languageButton)
        {
            var language = new AdaptiveButton { Text = UiText.Language.ToUpperInvariant(), AutoSize = true, AccessibleName = UiText.Get("language.button"), Margin = new Padding(3,0,3,0) };
            language.Click += (_,_) => { using var picker = new LanguagePicker(); picker.ShowDialog(form); };
            void UpdateLanguage() { language.Text = UiText.Language.ToUpperInvariant(); tooltip.SetToolTip(language, UiText.Get("language.button")); UpdateToggle(); }
            UiText.Changed += UpdateLanguage; form.Disposed += (_,_) => UiText.Changed -= UpdateLanguage;
            tooltip.SetToolTip(language, UiText.Get("language.button"));
            right.Controls.Add(language);
        }
        right.Controls.Add(toggle); header.Controls.Add(title); header.Controls.Add(picture); header.Controls.Add(right);
        form.Controls.Add(header); header.SendToBack();
        void RefreshTheme() { if (!form.IsDisposed) { Theme(form); UpdateToggle(); } }
        Action changed = RefreshTheme; ThemeChanged += changed;
        Microsoft.Win32.UserPreferenceChangedEventHandler preference = (_, _) => { if (form.IsHandleCreated && !form.IsDisposed) { try { form.BeginInvoke((Action)RefreshTheme); } catch (InvalidOperationException) { } } };
        Microsoft.Win32.SystemEvents.UserPreferenceChanged += preference;
        form.Disposed += (_, _) => { ThemeChanged -= changed; Microsoft.Win32.SystemEvents.UserPreferenceChanged -= preference; tooltip.Dispose(); };
        LocalizedControls.Attach(form);
        RefreshTheme();
    }
}
internal sealed class AboutForm : Form
{
    public AboutForm()
    {
        Text = UiText.Get("about.title"); ClientSize = new Size(810, 520); MinimumSize = new Size(600, 400); AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterParent;
        var body = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, Padding = new Padding(18), AutoScroll = true };
        body.Controls.Add(new EmphasisLabel { AutoSize = true, MaximumSize = new Size(740, 0), Text = UiText.Format("about.description", ("version", UiText.ProductVersion)) });
        foreach (var item in new[] { (UiText.Get("about.website"), "https://zagotools.top/"), (UiText.Get("about.licensesWebsite"), "https://zagotools.top/legal.html#licencas"), (UiText.Get("about.upstream"), "https://github.com/SwatiK425/open-in-google/"), (UiText.Get("about.upstreamAuthor"), "https://github.com/SwatiK425"), (UiText.Get("about.sourceCode"), "https://github.com/zagozago/ZagoSheetsWin"), (UiText.Get("legal.privacy"), "https://zagotools.top/legal.html#privacidade"), (UiText.Get("legal.terms"), "https://zagotools.top/legal.html#termos") })
        {
            var link = new LinkLabel { Text = item.Item1, AutoSize = true, Margin = new Padding(0, 8, 0, 8) };
            link.LinkClicked += (_, _) => { try { new BrowserLauncher().Open(new Uri(item.Item2)); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { MessageBox.Show(item.Item2, UiText.Get("about.linkDialog")); } };
            body.Controls.Add(link);
        }
        var license = Path.Combine(AppContext.BaseDirectory, "LICENSE");
        body.Controls.Add(new TextBox { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Width = 740, Height = 170, Text = File.ReadAllText(license) });
        body.Controls.Add(new EmphasisLabel { Text = UiText.Get("about.dependencies"), AutoSize = true, MaximumSize = new Size(740, 0) });
        Controls.Add(body); Branding.Apply(this, aboutButton: false);
    }
}
