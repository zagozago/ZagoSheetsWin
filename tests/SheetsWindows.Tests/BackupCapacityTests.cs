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
        var ready = 0;
        var tasks = Enumerable.Range(0, count).Select(async i =>
        {
            var path = Path.Combine(w.Root, $"parallel-{i}.csv"); File.WriteAllBytes(path, new byte[600_000]);
            var op = await storage.CreatePreparation().PrepareAsync("A", path, timeout.Token);
            if (Interlocked.Increment(ref ready) == count) allUploading.SetResult();
            // Simulate arbitrarily slow Google calls. Every file must pass backup admission first.
            await allUploading.Task.WaitAsync(timeout.Token);
            await Complete(storage, op);
        }).ToArray();
        await Task.WhenAll(tasks);
        Assert.Equal(count, ready);
        Assert.Equal(count * 600_000L, new BackupManagement(storage).UsedBytes());
    }

    [Theory] [InlineData(12)] [InlineData(20)]
    public async Task FullBatchFailsWithoutWaitingRotatingOrTouchingOriginals(int count)
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!);
        await BackupPolicy.SaveAsync(storage, new(30, 1));
        File.WriteAllBytes(w.Source, new byte[600_000]);
        var completed = await storage.CreatePreparation().PrepareAsync("A", w.Source);
        await Complete(storage, completed);
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        await Task.WhenAll(Enumerable.Range(0, count).Select(async i =>
        {
            var path = Path.Combine(w.Root, $"full-{i}.csv"); var original = new byte[600_000]; original[^1] = (byte)i;
            File.WriteAllBytes(path, original);
            await Assert.ThrowsAsync<BackupQuotaException>(() => storage.CreatePreparation().PrepareAsync("A", path, timeout.Token));
            Assert.Equal(original, File.ReadAllBytes(path));
        }));
        Assert.True(File.Exists(completed.Snapshot!.BackupPath));
        Assert.Equal(600_000, new BackupManagement(storage).UsedBytes());
    }
}
