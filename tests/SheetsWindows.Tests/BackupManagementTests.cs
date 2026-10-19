using SheetsWindows.Core;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class BackupManagementTests
{
    private static LocalStorage Storage(Workspace w) => new(Path.GetDirectoryName(w.Database)!);
    private static async Task<ImportOperation> Completed(Workspace w, DateTimeOffset? captured = null, bool replacement = false, string? sourcePath = null)
    {
        var storage = Storage(w); var op = await w.Coordinator().PrepareAsync("A", sourcePath ?? w.Source);
        var remote = new GoogleRemoteRegistry(Path.Combine(storage.Root, "google.db")); var key = "sheet:" + op.Id.ToString("N");
        remote.Begin(key, "A", "sheet", op.Snapshot!.Sha256); remote.Candidate(key, "sheet-id"); remote.Verify(key, "sheet-id");
        var catalog = new BackupCatalog(storage); catalog.Register(op.Id, op.Snapshot, captured ?? DateTimeOffset.UtcNow);
        if (replacement)
        {
            var journal = new ReplacementJournal(Path.Combine(storage.Root, "replacement.db")); journal.Begin(op.Id, w.Source + ".url", GoogleDriveClient.Editor("sheet-id"), w.Source);
            for (var step = 0; step < 4; step++) journal.Advance(op.Id, step, step + 1);
        }
        else await new BackupManagement(storage).RegisterCopyCompletionAsync(new(op, GoogleDriveClient.Editor("sheet-id")));
        return op;
    }
    [Fact] public void DefaultsAreNonDestructiveAndReadOnly()
    {
        using var w = new Workspace(); var storage = Storage(w); var policy = BackupPolicy.Load(storage);
        Assert.Equal(30, policy.RetentionDays); Assert.Equal(200_000_000, policy.QuotaBytes); Assert.False(policy.AutomaticCleanup);
        Assert.Empty(new BackupManagement(storage).Inspect().Entries); Assert.False(Directory.Exists(storage.Root));
    }
    [Theory] [InlineData(0,200)] [InlineData(366,200)] [InlineData(30,0)] [InlineData(30,1001)]
    public async Task InvalidPolicyCannotBeSaved(int days, int quota)
    {
        using var w = new Workspace(); await Assert.ThrowsAsync<InvalidDataException>(() => BackupPolicy.SaveAsync(Storage(w), new(days, quota)));
        Assert.False(Directory.Exists(Storage(w).Root));
    }
    [Fact] public async Task PolicyRoundTripAndMaximum()
    {
        using var w = new Workspace(); await BackupPolicy.SaveAsync(Storage(w),new(45,1000,true));
        Assert.Equal(new BackupPolicy(45,1000,false),BackupPolicy.Load(Storage(w))); Assert.Equal(1_000_000_000,BackupPolicy.Load(Storage(w)).QuotaBytes);
    }
    [Fact] public void LegacyBackupCatalogMigrationPreservesRowsAndAddsOnlyTheDiscardFlag()
    {
        using var w = new Workspace(); var storage = Storage(w); Directory.CreateDirectory(storage.Root); var id = Guid.NewGuid();
        using (var db = new Microsoft.Data.Sqlite.SqliteConnection(new Microsoft.Data.Sqlite.SqliteConnectionStringBuilder { DataSource = BackupCatalog.PathFor(storage), Pooling = false }.ToString()))
        {
            db.Open(); using var cmd = db.CreateCommand();
            cmd.CommandText = "CREATE TABLE backups(id TEXT PRIMARY KEY,captured TEXT NOT NULL,hash TEXT NOT NULL,bytes INTEGER NOT NULL,copy_completed INTEGER NOT NULL DEFAULT 0,state INTEGER NOT NULL DEFAULT 0); CREATE TABLE events(seq INTEGER PRIMARY KEY,id TEXT NOT NULL,kind TEXT NOT NULL,at TEXT NOT NULL); PRAGMA user_version=1; INSERT INTO backups VALUES($id,$at,$hash,4,1,0)";
            cmd.Parameters.AddWithValue("$id",id.ToString("N")); cmd.Parameters.AddWithValue("$at",DateTimeOffset.UtcNow.ToString("O")); cmd.Parameters.AddWithValue("$hash",new string('A',64)); cmd.ExecuteNonQuery();
        }
        var row = Assert.Single(new BackupCatalog(storage).All()).Value;
        Assert.Equal(id,row.Id); Assert.Equal(4,row.Bytes); Assert.True(row.CopyCompleted); Assert.Equal(0,row.State); Assert.False(row.DiscardRequested);
        Assert.Equal(row,new BackupCatalog(storage).Get(id));
    }
    [Theory] [InlineData(false)] [InlineData(true)] public async Task CompletedBackupCanBeDeletedWithoutDeletingOriginalOrHistory(bool replacement)
    {
        using var w = new Workspace(); var op = await Completed(w,replacement:replacement); var manager = new BackupManagement(Storage(w));
        File.WriteAllText(w.Source+".url","preserved shortcut"); Assert.True(Assert.Single(manager.Inspect().Entries).CanClean);
        var result = await manager.DeleteAsync(op.Id); Assert.Equal(new CleanupResult(1,4),result);
        Assert.False(File.Exists(op.Snapshot!.BackupPath)); Assert.True(File.Exists(w.Source)); Assert.Equal("preserved shortcut",File.ReadAllText(w.Source+".url"));
        Assert.Equal(op,w.Registry().Get(op.Id)); Assert.True(new GoogleRemoteRegistry(Path.Combine(Storage(w).Root,"google.db")).Get("sheet:"+op.Id.ToString("N"))!.Verified);
        Assert.Equal(2,Assert.Single(manager.Inspect().Entries).CleanupState); Assert.Equal(new CleanupResult(0,0),await manager.DeleteAsync(op.Id));
        Assert.Empty(new BackupRecovery(Storage(w)).List());
        await Assert.ThrowsAsync<BackupRemovedException>(()=>new BackupRecovery(Storage(w)).RestoreAsync(op.Id,w.Source+".restored"));
        await Assert.ThrowsAsync<BackupRemovedException>(()=>Storage(w).CreatePreparation().PrepareAsync("A",w.Source));
        Assert.Single(w.Registry().Pending());
    }
    [Fact] public async Task MultipleDeletionDeduplicatesIdsAndPreservesUnselectedPendingBackup()
    {
        using var w = new Workspace(); var first = await Completed(w);
        var secondPath = Path.Combine(w.Root, "second.xlsx"); File.WriteAllBytes(secondPath, [5,6,7]);
        var second = await Completed(w, sourcePath: secondPath);
        var pendingPath = Path.Combine(w.Root, "pending.xlsx"); File.WriteAllBytes(pendingPath, [8,9]);
        var pending = await w.Coordinator().PrepareAsync("A", pendingPath);
        var result = await new BackupManagement(Storage(w)).CleanAsync([first.Id, second.Id, first.Id]);
        Assert.Equal(new CleanupResult(2,7), result);
        Assert.False(File.Exists(first.Snapshot!.BackupPath)); Assert.False(File.Exists(second.Snapshot!.BackupPath));
        Assert.True(File.Exists(pending.Snapshot!.BackupPath)); Assert.True(File.Exists(w.Source)); Assert.True(File.Exists(secondPath));
        Assert.Equal(pending.Id, Assert.Single(new BackupRecovery(Storage(w)).List()).Id);
        Assert.NotNull(w.Registry().Get(first.Id)); Assert.NotNull(w.Registry().Get(second.Id));
        Assert.Equal(0, (await new BackupManagement(Storage(w)).CleanAsync([first.Id,second.Id])).Count);
    }
    [Fact] public async Task PendingAndLegacyUnprovenCopiesAreProtected()
    {
        using var w=new Workspace();var op=await w.Coordinator().PrepareAsync("A",w.Source);var manager=new BackupManagement(Storage(w));
        Assert.False(Assert.Single(manager.Inspect().Entries).CanClean);
        var remote=new GoogleRemoteRegistry(Path.Combine(Storage(w).Root,"google.db"));var key="sheet:"+op.Id.ToString("N");remote.Begin(key,"A","sheet",op.Snapshot!.Sha256);remote.Candidate(key,"sheet-id");remote.Verify(key,"sheet-id");
        Assert.False(Assert.Single(manager.Inspect().Entries).CanClean);await Assert.ThrowsAsync<InvalidOperationException>(()=>manager.DeleteAsync(op.Id));Assert.True(File.Exists(op.Snapshot.BackupPath));
    }
    [Fact] public async Task ExplicitDiscardEndsPendingAttemptAndAllowsFreshImportWithoutRemovingOriginalOrRemote()
    {
        using var w = new Workspace(); var storage = Storage(w); var op = await w.Coordinator().PrepareAsync("A", w.Source);
        var remote = new GoogleRemoteRegistry(Path.Combine(storage.Root,"google.db")); var key = "sheet:" + op.Id.ToString("N");
        remote.Begin(key,"A","sheet",op.Snapshot!.Sha256); remote.Candidate(key,"sheet-id"); remote.Verify(key,"sheet-id");
        File.WriteAllText(w.Source + ".url", "existing shortcut"); var manager = new BackupManagement(storage);
        Assert.False(Assert.Single(manager.Inspect().Entries).CanClean); Assert.Empty(manager.Plan());
        Assert.Equal(new CleanupResult(1,4), await manager.DiscardAsync(op.Id));
        Assert.Equal(new byte[] {1,2,3,4}, File.ReadAllBytes(w.Source)); Assert.Equal("existing shortcut", File.ReadAllText(w.Source + ".url"));
        Assert.True(remote.Get(key)!.Verified); Assert.Empty(new BackupRecovery(storage).List());
        Assert.Contains(w.Registry().Events(op.Id), e => e.Kind == "Discarded");
        var fresh = await storage.CreatePreparation().PrepareAsync("A", w.Source);
        Assert.NotEqual(op.Id, fresh.Id); Assert.Equal(op.SourceKey, fresh.SourceKey); Assert.True(File.Exists(fresh.Snapshot!.BackupPath));
        Assert.Equal(new CleanupResult(0,0), await manager.DiscardAsync(op.Id));
    }
    [Theory] [InlineData(false)] [InlineData(true)] public async Task InterruptedDiscardCanFinishAndReleaseTheOldBinding(bool finishWasRecorded)
    {
        using var w = new Workspace(); var storage = Storage(w); var op = await w.Coordinator().PrepareAsync("A", w.Source);
        var catalog = new BackupCatalog(storage); catalog.Register(op.Id,op.Snapshot!,DateTimeOffset.UtcNow); catalog.BeginDiscard(op.Id,DateTimeOffset.UtcNow);
        File.Delete(op.Snapshot!.BackupPath); if (finishWasRecorded) catalog.FinishDelete(op.Id,DateTimeOffset.UtcNow);
        if (!finishWasRecorded) await new BackupManagement(storage).DiscardAsync(op.Id);
        var fresh = await storage.CreatePreparation().PrepareAsync("A", w.Source);
        Assert.NotEqual(op.Id,fresh.Id); Assert.True(File.Exists(fresh.Snapshot!.BackupPath)); Assert.NotNull(w.Registry().Get(op.Id));
    }
    [Fact] public async Task ExplicitDiscardStillRejectsTamperedBackupAndCancellation()
    {
        using var w = new Workspace(); var storage = Storage(w); var op = await w.Coordinator().PrepareAsync("A", w.Source); var manager = new BackupManagement(storage);
        using var cancelled = new CancellationTokenSource(); cancelled.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => manager.DiscardAsync(op.Id,cancelled.Token)); Assert.True(File.Exists(op.Snapshot!.BackupPath));
        File.WriteAllBytes(op.Snapshot.BackupPath,[9,9,9,9]); await Assert.ThrowsAsync<InvalidDataException>(() => manager.DiscardAsync(op.Id));
        Assert.True(File.Exists(op.Snapshot.BackupPath)); Assert.Empty(manager.Plan()); Assert.Equal(op.SourceKey,w.Registry().Get(op.Id)!.SourceKey);
    }
    [Fact] public async Task IncompleteReplacementOverridesCopyCompletion()
    {
        using var w=new Workspace();var op=await Completed(w);var journal=new ReplacementJournal(Path.Combine(Storage(w).Root,"replacement.db"));journal.Begin(op.Id,w.Source+".url",GoogleDriveClient.Editor("sheet-id"),w.Source);
        Assert.False(Assert.Single(new BackupManagement(Storage(w)).Inspect().Entries).CanClean);
    }
    [Fact] public async Task CleanupIsManualEvenForLegacyAutomaticPolicy()
    {
        using var w=new Workspace();var op=await Completed(w,DateTimeOffset.UtcNow.AddDays(-31));var manager=new BackupManagement(Storage(w));
        await BackupPolicy.SaveAsync(Storage(w),new(30,200,false)); Assert.False(BackupPolicy.Load(Storage(w)).AutomaticCleanup); Assert.True(File.Exists(op.Snapshot!.BackupPath));
        await BackupPolicy.SaveAsync(Storage(w),new(30,200,true));Assert.Empty(manager.Plan(excludeId:op.Id));Assert.Equal(0,(await manager.CleanAsync(manager.Plan(excludeId:op.Id))).Count);
        Assert.Equal(1,(await manager.CleanAsync(manager.Plan())).Count);Assert.False(File.Exists(op.Snapshot.BackupPath));
    }
    [Theory] [InlineData(false)] [InlineData(true)] public async Task RecordedDeletionRecoversBeforeOrAfterPhysicalDelete(bool deleted)
    {
        using var w=new Workspace();var op=await Completed(w);var catalog=new BackupCatalog(Storage(w));catalog.BeginDelete(op.Id,DateTimeOffset.UtcNow);if(deleted)File.Delete(op.Snapshot!.BackupPath);
        var manager=new BackupManagement(Storage(w));Assert.False(Assert.Single(manager.Inspect().Entries).Available);
        Assert.Equal(1,(await manager.CleanAsync(manager.Plan())).Count);Assert.Equal(2,new BackupCatalog(Storage(w)).Get(op.Id)!.State);Assert.False(File.Exists(op.Snapshot!.BackupPath));
    }
    [Fact] public async Task CancellationDoesNotDeleteBackup()
    {
        using var w=new Workspace();var op=await Completed(w);using var ct=new CancellationTokenSource();ct.Cancel();await Assert.ThrowsAnyAsync<OperationCanceledException>(()=>new BackupManagement(Storage(w)).DeleteAsync(op.Id,ct.Token));Assert.True(File.Exists(op.Snapshot!.BackupPath));
    }
    [Fact] public async Task TamperedBackupIsNeverDeleted()
    {
        using var w=new Workspace();var op=await Completed(w);File.WriteAllBytes(op.Snapshot!.BackupPath,[9,9,9,9]);await Assert.ThrowsAsync<InvalidDataException>(()=>new BackupManagement(Storage(w)).DeleteAsync(op.Id));Assert.True(File.Exists(op.Snapshot.BackupPath));Assert.Equal(1,new BackupCatalog(Storage(w)).Get(op.Id)!.State);
    }
    [Fact] public async Task MissingWithoutDeletionIntentIsProtected()
    {
        using var w=new Workspace();var op=await Completed(w);File.Delete(op.Snapshot!.BackupPath);Assert.False(Assert.Single(new BackupManagement(Storage(w)).Inspect().Entries).CanClean);
    }
    [Fact] public async Task QuotaIncludesUnknownFilesAndPreservesSource()
    {
        using var w=new Workspace();var storage=Storage(w);await BackupPolicy.SaveAsync(storage,new(30,1));Directory.CreateDirectory(storage.BackupsPath);File.WriteAllBytes(Path.Combine(storage.BackupsPath,"unknown.bin"),new byte[1_000_000]);
        await Assert.ThrowsAsync<BackupQuotaException>(()=>storage.CreatePreparation().PrepareAsync("A",w.Source));Assert.Equal(new byte[]{1,2,3,4},File.ReadAllBytes(w.Source));Assert.Equal(1_000_000,new BackupManagement(storage).Inspect().UnmanagedBytes);Assert.Single(Directory.GetFiles(storage.BackupsPath));
    }
    [Fact] public async Task CleanupWaitsForRecoverySourceLock()
    {
        using var w=new Workspace();var op=await Completed(w);var held=await new FileOperationLock(w.Locks).AcquireAsync(op.SourceKey);var task=new BackupManagement(Storage(w)).DeleteAsync(op.Id);
        await Task.Delay(100);Assert.False(task.IsCompleted);Assert.True(File.Exists(op.Snapshot!.BackupPath));await held.DisposeAsync();Assert.Equal(1,(await task).Count);
    }
    [Theory] [InlineData(false)] [InlineData(true)] public async Task QuotaCleanupChoosesOldestEligibleAndKeepsNewer(bool automatic)
    {
        using var w = new Workspace(); var older = await Completed(w, DateTimeOffset.UtcNow.AddDays(-2));
        var other = Path.Combine(w.Root, "newer.xlsx"); File.WriteAllBytes(other, [1,2,3,4]);
        var newer = await Completed(w, DateTimeOffset.UtcNow.AddDays(-1), sourcePath: other);
        await BackupPolicy.SaveAsync(Storage(w), new(30,1,automatic)); var manager = new BackupManagement(Storage(w));
        Assert.Equal(new[] { older.Id }, manager.Plan(999_995)); await manager.CleanAsync(manager.Plan(999_995));
        Assert.False(File.Exists(older.Snapshot!.BackupPath)); Assert.True(File.Exists(newer.Snapshot!.BackupPath));
    }
    [Fact] public async Task OversizedIncomingFileDoesNotDeleteExistingBackups()
    {
        using var w = new Workspace(); var op = await Completed(w); await BackupPolicy.SaveAsync(Storage(w),new(30,1,true));
        var other = Path.Combine(w.Root,"too-big.xlsx"); File.WriteAllBytes(other,new byte[1_000_001]);
        await Assert.ThrowsAsync<BackupQuotaException>(()=>Storage(w).CreatePreparation().PrepareAsync("A",other)); Assert.True(File.Exists(op.Snapshot!.BackupPath));
    }
    [Fact] public async Task ConcurrentCapturesCannotExceedQuota()
    {
        using var w = new Workspace();var storage=Storage(w);await BackupPolicy.SaveAsync(storage,new(30,1));
        File.WriteAllBytes(w.Source,new byte[600_000]);var other=Path.Combine(w.Root,"other.xlsx");File.WriteAllBytes(other,new byte[600_000]);
        var results=await Task.WhenAll(new[]{w.Source,other}.Select(async path=>{try{await storage.CreatePreparation().PrepareAsync("A",path);return true;}catch(BackupQuotaException){return false;}}));
        Assert.Single(results, ok=>ok);Assert.Equal(600_000,new BackupManagement(storage).UsedBytes());Assert.True(File.Exists(w.Source));Assert.True(File.Exists(other));
    }
    [WindowsFact] public async Task WindowsCleanupRefusesAnExternallyHeldBackup()
    {
        using var w=new Workspace();var op=await Completed(w);
        using(var external=new FileStream(op.Snapshot!.BackupPath,FileMode.Open,FileAccess.Read,FileShare.Read))
        {await Assert.ThrowsAsync<System.ComponentModel.Win32Exception>(()=>new BackupManagement(Storage(w)).DeleteAsync(op.Id));Assert.True(File.Exists(op.Snapshot.BackupPath));}
        Assert.Equal(1,(await new BackupManagement(Storage(w)).CleanAsync(new BackupManagement(Storage(w)).Plan())).Count);
    }

}
