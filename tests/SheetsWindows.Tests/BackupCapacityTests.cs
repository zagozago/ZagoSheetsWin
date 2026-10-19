using SheetsWindows.Core;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class BackupCapacityTests
{
    private static async Task Complete(LocalStorage storage, ImportOperation op)
    {
        var remote = new GoogleRemoteRegistry(Path.Combine(storage.Root, "google.db"));
        var key = "sheet:" + op.Id.ToString("N");
        remote.Begin(key, "A", "sheet", op.Snapshot!.Sha256); remote.Candidate(key, "sheet-id"); remote.Verify(key, "sheet-id");
        await new BackupManagement(storage).RegisterCopyCompletionAsync(new(op, GoogleDriveClient.Editor("sheet-id")));
    }

    [Theory] [InlineData(12)] [InlineData(20)]
    public async Task AvailableCapacityAllowsWholeBatchToReachUploadBeforeAnyUploadFinishes(int count)
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!);
        await BackupPolicy.SaveAsync(storage, new(30, 20));
        var allUploading = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var ready = 0; var waits = 0;
        var tasks = Enumerable.Range(0, count).Select(async i =>
        {
            var path = Path.Combine(w.Root, $"parallel-{i}.csv"); File.WriteAllBytes(path, new byte[600_000]);
            var op = await new BackupManagement(storage).PrepareWithCapacityAsync(
                () => storage.CreatePreparation().PrepareAsync("A", path, timeout.Token), 600_000,
                waiting: () => Interlocked.Increment(ref waits), ct: timeout.Token);
            if (Interlocked.Increment(ref ready) == count) allUploading.SetResult();
            // Simulate arbitrarily slow Google calls. Every file must pass backup admission first.
            await allUploading.Task.WaitAsync(timeout.Token);
            await Complete(storage, op);
        }).ToArray();
        await Task.WhenAll(tasks);
        Assert.Equal(count, ready); Assert.Equal(0, waits);
        Assert.Equal(count * 600_000L, new BackupManagement(storage).UsedBytes());
    }

    [Theory] [InlineData(12)] [InlineData(20)]
    public async Task FullBatchWaitsOnlyForCapacityAndRotatesCompletedBackups(int count)
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!);
        await BackupPolicy.SaveAsync(storage, new(30, 1));
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(45));
        var waits = 0;
        await Task.WhenAll(Enumerable.Range(0, count).Select(async i =>
        {
            var path = Path.Combine(w.Root, $"full-{i}.csv"); File.WriteAllBytes(path, new byte[600_000]);
            var manager = new BackupManagement(storage);
            var op = await manager.PrepareWithCapacityAsync(
                () => storage.CreatePreparation().PrepareAsync("A", path, timeout.Token), 600_000,
                waiting: () => Interlocked.Increment(ref waits), ct: timeout.Token);
            Assert.InRange(manager.UsedBytes(), 600_000, 1_000_000);
            await Task.Delay(10, timeout.Token); // Keep this pending briefly while the next capture competes.
            await Complete(storage, op);
            Assert.Equal(600_000, new FileInfo(path).Length);
        }));
        Assert.True(waits > 0);
        Assert.Equal(600_000, new BackupManagement(storage).UsedBytes());
        Assert.Single(new BackupManagement(storage).Inspect().Entries, e => e.Available);
    }

    [Fact]
    public async Task ProtectedPendingBackupWaitIsCancelableWithoutHoldingMaintenanceOrSource()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!);
        await BackupPolicy.SaveAsync(storage, new(30, 1)); File.WriteAllBytes(w.Source, new byte[600_000]);
        var pending = await storage.CreatePreparation().PrepareAsync("A", w.Source);
        var other = Path.Combine(w.Root, "waiting.csv"); File.WriteAllBytes(other, new byte[600_000]);
        var waiting = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancel = new CancellationTokenSource();
        var task = new BackupManagement(storage).PrepareWithCapacityAsync(
            () => storage.CreatePreparation().PrepareAsync("A", other, cancel.Token), 600_000,
            waiting: () => waiting.SetResult(), ct: cancel.Token);
        await waiting.Task.WaitAsync(TimeSpan.FromSeconds(10));
        cancel.Cancel(); await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
        await using var source = new SourceReader().Open(other);
        await using var heldSource = await new FileOperationLock(storage.LocksPath).AcquireAsync(source.Source.IdentityKey);
        await using var heldMaintenance = await new FileOperationLock(storage.LocksPath).AcquireAsync(BackupManagement.LockKey);
        Assert.True(File.Exists(pending.Snapshot!.BackupPath)); Assert.Equal(600_000, new FileInfo(other).Length);
    }
    [Fact]
    public async Task AutomaticRotationSkipsBusyCompletedSourceAndLaterRetriesIt()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!);
        await BackupPolicy.SaveAsync(storage, new(30, 1)); File.WriteAllBytes(w.Source, new byte[600_000]);
        var op = await storage.CreatePreparation().PrepareAsync("A", w.Source); await Complete(storage, op);
        var manager = new BackupManagement(storage);
        await using (var held = await new FileOperationLock(storage.LocksPath).AcquireAsync(op.SourceKey))
        {
            using var deadline = new CancellationTokenSource(TimeSpan.FromSeconds(2));
            await manager.MakeRoomAsync(600_000, deadline.Token);
            Assert.True(File.Exists(op.Snapshot!.BackupPath));
        }
        await manager.MakeRoomAsync(600_000);
        Assert.False(File.Exists(op.Snapshot!.BackupPath)); Assert.True(File.Exists(w.Source));
    }

}
