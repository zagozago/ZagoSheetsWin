using SheetsWindows.Infrastructure;
using SheetsWindows.Core;

namespace SheetsWindows.Windows;

// The post-install configuration reuses the help illustrations and translated copy.
// The help dialog remains independent, including its skip and preference controls.
internal sealed class FirstUseWizard : Form
{
    private readonly LocalStorage storage;
    private readonly bool preview;
    private readonly Func<CancellationToken, Task> authorize;
    private readonly Func<CancellationToken, Task> verify;
    private readonly Func<CancellationToken, Task> save;
    private CancellationTokenSource cancellation = new();
    private readonly TableLayoutPanel layout = new() { Dock = DockStyle.Fill, Padding = new Padding(20), ColumnCount = 1, RowCount = 5 };
    private readonly Label heading = Ui.Text("", true);
    private readonly TutorialPicture picture = new() { Dock = DockStyle.Fill };
    private readonly FlowLayoutPanel content = new() { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
    private readonly FlowLayoutPanel actions = new() { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
    private readonly Label count = new() { AutoSize = true };
    private readonly Label connection = Ui.Text("", true);
    private readonly Label status = Ui.Text("");
    private readonly CheckBox noSync;
    private readonly TextBox folder = new() { ReadOnly = true };
    private readonly Button cancel = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.cancelConnection"), Visible = false };
    private string clientPath = "";
    private bool busy;
    private bool checking;
    internal int CurrentPage { get; private set; }
    internal bool AuthorizationConfirmed { get; private set; }
    internal bool Completed { get; private set; }
    internal Button NextButton { get; } = new AdaptiveButton { AutoSize = true };
    internal Button PreviousButton { get; } = new AdaptiveButton { AutoSize = true, Text = UiText.Get("action.back") };
    internal Button ConnectButton { get; } = new AdaptiveButton { AutoSize = true, Text = UiText.Get("action.authorizeGoogle") };
    internal CheckBox Acknowledgement { get; }

    internal FirstUseWizard(bool preview = false, int initialPage = 0, LocalStorage? storage = null,
        Func<CancellationToken, Task>? authorize = null, Func<CancellationToken, Task>? verify = null,
        Func<CancellationToken, Task>? save = null)
    {
        this.storage = storage ?? LocalStorage.ForCurrentUser(); this.preview = preview;
        this.authorize = authorize ?? Authorize; this.verify = verify ?? Verify; this.save = save ?? Save;
        var opening = OpeningPolicy.Load(this.storage);
        noSync = new CheckBox { AutoSize = true, Text = UiText.Get("setup.noSync"), Checked = !opening.RestrictToFolder };
        folder.Text = opening.Folder ?? "";
        Acknowledgement = new CheckBox { AutoSize = true, Text = UiText.Get("setup.replacementConsent"), Checked = OpeningPolicy.IsConfigured(this.storage) };
        CurrentPage = Math.Clamp(initialPage, 0, 3);
        Text = UiText.Get("setup.firstUseTitle"); ClientSize = new Size(760, 740); MinimumSize = new Size(620, 620);
        StartPosition = FormStartPosition.CenterScreen; AutoScaleMode = AutoScaleMode.Dpi;
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 220));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        heading.Dock = DockStyle.Fill;
        layout.Controls.Add(heading, 0, 0); layout.Controls.Add(picture, 0, 1); layout.Controls.Add(content, 0, 2); layout.Controls.Add(actions, 0, 3);
        actions.Controls.Add(PreviousButton); actions.Controls.Add(NextButton); actions.Controls.Add(count); actions.Controls.Add(cancel);
        var legal = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, WrapContents = true };
        foreach (var (key, url) in new[] { ("legal.privacy", "https://zagotools.top/legal.html#privacidade"), ("legal.terms", "https://zagotools.top/legal.html#termos") })
        {
            var link = new LinkLabel { AutoSize = true, Text = UiText.Get(key), Margin = new Padding(4, 6, 18, 4) };
            link.LinkClicked += (_, _) => { try { new BrowserLauncher().Open(new Uri(url)); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = url; } };
            legal.Controls.Add(link);
        }
        layout.Controls.Add(legal, 0, 4); Controls.Add(layout); Ui.Primary(NextButton);
        PreviousButton.Click += (_, _) => { if (busy || CurrentPage == 0 || CurrentPage == 1 && !AuthorizationConfirmed) return; CurrentPage--; RefreshPage(); };
        NextButton.Click += async (_, _) =>
        {
            if (busy || CurrentPage == 1 && !AuthorizationConfirmed) return;
            if (CurrentPage < 3) { CurrentPage++; RefreshPage(); if (CurrentPage == 1) await CheckExistingConnection(); return; }
            await Run(this.save, authentication: false);
            if (Completed) Close();
        };
        ConnectButton.Click += async (_, _) => await Run(this.authorize, authentication: true);
        cancel.Click += (_, _) => cancellation.Cancel();
        Acknowledgement.CheckedChanged += (_, _) => UpdateNavigation();
        UiText.Changed += RefreshPage; Disposed += (_, _) => UiText.Changed -= RefreshPage;
        FormClosing += (_, e) =>
        {
            if (Completed || preview) return;
            if (busy || CurrentPage == 1 && !AuthorizationConfirmed) { e.Cancel = true; if (!busy) connection.Text = UiText.Get("setup.authMissing"); }
        };
        Branding.Apply(this); Ui.Adapt(content); RefreshPage();
        if (!preview) Shown += async (_, _) => { if (CurrentPage == 1) await CheckExistingConnection(); };
    }
    private static string SectionTitle(string key) => System.Text.RegularExpressions.Regex.Replace(UiText.Get(key), @"^\s*\d+[.、．]?\s*", "");
    private void UpdateNavigation()
    {
        PreviousButton.Enabled = !busy && CurrentPage > 0 && (CurrentPage != 1 || AuthorizationConfirmed);
        NextButton.Enabled = !busy && (CurrentPage != 1 || AuthorizationConfirmed) &&
            (CurrentPage != 3 || !ShortcutSettings.Load(storage) || Acknowledgement.Checked);
        ConnectButton.Enabled = !busy;
    }
    private void RefreshPage()
    {
        // Persistent input controls survive both page navigation and language changes.
        foreach (var persistent in new Control[] { noSync, folder, Acknowledgement, ConnectButton, connection, status }) persistent.Parent?.Controls.Remove(persistent);
        var old = content.Controls.Cast<Control>().ToArray(); content.Controls.Clear(); foreach (var control in old) control.Dispose();
        heading.Text = UiText.Format("tutorial.numberedTitle", ("number", CurrentPage + 1), ("title", TutorialForm.Pages[CurrentPage].Title));
        picture.Page = CurrentPage; picture.AccessibleName = TutorialForm.Pages[CurrentPage].Title; picture.Invalidate();
        layout.RowStyles[1].Height = (CurrentPage == 3 ? 165 : 220) * DeviceDpi / 96f;
        foreach (var paragraph in TutorialForm.Pages[CurrentPage].Text.Split('|')) content.Controls.Add(Ui.Text(paragraph));
        if (CurrentPage == 1)
        {
            connection.Text = UiText.Get(AuthorizationConfirmed ? "setup.authConfirmed" : "setup.authMissing");
            ConnectButton.Text = UiText.Get(AuthorizationConfirmed ? "action.switchGoogleAccount" : "action.authorizeGoogle");
            content.Controls.Add(connection); content.Controls.Add(ConnectButton);
            if (!File.Exists(LauncherConfiguration.ClientPath(storage)) &&
                !typeof(LauncherConfiguration).Assembly.GetManifestResourceNames().Contains("OAuth.official.desktop.json"))
            {
                var choose = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.chooseClient") };
                choose.Click += (_, _) => { using var dialog = new OpenFileDialog { Filter = UiText.Get("dialog.oauthFileFilter"), CheckFileExists = true }; if (dialog.ShowDialog(this) == DialogResult.OK) { clientPath = dialog.FileName; status.Text = UiText.Get("setup.clientSelected"); } };
                content.Controls.Add(choose);
            }
        }
        else if (CurrentPage == 2)
        {
            content.Controls.Add(Ui.Text(SectionTitle("setup.defaultsHeading"), true)); content.Controls.Add(Ui.Text(UiText.Get("setup.defaultsExplanation")));
            var defaults = new AdaptiveButton { AutoSize = true, Text = UiText.Get("tutorial.defineDefaults") }; Ui.Primary(defaults);
            defaults.Click += (_, _) => { try { new BrowserLauncher().Open(WindowsAssociationPlan.DefaultsUri); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = UiText.Get("setup.windowsDefaultsInstructions"); } };
            content.Controls.Add(defaults);
        }
        else if (CurrentPage == 3)
        {
            noSync.Text = UiText.Get("setup.noSync"); Acknowledgement.Text = UiText.Get("setup.replacementConsent");
            content.Controls.Add(Ui.Text(SectionTitle("setup.syncHeading"), true)); content.Controls.Add(noSync);
            var folderOptions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Visible = !noSync.Checked };
            folderOptions.Controls.Add(Ui.Text(UiText.Get("setup.syncExplanation"))); folderOptions.Controls.Add(folder);
            var choose = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.chooseFolder") };
            choose.Click += (_, _) => { using var dialog = new FolderBrowserDialog(); if (dialog.ShowDialog(this) == DialogResult.OK) folder.Text = dialog.SelectedPath; };
            folderOptions.Controls.Add(choose); content.Controls.Add(folderOptions); Ui.Adapt(folderOptions);
            // Remove the page-specific handler when leaving this page.
            EventHandler change = (_, _) => folderOptions.Visible = !noSync.Checked;
            noSync.CheckedChanged += change; folderOptions.Disposed += (_, _) => noSync.CheckedChanged -= change;
            if (ShortcutSettings.Load(storage)) content.Controls.Add(Acknowledgement);
            var backups = new AdaptiveButton { AutoSize = true, Text = UiText.Get("tutorial.openBackups") };
            backups.Click += (_, _) => { using var recovery = new RecoveryForm(preview: preview); recovery.ShowDialog(this); }; content.Controls.Add(backups);
        }
        content.Controls.Add(status);
        NextButton.Text = UiText.Get(CurrentPage == 3 ? "tutorial.start" : "action.next"); PreviousButton.Text = UiText.Get("action.back");
        count.Text = UiText.Format("tutorial.pageCount", ("number", CurrentPage + 1), ("total", 4));
        Branding.Refresh(this); content.PerformLayout(); UpdateNavigation();
    }
    private async Task CheckExistingConnection()
    {
        if (preview || checking || FirstUseState.NeedsAuthorization(storage)) return;
        checking = true; await Run(verify, authentication: true); checking = false;
    }
    private async Task Run(Func<CancellationToken, Task> action, bool authentication)
    {
        if (busy) return;
        busy = true;
        if (authentication) AuthorizationConfirmed = false;
        content.Enabled = false; cancel.Visible = authentication; UpdateNavigation();
        status.Text = UiText.Get(authentication ? "setup.savingAuthorizing" : "setup.saving");
        try
        {
            await action(cancellation.Token);
            if (authentication) { AuthorizationConfirmed = true; connection.Text = UiText.Get("setup.authConfirmed"); ConnectButton.Text = UiText.Get("action.switchGoogleAccount"); }
            else Completed = true;
            status.Text = UiText.Get(authentication ? "setup.authorizedReviewSettings" : "setup.saved");
        }
        catch (Exception ex) when (LauncherErrors.Expected(ex))
        {
            if (authentication) connection.Text = UiText.Get("setup.authNotCompleted");
            status.Text = ex is ArgumentException ? UiText.Get("setup.consentRequired") : LauncherErrors.Message(ex);
        }
        finally { busy = false; content.Enabled = true; cancel.Visible = false; if (cancellation.IsCancellationRequested) { cancellation.Dispose(); cancellation = new CancellationTokenSource(); } UpdateNavigation(); }
    }
    private async Task Authorize(CancellationToken ct)
    {
        await using (var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("windows-registration", ct))
        {
            LauncherConfiguration.SaveClient(storage, await LauncherConfiguration.SetupClientJsonAsync(storage, clientPath, ct));
            new WindowsAssociationRegistration(Microsoft.Win32.Registry.CurrentUser).Register(Environment.ProcessPath!); WindowsAssociationRegistration.NotifyShell();
        }
        using var handler = new HttpClientHandler { AllowAutoRedirect = false }; using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(90) };
        await Task.Run(() => new WindowsLauncher(storage, http, new BrowserLauncher()).LoginAsync(ct), ct);
    }
    private async Task Verify(CancellationToken ct)
    {
        using var handler = new HttpClientHandler { AllowAutoRedirect = false }; using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
        await new WindowsLauncher(storage, http, new BrowserLauncher()).CheckConnectionAsync(ct);
    }
    private async Task Save(CancellationToken ct)
    {
        if (!AuthorizationConfirmed || FirstUseState.NeedsAuthorization(storage)) throw new AuthorizationRequiredException();
        await OpeningPolicy.SaveAsync(storage, !noSync.Checked, folder.Text, !ShortcutSettings.Load(storage) || Acknowledgement.Checked, ct);
        try { _ = ExtendedConfiguration.Load(storage); } catch (LauncherNotConfiguredException) { ExtendedConfiguration.Save(storage, new(), true); }
        await FirstUseCompletion.SaveAsync(storage, ct);
    }
    protected override void Dispose(bool disposing) { if (disposing && !IsDisposed) { cancellation.Cancel(); cancellation.Dispose(); } base.Dispose(disposing); }
}
