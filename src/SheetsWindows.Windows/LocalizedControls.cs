using SheetsWindows.Infrastructure;
using System.Runtime.CompilerServices;

namespace SheetsWindows.Windows;

// Each text is bound to its semantic descriptor when assigned. Data fields and
// file names are never translated. Existing instances keep state and handlers.
internal static class LocalizedControls
{
    private sealed class Binding { internal LocalizedMessage? Message; }
    private static readonly ConditionalWeakTable<Control, Binding> bound = new();
    internal static void Attach(Control form)
    {
        void Track(Control control)
        {
            if (bound.TryGetValue(control,out _)) return;
            var binding=new Binding();bound.Add(control,binding);
            if(control is not TextBoxBase && control is not NumericUpDown && control is not ComboBox && control is not ListBox)
            {
                binding.Message=UiText.Descriptor(control.Text);
                control.TextChanged+=(_,_)=>binding.Message=UiText.Descriptor(control.Text) ?? (binding.Message?.Text == control.Text ? binding.Message : null);
            }
            control.ControlAdded+=(_,e)=> {if(e.Control is {} added)Track(added);};
            foreach(Control child in control.Controls)Track(child);
        }
        Track(form);
        void Refresh(Control control)
        {
            var previousText=control.Text; var previousName=control.AccessibleName;
            if(bound.TryGetValue(control,out var binding) && binding.Message is {} message)
            {var saved=message;control.Text=message.Text;binding.Message=saved;}
            if(previousName is not null && previousName==previousText.Replace("&", "")) control.AccessibleName=control.Text.Replace("&", "");
            if(UiText.Descriptor(control.AccessibleName) is {} name)control.AccessibleName=UiText.GetFromDescriptor(name);
            if(UiText.Descriptor(control.AccessibleDescription) is {} description)control.AccessibleDescription=UiText.GetFromDescriptor(description);
            if(control is ComboBox combo)
            {
                var index=combo.SelectedIndex;
                for(var i=0;i<combo.Items.Count;i++) if(combo.Items[i] is string text && UiText.Descriptor(text) is {} value) combo.Items[i]=UiText.GetFromDescriptor(value);
                combo.SelectedIndex=index;
            }
            if(control is ListView list)
            {
                foreach(ColumnHeader column in list.Columns)if(UiText.Descriptor(column.Text) is {} value)column.Text=UiText.GetFromDescriptor(value);
                foreach(ListViewItem row in list.Items)for(var i=2;i<row.SubItems.Count;i++)if(UiText.Descriptor(row.SubItems[i].Text) is {} value)row.SubItems[i].Text=UiText.GetFromDescriptor(value);
            }
            if(control.ContextMenuStrip is {} menu)foreach(ToolStripItem item in menu.Items)if(UiText.Descriptor(item.Text) is {} value)item.Text=UiText.GetFromDescriptor(value);
            foreach(Control child in control.Controls)Refresh(child);
            control.Invalidate();control.PerformLayout();
        }
        void Changed(){if(!form.IsDisposed){form.SuspendLayout();try{Refresh(form);}finally{form.ResumeLayout(true);}}}
        UiText.Changed+=Changed;form.Disposed+=(_,_)=>UiText.Changed-=Changed;
        Changed(); // A lazily opened pop-up may contain controls created before a language change.
    }
}

