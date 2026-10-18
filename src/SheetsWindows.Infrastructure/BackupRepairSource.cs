using System.Security.Cryptography;
using SheetsWindows.Core;

namespace SheetsWindows.Infrastructure;

// A fresh identity guarantees a new operation and remote marker. It never retires or
// repoints the old operation, its original source, its shortcut or its Google document.
public sealed class BackupRepairSource(ImportOperation original, Guid repairId) : ISourceReader
{
    public ISourceLease Open(string path)
    {
        if (path != original.SourcePath) throw new LocalConflictException("Repair source changed.");
        var snapshot = original.Snapshot ?? throw new InvalidDataException("Backup required.");
        if ((File.GetAttributes(snapshot.BackupPath) & FileAttributes.ReparsePoint) != 0) throw new InvalidDataException("Invalid backup.");
        var stream = new FileStream(snapshot.BackupPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        try
        {
            if (stream.Length != snapshot.Length || stream.Length > GoogleImport.MaxBytes
                || Convert.ToHexString(SHA256.HashData(stream)) != snapshot.Sha256) throw new InvalidDataException("Snapshot integrity failure.");
            stream.Position = 0;
            return new Lease(stream, new("repair:" + repairId.ToString("N"), original.SourcePath, original.Format));
        }
        catch { stream.Dispose(); throw; }
    }
    private sealed class Lease(Stream content, SourceDescriptor source) : ISourceLease
    {
        public SourceDescriptor Source => source;
        public Stream Content => content;
        public ValueTask DisposeAsync() => content.DisposeAsync();
    }
}
