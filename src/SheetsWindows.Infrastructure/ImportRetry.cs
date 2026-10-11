using System.ComponentModel;
using Microsoft.Data.Sqlite;
using SheetsWindows.Core;

namespace SheetsWindows.Infrastructure;

public static class ImportRetry
{
    public static bool IsTransient(Exception failure) => failure switch
    {
        GoogleApiException api => api.Status == 429 || api.Status >= 500,
        HttpRequestException http => http.StatusCode is null || (int)http.StatusCode == 429 || (int)http.StatusCode >= 500,
        SqliteException sqlite => sqlite.SqliteErrorCode is 5 or 6,
        Win32Exception native => native.NativeErrorCode is 21 or 32 or 33 or 170 or 1237,
        IOException io => (io.HResult & 0xffff) is 21 or 32 or 33 or 170 or 1237,
        TimeoutException => true,
        OperationCanceledException => true, // The caller's cancellation is checked separately.
        _ => false
    };
    public static async Task ExecuteAsync(Func<CancellationToken, Task> execute, Action<int, int>? countdown = null,
        CancellationToken ct = default, Func<TimeSpan, CancellationToken, Task>? delay = null)
    {
        delay ??= Task.Delay;
        for (var attempt = 0; ; attempt++)
        {
            ct.ThrowIfCancellationRequested();
            try { await execute(ct); return; }
            catch (Exception ex) when (attempt < 2 && !ct.IsCancellationRequested && IsTransient(ex))
            {
                for (var seconds = 3; seconds > 0; seconds--)
                { countdown?.Invoke(seconds, attempt + 1); await delay(TimeSpan.FromSeconds(1), ct); }
            }
        }
    }
}
