namespace SheetsWindows.Core;

public sealed class GoogleAccess(string accountId, string token, string? email = null)
{
    public string AccountId { get; } = accountId;
    public string Token { get; } = token;
    public string? Email { get; } = email;
    public override string ToString() => "GoogleAccess [redacted]";
}
public interface IGoogleAuth { Task<GoogleAccess> AccessAsync(bool refresh = false, CancellationToken cancellationToken = default); }
public sealed record RemoteAttempt(string Key, string AccountId, string Kind, string Hash, string Marker, string? FileId, bool Verified);
public interface IRemoteRegistry
{
    RemoteAttempt? Get(string key);
    RemoteAttempt Begin(string key, string accountId, string kind, string hash);
    void Candidate(string key, string id);
    void Verify(string key, string id);
    void RetireTrashedAssociation(string key, string expectedFileId) => throw new NotSupportedException("Association retirement is unavailable.");
}
public sealed class ReconciliationRequiredException(string? missingKey = null) : IOException("Remote creation outcome is uncertain; reconciliation required. No new upload was attempted.")
{
    // Set only after a successful, empty marker lookup; network and validation failures never qualify.
    public string? MissingKey { get; } = missingKey;
}
public sealed class GoogleApiException(int status, string? reason = null, string? endpoint = null, bool retryDeferred = false) : IOException($"Google request failed (HTTP {status}).")
{
    public int Status { get; } = status;
    public bool RetryDeferred { get; } = retryDeferred;
    public string? Reason { get; } = SafeReason(reason);
    public string? Endpoint { get; } = endpoint is "drive.read" or "drive.export" or "drive.create" ? endpoint : null;
    public static string? SafeReason(string? reason) => reason is "rateLimitExceeded" or "userRateLimitExceeded" or "sharingRateLimitExceeded"
        or "dailyLimitExceeded" or "storageQuotaExceeded" or "insufficientFilePermissions" or "appNotAuthorizedToFile"
        or "forbidden" or "insufficientPermissions" or "downloadQuotaExceeded" or "exportSizeLimitExceeded" ? reason : null;
    public bool IsRateLimited => Status == 403 && Reason is "rateLimitExceeded" or "userRateLimitExceeded";
}
public sealed class AuthorizationRequiredException() : IOException("Google sign-in is required.");


