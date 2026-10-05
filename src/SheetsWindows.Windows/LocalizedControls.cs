using SheetsWindows.Infrastructure;
using System.Runtime.CompilerServices;

namespace SheetsWindows.Windows;

// Each text is bound to its semantic descriptor when assigned. Data fields and
// file names are never translated. Existing instances keep state and handlers.
internal static class LocalizedControls
{
    private sealed class Binding { internal LocalizedMessage? Message; }
    private static readonly ConditionalWeakTable<Control, Binding> bound = new();
    internal static void Attach(Form form)
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
            if(bound.TryGetValue(control,out var binding) && binding.Message is {} message)
            {var saved=message;control.Text=message.Text;binding.Message=saved;}
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
    }
}

// Native button wrapping, with the actual font and DPI, including padding and
// focus space. Flow rows wrap; constrained full-width buttons grow in height.
internal class AdaptiveButton : Button
{
    private bool fitting;
    public AdaptiveButton(){Padding=new Padding(12,6,12,6);AutoEllipsis=false;}
    public override Size GetPreferredSize(Size proposedSize)
    {
        var max=Parent is FlowLayoutPanel flow ? Math.Max(80,flow.ClientSize.Width-flow.Padding.Horizontal-Margin.Horizontal) : proposedSize.Width;
        if(max<=0)max=int.MaxValue;
        var natural=TextRenderer.MeasureText(Text,Font,Size.Empty,TextFormatFlags.SingleLine);
        var width=Math.Min(max,natural.Width+Padding.Horizontal+12);
        var text=TextRenderer.MeasureText(Text,Font,new Size(Math.Max(20,width-Padding.Horizontal-12),int.MaxValue),TextFormatFlags.WordBreak);
        return new Size(Math.Max(MinimumSize.Width,width),Math.Max(MinimumSize.Height,text.Height+Padding.Vertical+10));
    }
    private void Fit()
    {
        if(fitting || Width<30)return;
        fitting=true;
        try
        {
            var measured=TextRenderer.MeasureText(Text,Font,new Size(Math.Max(20,Width-Padding.Horizontal-12),int.MaxValue),TextFormatFlags.WordBreak);
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
    protected override void OnTextChanged(EventArgs e){base.OnTextChanged(e);Fit();Parent?.PerformLayout();}
    protected override void OnFontChanged(EventArgs e){base.OnFontChanged(e);Fit();}
    protected override void OnResize(EventArgs e){base.OnResize(e);Fit();}
    protected override void OnDpiChangedAfterParent(EventArgs e){base.OnDpiChangedAfterParent(e);Fit();}
}
