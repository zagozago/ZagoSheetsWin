using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal sealed class AdvancedPreferencesForm : Form
{
    private readonly Func<string, Task> applyClient;
    internal Task ApplyClientAsync(string path) => applyClient(path);
    internal AdvancedPreferencesForm(LocalStorage storage, bool preview = false)
    {
        Text = UiText.Get("setup.advanced"); ClientSize = new Size(580, 550); MinimumSize = new Size(480, 480);
        AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterParent;
        var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18, 4, 18, 8), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };
        var status = Ui.Text(""); status.Visible = false;
        status.TextChanged += (_, _) => status.Visible = status.Text.Length > 0;
        var preferences = new PreferenceAutosave(this, layout, status, preview);
        var section = VisualStyle.Card(); layout.Controls.Add(section);
        void Heading(string key, Func<FlowLayoutPanel> help)
        {
            var row = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0, 2, 0, 2) };
            string HeadingText() => key == "setup.syncHeading" ? System.Text.RegularExpressions.Regex.Replace(UiText.Get(key), @"^\d+\.\s*", "") : UiText.Get(key);
            var label = Ui.Text(HeadingText(), true);
            void RefreshHeading() => label.Text = HeadingText();
            UiText.Changed += RefreshHeading; Disposed += (_, _) => UiText.Changed -= RefreshHeading;
            label.Margin = new Padding(0, 2, 0, 2);
            var info = new InformationButton(help) { AccessibleDescription = UiText.Get(key) };
            row.Controls.Add(label); row.Controls.Add(info);
            row.SizeChanged += (_, _) => label.MaximumSize = new Size(Math.Max(100, row.Width - info.Width - info.Margin.Horizontal - 12), 0);
            section.Controls.Add(row);
        }
        void Divider() { section = VisualStyle.Card(); layout.Controls.Add(section); }
        var connectionHeading = Ui.Text("", true); var connectionExplanation = Ui.Text("");
        void RefreshConnectionCopy()
        {
            var parts = UiText.Get("setup.connectionExplanation").Split('\n', 2);
            connectionHeading.Text = parts[0]; connectionExplanation.Text = parts.Length > 1 ? parts[1] : "";
        }
        RefreshConnectionCopy(); UiText.Changed += RefreshConnectionCopy; Disposed += (_, _) => UiText.Changed -= RefreshConnectionCopy;
        connectionHeading.Margin = new Padding(0, 2, 0, 4); connectionExplanation.Margin = new Padding(0, 2, 0, 6);
        section.Controls.Add(connectionHeading); section.Controls.Add(connectionExplanation);
        var clientState = new TextBox { ReadOnly = true, BorderStyle = BorderStyle.None, Tag = "status", Margin = new Padding(0, 8, 0, 2) };
        var chooseClient = new AdaptiveButton { Text = UiText.Get("setup.chooseClient") };
        void RefreshClient()
        {
            var configured = File.Exists(LauncherConfiguration.ClientPath(storage));
            clientState.Visible = configured;
            clientState.Text = configured ? UiText.Get("setup.clientPreserved") : "";
            chooseClient.Enabled = !configured;
        }
        VisualStyle.Icon(chooseClient, UiIcon.File);
        RefreshClient(); UiText.Changed += RefreshClient; Disposed += (_, _) => UiText.Changed -= RefreshClient;
        applyClient = path => preferences.SaveAsync(async () =>
        {
            await using var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("windows-registration");
            var json = await LauncherConfiguration.SetupClientJsonAsync(storage, path);
            LauncherConfiguration.SaveClient(storage, json);
            RefreshClient();
        }, RefreshClient);
        chooseClient.Click += async (_, _) =>
        {
            using var dialog = new OpenFileDialog { Filter = UiText.Get("dialog.oauthFileFilter"), CheckFileExists = true };
            if (dialog.ShowDialog(this) == DialogResult.OK) await ApplyClientAsync(dialog.FileName);
        };
        section.Controls.Add(chooseClient); section.Controls.Add(clientState); Divider();
        var opening = OpeningPolicy.Load(storage);
        var sync = new ToggleSwitch { Checked = !opening.RestrictToFolder, Text = UiText.Get("setup.noSync") };
        var consent = new ToggleSwitch { Checked = OpeningPolicy.IsConfigured(storage), Text = UiText.Get("setup.replacementConsent") };
        var folder = new TextBox { ReadOnly = true, Text = opening.Folder ?? "" };
        var folderOptions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Visible = !sync.Checked };
        var choose = new AdaptiveButton { Text = UiText.Get("setup.chooseFolder") };
        Heading("setup.syncHeading", () => InformationButton.TextPanel("setup.replacementExplanation"));
        section.Controls.Add(Ui.Pair(sync, consent));
        folderOptions.Controls.Add(Ui.Text(UiText.Get("setup.syncExplanation"))); folderOptions.Controls.Add(Ui.Pair(choose, folder)); section.Controls.Add(folderOptions);
        void RestoreOpening() { var saved = OpeningPolicy.Load(storage); sync.Checked = !saved.RestrictToFolder; folder.Text = saved.Folder ?? ""; consent.Checked = OpeningPolicy.IsConfigured(storage); folderOptions.Visible = !sync.Checked; }
        Task SaveOpening()
        {
            var restrict = !sync.Checked; var path = folder.Text;
            var acknowledged = CopyImportSettings.Load(storage) || !ShortcutSettings.Load(storage) || consent.Checked;
            return preferences.SaveAsync(() => OpeningPolicy.SaveAsync(storage, restrict, path, acknowledged), RestoreOpening);
        }
        choose.Click += async (_, _) => { using var dialog = new FolderBrowserDialog(); if (dialog.ShowDialog(this) == DialogResult.OK) { folder.Text = dialog.SelectedPath; await SaveOpening(); } };
        sync.CheckedChanged += async (_, _) =>
        {
            folderOptions.Visible = !sync.Checked;
            if (!preview && !sync.Checked && string.IsNullOrWhiteSpace(folder.Text))
            {
                using var dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog(this) != DialogResult.OK) { sync.Checked = true; return; }
                folder.Text = dialog.SelectedPath;
            }
            await SaveOpening();
        };
        consent.CheckedChanged += async (_, _) => await SaveOpening();
        Divider();
        Heading("setup.xlsHeading", () => InformationButton.TextPanel("setup.xlsExplanation"));
        var xls = new ToggleSwitch { Checked = XlsReplacementSettings.Load(storage), Text = UiText.Get("setup.replaceXls") }; section.Controls.Add(xls);
        xls.CheckedChanged += async (_, _) =>
        {
            var value = xls.Checked;
            await preferences.SaveAsync(() => XlsReplacementSettings.SaveAsync(storage, value), () => xls.Checked = XlsReplacementSettings.Load(storage));
        };
        layout.Controls.Add(status);
        var close = new AdaptiveButton { Text = UiText.Get("action.close"), Dock = DockStyle.Bottom, Height = 44 };
        close.Click += (_, _) => Close(); CancelButton = close;
        var footer = new Panel { Dock = DockStyle.Bottom, Height = 58, Padding = new Padding(18, 0, 18, 14) };
        close.Dock = DockStyle.Fill; footer.Controls.Add(close);
        Controls.Add(layout); Controls.Add(footer); Branding.Apply(this); Ui.Adapt(layout); Ui.Adapt(folderOptions);
    }
}
