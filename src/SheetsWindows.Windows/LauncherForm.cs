using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal sealed class LauncherForm : Form
{
    private readonly Label status = new EmphasisLabel() { AutoSize = true, MaximumSize = new Size(382, 0), Text = UiText.Get("home.description") };
    public int ExitCode { get; private set; }
    public LauncherForm(LauncherRequest request, bool expanded = false)
    {
        if (request.Action != LauncherAction.Home) throw new ArgumentException("Home request required.");
        Text = "ZagoSheetsWin"; ClientSize = new Size(500, 380); MinimumSize = new Size(500, 380); StartPosition = FormStartPosition.CenterScreen; AutoScaleMode = AutoScaleMode.Dpi;
        var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };
        Button Action(string text, EventHandler action) { var b = new Button { Text = text, Width = 382, Height = 38, AccessibleName = text.Replace("&", "") }; b.Click += action; return b; }
        void Pick(LauncherAction action)
        {
            using var dialog = new OpenFileDialog { Filter = UiText.Get("dialog.spreadsheetFilter"), CheckFileExists = true, Multiselect = false, Title = action == LauncherAction.Copy ? UiText.Get("dialog.importCopyTitle") : UiText.Get("dialog.openSpreadsheetTitle") };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            using var processing = new ProcessingForm(new LauncherRequest(action, dialog.FileName)); processing.ShowDialog(this); ExitCode = processing.ExitCode;
        }
        layout.Controls.Add(status);
        var open = Action(UiText.Get("home.open"), (_, _) => Pick(LauncherAction.Open)); Ui.Primary(open); layout.Controls.Add(open); layout.Controls.Add(Ui.Separator());
        var navigation = new TableLayoutPanel { ColumnCount = 3, RowCount = 1, Height = 48, Width = 440, Margin = new Padding(0, 4, 0, 12) };
        for (var i = 0; i < 3; i++) navigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / 3));
        var settings = Action(UiText.Get("home.settings"), (_, _) => { using var setup = new SetupForm(); setup.ShowDialog(this); });
        var backups = Action(UiText.Get("home.backups"), (_, _) => { using var recovery = new RecoveryForm(); recovery.ShowDialog(this); });
        var help = Action(UiText.Get("home.help"), (_, _) => { using var tutorial = new TutorialForm(); tutorial.ShowDialog(this); });
        foreach (var button in new[] { settings, backups, help }) { button.Dock = DockStyle.Fill; button.Font = new Font("Segoe UI", 9); navigation.Controls.Add(button); }
        layout.Controls.Add(navigation);
        var advanced = new FlowLayoutPanel { AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Visible = false };
        var toggle = new Button { Text = UiText.Get("home.advancedCollapsed"), AutoSize = true, AccessibleName = UiText.Get("accessibility.showAdvanced") };
        toggle.Click += (_, _) => { advanced.Visible = !advanced.Visible; toggle.Text = advanced.Visible ? UiText.Get("home.advancedExpanded") : UiText.Get("home.advancedCollapsed"); toggle.AccessibleName = advanced.Visible ? UiText.Get("accessibility.hideAdvanced") : UiText.Get("accessibility.showAdvanced"); ClientSize = new Size(ClientSize.Width, (int)((advanced.Visible ? 530 : 380) * DeviceDpi / 96.0)); };
        advanced.Controls.Add(Ui.Text(UiText.Get("home.copyExplanation")));
        advanced.Controls.Add(Action(UiText.Get("home.importCopy"), (_, _) => Pick(LauncherAction.Copy)));
        advanced.Controls.Add(Action(UiText.Get("home.openShortcutsFolder"), (_, _) =>
        {
            var path = Path.Combine(LocalStorage.ForCurrentUser().Root, "shortcuts");
            try { if (Directory.Exists(path)) System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(path) { UseShellExecute = true }); else status.Text = UiText.Get("home.noCopyShortcut"); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = UiText.Get("home.openFolderFailed"); }
        }));
        layout.Controls.Add(toggle); layout.Controls.Add(advanced); Controls.Add(layout); Branding.Apply(this, compact: true); Ui.Adapt(layout); Ui.Adapt(advanced);
        if (expanded) { advanced.Visible = true; toggle.Text = UiText.Get("home.advancedExpanded"); toggle.AccessibleName = UiText.Get("accessibility.hideAdvanced"); ClientSize = new Size(500, 530); }
    }
}
