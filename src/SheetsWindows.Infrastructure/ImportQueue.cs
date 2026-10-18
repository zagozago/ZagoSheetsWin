using SheetsWindows.Core;

namespace SheetsWindows.Infrastructure;

// One complete import per local profile, including verification and backup completion.
// Explorer starts separate processes: ownership is a file handle, released on exit/crash.
// Always acquire BEFORE any source/account/maintenance lock; no queue lock in lower layers.
public sealed class ImportQueue(LocalStorage storage, ProcessingTelemetry? telemetry = null)
{
    public const string LockKey = "import-queue";
    public async ValueTask<IAsyncDisposable> EnterAsync(CancellationToken ct = default)
    {
        telemetry?.Mark(ProcessingStep.ImportQueue, CheckpointOutcome.Started);
        var turn = await new FileOperationLock(storage.LocksPath, TimeSpan.FromHours(1)).AcquireAsync(LockKey, ct);
        telemetry?.Mark(ProcessingStep.ImportQueue);
        return turn;
    }
}
