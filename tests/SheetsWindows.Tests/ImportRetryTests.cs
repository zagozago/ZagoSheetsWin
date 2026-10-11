using SheetsWindows.Infrastructure;
using SheetsWindows.Core;
using Xunit;

namespace SheetsWindows.Tests;

public sealed class ImportRetryTests
{
    private static Task NoWait(TimeSpan duration, CancellationToken ct) { ct.ThrowIfCancellationRequested(); return Task.CompletedTask; }
    [Fact] public async Task TwoRetriesShowThreeSecondCountdownAndThenSucceed()
    {
        var calls = 0; var ticks = new List<(int, int)>();
        await ImportRetry.ExecuteAsync(_ => ++calls < 3 ? Task.FromException(new HttpRequestException()) : Task.CompletedTask,
            (seconds, attempt) => ticks.Add((seconds, attempt)), delay: NoWait);
        Assert.Equal(3, calls); Assert.Equal(new[] {(3,1),(2,1),(1,1),(3,2),(2,2),(1,2)}, ticks);
    }
    [Fact] public async Task PersistentTransientFailureStopsAfterThreeAttempts()
    {
        var calls = 0;
        await Assert.ThrowsAsync<TimeoutException>(() => ImportRetry.ExecuteAsync(_ => { calls++; throw new TimeoutException(); }, delay: NoWait));
        Assert.Equal(3, calls);
    }
    [Fact] public async Task InvalidContentDoesNotRetry()
    {
        var calls = 0;
        await Assert.ThrowsAsync<InvalidDataException>(() => ImportRetry.ExecuteAsync(_ => { calls++; throw new InvalidDataException(); }, delay: NoWait));
        Assert.Equal(1, calls);
    }
    [Fact] public async Task CancellationDuringCountdownPreventsAnotherAttempt()
    {
        using var cancel = new CancellationTokenSource(); var calls = 0;
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ImportRetry.ExecuteAsync(_ => { calls++; throw new HttpRequestException(); },
            (_, _) => cancel.Cancel(), cancel.Token, NoWait)); Assert.Equal(1, calls);
    }
    [Fact] public void UnsafeOrPermanentFailuresDoNotRetry()
    {
        Assert.False(ImportRetry.IsTransient(new AuthorizationRequiredException()));
        Assert.False(ImportRetry.IsTransient(new ReconciliationRequiredException()));
        Assert.False(ImportRetry.IsTransient(new HttpRequestException("denied", null, System.Net.HttpStatusCode.Forbidden)));
        Assert.True(ImportRetry.IsTransient(new System.ComponentModel.Win32Exception(32)));
    }
}
