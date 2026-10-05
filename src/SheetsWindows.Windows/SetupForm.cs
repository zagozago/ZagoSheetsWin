using SheetsWindows.Infrastructure;
using SheetsWindows.Core;

namespace SheetsWindows.Windows;

internal sealed class SetupForm : Form
{
    public int ExitCode { get; private set; }
    private readonly TextBox client = new() { Width = 570, ReadOnly = true };
    private readonly TextBox folder = new() { Width = 570, ReadOnly = true };
    private readonly Label status = new EmphasisLabel() { AutoSize = true, MaximumSize = new Size(570, 0) };
    private CancellationTokenSource cancellation = new();
    private bool busy;
    internal Form AdvancedDialog { get; private set; } = null!;
    public SetupForm(bool firstUse = false, bool preview = false)
    {
        var storage = LocalStorage.ForCurrentUser();
        Text = firstUse ? UiText.Get("setup.firstUseTitle") : UiText.Get("setup.title");
        ClientSize = new Size(560, 700); MinimumSize = new Size(480, 520); AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterScreen;
        var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };
        Label Info(string text) => Ui.Text(text);
        layout.Controls.Add(Info(UiText.Get("setup.description")));
        var configured = !FirstUseState.NeedsSetup(storage);
        var opening = OpeningPolicy.Load(storage);
        var noSync = new CheckBox { AutoSize = true, Checked = true, Text = UiText.Get("setup.noSync") };
        var folderOptions = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        folder.Text = opening.Folder ?? "";
        var chooseFolder = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.chooseFolder") };
        chooseFolder.Click += (_, _) => { using var dialog = new FolderBrowserDialog(); if (dialog.ShowDialog(this) == DialogResult.OK) folder.Text = dialog.SelectedPath; };
        folderOptions.Controls.Add(Info(UiText.Get("setup.syncExplanation")));
        var folderRow = new TableLayoutPanel { AutoSize = false, Height = 38, ColumnCount = 2, RowCount = 1, Margin = new Padding(0, 4, 0, 4) };
        folderRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); folderRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        chooseFolder.AutoSize = false; chooseFolder.Dock = DockStyle.Fill; chooseFolder.Height = 34;
        folder.Dock = DockStyle.Fill; folder.Margin = new Padding(3, 7, 3, 3);
        folderRow.Controls.Add(chooseFolder, 0, 0); folderRow.Controls.Add(folder, 1, 0);
        folderOptions.Controls.Add(folderRow);
        var consent = new CheckBox { AutoSize = true, Checked = configured, Text = UiText.Get("setup.replacementConsent") };
        void ShowFolderOptions() { folderOptions.Visible = !noSync.Checked; layout.PerformLayout(); }
        noSync.CheckedChanged += (_, _) => ShowFolderOptions();
        var advanced = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(18), FlowDirection = FlowDirection.TopDown, WrapContents = false };
        var advancedDialog = new Form { Text = UiText.Get("setup.advancedTitle"), ClientSize = new Size(550, 640), MinimumSize = new Size(480, 480), StartPosition = FormStartPosition.CenterParent, AutoScaleMode = AutoScaleMode.Dpi, ShowInTaskbar = false, MinimizeBox = false };
        var closeAdvanced = new AdaptiveButton { Text = UiText.Get("action.close"), Dock = DockStyle.Bottom, Height = 44 };
        advancedDialog.Controls.Add(advanced); advancedDialog.Controls.Add(closeAdvanced);
        closeAdvanced.Click += (_, _) => advancedDialog.Close();
        AdvancedDialog = advancedDialog;
        advancedDialog.CancelButton = closeAdvanced;
        Disposed += (_, _) => advancedDialog.Dispose();
        var toggle = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.advanced") };
        toggle.Click += (_, _) => advancedDialog.ShowDialog(this);

        advanced.Controls.Add(Info(UiText.Get("setup.connectionExplanation")));
        var chooseClient = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.chooseClient"), Enabled = !File.Exists(LauncherConfiguration.ClientPath(storage)) };
        chooseClient.Click += (_, _) => { using var dialog = new OpenFileDialog { Filter = UiText.Get("dialog.oauthFileFilter"), CheckFileExists = true }; if (dialog.ShowDialog(this) == DialogResult.OK)  { client.Text = dialog.FileName;  status.Text = UiText.Get("setup.clientSelected"); } };
        advanced.Controls.Add(Info(UiText.Get("setup.connectionPrivacy")));
        advanced.Controls.Add(chooseClient); advanced.Controls.Add(client);
        if (File.Exists(LauncherConfiguration.ClientPath(storage)))
        {
            client.Text = UiText.Get("setup.clientPreserved");
            void UpdateClientLabel() { client.Text = UiText.Get("setup.clientPreserved"); }
            UiText.Changed += UpdateClientLabel; Disposed += (_,_) => UiText.Changed -= UpdateClientLabel;
        }
        var extended = new CheckBox { AutoSize = true, Checked = true, Text = UiText.Get("setup.extendedFormats") };
        var encoding = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 570 };
        encoding.Items.AddRange([UiText.Get("setup.encodingUnicode"), UiText.Get("setup.encodingLegacy")]); encoding.SelectedIndex = 0;
        var delimiter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 570 };
        delimiter.Items.AddRange([UiText.Get("setup.delimiterAuto"), UiText.Get("setup.delimiterComma"), UiText.Get("setup.delimiterSemicolon")]); delimiter.SelectedIndex = 0;
        advanced.Controls.Add(Ui.Separator()); advanced.Controls.Add(Ui.Text(UiText.Get("setup.formatsHeading"), true)); advanced.Controls.Add(extended); advanced.Controls.Add(encoding); advanced.Controls.Add(delimiter);
        advanced.Controls.Add(Info(UiText.Get("setup.formatLimits")));
        try { var previous = ExtendedConfiguration.Load(storage); extended.Enabled = false; encoding.Enabled = false; delimiter.Enabled = false; encoding.SelectedIndex = previous.Encoding == "auto" ? 0 : 1; delimiter.SelectedIndex = previous.Delimiter switch { "comma" => 1, "semicolon" => 2, _ => 0 }; }
        catch (LauncherNotConfiguredException) { }
        var xls = new CheckBox { AutoSize = true, Checked = XlsReplacementSettings.Load(storage), Text = UiText.Get("setup.replaceXls") };
        advanced.Controls.Add(Ui.Separator()); advanced.Controls.Add(Ui.Text(UiText.Get("setup.xlsHeading"), true)); advanced.Controls.Add(xls);
        advanced.Controls.Add(Info(UiText.Get("setup.xlsExplanation")));
        var saveXls = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.saveXls") };
        saveXls.Click += async (_, _) => { saveXls.Enabled = false; try { await XlsReplacementSettings.SaveAsync(storage, xls.Checked); status.Text = UiText.Get("setup.xlsSaved"); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = LauncherErrors.Message(ex); } finally { saveXls.Enabled = true; } };
        advanced.Controls.Add(saveXls);
        var advancedStatus = Ui.Text(""); advanced.Controls.Add(advancedStatus);
        status.TextChanged += (_, _) => advancedStatus.Text = status.Text;
        layout.Controls.Add(Ui.Separator()); layout.Controls.Add(Ui.Text(UiText.Get("setup.googleHeading"), true));
        layout.Controls.Add(Info(UiText.Get("setup.googleExplanation")));
        layout.Controls.Add(Info(UiText.Get("setup.privacyExplanation")));
        var connectionState = Ui.Text(FirstUseState.NeedsAuthorization(storage) ? UiText.Get("setup.authMissing") : UiText.Get("setup.authSavedChecking"), true);
        connectionState.AccessibleName = UiText.Get("accessibility.authorizationStatus");
        layout.Controls.Add(connectionState);
        var legalLinks = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.LeftToRight, WrapContents = true };
        var privacyLink = new LinkLabel { Text = UiText.Get("legal.privacy"), AutoSize = true, Margin = new Padding(0, 4, 18, 4) };
        privacyLink.LinkClicked += (_, _) => { try { new BrowserLauncher().Open(new Uri("https://zagotools.top/legal.html#privacidade")); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { MessageBox.Show("https://zagotools.top/legal.html#privacidade", UiText.Get("legal.privacy")); } };
        var termsLink = new LinkLabel { Text = UiText.Get("legal.terms"), AutoSize = true, Margin = new Padding(0, 4, 0, 4) };
        termsLink.LinkClicked += (_, _) => { try { new BrowserLauncher().Open(new Uri("https://zagotools.top/legal.html#termos")); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { MessageBox.Show("https://zagotools.top/legal.html#termos", UiText.Get("legal.terms")); } };
        legalLinks.Controls.Add(privacyLink); legalLinks.Controls.Add(termsLink); layout.Controls.Add(legalLinks);
        var connect = new AdaptiveButton { AutoSize = true, Text = FirstUseState.NeedsAuthorization(storage) ? UiText.Get("action.authorizeGoogle") : UiText.Get("action.switchGoogleAccount") };
        var verify = new AdaptiveButton { Text = UiText.Get("setup.verifyGoogle"), AutoSize = true, Enabled = !FirstUseState.NeedsAuthorization(storage) };
        var save = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.save") };
        var defaults = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.openWindowsDefaults") };
        var finish = new AdaptiveButton { AutoSize = true, Text = firstUse ? UiText.Get("setup.saveFinish") : UiText.Get("action.close") };
        var cancel = new AdaptiveButton { AutoSize = true, Text = UiText.Get("setup.cancelConnection"), Visible = false };
        cancel.Click += (_, _) => cancellation.Cancel();
        var authorizationValid = !FirstUseState.NeedsAuthorization(storage);
        bool Ready() => authorizationValid && !FirstUseState.NeedsAuthorization(storage);
        if (firstUse) finish.Enabled = Ready();
        async Task Save(bool authorize)
        {
            if (busy) return;
            busy = true; layout.Enabled = false; cancel.Visible = authorize; cancel.Enabled = true; status.Text = authorize ? UiText.Get("setup.savingAuthorizing") : UiText.Get("setup.saving");
            try
            {
                await using (var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("windows-registration", cancellation.Token))
                {
                    var json = await LauncherConfiguration.SetupClientJsonAsync(storage, client.Text, cancellation.Token);
                    if (!authorize)
                    {
                        await OpeningPolicy.SaveAsync(storage, !noSync.Checked, folder.Text, consent.Checked, cancellation.Token);
                        ExtendedConfiguration.Save(storage, new(encoding.SelectedIndex == 0 ? "auto" : "windows-1252", delimiter.SelectedIndex switch { 1 => "comma", 2 => "semicolon", _ => "auto" }), extended.Checked);
                        await XlsReplacementSettings.SaveAsync(storage, xls.Checked, cancellation.Token);
                    }
                    LauncherConfiguration.SaveClient(storage, json);
                    new WindowsAssociationRegistration(Microsoft.Win32.Registry.CurrentUser).Register(Environment.ProcessPath!); WindowsAssociationRegistration.NotifyShell();
                }
                if (authorize)
                {
                    using var handler = new HttpClientHandler { AllowAutoRedirect = false }; using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(90) };
                    await Task.Run(() => new WindowsLauncher(storage, http, new BrowserLauncher()).LoginAsync(cancellation.Token));
                }
                if (authorize) { authorizationValid = true; connectionState.Text = UiText.Get("setup.authConfirmed"); connect.Text = UiText.Get("action.switchGoogleAccount"); }
                ExitCode = 0; status.Text = authorize ? UiText.Get("setup.authorizedReviewSettings") : UiText.Get("setup.saved");
                if (firstUse) finish.Enabled = Ready();
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { ExitCode = 1; if (authorize) { authorizationValid = false; connectionState.Text = UiText.Get("setup.authNotCompleted"); connect.Text = UiText.Get("action.authorizeGoogle"); if (firstUse) finish.Enabled = false; }
                status.Text = ex is ArgumentException ? UiText.Get("setup.consentRequired") : LauncherErrors.Message(ex); }
            finally { busy = false; layout.Enabled = true; verify.Enabled = !FirstUseState.NeedsAuthorization(storage); cancel.Visible = false; if (cancellation.IsCancellationRequested) { cancellation.Dispose(); cancellation = new CancellationTokenSource(); } }
        }
        connect.Click += async (_, _) => await Save(true); save.Click += async (_, _) => await Save(false);
        Ui.Primary(FirstUseState.NeedsAuthorization(storage) ? connect : save); layout.Controls.Add(Ui.Separator()); layout.Controls.Add(connect);
        layout.Controls.Add(Ui.Separator()); layout.Controls.Add(Ui.Text(UiText.Get("setup.defaultsHeading"), true)); layout.Controls.Add(Info(UiText.Get("setup.defaultsExplanation")));
        defaults.Click += (_, _) => { try { new BrowserLauncher().Open(WindowsAssociationPlan.DefaultsUri); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = UiText.Get("setup.windowsDefaultsInstructions"); } };
        layout.Controls.Add(defaults);
        layout.Controls.Add(Ui.Separator()); layout.Controls.Add(Ui.Text(UiText.Get("setup.syncHeading"), true));
        layout.Controls.Add(noSync); layout.Controls.Add(folderOptions); layout.Controls.Add(consent);
        layout.Controls.Add(Info(UiText.Get("setup.replacementExplanation")));
        layout.Controls.Add(save);
        ShowFolderOptions(); Ui.Adapt(folderOptions);
        async Task CheckConnection()
        {
            if (busy || FirstUseState.NeedsAuthorization(storage)) return;
            busy = true; verify.Enabled = connect.Enabled = false; connectionState.Text = UiText.Get("setup.authChecking");
            try
            {
                using var handler = new HttpClientHandler { AllowAutoRedirect = false };
                using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
                await new WindowsLauncher(storage, http, new BrowserLauncher()).CheckConnectionAsync(cancellation.Token);
                authorizationValid = true; connectionState.Text = UiText.Get("setup.authValid"); connect.Text = UiText.Get("action.switchGoogleAccount"); if (firstUse) finish.Enabled = Ready();
            }
            catch (AuthorizationRequiredException) { authorizationValid = false; connectionState.Text = UiText.Get("setup.authRenew"); connect.Text = UiText.Get("action.authorizeGoogle"); if (firstUse) finish.Enabled = false; }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { connectionState.Text = UiText.Get("setup.authSavedOffline"); }
            finally { busy = false; verify.Enabled = connect.Enabled = true; if (cancellation.IsCancellationRequested) { cancellation.Dispose(); cancellation = new CancellationTokenSource(); } }
        }
        verify.Click += async (_, _) => await CheckConnection();
        layout.Controls.Add(verify); layout.Controls.SetChildIndex(verify, layout.Controls.GetChildIndex(connect) + 1);
        if (!preview) Shown += async (_, _) => await CheckConnection();
        layout.Controls.Add(Ui.Separator()); layout.Controls.Add(toggle); layout.Controls.Add(status); 
        finish.Click += async (_, _) => { if (busy) return; if (firstUse) { await Save(false); if (ExitCode != 0 || !Ready()) return; } Close(); };
        Controls.Add(layout); Controls.Add(finish); finish.Dock = DockStyle.Bottom; finish.Height = 44; Controls.Add(cancel); cancel.Dock = DockStyle.Bottom;
        FormClosing += (_, e) => { if (busy) { e.Cancel = true; cancellation.Cancel(); } };
        Branding.Apply(this); Branding.Apply(advancedDialog); Ui.Adapt(layout); Ui.Adapt(advanced);
    }
    protected override void Dispose(bool disposing) { if (disposing) cancellation.Dispose(); base.Dispose(disposing); }
}
