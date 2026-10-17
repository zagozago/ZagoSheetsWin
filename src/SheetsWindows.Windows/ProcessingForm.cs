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
    private readonly long? startedAt;
    private readonly Func<TimeSpan, CancellationToken, Task>? retryDelay;
    private bool busy;
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
        Text = UiText.Get("processing.title"); ClientSize = new Size(620, 240); MinimumSize = new Size(620, 240);
        StartPosition = FormStartPosition.CenterScreen; AutoScaleMode = AutoScaleMode.Dpi;
        cancel.Click += (_, _) => { if (busy) { cancellation.Cancel(); cancel.Enabled = false; status.Text = UiText.Get("progress.stopping"); } else Close(); };
        actions.Controls.Add(cancel); Controls.Add(status); Controls.Add(actions);
        FormClosing += (_, e) => { if (busy) { e.Cancel = true; cancellation.Cancel(); cancel.Enabled = false; status.Text = UiText.Get("progress.stopping"); } };
        Branding.Apply(this, aboutButton: false, compact: true);
        if (preview) { status.Text = UiText.Get("progress.verification"); if (previewError) ShowFailure(new ConversionMismatchException()); }
        // Start after the modal/native message loop installs its UI synchronization context.
        else Shown += (_, _) => BeginInvoke((Action)(async () => await RunAsync()));
    }
    private void ShowFailure(Exception ex)
    {
        Text = UiText.Get("processing.failureTitle");
        status.Text = ex is OperationCanceledException && !cancellation.IsCancellationRequested ? UiText.Get("error.connectionTimeout") : LauncherErrors.Message(ex);
        var recovery = new AdaptiveButton { Text = UiText.Get("action.recovery"), AutoSize = true };
        recovery.Click += (_, _) => { using var form = new RecoveryForm(); form.ShowDialog(this); };
        var export = new AdaptiveButton { Text = UiText.Get("action.exportDiagnostics"), AutoSize = true };
        export.Click += async (_, _) =>
        {
            using var dialog = new SaveFileDialog { Filter = UiText.Get("dialog.diagnosticFilter"), FileName = DiagnosticLog.SuggestedFileName(request.Path), OverwritePrompt = true };
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            try { await new DiagnosticLog(LocalStorage.ForCurrentUser()).ExportAsync(dialog.FileName, overwrite: true, currentAttempt: latestDiagnostic); status.Text = UiText.Get("diagnostics.exported"); }
            catch (Exception failure) when (LauncherErrors.Expected(failure)) { status.Text = UiText.Get("diagnostics.destinationInvalid"); }
        };
        var setup = new AdaptiveButton { Text = UiText.Get("action.settings"), AutoSize = true };
        setup.Click += (_, _) => { using var form = new SetupForm(); form.ShowDialog(this); };
        actions.Controls.AddRange([recovery, export, setup]); cancel.Text = UiText.Get("action.close"); cancel.Enabled = true;
        if (request.Action is LauncherAction.Open or LauncherAction.Copy)
        {
            var retry = new AdaptiveButton { Text = UiText.Get("action.retry"), AutoSize = true };
            retry.Click += async (_, _) => { if (!busy) await RunAsync(); };
            actions.Controls.Add(retry);
        }
        ClientSize = new Size((int)(620 * DeviceDpi / 96.0), (int)(330 * DeviceDpi / 96.0)); Branding.Refresh(this);
    }
    private async Task RunAsync()
    {
        if (busy) return;
        // Successive modal loops can restore an empty ambient context. All awaits
        // below must return to this window's thread, including error-button creation.
        SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
        busy = true;
        ExitCode = 0;
        foreach (Control control in actions.Controls.Cast<Control>().Where(control => control != cancel).ToArray()) { actions.Controls.Remove(control); control.Dispose(); }
        cancel.Text = UiText.Get("action.cancel"); cancel.Enabled = true;
        Text = UiText.Get("processing.title");
        IProgress<string> progress = new Progress<string>(text => { if (!IsDisposed && !Disposing && busy && !cancellation.IsCancellationRequested) status.Text = text; });
        var telemetry = new ProcessingTelemetry(startedAt, progress); telemetry.MarkReady(); telemetry.SampleCpu();
        using var cpuSampler = new System.Windows.Forms.Timer { Interval = 100 };
        cpuSampler.Tick += (_, _) => telemetry.SampleCpu(); cpuSampler.Start();
        var diagnostics = new DiagnosticLog(LocalStorage.ForCurrentUser());
        var outcome = DiagnosticEvent.Completed;
        var attemptId = Guid.NewGuid();
        Exception? failure = null;
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
                    else if (request.Action == LauncherAction.Copy) await launcher.CopyAsync(request.Path!, progress, ct);
                    else await launcher.OpenAsync(request.Path!, progress, ct);
                });
                importNotice = launcher.ImportNotice;
            }
            if (request.Action == LauncherAction.Login) await ExecuteOnce(cancellation.Token);
            else await ImportRetry.ExecuteAsync(ExecuteOnce, (seconds, attempt) => progress.Report(UiText.Format("progress.retry", ("seconds", seconds), ("attempt", attempt))), cancellation.Token, retryDelay);
        }
        catch (Exception ex) when (LauncherErrors.Expected(ex))
        {
            failure = ex; ExitCode = 1; outcome = DiagnosticLog.Failure(ex);
            if (ex is not OperationCanceledException || !cancellation.IsCancellationRequested) ShowFailure(ex);
        }
        finally
        {
            cpuSampler.Stop();
            LastMetrics = telemetry.Capture();
            latestDiagnostic = DiagnosticLog.CreateEntry(outcome, metrics: LastMetrics, failure: failure, attemptId: attemptId, stage: telemetry.LastPhase, trace: telemetry.CaptureTrace());
            if (recordDiagnostics) await diagnostics.RecordAsync(outcome, metrics: LastMetrics, failure: failure, attemptId: attemptId, stage: telemetry.LastPhase, trace: telemetry.CaptureTrace());
            busy = false;
        }
        if (ExitCode == 0 && importNotice is not null)
        {
            ShowImportNotice(importNotice);
        }
        else if (ExitCode == 0 || cancellation.IsCancellationRequested) Close();
    }
    internal void ShowImportNotice(string notice)
    {
        Text = Branding.Name; status.Text = notice;
        cancel.Text = UiText.Get("action.close"); cancel.Enabled = true;
        var recovery = new AdaptiveButton { Text = UiText.Get("home.backups"), AutoSize = true, Name = "importNoticeBackups" };
        recovery.Click += (_, _) => { using var form = new RecoveryForm(); form.ShowDialog(this); };
        actions.Controls.Add(recovery);
        PerformLayout();
        var needed = TextRenderer.MeasureText(notice, status.Font,
            new Size(Math.Max(100, status.ClientSize.Width - status.Padding.Horizontal), int.MaxValue), TextFormatFlags.WordBreak).Height + status.Padding.Vertical + 8;
        if (status.ClientSize.Height < needed) ClientSize = new Size(ClientSize.Width, ClientSize.Height + needed - status.ClientSize.Height);
        MinimumSize = Size;
    }
    protected override void Dispose(bool disposing) { if (disposing) cancellation.Dispose(); base.Dispose(disposing); }
}

