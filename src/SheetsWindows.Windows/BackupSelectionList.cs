using System.Runtime.InteropServices;
using SheetsWindows.Infrastructure;
namespace SheetsWindows.Windows;

// SelectedItems remains the sole source for batch actions. Checkboxes mirror it.
internal sealed class BackupSelectionList : ListView
{
    private bool synchronizing;
    internal CheckBox SelectAllHeader { get; } = new() { ThreeState = true, AutoCheck = false, Size = new Size(22, 20), Text = "", AccessibleName = UiText.Get("recovery.selectAll") };
    internal BackupSelectionList()
    {
        CheckBoxes = true; Controls.Add(SelectAllHeader);
        SelectAllHeader.Click += (_, _) => { var select = SelectAllHeader.CheckState != CheckState.Checked; foreach(ListViewItem item in Items) item.Selected = select && (item.Tag as RecoveryEntry)?.CleanupState != 2; Synchronize(); };
        SelectedIndexChanged += (_, _) => Synchronize();
        ItemCheck += (_, e) =>
        {
            if (synchronizing) return;
            synchronizing = true;
            try { Items[e.Index].Selected = e.NewValue == CheckState.Checked; }
            finally { synchronizing = false; }
            if (IsHandleCreated) BeginInvoke((Action)Synchronize);
        };
    }
    internal void Synchronize()
    {
        if (synchronizing || IsDisposed) return;
        synchronizing = true;
        try
        {
            foreach(ListViewItem item in Items) if(item.Checked != item.Selected) item.Checked = item.Selected;
            var relevant = Items.Cast<ListViewItem>().Where(i => (i.Tag as RecoveryEntry)?.CleanupState != 2).ToArray();
            var selected = relevant.Count(i => i.Selected);
            SelectAllHeader.CheckState = selected == 0 ? CheckState.Unchecked : selected == relevant.Length ? CheckState.Checked : CheckState.Indeterminate;
        }
        finally { synchronizing = false; }
    }
    protected override void OnLayout(LayoutEventArgs e) { base.OnLayout(e); PositionHeader(); }
    private void PositionHeader()
    {
        if (!IsHandleCreated) return;
        var offset = GetScrollPos(Handle, 0);
        SelectAllHeader.Location = new Point(RightToLeftLayout ? ClientSize.Width - 26 + offset : 4 - offset, 1);
        SelectAllHeader.Visible = offset < 22; 
    }
    protected override void WndProc(ref Message m) { base.WndProc(ref m); if(m.Msg is 0x114 or 0x5) PositionHeader(); }
    [DllImport("user32.dll")] private static extern int GetScrollPos(IntPtr handle, int bar);
}
