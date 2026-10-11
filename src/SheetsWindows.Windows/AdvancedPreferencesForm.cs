using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal sealed class AdvancedPreferencesForm : Form
{
    internal AdvancedPreferencesForm(LocalStorage storage, bool preview = false)
    {
        Text = UiText.Get("setup.advanced"); ClientSize = new Size(560, 630); MinimumSize = new Size(480, 480);
        AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterParent;
        var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18, 4, 18, 8), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };
        var status = Ui.Text(""); status.Visible = false;
        status.TextChanged += (_, _) => status.Visible = status.Text.Length > 0;
        var preferences = new PreferenceAutosave(this, layout, status, preview);
        void Heading(string key, Func<FlowLayoutPanel> help)
        {
            var row = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0, 4, 0, 4) };
            string HeadingText() => key == "setup.syncHeading" ? System.Text.RegularExpressions.Regex.Replace(UiText.Get(key), @"^\d+\.\s*", "") : UiText.Get(key);
            var label = Ui.Text(HeadingText(), true);
            void RefreshHeading() => label.Text = HeadingText();
            UiText.Changed += RefreshHeading; Disposed += (_, _) => UiText.Changed -= RefreshHeading;
            label.Margin = new Padding(0, 2, 0, 2);
            var info = new InformationButton(help) { AccessibleDescription = UiText.Get(key) };
            row.Controls.Add(label); row.Controls.Add(info);
            row.SizeChanged += (_, _) => label.MaximumSize = new Size(Math.Max(100, row.Width - info.Width - info.Margin.Horizontal - 12), 0);
            layout.Controls.Add(row);
        }
        var opening = OpeningPolicy.Load(storage);
        var sync = new ToggleSwitch { Checked = !opening.RestrictToFolder, Text = UiText.Get("setup.noSync") };
        var consent = new ToggleSwitch { Checked = OpeningPolicy.IsConfigured(storage), Text = UiText.Get("setup.replacementConsent") };
        var folder = new TextBox { ReadOnly = true, Text = opening.Folder ?? "" };
        var folderOptions = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Visible = !sync.Checked };
        var choose = new AdaptiveButton { Text = UiText.Get("setup.chooseFolder") };
        Heading("setup.syncHeading", () => InformationButton.TextPanel("setup.replacementExplanation"));
        layout.Controls.Add(Ui.Pair(sync, consent));
        folderOptions.Controls.Add(Ui.Text(UiText.Get("setup.syncExplanation"))); folderOptions.Controls.Add(Ui.Pair(choose, folder)); layout.Controls.Add(folderOptions);
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
        layout.Controls.Add(Ui.Separator());
        // Reuse the two existing paragraphs verbatim, in their respective places.
        string FormatPart(int part) { var blocks = UiText.Get("setup.formatLimits").Replace("\\n", "\n").Split("\n\n", 2); return part < blocks.Length ? blocks[part] : ""; }
        Heading("setup.formatsHeading", () => { var panel = new FlowLayoutPanel(); panel.Controls.Add(Ui.Text(FormatPart(1))); return panel; });
        var extended = new ToggleSwitch { Checked = true, Text = UiText.Get("setup.extendedFormats") };
        var encoding = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        encoding.Items.AddRange([UiText.Get("setup.encodingUnicode"), UiText.Get("setup.encodingLegacy")]); encoding.SelectedIndex = 0;
        var delimiter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        delimiter.Items.AddRange([UiText.Get("setup.delimiterAuto"), UiText.Get("setup.delimiterComma"), UiText.Get("setup.delimiterSemicolon")]); delimiter.SelectedIndex = 0;
        void RestoreFormats()
        {
            try { var saved = ExtendedConfiguration.Load(storage); extended.Checked = true; extended.Enabled = encoding.Enabled = delimiter.Enabled = false; encoding.SelectedIndex = saved.Encoding == "auto" ? 0 : 1; delimiter.SelectedIndex = saved.Delimiter switch { "comma" => 1, "semicolon" => 2, _ => 0 }; }
            catch (LauncherNotConfiguredException) { encoding.Enabled = delimiter.Enabled = extended.Checked; }
        }
        RestoreFormats();
        layout.Controls.Add(extended); layout.Controls.Add(encoding); layout.Controls.Add(delimiter);
        var limits = Ui.Text(FormatPart(0)); layout.Controls.Add(limits);
        void RefreshLimits() => limits.Text = FormatPart(0);
        UiText.Changed += RefreshLimits; Disposed += (_, _) => UiText.Changed -= RefreshLimits;
        Task SaveFormats()
        {
            var options = new TextImportOptions(encoding.SelectedIndex == 0 ? "auto" : "windows-1252", delimiter.SelectedIndex switch { 1 => "comma", 2 => "semicolon", _ => "auto" });
            var enabled = extended.Checked;
            return preferences.SaveAsync(() => { ExtendedConfiguration.Save(storage, options, enabled); return Task.CompletedTask; }, RestoreFormats);
        }
        extended.CheckedChanged += async (_, _) => { encoding.Enabled = delimiter.Enabled = extended.Checked; await SaveFormats(); };
        encoding.SelectedIndexChanged += async (_, _) => await SaveFormats(); delimiter.SelectedIndexChanged += async (_, _) => await SaveFormats();
        layout.Controls.Add(Ui.Separator()); Heading("setup.xlsHeading", () => InformationButton.TextPanel("setup.xlsExplanation"));
        var xls = new ToggleSwitch { Checked = XlsReplacementSettings.Load(storage), Text = UiText.Get("setup.replaceXls") }; layout.Controls.Add(xls);
        xls.CheckedChanged += async (_, _) =>
        {
            var value = xls.Checked;
            await preferences.SaveAsync(() => XlsReplacementSettings.SaveAsync(storage, value), () => xls.Checked = XlsReplacementSettings.Load(storage));
        };
        layout.Controls.Add(status);
        var close = new AdaptiveButton { Text = UiText.Get("action.close"), Dock = DockStyle.Bottom, Height = 44 };
        close.Click += (_, _) => Close(); CancelButton = close;
        Controls.Add(layout); Controls.Add(close); Branding.Apply(this); Ui.Adapt(layout); Ui.Adapt(folderOptions);
    }
}
