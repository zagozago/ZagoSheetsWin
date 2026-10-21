using SheetsWindows.Core;

namespace SheetsWindows.Infrastructure;

// One scope covers the whole user invocation, including automatic network retries.
// A waiter cannot treat the operation it just waited for as an abandoned upload.
public sealed class SpreadsheetOpening : IAsyncDisposable
{
    private readonly IAsyncDisposable held;
    public IReadOnlySet<Guid> Candidates { get; }
    private SpreadsheetOpening(IAsyncDisposable held, IReadOnlySet<Guid> candidates)
    { this.held = held; Candidates = candidates; }

    public static async Task<SpreadsheetOpening> AcquireAsync(LocalStorage storage, string path, bool allowFreshAttempt = true, CancellationToken ct = default)
    {
        string identity;
        await using (var source = new SourceReader(copyEnvironments: true).Open(path)) identity = source.Source.IdentityKey;
        var locks = new FileOperationLock(storage.LocksPath);
        var key = "opening:" + identity;
        var held = locks.TryAcquire(key);
        var uncontended = held is not null;
        held ??= await locks.AcquireAsync(key, ct);
        try
        {
            // Also respect importers/recovery from existing versions that only hold the source lock.
            await using var sourceLock = locks.TryAcquire(identity);
            var candidates = allowFreshAttempt && uncontended && sourceLock is not null
                ? new SqliteOperationRegistry(storage.DatabasePath).Pending()
                    .Where(op => op.SourceKey == identity && op.Snapshot is not null)
                    .Select(op => op.Id).ToHashSet()
                : new HashSet<Guid>();
            return new(held, candidates);
        }
        catch { await held.DisposeAsync(); throw; }
    }
    public ValueTask DisposeAsync() => held.DisposeAsync();
}
