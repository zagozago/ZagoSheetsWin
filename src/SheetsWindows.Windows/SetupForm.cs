using SheetsWindows.Infrastructure;
using SheetsWindows.Core;

namespace SheetsWindows.Windows;

internal sealed class SetupForm : Form
{
    public int ExitCode { get; private set; }
    private readonly Label status = Ui.Text("");
    private CancellationTokenSource cancellation = new();
    private bool busy;
    private bool disconnectRequested;
    private bool cancellationDisposed;
    public SetupForm(bool firstUse = false, bool preview = false, string? previewEmail = null)
    {
        var storage = LocalStorage.ForCurrentUser();
        Text = firstUse ? UiText.Get("setup.firstUseTitle") : UiText.Get("setup.title");
        ClientSize = new Size(560, 570); MinimumSize = new Size(480, 480); AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterScreen;
        var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18, 4, 18, 8), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };
        var shortcuts = new ToggleSwitch { Checked = ShortcutSettings.Load(storage), Text = UiText.Get("setup.createShortcuts") };
        var copy = new ToggleSwitch { Checked = CopyImportSettings.Load(storage), Text = UiText.Get("emphasis.importCopy") };
        var googleInfo = new InformationButton(() =>
        {
            var panel = InformationButton.TextPanel("setup.privacyExplanation"); InformationButton.AddLegalLinks(panel);
            panel.Controls.Add(Ui.Separator()); panel.Controls.Add(Ui.Text(UiText.Get("setup.connectionExplanation"))); return panel;
        });
        var defaultsInfo = new InformationButton(() => InformationButton.TextPanel("setup.defaultsHelp"));
        void Section(string key, InformationButton? info = null)
        {
            var heading = Ui.Text(UiText.Get(key), true); heading.Margin = new Padding(0, 2, 0, 2);
            var row = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.LeftToRight, WrapContents = true, Margin = new Padding(0, 0, 0, 2) };
            row.Controls.Add(heading); if (info is not null) { info.Margin = new Padding(4, 2, 0, 0); row.Controls.Add(info); }
            if (info is not null) info.AccessibleDescription = UiText.Get(key);
            row.SizeChanged += (_, _) => heading.MaximumSize = new Size(Math.Max(100, row.Width - (info is null ? 8 : info.Width + info.Margin.Horizontal + 12)), 0);
            layout.Controls.Add(row);
        }
        void Divider() { var line = Ui.Separator(); line.Margin = new Padding(0, 4, 0, 4); layout.Controls.Add(line); }
        Section("setup.googleHeading", googleInfo);
        var googleExplanation = Ui.Text(UiText.Get("setup.googleExplanation")); googleExplanation.Margin = new Padding(0, 2, 0, 4); layout.Controls.Add(googleExplanation);
        var confirmed = preview; var currentEmail = previewEmail;
        var connectionState = Ui.Hero(UiText.Get(preview ? "setup.authConfirmed" : FirstUseState.NeedsAuthorization(storage) ? "setup.authMissing" : "setup.authSavedChecking"));
        connectionState.AccessibleName = UiText.Get("accessibility.authorizationStatus"); connectionState.Margin = new Padding(0, 4, 0, 4); layout.Controls.Add(connectionState);
        var connectKey = !preview && FirstUseState.NeedsAuthorization(storage) ? "action.authorizeGoogle" : "action.switchGoogleAccount";
        string ShortCaption(string key) => System.Text.RegularExpressions.Regex.Replace(UiText.Get(key).Replace("Google", "", StringComparison.Ordinal), @"\s+", " ").Trim().Replace(" …", "…", StringComparison.Ordinal);
        var connect = new AdaptiveButton { Text = ShortCaption(connectKey) };
        var verify = new AdaptiveButton { Text = ShortCaption("setup.verifyGoogle"), Enabled = !FirstUseState.NeedsAuthorization(storage) };
        var cancel = new AdaptiveButton { Text = UiText.Get("setup.cancelConnection") };
        layout.Controls.Add(Ui.Columns(connect, verify, cancel));
        void RefreshConnection()
        {
            connect.Text = ShortCaption(connectKey); verify.Text = ShortCaption("setup.verifyGoogle");
            if (confirmed)
            {
                var text = UiText.Get("setup.authConfirmed");
                connectionState.Text = string.IsNullOrEmpty(currentEmail) ? text : text.Split('·')[0].TrimEnd() + " · " + currentEmail;
            }
        }
        RefreshConnection();
        Divider(); Section("setup.defaultsHeading", defaultsInfo);
        var defaults = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.openWindowsDefaults") };
        defaults.Click += (_, _) => { try { new BrowserLauncher().Open(WindowsAssociationPlan.DefaultsUri); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = UiText.Get("setup.windowsDefaultsInstructions"); } }; layout.Controls.Add(defaults);
        Divider(); Section("setup.importHeading", new InformationButton(() => InformationButton.TextPanel("home.copyExplanation")));
        layout.Controls.Add(Ui.Pair(shortcuts, copy));
        var advanced = new AdaptiveButton { Text = UiText.Get("setup.advanced"), Dock = DockStyle.Bottom, Height = 42 };
        advanced.Click += (_, _) => { using var form = new AdvancedPreferencesForm(storage, preview); form.ShowDialog(this); };
        var preferences = new PreferenceAutosave(this, layout, status, preview);
        shortcuts.CheckedChanged += async (_, _) =>
        {
            var value = shortcuts.Checked;
            await preferences.SaveAsync(() => ShortcutSettings.SaveAsync(storage, value), () => shortcuts.Checked = ShortcutSettings.Load(storage));
        };
        copy.CheckedChanged += async (_, _) =>
        {
            var value = copy.Checked;
            await preferences.SaveAsync(() => CopyImportSettings.SaveAsync(storage, value), () => copy.Checked = CopyImportSettings.Load(storage));
        };
        status.Visible = false; status.TextChanged += (_, _) => status.Visible = !string.IsNullOrEmpty(status.Text); layout.Controls.Add(status);
        var finish = new AdaptiveButton { Text = UiText.Get("action.close"), Dock = DockStyle.Bottom, Height = 44 };
        async Task Disconnect()
        {
            busy = true; connect.Enabled = verify.Enabled = advanced.Enabled = false; cancel.Enabled = false;
            confirmed = false; currentEmail = null;
            try
            {
                using var handler = new HttpClientHandler { AllowAutoRedirect = false };
                using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
                await new WindowsLauncher(storage, http, new BrowserLauncher()).DisconnectAsync(cancellation.Token);
                status.Text = "";
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = LauncherErrors.Message(ex); }
            finally
            {
                var disconnected = FirstUseState.NeedsAuthorization(storage);
                confirmed = false; currentEmail = null; connectKey = disconnected ? "action.authorizeGoogle" : "action.switchGoogleAccount";
                connectionState.Text = UiText.Get(disconnected ? "setup.authMissing" : "setup.authSavedOffline");
                busy = false; connect.Enabled = advanced.Enabled = cancel.Enabled = true;
                verify.Enabled = !FirstUseState.NeedsAuthorization(storage); ResetCancellation(); RefreshConnection();
            }
        }
        cancel.Click += async (_, _) =>
        {
            if (preview) { confirmed = false; currentEmail = null; connectKey = "action.authorizeGoogle"; connectionState.Text = UiText.Get("setup.authMissing"); RefreshConnection(); return; }
            if (busy) { disconnectRequested = true; cancellation.Cancel(); return; }
            await Disconnect();
        };
        async Task Save(bool authorize)
        {
            if (busy) return;
            SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            busy = true; connect.Enabled = verify.Enabled = advanced.Enabled = false; cancel.Enabled = true; status.Text = UiText.Get(authorize ? "setup.savingAuthorizing" : "setup.saving");
            try
            {
                await using (var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("windows-registration", cancellation.Token))
                {
                    var json = await LauncherConfiguration.SetupClientJsonAsync(storage, ct: cancellation.Token);
                    LauncherConfiguration.SaveClient(storage, json);
                    new WindowsAssociationRegistration(Microsoft.Win32.Registry.CurrentUser).Register(Environment.ProcessPath!); WindowsAssociationRegistration.NotifyShell();
                }
                if (authorize)
                {
                    using var handler = new HttpClientHandler { AllowAutoRedirect = false }; using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(90) };
                    await Task.Run(() => new WindowsLauncher(storage, http, new BrowserLauncher()).LoginAsync(cancellation.Token));
                    confirmed = true; currentEmail = new WindowsLauncher(storage, http, new BrowserLauncher()).SavedAccountEmail(); connectKey = "action.switchGoogleAccount"; RefreshConnection();
                }
                ExitCode = 0; status.Text = UiText.Get(authorize ? "setup.authorizedReviewSettings" : "setup.saved");
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { ExitCode = 1; if (authorize) { confirmed = false; connectionState.Text = UiText.Get("setup.authNotCompleted"); } status.Text = ex is ArgumentException ? UiText.Get("setup.consentRequired") : LauncherErrors.Message(ex); }
            finally
            {
                busy = false; connect.Enabled = advanced.Enabled = true; verify.Enabled = !FirstUseState.NeedsAuthorization(storage); ResetCancellation();
                if (disconnectRequested) { disconnectRequested = false; await Disconnect(); }
            }
        }
        connect.Click += async (_, _) => await Save(true);
        async Task CheckConnection()
        {
            if (busy || FirstUseState.NeedsAuthorization(storage)) return;
            SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            busy = true; confirmed = false; verify.Enabled = connect.Enabled = advanced.Enabled = false; connectionState.Text = UiText.Get("setup.authChecking");
            try
            {
                using var handler = new HttpClientHandler { AllowAutoRedirect = false }; using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
                currentEmail = await new WindowsLauncher(storage, http, new BrowserLauncher()).CheckConnectionAsync(cancellation.Token);
                confirmed = true; connectKey = "action.switchGoogleAccount"; RefreshConnection();
            }
            catch (AuthorizationRequiredException) { confirmed = false; connectionState.Text = UiText.Get("setup.authRenew"); connectKey = "action.authorizeGoogle"; RefreshConnection(); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { connectionState.Text = UiText.Get("setup.authSavedOffline"); }
            finally
            {
                busy = false; connect.Enabled = advanced.Enabled = true; verify.Enabled = !FirstUseState.NeedsAuthorization(storage); ResetCancellation();
                if (disconnectRequested) { disconnectRequested = false; await Disconnect(); }
            }
        }
        verify.Click += async (_, _) => await CheckConnection(); if (!preview) Shown += async (_, _) => await CheckConnection();
        finish.Click += (_, _) => { if (!busy) Close(); };
        Controls.Add(layout); Controls.Add(advanced); Controls.Add(finish);
        FormClosing += (_, e) => { if (busy) { e.Cancel = true; cancellation.Cancel(); } };
        Branding.Apply(this); Ui.Adapt(layout);
        var fitting = false; Font? fittedFont = null;
        void FitCenter()
        {
            if (fitting || connectionState.IsDisposed) return;
            fitting = true;
            try
            {
                connectionState.Tag = "accountSingleLine"; connectionState.AutoSize = false;
                connectionState.Width = Math.Max(120, layout.ClientSize.Width - layout.Padding.Horizontal - 24);
                var size = 16f; Font next;
                while (true)
                {
                    next = new Font(LanguageVisuals.Family, size, FontStyle.Bold);
                    if (size <= 0.5f || TextRenderer.MeasureText(connectionState.Text, next, Size.Empty, TextFormatFlags.SingleLine | TextFormatFlags.NoPadding).Width <= connectionState.Width - 12) break;
                    next.Dispose(); size -= 0.25f;
                }
                var previous = fittedFont; fittedFont = next; connectionState.Font = next; previous?.Dispose();
                connectionState.Height = (int)Math.Ceiling(16 * DeviceDpi / 72.0) + 16;
            }
            finally { fitting = false; }
        }
        connectionState.TextChanged += (_, _) => FitCenter(); layout.SizeChanged += (_, _) => FitCenter();
        void LanguageChanged() { RefreshConnection(); FitCenter(); }
        UiText.Changed += LanguageChanged;
        Disposed += (_, _) => { UiText.Changed -= LanguageChanged; fittedFont?.Dispose(); };
        FitCenter();
    }
    private void ResetCancellation() { if (cancellation.IsCancellationRequested) { cancellation.Dispose(); cancellation = new(); } }
    protected override void Dispose(bool disposing) { if (disposing && !cancellationDisposed) { cancellationDisposed = true; cancellation.Cancel(); cancellation.Dispose(); } base.Dispose(disposing); }
}

