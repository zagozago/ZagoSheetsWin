using System.Diagnostics;
using SheetsWindows.Core;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class ImportQueueTests
{
    [Theory] [InlineData(12)] [InlineData(20)]
    public async Task BatchRotatesCompletedBackupsWithoutCompetingForTheSameSpace(int count)
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!);
        await BackupPolicy.SaveAsync(storage, new(30, 1));
        var paths = Enumerable.Range(0, count).Select(i => Path.Combine(w.Root, $"batch-{i}.csv")).ToArray();
        foreach (var path in paths) File.WriteAllBytes(path, new byte[600_000]);
        var active = 0; var maximum = 0; var completed = 0;
        await Task.WhenAll(paths.Select(async path =>
        {
            await using var turn = await new ImportQueue(storage).EnterAsync();
            maximum = Math.Max(maximum, ++active);
            try
            {
                var manager = new BackupManagement(storage);
                await manager.MaintainAsync(); await manager.MakeRoomAsync(new FileInfo(path).Length);
                var op = await storage.CreatePreparation().PrepareAsync("A", path);
                Assert.Equal(600_000, manager.UsedBytes());
                // A completed, verified copy is the earliest point at which this backup can rotate.
                var remote = new GoogleRemoteRegistry(Path.Combine(storage.Root, "google.db")); var key = "sheet:" + op.Id.ToString("N");
                remote.Begin(key, "A", "sheet", op.Snapshot!.Sha256); remote.Candidate(key, "sheet-id"); remote.Verify(key, "sheet-id");
                await manager.RegisterCopyCompletionAsync(new(op, GoogleDriveClient.Editor("sheet-id")));
                completed++;
            }
            finally { active--; }
        }));
        Assert.Equal(1, maximum); Assert.Equal(count, completed);
        Assert.Equal(600_000, new BackupManagement(storage).UsedBytes());
        Assert.All(paths, path => Assert.Equal(600_000, new FileInfo(path).Length));
        Assert.Single(new BackupManagement(storage).Inspect().Entries, e => e.Available);
    }
    [Fact]
    public async Task QueueSupportsCancellationAndCrashRecoveryAcrossProcesses()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!);
        var marker = Path.Combine(w.Root, "queue-ready");
        var start = new ProcessStartInfo(Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet") { UseShellExecute = false, RedirectStandardError = true, RedirectStandardOutput = true };
        start.ArgumentList.Add(typeof(SheetsWindows.LockProbe.Marker).Assembly.Location);
        start.ArgumentList.Add(storage.LocksPath); start.ArgumentList.Add(ImportQueue.LockKey); start.ArgumentList.Add(marker);
        using var process = Process.Start(start)!;
        try
        {
            var timer = Stopwatch.StartNew(); while (!File.Exists(marker) && !process.HasExited && timer.Elapsed < TimeSpan.FromSeconds(15)) await Task.Delay(25);
            Assert.True(File.Exists(marker), process.HasExited ? await process.StandardError.ReadToEndAsync() : "probe timeout");
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(100));
            await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => { await using var turn = await new ImportQueue(storage).EnterAsync(cancellation.Token); });
            process.Kill(); await process.WaitForExitAsync();
            await using var recovered = await new ImportQueue(storage).EnterAsync();
        }
        finally { if (!process.HasExited) { process.Kill(); await process.WaitForExitAsync(); } }
    }
}
