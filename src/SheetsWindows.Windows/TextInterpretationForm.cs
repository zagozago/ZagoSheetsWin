using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

// Per-import choice, shown only for ambiguity or deliberate repair; no global setting.
internal sealed class TextInterpretationForm : Form
{
    private readonly ComboBox delimiter = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top };
    private readonly ComboBox encoding = new() { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Top };
    internal TextImportOptions Options => new(encoding.SelectedIndex == 1 ? "windows-1252" : "auto",
        new[] { "auto", "comma", "semicolon", "tab", "pipe" }[delimiter.SelectedIndex]);
    internal TextInterpretationForm(bool repair = false, string? sourcePath = null)
    {
        Text = UiText.Get(repair ? "recovery.repairNew" : "processing.title");
        ClientSize = new Size(620, sourcePath is not null ? 510 : repair ? 300 : 220); MinimumSize = Size;
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
        if (sourcePath is not null)
        {
            var preview = new DataGridView { Width = 560, Height = 190, ReadOnly = true, AllowUserToAddRows = false,
                AllowUserToDeleteRows = false, RowHeadersVisible = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            var feedback = new Label { AutoSize = true, MaximumSize = new Size(560, 0) };
            body.Controls.Add(preview); body.Controls.Add(feedback);
            var generation = 0;
            async void UpdatePreview(object? sender, EventArgs args)
            {
                var current = ++generation; var options = Options;
                accept.Enabled = false; preview.Columns.Clear(); feedback.Text = "";
                if (options.Delimiter == "auto") return;
                try
                {
                    var rows = await Task.Run(() =>
                    {
                        using var input = new FileStream(sourcePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        if (input.Length > GoogleImport.MaxBytes) throw new SpreadsheetCapacityException(UiText.Message("capacity.importSize"));
                        var bytes = new byte[checked((int)input.Length)]; input.ReadExactly(bytes);
                        return SpreadsheetFormats.ReadText(bytes, SpreadsheetFormats.Format(sourcePath), options).Take(3).Select(r => r.Take(12).ToArray()).ToArray();
                    });
                    if (IsDisposed || current != generation) return;
                    for (var c = 0; c < rows[0].Length; c++) preview.Columns.Add("c" + c, (c + 1).ToString(System.Globalization.CultureInfo.CurrentCulture));
                    foreach (var row in rows) preview.Rows.Add(row.Select(value => value ?? "").ToArray());
                    accept.Enabled = true;
                }
                catch (Exception failure) when (LauncherErrors.Expected(failure))
                { if (!IsDisposed && current == generation) feedback.Text = LauncherErrors.Message(failure); }
            }
            delimiter.SelectedIndexChanged += UpdatePreview; encoding.SelectedIndexChanged += UpdatePreview;
        }
        var actions = new FlowLayoutPanel { Dock = DockStyle.Bottom, AutoSize = true, Padding = new Padding(12), WrapContents = true };
        actions.Controls.AddRange([accept, cancel]); Controls.Add(body); Controls.Add(actions);
        AcceptButton = accept; CancelButton = cancel; Branding.Apply(this, aboutButton: false, compact: true);
    }
}
