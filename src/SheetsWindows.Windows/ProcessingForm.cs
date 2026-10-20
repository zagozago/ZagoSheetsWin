using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal sealed class ProcessingForm : Form
{
    private readonly Label status = new EmphasisLabel() { Dock = DockStyle.Fill, Padding = new Padding(18), TextAlign = ContentAlignment.MiddleLeft, Text = UiText.Get("progress.preparing"), AccessibleName = UiText.Get("accessibility.processingStatus") };
    private readonly Button cancel = new AdaptiveButton() { AutoSize = true, Text = UiText.Get("action.cancel") };
    private readonly FlowLayoutPanel actions = new() { Dock = DockStyle.Bottom, AutoSize = true, Padding = new Padding(12), WrapContents = true };
    private readonly CancellationTokenSource cancellation = new();
    private readonly LauncherRequest request;
    private readonly Func<IProgress<string>, CancellationToken, Task>? execute;
    private readonly bool recordDiagnostics;
    private readonly string memoryFailureMessage = new OutOfMemoryException().Message + "\n\n" + UiText.Get("error.generic");
    private readonly long? startedAt;
    private readonly Func<TimeSpan, CancellationToken, Task>? retryDelay;
    private bool busy;
    private int generation;
    private readonly System.Windows.Forms.Timer retryWatcher = new() { Interval = 500 };
    private (long Length, long Written)? inputStamp;
    private bool needsInputChange;
    private Action? updateRetry;
    private (long Length, long Written)? InputStamp()
    {
        try { var file = new FileInfo(request.Path!); return file.Exists ? (file.Length, file.LastWriteTimeUtc.Ticks) : null; }
        catch (Exception ex) when (LauncherErrors.Expected(ex)) { return null; }
    }
    private bool terminal;
    private Exception? displayedFailure;
    private string? diagnosticFeedback;
    private TextImportOptions? interpretation;
    private bool renderingFailure;
    private void RenderFailure()
    {
        if (displayedFailure is null || renderingFailure) return;
        renderingFailure = true;
        try
        {
            status.Text = (displayedFailure is OperationCanceledException && !cancellation.IsCancellationRequested
                ? UiText.Get("error.connectionTimeout") : LauncherErrors.Message(displayedFailure))
                + (needsInputChange ? "\n\n" + UiText.Get("retry.editInput") : "")
                + (diagnosticFeedback is null ? "" : "\n\n" + UiText.Get(diagnosticFeedback));
            if (IsHandleCreated && WindowState == FormWindowState.Normal && ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                PerformLayout();
                var needed = TextRenderer.MeasureText(status.Text, status.Font,
                    new Size(Math.Max(100, status.ClientSize.Width - status.Padding.Horizontal), int.MaxValue), TextFormatFlags.WordBreak).Height + status.Padding.Vertical + 8;
                if (status.ClientSize.Height < needed) ClientSize = new Size(ClientSize.Width, ClientSize.Height + needed - status.ClientSize.Height);
                // Windows may restore to the old minimum after minimizing a modal.
                // Keep enough room for the complete message, header and wrapped actions.
                MinimumSize = new Size(Math.Max(MinimumSize.Width, Width), Math.Max(MinimumSize.Height, Height));
            }
            status.Invalidate();
        }
        finally { renderingFailure = false; }
    }
    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (WindowState == FormWindowState.Normal) RenderFailure();
    }
    internal string StatusText => status.Text;
    internal void SetDiagnosticFeedback(string key) { diagnosticFeedback = key; RenderFailure(); }

    private Mutex? noticeMutex;
    private static bool noticeActive;
    private Task noticeSave = Task.CompletedTask;
    public int ExitCode { get; private set; }
    internal bool IsBusy => busy;
    internal ProcessingMetrics? LastMetrics { get; private set; }
    private DiagnosticEntry? latestDiagnostic;
    public ProcessingForm(LauncherRequest request, long? startedAt = null, bool preview = false, bool previewError = false,
        Func<IProgress<string>, CancellationToken, Task>? execute = null, bool recordDiagnostics = true,
        Func<TimeSpan, CancellationToken, Task>? retryDelay = null)
    {
        if (request.Action is not (LauncherAction.Open or LauncherAction.Copy or LauncherAction.Login)) throw new ArgumentException("Processing request required.");
        this.request = request; this.execute = execute; this.recordDiagnostics = recordDiagnostics; this.startedAt = startedAt ?? System.Diagnostics.Stopwatch.GetTimestamp();
        this.retryDelay = retryDelay;
        retryWatcher.Tick += (_, _) => updateRetry?.Invoke();
        Text = UiText.Get("processing.title"); ClientSize = new Size(620, 240); MinimumSize = new Size(620, 240);
        StartPosition = FormStartPosition.CenterScreen; AutoScaleMode = AutoScaleMode.Dpi;
        cancel.Click += (_, _) => { if (busy) { cancellation.Cancel(); cancel.Enabled = false; status.Text = UiText.Get("progress.stopping"); } else Close(); };
        actions.Controls.Add(cancel); Controls.Add(status); Controls.Add(actions);
        FormClosing += (_, e) => { if (busy) { e.Cancel = true; cancellation.Cancel(); cancel.Enabled = false; status.Text = UiText.Get("progress.stopping"); } };
        Branding.Apply(this, aboutButton: false, compact: true);
        UiText.Changed += RenderFailure;
        if (preview) { status.Text = UiText.Get("progress.verification"); if (previewError) ShowFailure(new ConversionMismatchException()); }
        // Start after the modal/native message loop installs its UI synchronization context.
        else Shown += (_, _) => BeginInvoke((Action)(async () => await RunSafelyAsync()));
    }
    private void ShowFailure(Exception ex)
    {
        Text = UiText.Get("processing.failureTitle");
        displayedFailure = ex; diagnosticFeedback = null; terminal = true; needsInputChange = ImportRetry.RequiresInputChange(ex); RenderFailure();
        var recovery = new AdaptiveButton { Text = UiText.Get("action.recovery"), AutoSize = true };
        recovery.Click += (_, _) => { using var form = new RecoveryForm(); form.ShowDialog(this); };
        var export = new AdaptiveButton { Text = UiText.Get("action.exportDiagnostics"), AutoSize = true };
        export.Click += async (_, _) =>
        {
            using var dialog = new SaveFileDialog { Filter = UiText.Get("dialog.diagnosticFilter"), FileName = DiagnosticLog.SuggestedFileName(request.Path), OverwritePrompt = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            try { await new DiagnosticLog(LocalStorage.ForCurrentUser()).ExportAsync(dialog.FileName, overwrite: true, currentAttempt: latestDiagnostic); SetDiagnosticFeedback("diagnostics.exported"); }
            catch (Exception failure) when (LauncherErrors.Expected(failure)) { SetDiagnosticFeedback("diagnostics.destinationInvalid"); }
        };
        var setup = new AdaptiveButton { Text = UiText.Get("action.settings"), AutoSize = true };
        setup.Click += (_, _) => { using var form = new SetupForm(); form.ShowDialog(this); };
        actions.Controls.AddRange([recovery, export, setup]); cancel.Text = UiText.Get("action.close"); cancel.Enabled = true;
        if (request.Action is LauncherAction.Open or LauncherAction.Copy)
        {
            var chooseSeparator = ex is InvalidDataException ambiguous && ambiguous.Message == "Ambiguous CSV delimiter; configure it explicitly.";
            var retry = new AdaptiveButton { Text = UiText.Get(chooseSeparator ? "action.chooseSeparator" : "action.retry"), AutoSize = true };
            if (chooseSeparator) Ui.Primary(retry);
            if (needsInputChange)
            {
                retry.Enabled = InputStamp() != inputStamp;
                updateRetry = () => { if (!retry.IsDisposed && !busy) retry.Enabled = InputStamp() != inputStamp; };
                retryWatcher.Start();
            }
            retry.Click += async (_, _) =>
            {
                if (busy) return;
                if (ex is InvalidDataException data && data.Message == "Ambiguous CSV delimiter; configure it explicitly.")
                {
                    using var choice = new TextInterpretationForm(sourcePath: request.Path);
                    if (choice.ShowDialog(this) != DialogResult.OK) return;
                    interpretation = choice.Options;
                }
                await RunSafelyAsync();
            };
            actions.Controls.Add(retry);
        }
        ClientSize = new Size((int)(620 * DeviceDpi / 96.0), (int)(330 * DeviceDpi / 96.0)); Branding.Refresh(this); RenderFailure();
    }
    private async Task RunSafelyAsync()
    {
        try { await RunAsync(); }
        catch (OutOfMemoryException)
        {
            // No retries, diagnostics serialization or new dialogs under memory pressure.
            busy = false; terminal = true; ExitCode = 1;
            status.Text = memoryFailureMessage;
            cancel.Text = UiText.Get("action.close"); cancel.Enabled = true;
        }
    }
    private async Task RunAsync()
    {
        if (busy) return;
        // Successive modal loops can restore an empty ambient context. All awaits
        // below must return to this window's thread, including error-button creation.
        SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
        retryWatcher.Stop(); updateRetry = null; inputStamp = InputStamp(); needsInputChange = false;
        busy = true; terminal = false; displayedFailure = null; diagnosticFeedback = null; latestDiagnostic = null;
        var currentGeneration = ++generation;
        ExitCode = 0;
        foreach (Control control in actions.Controls.Cast<Control>().Where(control => control != cancel).ToArray()) { actions.Controls.Remove(control); control.Dispose(); }
        cancel.Text = UiText.Get("action.cancel"); cancel.Enabled = true;
        Text = UiText.Get("processing.title");
        IProgress<string> progress = new Progress<string>(text => { if (!IsDisposed && !Disposing && busy && !terminal && generation == currentGeneration && !cancellation.IsCancellationRequested) status.Text = text; });
        var telemetry = new ProcessingTelemetry(startedAt, progress); telemetry.MarkReady(); telemetry.SampleCpu();
        using var cpuSampler = new System.Windows.Forms.Timer { Interval = 100 };
        cpuSampler.Tick += (_, _) => telemetry.SampleCpu(); cpuSampler.Start();
        var diagnostics = new DiagnosticLog(LocalStorage.ForCurrentUser());
        var outcome = DiagnosticEvent.Completed;
        var attemptId = Guid.NewGuid();
        Exception? failure = null;
        var memoryExhausted = false;
        string? importNotice = null;
        if (recordDiagnostics) await diagnostics.RecordAsync(DiagnosticEvent.Started, attemptId: attemptId);
        try
        {
            status.Text = request.Action == LauncherAction.Login ? UiText.Get("progress.authorizing") : UiText.Get("progress.fileAndBackup");
            async Task ExecuteOnce(CancellationToken ct)
            {
                if (execute is not null) { await execute(progress, ct); return; }
                using var handler = new HttpClientHandler { AllowAutoRedirect = false };
                using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(90) };
                var launcher = new WindowsLauncher(LocalStorage.ForCurrentUser(), http, new BrowserLauncher(), telemetry);
                await Task.Run(async () =>
                {
                    if (request.Action == LauncherAction.Login) await launcher.LoginAsync(ct);
                    else if (request.Action == LauncherAction.Copy) await launcher.CopyAsync(request.Path!, progress, ct, interpretation);
                    else await launcher.OpenAsync(request.Path!, progress, ct, interpretation);
                });
                importNotice = launcher.ImportNotice;
            }
            if (request.Action == LauncherAction.Login) await ExecuteOnce(cancellation.Token);
            else await ImportRetry.ExecuteAsync(ExecuteOnce, (seconds, attempt) => progress.Report(UiText.Format("progress.retry", ("seconds", seconds), ("attempt", attempt))), cancellation.Token, retryDelay);
        }
        catch (OutOfMemoryException) { memoryExhausted = true; throw; }
        catch (Exception ex) when (LauncherErrors.Expected(ex))
        {
            failure = ex; ExitCode = 1; outcome = DiagnosticLog.Failure(ex);
            terminal = true;
        }
        finally
        {
            cpuSampler.Stop();
            if (!memoryExhausted)
            {
                LastMetrics = telemetry.Capture();
                latestDiagnostic = DiagnosticLog.CreateEntry(outcome, operation: telemetry.OperationId, metrics: LastMetrics, failure: failure, attemptId: attemptId, stage: telemetry.LastPhase, trace: telemetry.CaptureTrace());
                if (recordDiagnostics) await diagnostics.RecordAsync(outcome, operation: telemetry.OperationId, metrics: LastMetrics, failure: failure, attemptId: attemptId, stage: telemetry.LastPhase, trace: telemetry.CaptureTrace());
            }
            busy = false;
        }
        if (failure is not null && (failure is not OperationCanceledException || !cancellation.IsCancellationRequested)) ShowFailure(failure);
        if (ExitCode == 0 && !cancellation.IsCancellationRequested && importNotice is not null)
        {
            var replacement = importNotice == UiText.Format("import.notice.partialConversion", ("backups", UiText.Get("home.backups").Replace("&", "", StringComparison.Ordinal)));
            if (replacement && !AcquireNotice()) { Close(); return; }
            ShowImportNotice(importNotice, replacement);
        }
        else if (ExitCode == 0 || cancellation.IsCancellationRequested) Close();
    }
    internal bool AcquireNotice()
    {
        if (noticeActive) return false;
        var storage = LocalStorage.ForCurrentUser();
        if (ImportNoticeSettings.Load(storage)) return false;
        var key = Convert.ToHexString(System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(storage.Root)));
        var gate = new Mutex(false, @"Local\ZSW-import-notice-" + key);
        bool acquired;
        try { acquired = gate.WaitOne(0); }
        catch (AbandonedMutexException) { acquired = true; }
        if (!acquired) { gate.Dispose(); return false; }
        if (ImportNoticeSettings.Load(storage)) { gate.ReleaseMutex(); gate.Dispose(); return false; }
        noticeMutex = gate;
        noticeActive = true;
        return true;
    }
    internal void ShowImportNotice(string notice, bool replacement = true)
    {
        Text = Branding.Name; status.Text = notice;
        cancel.Text = UiText.Get("action.close"); cancel.Enabled = true;
        var recovery = new AdaptiveButton { Text = UiText.Get("home.backups"), AutoSize = true, Name = "importNoticeBackups" };
        recovery.Click += (_, _) => { using var form = new RecoveryForm(); form.ShowDialog(this); };
        actions.Controls.Add(recovery);
        if (replacement)
        {
            var hide = new CheckBox { Name = "importNoticeDontShowAgain", Text = UiText.Get("import.notice.dontShowAgain"), Checked = false,
                AutoSize = true, Dock = DockStyle.Bottom, Padding = new Padding(18, 6, 18, 6) };
            Controls.Add(hide);
            Controls.SetChildIndex(hide, Controls.GetChildIndex(actions));
            async Task SaveNoticePreference(Task previous, bool hidden)
            {
                await previous;
                try { await ImportNoticeSettings.SaveAsync(LocalStorage.ForCurrentUser(), hidden); }
                catch (Exception ex) when (LauncherErrors.Expected(ex)) { if (!IsDisposed) status.Text = LauncherErrors.Message(ex); }
            }
            hide.CheckedChanged += (_, _) => noticeSave = SaveNoticePreference(noticeSave, hide.Checked);
            FormClosing += async (_, e) =>
            {
                if (noticeSave.IsCompleted) return;
                e.Cancel = true; await noticeSave; if (!IsDisposed) Close();
            };
        }
        PerformLayout();
        var needed = TextRenderer.MeasureText(notice, status.Font,
            new Size(Math.Max(100, status.ClientSize.Width - status.Padding.Horizontal), int.MaxValue), TextFormatFlags.WordBreak).Height + status.Padding.Vertical + 8;
        if (status.ClientSize.Height < needed) ClientSize = new Size(ClientSize.Width, ClientSize.Height + needed - status.ClientSize.Height);
        MinimumSize = Size;
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing) { UiText.Changed -= RenderFailure; retryWatcher.Dispose(); cancellation.Dispose(); if (noticeMutex is not null) { noticeMutex.ReleaseMutex(); noticeMutex.Dispose(); noticeMutex = null; noticeActive = false; } }
        base.Dispose(disposing);
    }
}

