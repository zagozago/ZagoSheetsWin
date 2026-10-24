using SheetsWindows.Infrastructure;
namespace SheetsWindows.Windows;

// SelectedItems remains the sole source for batch actions. Checkboxes mirror it.
internal sealed class BackupSelectionList : ListView
{
    private bool synchronizing;
    internal bool ApplyingSelection { get; private set; }
    internal BackupSelectionList()
    {
        CheckBoxes = true; OwnerDraw = true;
        DrawItem += (_, e) => e.DrawDefault = true;
        DrawSubItem += (_, e) => e.DrawDefault = true;
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
    internal async Task ApplySelectionAsync(bool select)
    {
        if (ApplyingSelection) return;
        ApplyingSelection = true; synchronizing = true;
        try
        {
            for (var start = 0; start < Items.Count; start += 32)
            {
                BeginUpdate();
                try
                {
                    for (var index = start; index < Math.Min(start + 32, Items.Count); index++)
                    {
                        var item = Items[index]; var selected = select && (item.Tag as RecoveryEntry)?.CleanupState != 2;
                        item.Selected = selected; item.Checked = selected;
                    }
                }
                finally { EndUpdate(); }
                if (start + 32 < Items.Count) await Task.Delay(1);
            }
        }
        finally { synchronizing = false; ApplyingSelection = false; Synchronize(); Invalidate(); }
    }
    protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
    {
        using var fill = new SolidBrush(BackColor); e.Graphics.FillRectangle(fill, e.Bounds);
        if (e.ColumnIndex > 0) TextRenderer.DrawText(e.Graphics, e.Header?.Text, Font, Rectangle.Inflate(e.Bounds, -4, 0), ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | (RightToLeftLayout ? TextFormatFlags.RightToLeft | TextFormatFlags.Right : 0));
        using var border = new Pen(VisualStyle.Border); e.Graphics.DrawLine(border, e.Bounds.Left, e.Bounds.Bottom-1, e.Bounds.Right, e.Bounds.Bottom-1);
    }
    internal void Synchronize()
    {
        if (synchronizing || IsDisposed) return;
        synchronizing = true;
        try
        {
            foreach(ListViewItem item in Items) if(item.Checked != item.Selected) item.Checked = item.Selected;

        }
        finally { synchronizing = false; }
    }
    protected override void OnClientSizeChanged(EventArgs e) { base.OnClientSizeChanged(e); FillLastColumn(); }
    protected override void OnHandleCreated(EventArgs e) { base.OnHandleCreated(e); FillLastColumn(); }
    private void FillLastColumn()
    {
        if (Columns.Count != 5) return;
        var used = Columns.Cast<ColumnHeader>().Take(4).Sum(c => c.Width);
        Columns[4].Width = Math.Max(150 * DeviceDpi / 96, ClientSize.Width - used);
    }
}
