using SheetsWindows.Infrastructure;
using SheetsWindows.Core;

namespace SheetsWindows.Windows;

// The post-install configuration reuses the help illustrations and translated copy.
// The help dialog remains independent, including its skip and preference controls.
internal sealed class FirstUseWizard : Form
{
    internal const int PageCount = 3;
    private readonly LocalStorage storage;
    private readonly bool preview;
    private readonly Func<CancellationToken, Task> authorize;
    private readonly Func<CancellationToken, Task> verify;
    private readonly Func<CancellationToken, Task> save;
    private CancellationTokenSource cancellation = new();
    private readonly TableLayoutPanel layout = new() { Dock = DockStyle.Fill, Padding = new Padding(20, 8, 20, 14), ColumnCount = 1, RowCount = 5 };
    private readonly Label heading = Ui.Text("", true);
    private readonly TutorialPicture picture = new() { Dock = DockStyle.Fill, FirstUse = true };
    private readonly FlowLayoutPanel content = new() { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
    private readonly FlowLayoutPanel actions = new() { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true };
    private readonly Label count = new() { AutoSize = true };
    private readonly Label connection = Ui.Hero("");
    private readonly Label status = Ui.Text("");
    private readonly CheckBox noSync;
    private readonly TextBox folder = new() { ReadOnly = true };
    private readonly Button cancel = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.cancelConnection"), Visible = false };
    private string clientPath = "";
    private bool busy;
    private bool checking;
    private bool fittingContent;
    internal int CurrentPage { get; private set; }
    internal bool AuthorizationConfirmed { get; private set; }
    internal bool Completed { get; private set; }
    internal Button NextButton { get; } = new AdaptiveButton { AutoSize = true };
    internal Button PreviousButton { get; } = new AdaptiveButton { AutoSize = true, Text = UiText.Get("action.back") };
    internal Button ConnectButton { get; } = new AdaptiveButton { AutoSize = true, Text = UiText.Get("action.authorizeGoogle") };

    internal FirstUseWizard(bool preview = false, int initialPage = 0, LocalStorage? storage = null,
        Func<CancellationToken, Task>? authorize = null, Func<CancellationToken, Task>? verify = null,
        Func<CancellationToken, Task>? save = null)
    {
        this.storage = storage ?? LocalStorage.ForCurrentUser(); this.preview = preview; AuthorizationConfirmed = preview;
        this.authorize = authorize ?? Authorize; this.verify = verify ?? Verify; this.save = save ?? Save;
        var opening = OpeningPolicy.Load(this.storage);
        noSync = new ToggleSwitch { AutoSize = true, Text = UiText.Get("setup.noSync"), Checked = !opening.RestrictToFolder };
        folder.Text = opening.Folder ?? "";
        CurrentPage = Math.Clamp(initialPage, 0, PageCount - 1);
        Text = UiText.Get("setup.firstUseTitle"); ClientSize = new Size(760, 440); MinimumSize = new Size(620, 420);
        StartPosition = FormStartPosition.CenterScreen; AutoScaleMode = AutoScaleMode.Dpi;
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 200));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); layout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        connection.Font = new Font(LanguageVisuals.Family, 11, FontStyle.Bold); connection.Margin = new Padding(0, 2, 0, 6);
        heading.Dock = DockStyle.Fill; heading.Margin = new Padding(0, 0, 0, 8);
        layout.Controls.Add(heading, 0, 0); layout.Controls.Add(picture, 0, 1); layout.Controls.Add(content, 0, 2); layout.Controls.Add(actions, 0, 3);
        actions.Controls.Add(PreviousButton); actions.Controls.Add(NextButton); actions.Controls.Add(count); actions.Controls.Add(cancel);
        var legal = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, WrapContents = true };
        foreach (var (key, url) in new[] { ("legal.privacy", "https://zagotools.top/legal.html#privacidade"), ("legal.terms", "https://zagotools.top/legal.html#termos") })
        {
            var link = new LinkLabel { AutoSize = true, Text = UiText.Get(key), Margin = new Padding(0, 6, 18, 4) };
            link.LinkClicked += (_, _) => { try { new BrowserLauncher().Open(new Uri(url)); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = url; } };
            legal.Controls.Add(link);
        }
        layout.Controls.Add(legal, 0, 4); Controls.Add(layout); Ui.Primary(NextButton);
        PreviousButton.Click += (_, _) => { if (busy || CurrentPage == 0 ) return; CurrentPage--; RefreshPage(); };
        NextButton.Click += async (_, _) =>
        {
            if (busy || CurrentPage == 1 && !AuthorizationConfirmed) return;
            if (CurrentPage < PageCount - 1) { CurrentPage++; RefreshPage(); if (CurrentPage == 1) await CheckExistingConnection(); return; }
            await Run(this.save, authentication: false);
            if (Completed) Close();
        };
        ConnectButton.Click += async (_, _) => await Run(this.authorize, authentication: true);
        cancel.Click += (_, _) => cancellation.Cancel();
        UiText.Changed += RefreshPage; Disposed += (_, _) => UiText.Changed -= RefreshPage;
        FormClosing += (_, e) =>
        {
            if (Completed || preview) return;
            if (busy) { e.Cancel = true; cancellation.Cancel(); }
        };
        Branding.Apply(this); Ui.Adapt(content); RefreshPage();
        Shown += (_, _) => FitContent(); DpiChanged += (_, _) => BeginInvoke((Action)FitContent);
        if (!preview) Shown += async (_, _) => { if (CurrentPage == 1) await CheckExistingConnection(); };
    }
    private static string SectionTitle(string key) => System.Text.RegularExpressions.Regex.Replace(UiText.Get(key), @"^\s*\d+[.、．]?\s*", "");
    private void UpdateNavigation()
    {
        PreviousButton.Enabled = !busy && CurrentPage > 0;
        NextButton.Enabled = !busy && (CurrentPage != 1 || AuthorizationConfirmed);
        ConnectButton.Enabled = !busy;
    }
    private void RefreshPage()
    {
        // Persistent input controls survive both page navigation and language changes.
        foreach (var persistent in new Control[] { noSync, folder, ConnectButton, connection, status }) persistent.Parent?.Controls.Remove(persistent);
        var old = content.Controls.Cast<Control>().ToArray(); content.Controls.Clear(); foreach (var control in old) control.Dispose();
        heading.Text = UiText.Format("tutorial.numberedTitle", ("number", CurrentPage + 1), ("title", TutorialForm.Pages[CurrentPage].Title));
        picture.Page = CurrentPage; picture.AccessibleName = TutorialForm.Pages[CurrentPage].Title; picture.Invalidate();
        layout.RowStyles[1].Height = 200 * DeviceDpi / 96f;
        if (CurrentPage == 1)
        {
            var accountExplanation = Ui.Center(Ui.Text(UiText.Get("firstUse.googleExplanation")));
            accountExplanation.Margin = new Padding(0, 0, 0, 6); content.Controls.Add(accountExplanation);
            connection.Text = UiText.Get(AuthorizationConfirmed ? "setup.authConfirmed" : "setup.authMissing");
            ConnectButton.Text = UiText.Get(AuthorizationConfirmed ? "action.switchGoogleAccount" : "action.authorizeGoogle");
            content.Controls.Add(Ui.Center(connection)); content.Controls.Add(ConnectButton);
            if (!preview && !File.Exists(LauncherConfiguration.ClientPath(storage)) &&
                !typeof(LauncherConfiguration).Assembly.GetManifestResourceNames().Contains("OAuth.official.desktop.json"))
            {
                var choose = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.chooseClient") };
                choose.Click += (_, _) => { using var dialog = new OpenFileDialog { Filter = UiText.Get("dialog.oauthFileFilter"), CheckFileExists = true }; if (dialog.ShowDialog(this) == DialogResult.OK) { clientPath = dialog.FileName; status.Text = UiText.Get("setup.clientSelected"); } };
                content.Controls.Add(choose);
            }
        }
        else if (CurrentPage == 2)
        {
            var defaultsHeading = Ui.Center(Ui.Text(SectionTitle("setup.defaultsHeading"), true)); defaultsHeading.Margin = new Padding(0, 2, 0, 6);
            var defaultsExplanation = Ui.Center(Ui.Text(UiText.Get("firstUse.defaultsExplanation"))); defaultsExplanation.Margin = new Padding(0, 0, 0, 6);
            content.Controls.Add(defaultsHeading); content.Controls.Add(defaultsExplanation);
            var defaults = new AdaptiveButton { AutoSize = true, Text = UiText.Get("tutorial.defineDefaults") }; Ui.Primary(defaults);
            defaults.Click += (_, _) => { try { new BrowserLauncher().Open(WindowsAssociationPlan.DefaultsUri); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = UiText.Get("setup.windowsDefaultsInstructions"); } };
            content.Controls.Add(defaults);
        }
        status.Visible = !string.IsNullOrEmpty(status.Text);
        content.Controls.Add(status);
        NextButton.Text = UiText.Get(CurrentPage == PageCount - 1 ? "tutorial.start" : "action.next"); PreviousButton.Text = UiText.Get("action.back");
        count.Margin = new Padding(12, 10, 0, 0);
        count.Text = UiText.Format("tutorial.pageCount", ("number", CurrentPage + 1), ("total", PageCount));
        Branding.Refresh(this); content.PerformLayout(); UpdateNavigation(); FitContent();
    }
    private void FitContent()
    {
        if (fittingContent || !IsHandleCreated || WindowState != FormWindowState.Normal) return;
        fittingContent = true;
        try
        {
            content.PerformLayout();
            var needed = content.Controls.Cast<Control>().Where(control => control.Visible).Sum(control => control.Height + control.Margin.Vertical) + content.Padding.Vertical + 8;
            if (needed <= content.ClientSize.Height) return;
            var maximum = Screen.FromControl(this).WorkingArea.Height - (Height - ClientSize.Height) - 24;
            ClientSize = new Size(ClientSize.Width, Math.Min(maximum, ClientSize.Height + needed - content.ClientSize.Height));
            content.PerformLayout();
        }
        finally { fittingContent = false; }
    }
    private async Task CheckExistingConnection()
    {
        if (preview || checking || FirstUseState.NeedsAuthorization(storage)) return;
        checking = true; await Run(verify, authentication: true); checking = false;
    }
    private async Task Run(Func<CancellationToken, Task> action, bool authentication)
    {
        if (busy) return;
        SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
        busy = true;
        if (authentication) AuthorizationConfirmed = false;
        content.Enabled = false; cancel.Visible = authentication; UpdateNavigation();
        status.Visible = true; status.Text = UiText.Get(authentication ? "setup.savingAuthorizing" : "setup.saving");
        try
        {
            await action(cancellation.Token);
            if (authentication) { AuthorizationConfirmed = true; connection.Text = UiText.Get("setup.authConfirmed"); ConnectButton.Text = UiText.Get("action.switchGoogleAccount"); }
            else Completed = true;
            status.Text = "";
        }
        catch (Exception ex) when (LauncherErrors.Expected(ex))
        {
            if (authentication) connection.Text = UiText.Get("setup.authNotCompleted");
            status.Text = ex is ArgumentException ? UiText.Get("setup.consentRequired") : LauncherErrors.Message(ex);
        }
        finally { status.Visible = !string.IsNullOrEmpty(status.Text); busy = false; content.Enabled = true; cancel.Visible = false; if (cancellation.IsCancellationRequested) { cancellation.Dispose(); cancellation = new CancellationTokenSource(); } UpdateNavigation(); FitContent(); }
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
        await OpeningPolicy.SaveAsync(storage, !noSync.Checked, folder.Text, true, ct);
        try { _ = ExtendedConfiguration.Load(storage); } catch (LauncherNotConfiguredException) { ExtendedConfiguration.Save(storage, new(), true); }
        var policy = BackupPolicy.Load(storage);
        await BackupPolicy.SaveAsync(storage, policy, ct);
        await FirstUseCompletion.SaveAsync(storage, ct);
    }
    protected override void Dispose(bool disposing) { if (disposing && !IsDisposed) { cancellation.Cancel(); cancellation.Dispose(); } base.Dispose(disposing); }
}

