using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal sealed class RecoveryForm : Form
{
    private bool busy;
    private CancellationTokenSource? activeCancellation;
    private int sortColumn = -1;
    private bool sortAscending;
    internal void SortByColumn(int column)
    {
        if (column is < 0 or > 3) return;
        sortAscending = column != sortColumn || !sortAscending;
        sortColumn = column;
        entries.ListViewItemSorter = new BackupRowComparer(column, sortAscending);
        entries.Sort();
    }
    private sealed class BackupRowComparer(int column, bool ascending) : System.Collections.IComparer
    {
        public int Compare(object? x, object? y)
        {
            var left = (ListViewItem)x!; var right = (ListViewItem)y!;
            var a = (RecoveryEntry)left.Tag!; var b = (RecoveryEntry)right.Tag!;
            var result = column switch
            {
                1 => Nullable.Compare(a.CapturedAt, b.CapturedAt),
                2 => a.Bytes.CompareTo(b.Bytes),
                _ => StringComparer.CurrentCultureIgnoreCase.Compare(left.SubItems[column].Text, right.SubItems[column].Text)
            };
            if (result == 0) result = StringComparer.CurrentCultureIgnoreCase.Compare(left.Text, right.Text);
            if (result == 0) result = a.Id.CompareTo(b.Id);
            return ascending ? result : -result;
        }
    }
    private readonly ListView entries = new ListView() { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, MultiSelect = true, CheckBoxes = false, HideSelection = false, AccessibleName = UiText.Get("accessibility.availableBackups") };
    private RecoveryEntry? Selected => entries.SelectedItems.Count != 1 ? null : entries.SelectedItems[0].Tag as RecoveryEntry;
    private void AddEntry(RecoveryEntry entry)
    {
        if (entry.CleanupState == 2) return;
        var item = new ListViewItem(Path.GetFileName(entry.OriginalPath)) { Tag = entry, ToolTipText = entry.OriginalPath };
        item.SubItems.Add(entry.CapturedAt?.ToLocalTime().ToString("dd/MM/yyyy HH:mm") ?? "-");
        item.SubItems.Add(UiText.Format("recovery.sizeMb", ("sizeMb", entry.Bytes / 1_000_000d)));
        item.SubItems.Add(entry.CleanupState == 2 ? UiText.Get("recovery.state.cleaned") : entry.CleanupState == 1 ? UiText.Get("recovery.state.cleaning") : !entry.Available ? UiText.Get("recovery.state.unavailable") : entry.CanClean ? UiText.Get("recovery.state.completed") : UiText.Get("recovery.state.protected")); entries.Items.Add(item);
    }
    private readonly Label status = new EmphasisLabel() { Dock = DockStyle.Top, Height = 85, Padding = new Padding(12), Text = UiText.Get("recovery.instructions") };
    public RecoveryForm(bool preview = false, bool previewBusy = false, LocalStorage? policyStorage = null)
    {
        Text = UiText.Get("recovery.title"); ClientSize = new Size(980, 680); MinimumSize = new Size(760, 560); AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterScreen;
        void FitStatus() { var width = status.Parent?.ClientSize.Width ?? ClientSize.Width; status.MaximumSize = new Size(Math.Max(120, width), 0); status.Height = status.GetPreferredSize(new Size(width, 0)).Height; }
        SizeChanged += (_, _) => FitStatus();
        status.TextChanged += (_, _) => FitStatus();
        Shown += (_, _) => FitStatus();
        entries.Columns.Add(UiText.Get("recovery.column.file"), 230); entries.Columns.Add(UiText.Get("recovery.column.date"), 140); entries.Columns.Add(UiText.Get("recovery.column.size"), 80); entries.Columns.Add(UiText.Get("recovery.column.state"), 180); entries.ShowItemToolTips = true;
        entries.ColumnClick += (_, e) => SortByColumn(e.Column);
        var storage = policyStorage ?? LocalStorage.ForCurrentUser();
        var manager = new BackupManagement(storage);
        var service = new BackupRecovery(storage);
        var policyPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(12) };
        var days = new NumericUpDown { Minimum = 1, Maximum = 365, Value = 30, Width = 70, AccessibleName = UiText.Get("accessibility.retentionDays") };
        var quota = new NumericUpDown { Minimum = 1, Maximum = 1000, Value = 200, Width = 80, AccessibleName = UiText.Get("accessibility.quotaMb") };
        var usage = new EmphasisLabel { AutoSize = true, Text = UiText.Get("recovery.limits"), Font = new Font("Segoe UI", 8, FontStyle.Bold), Margin = new Padding(10), Name = "backupStatistics", AccessibleName = UiText.Get("accessibility.availableBackups") };
        var statistics = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, BorderStyle = BorderStyle.FixedSingle, FlowDirection = FlowDirection.TopDown, WrapContents = false, Margin = new Padding(0, 18, 0, 0) };
        statistics.Controls.Add(usage);
        var limits = new TableLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, ColumnCount = 2, RowCount = 2, Margin = new Padding(0, 0, 0, 8) };
        limits.ColumnStyles.Add(new(SizeType.Percent, 50)); limits.ColumnStyles.Add(new(SizeType.Percent, 50));
        limits.RowStyles.Add(new(SizeType.AutoSize)); limits.RowStyles.Add(new(SizeType.AutoSize));
        var daysLabel = new Label { Text = UiText.Get("recovery.retentionLabel"), AutoSize = true };
        var quotaLabel = new Label { Text = UiText.Get("recovery.quotaLabel"), AutoSize = true };
        limits.Controls.Add(daysLabel, 0, 0); limits.Controls.Add(quotaLabel, 1, 0); limits.Controls.Add(days, 0, 1); limits.Controls.Add(quota, 1, 1);
        void FitPolicy()
        {
            var width = Math.Max(120, policyPanel.ClientSize.Width - policyPanel.Padding.Horizontal - 22);
            usage.MaximumSize = new Size(Math.Max(80, width - 24), 0);
            statistics.MinimumSize = new Size(width, 0); statistics.MaximumSize = new Size(width, 0);
            foreach (var label in policyPanel.Controls.OfType<Label>()) label.MaximumSize = new Size(width, 0);
            limits.MinimumSize = new Size(width, 0); limits.MaximumSize = new Size(width, 0); limits.Width = width;
            daysLabel.MaximumSize = quotaLabel.MaximumSize = new Size(Math.Max(45, width/2-6), 0);
            days.Width = quota.Width = Math.Max(45, width/2-6);
        }
        policyPanel.SizeChanged += (_, _) => FitPolicy();
        policyPanel.Controls.AddRange([limits, new Label { AutoSize = true, Text = UiText.Get("error.backupQuotaTip") }, statistics]);
        var restore = new AdaptiveButton { Text = UiText.Get("action.restore"), AutoSize = true };
        var repair = new MenuActionButton { Text = UiText.Get("recovery.repairNew"), AutoSize = true };
        var copy = new MenuActionButton { Text = UiText.Get("recovery.resumeCopy"), AutoSize = true };
        var resume = new MenuActionButton { Text = UiText.Get("recovery.completeReplacement"), AutoSize = true };
        var export = new MenuActionButton { Text = UiText.Get("action.exportDiagnosticHistory"), AutoSize = true };
        var cancel = new AdaptiveButton { Text = UiText.Get("recovery.cancelResume"), AutoSize = true, Enabled = previewBusy, Visible = previewBusy };
        cancel.Click += (_, _) => activeCancellation?.Cancel();
        var delete = new MenuActionButton { Text = UiText.Get("recovery.deleteSelected"), AutoSize = true };
        var clean = new MenuActionButton { Text = UiText.Get("recovery.cleanExpired"), AutoSize = true };
        var more = new AdaptiveButton { Text = UiText.Get("recovery.moreActions"), AutoSize = true, Height = 34 };
        var menu = new ContextMenuStrip();
        foreach (var action in new[] { copy, resume, repair, export, clean })
        {
            var item = new ToolStripMenuItem(action.Text);
            item.Click += (_, _) => action.InvokeAction();
            menu.Items.Add(item);
        }
        more.Click += (_, _) => { var buttons = new[] { copy, resume, repair, export, clean }; for (var i = 0; i < buttons.Length; i++) { menu.Items[i].Enabled = buttons[i].Enabled && !busy; menu.Items[i].Text = buttons[i].Text; } menu.Show(more, new Point(0, more.Height)); };
        var selectAll = new AdaptiveButton { Text = UiText.Get("recovery.selectAll"), AutoSize = true };
        var clearSelection = new AdaptiveButton { Text = UiText.Get("recovery.clearSelection"), AutoSize = true };
        selectAll.Click += (_, _) => { foreach (ListViewItem item in entries.Items) item.Selected = (item.Tag as RecoveryEntry)?.CleanupState != 2; };
        clearSelection.Click += (_, _) => { foreach (ListViewItem item in entries.Items) { item.Checked = false; item.Selected = false; } };
        var actions = Ui.Columns(restore, selectAll, clearSelection, delete, more);
        actions.Dock = DockStyle.Top; actions.Padding = new Padding(8);
        // Keep action buttons parented so native PerformClick executes their existing handlers.
        var hiddenActions = new Panel { Visible = false }; hiddenActions.Controls.AddRange([copy, resume, repair, export, clean]); Controls.Add(hiddenActions);
        Disposed += (_, _) => menu.Dispose(); Ui.Primary(restore);
        var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
        main.ColumnStyles.Add(new(SizeType.Percent, 75)); main.ColumnStyles.Add(new(SizeType.Percent, 25)); main.RowStyles.Add(new(SizeType.Percent, 100));
        var files = new Panel { Dock = DockStyle.Fill }; files.Controls.Add(entries); files.Controls.Add(actions); files.Controls.Add(status);
        files.SizeChanged += (_, _) => FitStatus(); main.Controls.Add(files, 0, 0); main.Controls.Add(policyPanel, 1, 0);
        Controls.Add(main); Controls.Add(cancel); cancel.Dock = DockStyle.Bottom;
        long shownBytes = 0; int shownCount = 0, shownProtected = 0;
        void RefreshUsage() { usage.Text = UiText.Format("recovery.usage", ("usedMb", shownBytes / 1_000_000d), ("limitMb", quota.Value), ("backupCount", shownCount), ("protectedCount", shownProtected)).Replace(" · ", "\n"); FitPolicy(); }
        UiText.Changed += RefreshUsage; Disposed += (_, _) => UiText.Changed -= RefreshUsage;
        quota.ValueChanged += (_, _) => RefreshUsage();
        async Task Resume(bool replace, TextImportOptions? repairOptions = null)
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
                await Task.Run(() => repairOptions is null ? launcher.ResumeAsync(entry.Id, replace, ct: cancellation.Token) : launcher.RepairAsync(entry.Id, repairOptions, cancellation.Token));
                await diagnostics.RecordAsync(DiagnosticEvent.Completed, entry.Id);
                await RefreshEntries();
                status.Text = replace ? UiText.Get("recovery.replacementCompleted") : UiText.Get("recovery.copyCompleted");
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { await diagnostics.RecordAsync(DiagnosticLog.Failure(ex), entry.Id, failure: ex); status.Text = LauncherErrors.Message(ex); }
            finally { activeCancellation = null; cancel.Enabled = false; cancel.Visible = false; busy = false; actions.Enabled = true; entries.Enabled = true; UpdateSelection(); }
        }
        repair.Click += async (_, _) =>
        {
            if (busy || Selected is not { Available: true }) return;
            using var choice = new TextInterpretationForm(repair: true);
            if (choice.ShowDialog(this) == DialogResult.OK) await Resume(false, choice.Options);
        };
        copy.Click += async (_, _) => await Resume(false);
        resume.Click += async (_, _) => await Resume(true);
        export.Click += async (_, _) =>
        {
            if (busy) return;
            using var dialog = new SaveFileDialog { Filter = UiText.Get("dialog.diagnosticFilter"), FileName = "ZSW-history.jsonl", OverwritePrompt = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            try { await new DiagnosticLog(LocalStorage.ForCurrentUser()).ExportAsync(dialog.FileName, overwrite: true); status.Text = UiText.Get("diagnostics.exported"); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = UiText.Get("diagnostics.fileExists"); }
        };
        FormClosing += (_, e) => { if (busy) { e.Cancel = true; activeCancellation?.Cancel(); } };
        async Task RefreshEntries()
        {
            var result = await Task.Run(() => (Rows: service.List(), Summary: manager.Inspect()));
            entries.Items.Clear(); foreach (var row in result.Rows) AddEntry(row);
            shownBytes = result.Summary.UsedBytes; shownCount = result.Rows.Count; shownProtected = result.Rows.Count(e => !e.CanClean && e.Available); RefreshUsage();
            if (entries.Items.Count == 0) status.Text = UiText.Get("recovery.noBackups");
        }
        var loadingPolicy = !preview;
        if (!preview) Shown += async (_, _) =>
        {
            busy = true;
            try { var policy = BackupPolicy.Load(storage); days.Value = policy.RetentionDays; quota.Value = policy.QuotaMb; await RefreshEntries(); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = LauncherErrors.Message(ex); }
            finally { busy = false; loadingPolicy = false; }
        };
        Task pendingPolicySave = Task.CompletedTask;
        async Task PersistPolicy(Task previous, BackupPolicy policy)
        {
            await previous;
            try
            {
                await BackupPolicy.SaveAsync(storage, policy);
                if (!IsDisposed) status.Text = UiText.Get("recovery.policySaved");
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex))
            { if (!IsDisposed) status.Text = LauncherErrors.Message(ex); }
        }
        void SavePolicy(object? sender, EventArgs args)
        {
            if (loadingPolicy || preview && policyStorage is null) return;
            // Capture every edit and write in order, including edits made while a save awaits its lock.
            pendingPolicySave = PersistPolicy(pendingPolicySave, new((int)days.Value, (int)quota.Value, true));
        }
        days.ValueChanged += SavePolicy; quota.ValueChanged += SavePolicy;
        FormClosing += async (_, e) =>
        {
            if (pendingPolicySave.IsCompleted) return;
            e.Cancel = true;
            await pendingPolicySave;
            if (!IsDisposed && !busy) Close();
        };
        RecoveryEntry[] Marked() => entries.SelectedItems.Cast<ListViewItem>().Select(item => (RecoveryEntry)item.Tag!).ToArray();
        async Task DeleteBackups(bool selected)
        {
            if (busy || preview) return;
            busy = true; actions.Enabled = false; restore.Enabled = false; policyPanel.Enabled = false; entries.Enabled = false;
            try
            {
                var marked = selected ? Marked() : [];
                var protectedCount = marked.Count(entry => !entry.CanClean);
                var ids = selected ? marked.Select(entry => entry.Id).Distinct().ToArray() : (await Task.Run(() => manager.Plan())).ToArray();
                if (ids.Length == 0) { status.Text = UiText.Get("recovery.noEligibleBackup"); return; }
                var prompt = UiText.Format(protectedCount > 0 ? "recovery.discardPrompt" : "recovery.deletePrompt", ("count", ids.Length), ("protectedCount", protectedCount));
                if (MessageBox.Show(this, prompt, UiText.Get("recovery.deleteTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
                var result = await Task.Run(async () => { var count = 0; long bytes = 0; foreach (var id in ids) { var cleaned = selected ? await manager.DiscardAsync(id) : await manager.DeleteAsync(id); count += cleaned.Count; bytes += cleaned.Bytes; } return new CleanupResult(count, bytes); }); await RefreshEntries(); status.Text = UiText.Format("recovery.deleted", ("count", result.Count), ("freedMb", result.Bytes / 1_000_000d));
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = LauncherErrors.Message(ex); }
            finally { busy = false; actions.Enabled = true; policyPanel.Enabled = true; entries.Enabled = true; UpdateSelection(); }
        }
        delete.Click += async (_, _) => await DeleteBackups(true);
        clean.Click += async (_, _) => await DeleteBackups(false);
        void UpdateSelection()
        {
            var row = Selected;
            repair.Enabled = restore.Enabled = copy.Enabled = resume.Enabled = !busy && row?.Available == true;
            delete.Enabled = !busy && Marked().Any(entry => entry.CleanupState != 2);
        }
        entries.SelectedIndexChanged += (_, _) => UpdateSelection();
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
            shownBytes = 3_300_000; shownCount = entries.Items.Count; shownProtected = 1; RefreshUsage();
        }
        UpdateSelection();
        Branding.Apply(this);
    }
}

internal sealed class MenuActionButton : AdaptiveButton
{
    internal void InvokeAction() { if (Enabled) OnClick(EventArgs.Empty); }
}
