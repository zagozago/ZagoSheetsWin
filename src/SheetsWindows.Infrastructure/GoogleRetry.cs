using System.Net.Http.Headers;
using System.Text.Json;
using SheetsWindows.Core;

namespace SheetsWindows.Infrastructure;

// Only read-only requests and queries of an existing upload session use this policy.
internal static class GoogleRetry
{
    internal static bool Transient(int status) => status == 429 || status is >= 500 and <= 599;
    internal static async Task<string?> ReasonAsync(HttpResponseMessage response, CancellationToken ct)
    {
        // Never keep messages, URLs or arbitrary reason strings from the server.
        const int max = 16 * 1024;
        if (response.IsSuccessStatusCode || response.Content.Headers.ContentLength > max) return null;
        try
        {
            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var buffer = new MemoryStream(); var chunk = new byte[2048]; int count;
            while ((count = await stream.ReadAsync(chunk, ct)) != 0)
            { if (buffer.Length + count > max) return null; buffer.Write(chunk, 0, count); }
            using var json = JsonDocument.Parse(buffer.ToArray());
            if (json.RootElement.ValueKind != JsonValueKind.Object || !json.RootElement.TryGetProperty("error", out var error)
                || error.ValueKind != JsonValueKind.Object || !error.TryGetProperty("errors", out var errors) || errors.ValueKind != JsonValueKind.Array) return null;
            // Mixed permanent/transient errors must not authorize a retry.
            string? selected = null;
            foreach (var item in errors.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object || !item.TryGetProperty("reason", out var value) || value.ValueKind != JsonValueKind.String) return null;
                var reason = GoogleApiException.SafeReason(value.GetString());
                if (reason is not ("rateLimitExceeded" or "userRateLimitExceeded")) return reason;
                selected = reason;
            }
            return selected;
        }
        catch (JsonException) { return null; }
    }
    internal static bool Transient(int status, string? reason) => Transient(status) || new GoogleApiException(status, reason).IsRateLimited;
    internal static bool WithinRetryWindow(HttpResponseMessage response)
    {
        var header = response.Headers.RetryAfter;
        var seconds = header?.Delta?.TotalSeconds ?? (header?.Date is { } date ? (date - DateTimeOffset.UtcNow).TotalSeconds : 0);
        return seconds <= 30; // Do not retry earlier than a longer server-directed wait.
    }
    internal static Task DelayAsync(int retry, HttpResponseMessage? response, CancellationToken ct, Action<int>? scheduled = null)
    {
        var wait = TimeSpan.FromSeconds(Math.Pow(2, retry) + Random.Shared.NextDouble()); // 2, 4, 8 seconds.
        RetryConditionHeaderValue? header = response?.Headers.RetryAfter;
        if (header?.Delta is {} delta) wait = delta;
        else if (header?.Date is {} date) wait = date - DateTimeOffset.UtcNow;
        wait = TimeSpan.FromSeconds(Math.Clamp(wait.TotalSeconds, 0, 30));
        scheduled?.Invoke((int)wait.TotalMilliseconds);
        return Task.Delay(wait, ct);
    }
}
