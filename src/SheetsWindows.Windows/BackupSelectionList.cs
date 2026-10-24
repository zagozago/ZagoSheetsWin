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
        CheckBoxes = true; OwnerDraw = true; Controls.Add(SelectAllHeader);
        DrawItem += (_, e) => e.DrawDefault = true;
        DrawSubItem += (_, e) => e.DrawDefault = true;
        ColumnClick += (_, e) => { if(e.Column == 0) ToggleAll(); };
        SelectAllHeader.CheckStateChanged += (_, _) => { if (IsHandleCreated) InvalidateRect(SendMessage(Handle, 0x101f, IntPtr.Zero, IntPtr.Zero), IntPtr.Zero, false); };
        SelectAllHeader.Click += (_, _) => ToggleAll();
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
    internal void ToggleAll()
    {
        var select = SelectAllHeader.CheckState != CheckState.Checked;
        foreach (ListViewItem item in Items) item.Selected = select && (item.Tag as RecoveryEntry)?.CleanupState != 2;
        Synchronize();
    }
    protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
    {
        using var fill = new SolidBrush(BackColor); e.Graphics.FillRectangle(fill, e.Bounds);
        if (e.ColumnIndex == 0)
        {
            var state = SelectAllHeader.CheckState switch { CheckState.Checked => System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal, CheckState.Indeterminate => System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal, _ => System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal };
            var size = CheckBoxRenderer.GetGlyphSize(e.Graphics, state);
            CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(e.Bounds.X + (e.Bounds.Width-size.Width)/2, e.Bounds.Y + (e.Bounds.Height-size.Height)/2), state);
        }
        else TextRenderer.DrawText(e.Graphics, e.Header?.Text, Font, Rectangle.Inflate(e.Bounds, -4, 0), ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | (RightToLeftLayout ? TextFormatFlags.RightToLeft | TextFormatFlags.Right : 0));
        using var border = new Pen(VisualStyle.Border); e.Graphics.DrawLine(border, e.Bounds.Left, e.Bounds.Bottom-1, e.Bounds.Right, e.Bounds.Bottom-1);
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
    [DllImport("user32.dll")] private static extern IntPtr SendMessage(IntPtr handle, int message, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")] private static extern bool InvalidateRect(IntPtr handle, IntPtr rectangle, bool erase);
    [DllImport("user32.dll")] private static extern int GetScrollPos(IntPtr handle, int bar);
}
