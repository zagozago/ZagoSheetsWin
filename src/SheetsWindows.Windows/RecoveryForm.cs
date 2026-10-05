using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal sealed class RecoveryForm : Form
{
    private bool busy;
    private CancellationTokenSource? activeCancellation;
    private readonly ListView entries = new() { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, MultiSelect = true, CheckBoxes = true, HideSelection = false, AccessibleName = UiText.Get("accessibility.availableBackups") };
    private RecoveryEntry? Selected => entries.SelectedItems.Count != 1 ? null : entries.SelectedItems[0].Tag as RecoveryEntry;
    private void AddEntry(RecoveryEntry entry)
    {
        var item = new ListViewItem(Path.GetFileName(entry.OriginalPath)) { Tag = entry, ToolTipText = entry.OriginalPath };
        item.SubItems.Add(entry.CapturedAt?.ToLocalTime().ToString("dd/MM/yyyy HH:mm") ?? "-");
        item.SubItems.Add(UiText.Format("recovery.sizeMb", ("sizeMb", entry.Bytes / 1_000_000d)));
        item.SubItems.Add(entry.CleanupState == 2 ? UiText.Get("recovery.state.cleaned") : entry.CleanupState == 1 ? UiText.Get("recovery.state.cleaning") : !entry.Available ? UiText.Get("recovery.state.unavailable") : entry.CanClean ? UiText.Get("recovery.state.completed") : UiText.Get("recovery.state.protected")); entries.Items.Add(item);
    }
    private readonly Label status = new EmphasisLabel() { Dock = DockStyle.Top, Height = 85, Padding = new Padding(12), Text = UiText.Get("recovery.instructions") };
    public RecoveryForm(bool preview = false, bool previewBusy = false)
    {
        Text = UiText.Get("recovery.title"); ClientSize = new Size(700, 650); MinimumSize = new Size(560, 540); AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterScreen;
        void FitStatus() { status.MaximumSize = new Size(Math.Max(120, ClientSize.Width), 0); status.Height = status.GetPreferredSize(new Size(ClientSize.Width, 0)).Height; }
        SizeChanged += (_, _) => FitStatus();
        status.TextChanged += (_, _) => FitStatus();
        Shown += (_, _) => FitStatus();
        entries.Columns.Add(UiText.Get("recovery.column.file"), 230); entries.Columns.Add(UiText.Get("recovery.column.date"), 140); entries.Columns.Add(UiText.Get("recovery.column.size"), 80); entries.Columns.Add(UiText.Get("recovery.column.state"), 180); entries.ShowItemToolTips = true;
        var storage = LocalStorage.ForCurrentUser();
        var manager = new BackupManagement(storage);
        var service = new BackupRecovery(storage);
        var policyPanel = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(8) };
        var days = new NumericUpDown { Minimum = 1, Maximum = 365, Value = 30, Width = 70, AccessibleName = UiText.Get("accessibility.retentionDays") };
        var quota = new NumericUpDown { Minimum = 1, Maximum = 1000, Value = 200, Width = 80, AccessibleName = UiText.Get("accessibility.quotaMb") };
        var automatic = new CheckBox { Text = UiText.Get("recovery.autoCleanup"), AutoSize = true };
        var save = new AdaptiveButton { Text = UiText.Get("recovery.savePolicy"), AutoSize = true };
        var usage = new EmphasisLabel { AutoSize = true, Text = UiText.Get("recovery.limits") };
        policyPanel.SizeChanged += (_, _) => usage.MaximumSize = new Size(Math.Max(120, policyPanel.ClientSize.Width - policyPanel.Padding.Horizontal - usage.Margin.Horizontal), 0);
        policyPanel.Controls.AddRange([new Label { Text = UiText.Get("recovery.retentionLabel"), AutoSize = true }, days, new Label { Text = UiText.Get("recovery.quotaLabel"), AutoSize = true }, quota, automatic, save, usage]);
        var restore = new AdaptiveButton { Text = UiText.Get("action.restore"), Dock = DockStyle.Bottom, Height = 44 };
        var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Padding = new Padding(8) };
        var copy = new MenuActionButton { Text = UiText.Get("recovery.resumeCopy"), AutoSize = true };
        var resume = new MenuActionButton { Text = UiText.Get("recovery.completeReplacement"), AutoSize = true };
        var export = new MenuActionButton { Text = UiText.Get("action.exportDiagnostics"), AutoSize = true };
        var cancel = new AdaptiveButton { Text = UiText.Get("recovery.cancelResume"), AutoSize = true, Enabled = previewBusy, Visible = previewBusy };
        cancel.Click += (_, _) => activeCancellation?.Cancel();
        var delete = new MenuActionButton { Text = UiText.Get("recovery.deleteSelected"), AutoSize = true };
        var clean = new MenuActionButton { Text = UiText.Get("recovery.cleanExpired"), AutoSize = true };
        var more = new AdaptiveButton { Text = UiText.Get("recovery.moreActions"), AutoSize = true, Height = 34 };
        var menu = new ContextMenuStrip();
        foreach (var action in new[] { copy, resume, export, delete, clean })
        {
            var item = new ToolStripMenuItem(action.Text);
            item.Click += (_, _) => action.InvokeAction();
            menu.Items.Add(item);
        }
        more.Click += (_, _) => { var buttons = new[] { copy, resume, export, delete, clean }; for (var i = 0; i < buttons.Length; i++) { menu.Items[i].Enabled = buttons[i].Enabled && !busy; menu.Items[i].Text = buttons[i].Text; } menu.Show(more, new Point(0, more.Height)); };
        var selectAll = new AdaptiveButton { Text = UiText.Get("recovery.selectAll"), AutoSize = true };
        var clearSelection = new AdaptiveButton { Text = UiText.Get("recovery.clearSelection"), AutoSize = true };
        selectAll.Click += (_, _) => { foreach (ListViewItem item in entries.Items) item.Checked = (item.Tag as RecoveryEntry)?.CanClean == true; };
        clearSelection.Click += (_, _) => { foreach (ListViewItem item in entries.Items) { item.Checked = false; item.Selected = false; } };
        actions.Controls.AddRange([selectAll, clearSelection, delete, more]);
        // Keep action buttons parented so native PerformClick executes their existing handlers.
        var hiddenActions = new Panel { Visible = false }; hiddenActions.Controls.AddRange([copy, resume, export, clean]); Controls.Add(hiddenActions);
        Disposed += (_, _) => menu.Dispose(); Ui.Primary(restore);
        Controls.Add(entries); Controls.Add(policyPanel); Controls.Add(status); Controls.Add(actions); Controls.Add(restore); Controls.Add(cancel); cancel.Dock = DockStyle.Bottom;
        async Task Resume(bool replace)
        {
            if (busy || Selected is not RecoveryEntry { Available: true } entry) return;
            busy = true; actions.Enabled = false; restore.Enabled = false; entries.Enabled = false;
            using var cancellation = new CancellationTokenSource(); activeCancellation = cancellation; cancel.Visible = true; cancel.Enabled = true;
            var diagnostics = new DiagnosticLog(LocalStorage.ForCurrentUser());
            await diagnostics.RecordAsync(DiagnosticEvent.Started, entry.Id);
            try
            {
                using var handler = new HttpClientHandler { AllowAutoRedirect = false };
                using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(90) };
                var launcher = new WindowsLauncher(LocalStorage.ForCurrentUser(), http, new BrowserLauncher());
                await Task.Run(() => launcher.ResumeAsync(entry.Id, replace, ct: cancellation.Token));
                await diagnostics.RecordAsync(DiagnosticEvent.Completed, entry.Id);
                await RefreshEntries();
                status.Text = replace ? UiText.Get("recovery.replacementCompleted") : UiText.Get("recovery.copyCompleted");
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { await diagnostics.RecordAsync(DiagnosticLog.Failure(ex), entry.Id, failure: ex); status.Text = LauncherErrors.Message(ex); }
            finally { activeCancellation = null; cancel.Enabled = false; cancel.Visible = false; busy = false; actions.Enabled = true; entries.Enabled = true; UpdateSelection(); }
        }
        copy.Click += async (_, _) => await Resume(false);
        resume.Click += async (_, _) => await Resume(true);
        export.Click += async (_, _) =>
        {
            if (busy) return;
            using var dialog = new SaveFileDialog { Filter = UiText.Get("dialog.diagnosticFilter"), FileName = "sheets-windows-diagnostico.jsonl", OverwritePrompt = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            try { await new DiagnosticLog(LocalStorage.ForCurrentUser()).ExportAsync(dialog.FileName); status.Text = UiText.Get("diagnostics.exported"); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = UiText.Get("diagnostics.fileExists"); }
        };
        FormClosing += (_, e) => { if (busy) { e.Cancel = true; activeCancellation?.Cancel(); } };
        async Task RefreshEntries()
        {
            var result = await Task.Run(() => (Rows: service.List(), Summary: manager.Inspect()));
            entries.Items.Clear(); foreach (var row in result.Rows) AddEntry(row);
            usage.Text = UiText.Format("recovery.usage", ("usedMb", result.Summary.UsedBytes / 1_000_000d), ("limitMb", quota.Value), ("backupCount", result.Summary.Entries.Count), ("protectedCount", result.Summary.Entries.Count(e => !e.CanClean && e.Available)));
            if (entries.Items.Count == 0) status.Text = UiText.Get("recovery.noBackups");
        }
        if (!preview) Shown += async (_, _) =>
        {
            busy = true;
            try { var policy = BackupPolicy.Load(storage); days.Value = policy.RetentionDays; quota.Value = policy.QuotaMb; automatic.Checked = policy.AutomaticCleanup; await RefreshEntries(); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = LauncherErrors.Message(ex); }
            finally { busy = false; }
        };
        save.Click += async (_, _) =>
        {
            if (busy || preview) return;
            if (automatic.Checked && MessageBox.Show(this, UiText.Get("recovery.enableCleanupPrompt"), UiText.Get("recovery.enableCleanupTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            busy = true;
            try { await BackupPolicy.SaveAsync(storage, new((int)days.Value, (int)quota.Value, automatic.Checked)); status.Text = UiText.Get("recovery.policySaved"); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = LauncherErrors.Message(ex); }
            finally { busy = false; }
        };
        RecoveryEntry[] Marked() => (entries.CheckedItems.Count > 0 ? entries.CheckedItems.Cast<ListViewItem>() : entries.SelectedItems.Cast<ListViewItem>()).Select(item => (RecoveryEntry)item.Tag!).ToArray();
        async Task DeleteBackups(bool selected)
        {
            if (busy || preview) return;
            busy = true; actions.Enabled = false; restore.Enabled = false; policyPanel.Enabled = false; entries.Enabled = false;
            try
            {
                var marked = selected ? Marked() : [];
                var protectedCount = marked.Count(entry => !entry.CanClean);
                var ids = selected ? marked.Where(entry => entry.CanClean).Select(entry => entry.Id).Distinct().ToArray() : (await Task.Run(() => manager.Plan())).ToArray();
                if (ids.Length == 0) { status.Text = UiText.Get("recovery.noEligibleBackup"); return; }
                if (MessageBox.Show(this, UiText.Format("recovery.deletePrompt", ("count", ids.Length), ("protectedCount", protectedCount)), UiText.Get("recovery.deleteTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                var result = await Task.Run(() => manager.CleanAsync(ids)); await RefreshEntries(); status.Text = UiText.Format("recovery.deleted", ("count", result.Count), ("freedMb", result.Bytes / 1_000_000d));
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = LauncherErrors.Message(ex); }
            finally { busy = false; actions.Enabled = true; policyPanel.Enabled = true; entries.Enabled = true; UpdateSelection(); }
        }
        delete.Click += async (_, _) => await DeleteBackups(true);
        clean.Click += async (_, _) => await DeleteBackups(false);
        void UpdateSelection()
        {
            var row = Selected;
            restore.Enabled = copy.Enabled = resume.Enabled = !busy && row?.Available == true;
            delete.Enabled = !busy && Marked().Any(entry => entry.CanClean);
        }
        entries.SelectedIndexChanged += (_, _) => UpdateSelection();
        entries.ItemChecked += (_, _) => UpdateSelection();
        restore.Click += async (_, _) =>
        {
            if (busy || Selected is not RecoveryEntry { Available: true } entry) { status.Text = UiText.Get("recovery.selectBackup"); return; }
            using var dialog = new SaveFileDialog { Filter = UiText.Get("dialog.originalFileFilter") + Path.GetExtension(entry.OriginalPath), DefaultExt = Path.GetExtension(entry.OriginalPath), FileName = Path.GetFileNameWithoutExtension(entry.OriginalPath) + "-restaurado" + Path.GetExtension(entry.OriginalPath), OverwritePrompt = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            busy = true; restore.Enabled = false; entries.Enabled = false; actions.Enabled = false;
            try { await Task.Run(() => service.RestoreAsync(entry.Id, dialog.FileName)); status.Text = UiText.Get("recovery.restored"); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = UiText.Get("recovery.restoreFailed"); }
            finally { busy = false; entries.Enabled = true; actions.Enabled = true; UpdateSelection(); }
        };
        if (preview)
        {
            foreach (var entry in new RecoveryEntry[] {
                new RecoveryEntry(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Relatório.xlsx", 2_400_000, 4, new DateTimeOffset(2026, 9, 1, 12, 0, 0, TimeSpan.Zero), true, true),
                new RecoveryEntry(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Orçamento.csv", 300_000, 4, new DateTimeOffset(2026, 9, 2, 12, 0, 0, TimeSpan.Zero), true, true),
                new RecoveryEntry(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Importação pendente.csv", 600_000, 1, new DateTimeOffset(2026, 10, 1, 12, 0, 0, TimeSpan.Zero)),
                new RecoveryEntry(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Arquivo antigo.xls", 100_000, 4, new DateTimeOffset(2026, 8, 1, 12, 0, 0, TimeSpan.Zero), false, false, 2) }) AddEntry(entry);
            usage.Text = UiText.Get("recovery.previewUsage");
        }
        UpdateSelection();
        Branding.Apply(this);
    }
}

internal sealed class MenuActionButton : AdaptiveButton
{
    internal void InvokeAction() { if (Enabled) OnClick(EventArgs.Empty); }
}
