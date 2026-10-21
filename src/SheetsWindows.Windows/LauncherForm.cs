using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal sealed class LauncherForm : Form
{
    public int ExitCode { get; private set; }
    public LauncherForm(LauncherRequest request)
    {
        if (request.Action != LauncherAction.Home) throw new ArgumentException("Home request required.");
        Text = "ZagoSheetsWin"; ClientSize = new Size(540, 240); FormBorderStyle = FormBorderStyle.FixedSingle; MaximizeBox = false; MinimizeBox = true; StartPosition = FormStartPosition.CenterScreen; AutoScaleMode = AutoScaleMode.Dpi;
        var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18, 2, 18, 14), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = false };
        Button Action(string text, EventHandler action) { var b = new AdaptiveButton { Text = text, Width = 382, Height = 38, AccessibleName = text.Replace("&", "") }; b.Click += action; return b; }
        void Pick(LauncherAction action)
        {
            using var dialog = new OpenFileDialog { Filter = UiText.Get("dialog.spreadsheetFilter"), CheckFileExists = true, Multiselect = false, Title = action == LauncherAction.Copy ? UiText.Get("dialog.importCopyTitle") : UiText.Get("dialog.openSpreadsheetTitle") };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            using var processing = new ProcessingForm(new LauncherRequest(action, dialog.FileName)); processing.ShowDialog(this); ExitCode = processing.ExitCode;
        }
        var tagline = Ui.Text(UiText.Get("tagline.open_spreadsheets_automatically"));
        tagline.Font = new Font(LanguageVisuals.Family, 9); tagline.Margin = new Padding(0, 0, 0, 10); layout.Controls.Add(tagline);
        var open = Action(UiText.Get("home.open"), (_, _) => Pick(LauncherAction.Open)); Ui.Primary(open); VisualStyle.Icon(open, UiIcon.Folder); layout.Controls.Add(open);
        var navigation = new TableLayoutPanel { ColumnCount = 3, RowCount = 1, Height = 48, Width = 440, Margin = new Padding(0, 4, 0, 4) };
        for (var i = 0; i < 3; i++) navigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 3));
        var settings = Action(UiText.Get("home.settings"), (_, _) => { using var setup = new SetupForm(); setup.ShowDialog(this); });
        var backups = Action(UiText.Get("home.backups"), (_, _) => { using var recovery = new RecoveryForm(); recovery.ShowDialog(this); });
        var help = Action(UiText.Get("home.help"), (_, _) =>
        {
            // Web manual complements the offline first-use wizard and optional startup tutorial.
            var url = "https://zagotools.top/apps/zagosheetswin/ajuda/?lang=" +
                Uri.EscapeDataString(UiText.Language);
            try { new BrowserLauncher().Open(new Uri(url)); }
            catch (Exception ex) when (LauncherErrors.Expected(ex))
            {
                MessageBox.Show(this, url, UiText.Get("home.help"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
        VisualStyle.Icon(settings, UiIcon.Settings); VisualStyle.Icon(backups, UiIcon.Backup); VisualStyle.Icon(help, UiIcon.Help);
        foreach (var button in new[] { settings, backups, help }) { button.Dock = DockStyle.Fill; navigation.Controls.Add(button); }
        layout.Controls.Add(navigation);
        Controls.Add(layout); Branding.Apply(this, compact: true); Ui.Adapt(layout);
    }
}
