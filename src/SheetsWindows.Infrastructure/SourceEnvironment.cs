using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace SheetsWindows.Infrastructure;

public static class SourceEnvironment
{
    public static bool IsKnownSynced(string path)
    {
        var full = Path.GetFullPath(path);
        bool Contains(string? root)
        {
            if (string.IsNullOrWhiteSpace(root) || !Path.IsPathFullyQualified(root)) return false;
            root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
            return full.Equals(root, StringComparison.OrdinalIgnoreCase) || full.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);
        }
        if (new[] { "OneDrive", "OneDriveConsumer", "OneDriveCommercial" }.Any(name => Contains(Environment.GetEnvironmentVariable(name)))) return true;
        if (!OperatingSystem.IsWindows()) return false;
        foreach (var hive in new[] { Microsoft.Win32.Registry.CurrentUser, Microsoft.Win32.Registry.LocalMachine })
        {
            using var providers = hive.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\SyncRootManager");
            if (providers is not null) foreach (var name in providers.GetSubKeyNames())
            {
                using var provider = providers.OpenSubKey(name + @"\UserSyncRoots");
                if (provider is not null) foreach (var key in provider.GetValueNames())
                    if (Contains(provider.GetValue(key) as string)) return true;
            }
        }
        return false;
    }
    public static bool RequiresCopy(string path)
    {
        if (IsNetwork(path) || IsKnownSynced(path)) return true;
        for (var current = Path.GetFullPath(path); current is not null; current = Path.GetDirectoryName(current))
            if (((int)File.GetAttributes(current) & (0x400 | 0x1000 | 0x40000 | 0x400000)) != 0) return true;
        return false;
    }
    public static bool IsCloudTag(uint tag) => (tag & ~0x0000F000u) == 0x9000001Au;
    public static bool IsNetwork(string path) => OperatingSystem.IsWindows() && (new Uri(Path.GetFullPath(path)).IsUnc || new DriveInfo(Path.GetPathRoot(path)!).DriveType == DriveType.Network);
    public static void ValidateRead(string full, bool copyOnly)
    {
        if (!copyOnly && IsNetwork(full)) throw new NotSupportedException("Network sources require copy mode.");
        for (var current = full; current is not null; current = Path.GetDirectoryName(current))
        {
            var attributes = File.GetAttributes(current);
            if (current == full && ((int)attributes & (0x1000 | 0x40000 | 0x400000)) != 0) throw new NotSupportedException("Make cloud file available locally before importing.");
            if ((attributes & FileAttributes.ReparsePoint) == 0) continue;
            if (!copyOnly || !OperatingSystem.IsWindows()) throw new NotSupportedException("Redirected sources excluded.");
            using var handle = CreateFileW(current, 0, 7, IntPtr.Zero, 3, 0x02200000, IntPtr.Zero);
            if (handle.IsInvalid || !GetFileInformationByHandleEx(handle, 9, out var info, 8)) throw new Win32Exception(Marshal.GetLastWin32Error());
            if (!IsCloudTag(info.Tag)) throw new NotSupportedException("Only hydrated cloud reparse points are allowed in copy mode.");
        }
    }
    internal static string ScopeIdentity(string path, string key)
    {
        if (!IsNetwork(path)) return key;
        var canonical = path;
        if (!new Uri(path).IsUnc)
        {
            uint size = 0; _ = WNetGetUniversalNameW(path, 1, IntPtr.Zero, ref size);
            if (size == 0 || size > 65536) throw new NotSupportedException("Network identity unavailable.");
            var buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                var result = WNetGetUniversalNameW(path, 1, buffer, ref size); if (result != 0) throw new Win32Exception((int)result);
                canonical = Marshal.PtrToStringUni(Marshal.ReadIntPtr(buffer)) ?? throw new InvalidDataException("Network identity unavailable.");
            }
            finally { Marshal.FreeHGlobal(buffer); }
        }
        var scope = Path.GetPathRoot(canonical)!.ToUpperInvariant();
        return "network:" + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(scope))) + ":" + key;
    }
    [StructLayout(LayoutKind.Sequential)] private struct AttributeTag { public uint Attributes, Tag; }
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)] private static extern SafeFileHandle CreateFileW(string path, uint access, uint share, IntPtr security, uint creation, uint flags, IntPtr template);
    [DllImport("kernel32.dll", SetLastError = true)][return: MarshalAs(UnmanagedType.Bool)] private static extern bool GetFileInformationByHandleEx(SafeFileHandle handle, int kind, out AttributeTag info, uint size);
    [DllImport("mpr.dll", CharSet = CharSet.Unicode)] private static extern uint WNetGetUniversalNameW(string path, uint level, IntPtr buffer, ref uint size);
}
