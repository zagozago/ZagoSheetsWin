using System.Net.Http.Headers;

namespace SheetsWindows.Infrastructure;

// Only read-only requests and queries of an existing upload session use this policy.
internal static class GoogleRetry
{
    internal static bool Transient(int status) => status == 429 || status is >= 500 and <= 599;
    internal static Task DelayAsync(int retry, HttpResponseMessage? response, CancellationToken ct)
    {
        var wait = TimeSpan.FromSeconds(Math.Pow(2, retry)); // 2, 4, 8 seconds.
        RetryConditionHeaderValue? header = response?.Headers.RetryAfter;
        if (header?.Delta is {} delta) wait = delta;
        else if (header?.Date is {} date) wait = date - DateTimeOffset.UtcNow;
        wait = TimeSpan.FromSeconds(Math.Clamp(wait.TotalSeconds, 0, 30));
        return Task.Delay(wait, ct);
    }
}
