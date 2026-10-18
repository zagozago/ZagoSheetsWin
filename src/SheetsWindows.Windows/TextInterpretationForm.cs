using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

// Per-import choice, shown only for ambiguity or deliberate repair; no global setting.
internal sealed class TextInterpretationForm : Form
{
    private readonly ComboBox delimiter = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top };
    private readonly ComboBox encoding = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top };
    internal TextImportOptions Options => new(encoding.SelectedIndex == 1 ? "windows-1252" : "auto",
        new[] { "auto", "comma", "semicolon", "tab", "pipe" }[delimiter.SelectedIndex]);
    internal TextInterpretationForm(bool repair = false)
    {
        Text = UiText.Get(repair ? "recovery.repairNew" : "processing.title");
        ClientSize = new Size(620, repair ? 300 : 220); MinimumSize = Size;
        AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterParent;
        delimiter.Items.AddRange([UiText.Get("setup.delimiterAuto"), UiText.Get("setup.delimiterComma"), UiText.Get("setup.delimiterSemicolon"), "TSV: TAB (↹)", "CSV: |"]);
        encoding.Items.AddRange([UiText.Get("setup.encodingUnicode") + " / Windows-1252", UiText.Get("setup.encodingLegacy")]);
        delimiter.SelectedIndex = 0; encoding.SelectedIndex = 0;
        delimiter.AccessibleName = UiText.Get("setup.delimiterAuto"); encoding.AccessibleName = UiText.Get("setup.encodingUnicode");
        var body = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(16), AutoScroll = true };
        if (repair) body.Controls.Add(new Label { Text = UiText.Get("recovery.repairExplanation"), AutoSize = true, MaximumSize = new Size(570, 0) });
        delimiter.Width = encoding.Width = 560; body.Controls.Add(delimiter); body.Controls.Add(encoding);
        var accept = new AdaptiveButton { Text = UiText.Get(repair ? "recovery.repairNew" : "action.retry"), AutoSize = true, DialogResult = DialogResult.OK };
        var cancel = new AdaptiveButton { Text = UiText.Get("action.cancel"), AutoSize = true, DialogResult = DialogResult.Cancel };
        accept.Enabled = repair;
        delimiter.SelectedIndexChanged += (_, _) => accept.Enabled = repair || delimiter.SelectedIndex > 0;
        var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, Padding = new Padding(12), WrapContents = true };
        actions.Controls.AddRange([accept, cancel]); Controls.Add(body); Controls.Add(actions);
        AcceptButton = accept; CancelButton = cancel; Branding.Apply(this, aboutButton: false, compact: true);
    }
}
