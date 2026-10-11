using SheetsWindows.Infrastructure;
using SheetsWindows.Core;

namespace SheetsWindows.Windows;

internal sealed class SetupForm : Form
{
    public int ExitCode { get; private set; }
    private readonly TextBox client = new() { ReadOnly = true };
    private readonly TextBox folder = new() { ReadOnly = true };
    private readonly Label status = Ui.Text("");
    private CancellationTokenSource cancellation = new();
    private bool busy;
    public SetupForm(bool firstUse = false, bool preview = false)
    {
        var storage = LocalStorage.ForCurrentUser();
        Text = firstUse ? UiText.Get("setup.firstUseTitle") : UiText.Get("setup.title");
        ClientSize = new Size(560, 710); MinimumSize = new Size(480, 520); AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterScreen;
        var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18, 4, 18, 8), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };
        var opening = OpeningPolicy.Load(storage);
        var noSync = new ToggleSwitch { Checked = !opening.RestrictToFolder, Text = UiText.Get("setup.noSync") };
        var consent = new ToggleSwitch { Checked = OpeningPolicy.IsConfigured(storage), Text = UiText.Get("setup.replacementConsent") };
        var shortcuts = new ToggleSwitch { Checked = ShortcutSettings.Load(storage), Text = UiText.Get("setup.createShortcuts") };
        var copy = new ToggleSwitch { Checked = CopyImportSettings.Load(storage), Text = UiText.Get("emphasis.importCopy") };
        var xls = new ToggleSwitch { Checked = XlsReplacementSettings.Load(storage), Text = UiText.Get("setup.replaceXls") };
        var extended = new ToggleSwitch { Checked = true, Text = UiText.Get("setup.extendedFormats") };
        var encoding = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        encoding.Items.AddRange([UiText.Get("setup.encodingUnicode"), UiText.Get("setup.encodingLegacy")]); encoding.SelectedIndex = 0;
        var delimiter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        delimiter.Items.AddRange([UiText.Get("setup.delimiterAuto"), UiText.Get("setup.delimiterComma"), UiText.Get("setup.delimiterSemicolon")]); delimiter.SelectedIndex = 0;
        try { var previous = ExtendedConfiguration.Load(storage); extended.Enabled = encoding.Enabled = delimiter.Enabled = false; encoding.SelectedIndex = previous.Encoding == "auto" ? 0 : 1; delimiter.SelectedIndex = previous.Delimiter switch { "comma" => 1, "semicolon" => 2, _ => 0 }; }
        catch (LauncherNotConfiguredException) { }
        var hasClient = File.Exists(LauncherConfiguration.ClientPath(storage));
        if (hasClient)
        {
            void RefreshClient() => client.Text = UiText.Get("setup.clientPreserved");
            RefreshClient(); UiText.Changed += RefreshClient; Disposed += (_, _) => UiText.Changed -= RefreshClient;
        }
        var chooseClient = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.chooseClient"), Enabled = !hasClient };
        chooseClient.Click += (_, _) => { using var dialog = new OpenFileDialog { Filter = UiText.Get("dialog.oauthFileFilter"), CheckFileExists = true }; if (dialog.ShowDialog(this) == DialogResult.OK) { client.Text = dialog.FileName; status.Text = UiText.Get("setup.clientSelected"); } };
        var googleInfo = new InformationButton(() =>
        {
            var panel = InformationButton.TextPanel("setup.privacyExplanation"); InformationButton.AddLegalLinks(panel);
            panel.Controls.Add(Ui.Separator()); panel.Controls.Add(Ui.Text(UiText.Get("setup.connectionExplanation"))); panel.Controls.Add(chooseClient); panel.Controls.Add(client); return panel;
        });
        var defaultsInfo = new InformationButton(() => InformationButton.TextPanel("setup.defaultsHelp"));
        var syncInfo = new InformationButton(() =>
        {
            var panel = InformationButton.TextPanel("setup.replacementExplanation", "home.copyExplanation");
            panel.Controls.Add(Ui.Separator()); panel.Controls.Add(Ui.Text(UiText.Get("setup.formatsHeading"), true)); panel.Controls.Add(extended); panel.Controls.Add(encoding); panel.Controls.Add(delimiter); panel.Controls.Add(Ui.Text(UiText.Get("setup.formatLimits")));
            panel.Controls.Add(Ui.Separator()); panel.Controls.Add(Ui.Text(UiText.Get("setup.xlsHeading"), true)); panel.Controls.Add(xls); panel.Controls.Add(Ui.Text(UiText.Get("setup.xlsExplanation"))); return panel;
        });
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
        var connectionState = Ui.Hero(UiText.Get(preview ? "setup.authConfirmed" : FirstUseState.NeedsAuthorization(storage) ? "setup.authMissing" : "setup.authSavedChecking"));
        connectionState.AccessibleName = UiText.Get("accessibility.authorizationStatus"); connectionState.Margin = new Padding(0, 4, 0, 4); layout.Controls.Add(connectionState);
        var connect = new AdaptiveButton { Text = UiText.Get(!preview && FirstUseState.NeedsAuthorization(storage) ? "action.authorizeGoogle" : "action.switchGoogleAccount") };
        var verify = new AdaptiveButton { Text = UiText.Get("setup.verifyGoogle"), Enabled = !FirstUseState.NeedsAuthorization(storage) };
        layout.Controls.Add(Ui.Pair(connect, verify));
        Divider(); Section("setup.defaultsHeading", defaultsInfo);
        var defaults = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.openWindowsDefaults") };
        defaults.Click += (_, _) => { try { new BrowserLauncher().Open(WindowsAssociationPlan.DefaultsUri); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = UiText.Get("setup.windowsDefaultsInstructions"); } }; layout.Controls.Add(defaults);
        Divider(); Section("setup.syncHeading", syncInfo); layout.Controls.Add(Ui.Pair(noSync, consent));
        var folderOptions = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.TopDown, WrapContents = false, Visible = !noSync.Checked };
        folder.Text = opening.Folder ?? "";
        var chooseFolder = new AdaptiveButton { Text = UiText.Get("setup.chooseFolder") };
        chooseFolder.Click += (_, _) => { using var dialog = new FolderBrowserDialog(); if (dialog.ShowDialog(this) == DialogResult.OK) folder.Text = dialog.SelectedPath; };
        folderOptions.Controls.Add(Ui.Text(UiText.Get("setup.syncExplanation"))); folderOptions.Controls.Add(Ui.Pair(chooseFolder, folder)); layout.Controls.Add(folderOptions);
        noSync.CheckedChanged += (_, _) => folderOptions.Visible = !noSync.Checked;
        Divider(); Section("setup.importHeading"); layout.Controls.Add(Ui.Pair(shortcuts, copy));
        var save = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.save") }; Ui.Primary(save); layout.Controls.Add(save);
        status.Visible = false; status.TextChanged += (_, _) => status.Visible = !string.IsNullOrEmpty(status.Text); layout.Controls.Add(status);
        var finish = new AdaptiveButton { Text = UiText.Get("action.close"), Dock = DockStyle.Bottom, Height = 44 };
        var cancel = new AdaptiveButton { Text = UiText.Get("setup.cancelConnection"), Dock = DockStyle.Bottom, Visible = false };
        cancel.Click += (_, _) => cancellation.Cancel();
        async Task Save(bool authorize)
        {
            if (busy) return;
            SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            busy = true; layout.Enabled = false; cancel.Visible = authorize; status.Text = UiText.Get(authorize ? "setup.savingAuthorizing" : "setup.saving");
            try
            {
                await using (var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("windows-registration", cancellation.Token))
                {
                    var json = await LauncherConfiguration.SetupClientJsonAsync(storage, client.Text, cancellation.Token);
                    if (!authorize)
                    {
                        await OpeningPolicy.SaveAsync(storage, !noSync.Checked, folder.Text, copy.Checked || !shortcuts.Checked || consent.Checked, cancellation.Token);
                        ExtendedConfiguration.Save(storage, new(encoding.SelectedIndex == 0 ? "auto" : "windows-1252", delimiter.SelectedIndex switch { 1 => "comma", 2 => "semicolon", _ => "auto" }), extended.Checked);
                        await ShortcutSettings.SaveAsync(storage, shortcuts.Checked, cancellation.Token);
                        await CopyImportSettings.SaveAsync(storage, copy.Checked, cancellation.Token);
                        await XlsReplacementSettings.SaveAsync(storage, xls.Checked, cancellation.Token);
                    }
                    LauncherConfiguration.SaveClient(storage, json);
                    new WindowsAssociationRegistration(Microsoft.Win32.Registry.CurrentUser).Register(Environment.ProcessPath!); WindowsAssociationRegistration.NotifyShell();
                }
                if (authorize)
                {
                    using var handler = new HttpClientHandler { AllowAutoRedirect = false }; using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(90) };
                    await Task.Run(() => new WindowsLauncher(storage, http, new BrowserLauncher()).LoginAsync(cancellation.Token));
                    connectionState.Text = UiText.Get("setup.authConfirmed"); connect.Text = UiText.Get("action.switchGoogleAccount");
                }
                ExitCode = 0; status.Text = UiText.Get(authorize ? "setup.authorizedReviewSettings" : "setup.saved");
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { ExitCode = 1; if (authorize) connectionState.Text = UiText.Get("setup.authNotCompleted"); status.Text = ex is ArgumentException ? UiText.Get("setup.consentRequired") : LauncherErrors.Message(ex); }
            finally { busy = false; layout.Enabled = true; verify.Enabled = !FirstUseState.NeedsAuthorization(storage); cancel.Visible = false; ResetCancellation(); }
        }
        connect.Click += async (_, _) => await Save(true); save.Click += async (_, _) => await Save(false);
        async Task CheckConnection()
        {
            if (busy || FirstUseState.NeedsAuthorization(storage)) return;
            SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
            busy = true; verify.Enabled = connect.Enabled = false; connectionState.Text = UiText.Get("setup.authChecking");
            try
            {
                using var handler = new HttpClientHandler { AllowAutoRedirect = false }; using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
                await new WindowsLauncher(storage, http, new BrowserLauncher()).CheckConnectionAsync(cancellation.Token);
                connectionState.Text = UiText.Get("setup.authConfirmed"); connect.Text = UiText.Get("action.switchGoogleAccount");
            }
            catch (AuthorizationRequiredException) { connectionState.Text = UiText.Get("setup.authRenew"); connect.Text = UiText.Get("action.authorizeGoogle"); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { connectionState.Text = UiText.Get("setup.authSavedOffline"); }
            finally { busy = false; verify.Enabled = connect.Enabled = true; ResetCancellation(); }
        }
        verify.Click += async (_, _) => await CheckConnection(); if (!preview) Shown += async (_, _) => await CheckConnection();
        finish.Click += (_, _) => { if (!busy) Close(); };
        Controls.Add(layout); Controls.Add(finish); Controls.Add(cancel);
        FormClosing += (_, e) => { if (busy) { e.Cancel = true; cancellation.Cancel(); } };
        Branding.Apply(this); Ui.Adapt(layout); Ui.Adapt(folderOptions);
        void FitCenter() { connectionState.AutoSize = false; connectionState.Width = Math.Max(120, layout.ClientSize.Width - layout.Padding.Horizontal - 24); connectionState.Height = TextRenderer.MeasureText(connectionState.Text, connectionState.Font, new Size(connectionState.Width, int.MaxValue), TextFormatFlags.WordBreak).Height + 12; }
        connectionState.TextChanged += (_, _) => FitCenter(); layout.SizeChanged += (_, _) => FitCenter(); FitCenter();
        Disposed += (_, _) => { foreach (var control in new Control[] { client, chooseClient, extended, encoding, delimiter, xls }) if (!control.IsDisposed) control.Dispose(); };
    }
    private void ResetCancellation() { if (cancellation.IsCancellationRequested) { cancellation.Dispose(); cancellation = new(); } }
    protected override void Dispose(bool disposing) { if (disposing) { cancellation.Cancel(); cancellation.Dispose(); } base.Dispose(disposing); }
}