// Native button wrapping, with the actual font and DPI, including padding and
// focus space. Flow rows wrap; constrained full-width buttons grow in height.
internal class AdaptiveButton : Button
{
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    internal float? FontSizeOverride { get; init; }
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    internal int ContentOffsetY { get; set; }
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    internal int IconOffsetY { get; set; }
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    internal bool KeepArrowWithWord { get; init; } = true;
    private string DisplayText => KeepArrowWithWord && System.Text.RegularExpressions.Regex.IsMatch(Text, @"\S+\s+\S+\s+[▾▼]$") ? System.Text.RegularExpressions.Regex.Replace(Text, @"\s+([▾▼])$", "\u00a0$1") : Text;
    private readonly System.Windows.Forms.Timer spinner = new() { Interval = 50 };
    private int spinnerAngle;
    private bool spinning;
    internal void SetSpinning(bool value) { spinning = value; if(value) spinner.Start(); else spinner.Stop(); Invalidate(); }
    private bool fitting;
    public AdaptiveButton(){Padding=new Padding(12,6,12,6);AutoEllipsis=false; spinner.Tick += (_,_) => { spinnerAngle = (spinnerAngle + 30) % 360; Invalidate(); };}
    protected override void OnPaint(PaintEventArgs e)
    {
        if (Image is null && Enabled && DisplayText == Text || SystemInformation.HighContrast) { base.OnPaint(e); return; }
        var hovered = Enabled && ClientRectangle.Contains(PointToClient(MousePosition));
        e.Graphics.Clear(hovered ? FlatAppearance.MouseOverBackColor : BackColor);
        using var border = new Pen(FlatAppearance.BorderColor);
        e.Graphics.DrawRectangle(border, 0, 0, Math.Max(0, Width - 1), Math.Max(0, Height - 1));
        var gap = Image is null ? 0 : 8 * DeviceDpi / 96;
        var room = Math.Max(20, Width - Padding.Horizontal - 12 - (Image?.Width ?? 0) - gap);
        var textSize = TextRenderer.MeasureText(DisplayText, Font, new Size(room, int.MaxValue), TextFormatFlags.WordBreak);
        var groupWidth = textSize.Width + (Image?.Width ?? 0) + gap;
        var x = (Width - groupWidth) / 2; var y = (Height - textSize.Height) / 2 + ContentOffsetY * DeviceDpi / 96;
        var rtl = RightToLeft == RightToLeft.Yes;
        if (Image is not null)
        {
            var iconX = rtl ? x + textSize.Width + gap : x; var iconY = (Height - Image.Height) / 2 + (ContentOffsetY + IconOffsetY) * DeviceDpi / 96;
            if (spinning) { e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; using var pen = new Pen(ForeColor, 2 * DeviceDpi / 96f); e.Graphics.DrawArc(pen, iconX + 1, iconY + 1, Image.Width - 3, Image.Height - 3, spinnerAngle, 270); }
            else e.Graphics.DrawImage(Image, iconX, iconY, Image.Width, Image.Height);
            if (!rtl) x += Image.Width + gap;
        }
        var flags = TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
        if (rtl) flags |= TextFormatFlags.RightToLeft;
        if (!ShowKeyboardCues) flags |= TextFormatFlags.HidePrefix;
        TextRenderer.DrawText(e.Graphics, DisplayText, Font, new Rectangle(x, y, textSize.Width, textSize.Height), ForeColor, flags);
        if (Focused && ShowFocusCues) ControlPaint.DrawFocusRectangle(e.Graphics, Rectangle.Inflate(ClientRectangle, -4, -4), ForeColor, BackColor);
    }
    public override Size GetPreferredSize(Size proposedSize)
    {
        var max=Parent is FlowLayoutPanel flow ? Math.Max(80,flow.ClientSize.Width-flow.Padding.Horizontal-Margin.Horizontal) : proposedSize.Width;
        if(max<=0)max=int.MaxValue;
        var natural=TextRenderer.MeasureText(DisplayText,Font,Size.Empty,TextFormatFlags.SingleLine);
        var width=Math.Min(max,natural.Width+Padding.Horizontal+12+(Image is null ? 0 : Image.Width + 8 * DeviceDpi / 96));
        if(Parent is FlowLayoutPanel { FlowDirection: FlowDirection.TopDown, WrapContents: false }) width=Math.Min(max,Math.Max(Width,width));
        var text=TextRenderer.MeasureText(DisplayText,Font,new Size(Math.Max(20,width-Padding.Horizontal-12-(Image is null ? 0 : Image.Width + 8 * DeviceDpi / 96)),int.MaxValue),TextFormatFlags.WordBreak);
        return new Size(Math.Max(MinimumSize.Width,width),Math.Max(MinimumSize.Height,text.Height+Padding.Vertical+10));
    }
    private void Fit()
    {
        if(fitting || Width<30)return;
        fitting=true;
        try
        {
            var measured=TextRenderer.MeasureText(DisplayText,Font,new Size(Math.Max(20,Width-Padding.Horizontal-12-(Image is null ? 0 : Image.Width + 8 * DeviceDpi / 96)),int.MaxValue),TextFormatFlags.WordBreak);
            var height=measured.Height+Padding.Vertical+10;
            if(!AutoSize && Dock != DockStyle.Fill)Height=Math.Max(MinimumSize.Height,height);
            else if(!AutoSize && Height<height)Height=height;
            if(Parent is TableLayoutPanel table && Dock==DockStyle.Fill)
            {
                var row=table.GetRow(this);if(row>=0 && row<table.RowStyles.Count && table.RowStyles[row].SizeType==SizeType.Absolute && table.RowStyles[row].Height<height+Margin.Vertical)table.RowStyles[row].Height=height+Margin.Vertical;
                if(table.RowCount==1 && table.Height<height+Margin.Vertical)table.Height=height+Margin.Vertical;
            }
        }finally{fitting=false;}
    }
    protected override void Dispose(bool disposing) { if (disposing) spinner.Dispose(); base.Dispose(disposing); }
    protected override void OnTextChanged(EventArgs e){base.OnTextChanged(e);Fit();Parent?.PerformLayout();}
    protected override void OnFontChanged(EventArgs e){base.OnFontChanged(e);Fit();}
    protected override void OnResize(EventArgs e){base.OnResize(e);Fit();}
    protected override void OnDpiChangedAfterParent(EventArgs e){base.OnDpiChangedAfterParent(e);Fit();}
}
