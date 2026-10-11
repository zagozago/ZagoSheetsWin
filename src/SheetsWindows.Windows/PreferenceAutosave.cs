using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

// Each setting uses its existing atomic writer. Disable input until it commits,
// and restore the persisted value on failure without recursively saving it.
internal sealed class PreferenceAutosave
{
    private readonly Form owner;
    private readonly Control content;
    private readonly Label status;
    private readonly bool preview;
    private readonly SemaphoreSlim gate = new(1, 1);
    private int pending;
    private bool restoring;
    internal PreferenceAutosave(Form owner, Control content, Label status, bool preview)
    {
        this.owner = owner; this.content = content; this.status = status; this.preview = preview;
        owner.FormClosing += (_, e) => { if (pending > 0) e.Cancel = true; };
    }
    internal async Task SaveAsync(Func<Task> save, Action restore)
    {
        if (restoring || preview) return;
        SynchronizationContext.SetSynchronizationContext(new WindowsFormsSynchronizationContext());
        pending++; content.Enabled = false;
        await gate.WaitAsync();
        status.Text = UiText.Get("setup.saving");
        try { await save(); status.Text = ""; }
        catch (Exception ex) when (LauncherErrors.Expected(ex))
        {
            restoring = true;
            try { if (pending == 1) restore(); }
            finally { restoring = false; }
            status.Text = ex is ArgumentException ? UiText.Get("setup.consentRequired") : LauncherErrors.Message(ex);
        }
        finally { pending--; gate.Release(); if (!owner.IsDisposed && pending == 0) content.Enabled = true; }
    }
}
