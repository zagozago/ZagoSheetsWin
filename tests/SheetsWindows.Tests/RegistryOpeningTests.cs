using Microsoft.Data.Sqlite;
using SheetsWindows.Infrastructure;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class RegistryOpeningTests
{
    // A reserved writer permits SQLite readers. Reopening a current registry must
    // remain a reader, rather than trying to migrate its already-current schema.
    [Theory] [InlineData("operations")] [InlineData("remote")] [InlineData("backups")] [InlineData("replacement")]
    public async Task CurrentRegistryCanOpenAndReadWhileAnotherWriterOwnsTheDatabase(string kind)
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!);
        var path = kind switch { "operations" => storage.DatabasePath, "remote" => Path.Combine(storage.Root, "google.db"),
            "backups" => BackupCatalog.PathFor(storage), _ => Path.Combine(storage.Root, "replacement.db") };
        void Read()
        {
            switch (kind)
            {
                case "operations": Assert.Empty(new SqliteOperationRegistry(path).Pending()); break;
                case "remote": Assert.Empty(new GoogleRemoteRegistry(path).All()); break;
                case "backups": Assert.Empty(new BackupCatalog(storage).All()); break;
                default: Assert.Empty(new ReplacementJournal(path).All()); break;
            }
        }
        Read();
        using var writer = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = path, Pooling = false }.ToString()); writer.Open();
        Task read; bool completedWhileWriterHeld;
        using (var transaction = writer.BeginTransaction(deferred: false))
        {
            read = Task.Factory.StartNew(Read, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            completedWhileWriterHeld = await Task.WhenAny(read, Task.Delay(TimeSpan.FromSeconds(5))) == read;
        }
        // Release the writer even on failure, then join the reader before workspace cleanup.
        await read; Assert.True(completedWhileWriterHeld, "Opening a current registry requested a write lock.");
    }

    [Fact]
    public async Task SavedInterpretationLoadsWhileAnotherWriterOwnsTheDatabase()
    {
        using var w = new Workspace(); var storage = new LocalStorage(Path.GetDirectoryName(w.Database)!);
        var op = await w.Coordinator().PrepareAsync("A", w.Source);
        var interpretations = new ImportInterpretations(storage); var expected = interpretations.Bind(op, new(), false);
        using var writer = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = storage.DatabasePath, Pooling = false }.ToString()); writer.Open();
        Task read; bool completedWhileWriterHeld;
        using (var transaction = writer.BeginTransaction(deferred: false))
        {
            read = Task.Factory.StartNew(() => Assert.Equal(expected, new ImportInterpretations(storage).Load(op, new())),
                CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            completedWhileWriterHeld = await Task.WhenAny(read, Task.Delay(TimeSpan.FromSeconds(5))) == read;
        }
        await read; Assert.True(completedWhileWriterHeld, "Loading a saved interpretation requested a schema write.");
    }
}
