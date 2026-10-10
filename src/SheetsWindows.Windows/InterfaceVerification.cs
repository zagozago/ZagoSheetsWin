using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

// Offline native regression checks on the same WinForms message loop used by the app.
internal static class InterfaceVerification
{
    public static int Run(string output)
    {
        var measurements = new Dictionary<string, ProcessingMetrics>();
        void Require(bool value, string detail) { if (!value) throw new InvalidOperationException(detail); }
        using (var languageOwner = new LauncherForm(new(LauncherAction.Home)))
        {
            languageOwner.Show(); Application.DoEvents();
            foreach (var code in new[] { "ar", "hi", "en", "pt" })
            {
                languageOwner.WindowState = FormWindowState.Minimized;
                UiText.Select(code); LanguageVisuals.RestoreAfterSelection(languageOwner); Application.DoEvents();
                Require(languageOwner.Visible && languageOwner.WindowState != FormWindowState.Minimized,
                    $"Language change to {code} must restore the existing window.");
                Require(languageOwner.RightToLeftLayout == UiText.RightToLeft, "Restored window must use the selected text direction.");
            }
            languageOwner.Hide();
        }
        using (var action = new MenuActionButton())
        {
            var invoked = 0; action.Click += (_, _) => invoked++;
            action.InvokeAction(); action.Enabled = false; action.InvokeAction();
            Require(invoked == 1, "Recovery menu must invoke enabled handlers and refuse disabled actions.");
        }
        Branding.PreviewTheme(ApplicationTheme.Light);
        using var home = new LauncherForm(new(LauncherAction.Home));
        using var preview = new ProcessingForm(new(LauncherAction.Open, "preview.xlsx"), preview: true);
        home.Show(); preview.Show(); Application.DoEvents();
        Require(Descendants(home).OfType<Button>().Any(b => b.Text == "&Ajuda"), "Home help must use the short Portuguese label.");
        Require(Descendants(home).OfType<EmphasisLabel>().All(l => l.Font.Bold || !l.HasEmphasis), "Home sentences must not apply automatic phrase emphasis.");
        using (var information = new AboutForm())
        {
            Require(Descendants(information).OfType<LinkLabel>().Any(l => l.Text == "Site do Zagotools"), "About must expose the Zagotools homepage.");
            Require(Descendants(information).OfType<LinkLabel>().Any(l => l.Text == "Licenças do Zagotools"), "About must expose public license information.");
        }
        var about = Descendants(home).OfType<LinkLabel>().Single(l => l.Text == "Sobre / Licenças");
        Require(about.Width >= TextRenderer.MeasureText(about.Text, about.Font, Size.Empty, TextFormatFlags.NoPadding).Width, "MIT credit link must not be clipped.");
        using (var recovery = new RecoveryForm(preview: true, previewBusy: true))
        {
            recovery.Show(); Application.DoEvents();
            var stop = Descendants(recovery).OfType<Button>().Single(b => b.Text == "Cancelar retomada");
            var list = Descendants(recovery).OfType<ListView>().Single();
            Require(stop.Visible && stop.Enabled && !stop.Bounds.IntersectsWith(list.Bounds) && stop.Bottom <= recovery.ClientSize.Height, "Recovery cancellation must remain visible outside the list.");
            var numbers = Descendants(recovery).OfType<NumericUpDown>().ToArray();
            Require(numbers.Any(n => n.Value == 30 && n.Maximum == 365) && numbers.Any(n => n.Value == 200 && n.Maximum == 1000), "Backup defaults and quota ceiling must be visible.");
            Require(!Descendants(recovery).OfType<CheckBox>().Single().Checked, "Automatic cleanup requires informed opt-in.");
            Require(list.Columns.Count == 4 && list.Items.Cast<ListViewItem>().All(i => i.Tag is RecoveryEntry), "Backup table must preserve recovery identity and expose four readable columns.");
            Require(list.Items.Count == 4, "Backup preview must cover completed, protected and cleaned history.");
            Require(list.MultiSelect && list.CheckBoxes, "Backups must support multiple selection.");
            Descendants(recovery).OfType<Button>().Single(b => b.Text == "Selecionar todos").PerformClick();
            Require(list.CheckedItems.Count == 2 && list.CheckedItems.Cast<ListViewItem>().All(i => ((RecoveryEntry)i.Tag!).CanClean), "Select all must select only deletable backups.");
            Descendants(recovery).OfType<Button>().Single(b => b.Text == "Limpar seleção").PerformClick();
            Require(list.CheckedItems.Count == 0, "Clear selection must remove all marks.");
            list.Items[0].Selected = list.Items[1].Selected = true; Application.DoEvents();
            Require(!Descendants(recovery).OfType<Button>().Single(b => b.Text == "Restaurar em…").Enabled, "Restoring must require exactly one selected file.");
            foreach (var size in new[] { new Size(700, 650), new Size(560, 540) })
            {
                recovery.Size = size; recovery.PerformLayout(); Application.DoEvents();
                foreach (var button in Descendants(recovery).OfType<Button>().Where(b => b.Visible))
                    Require(button.Bottom <= button.Parent!.ClientSize.Height && button.Right <= button.Parent.ClientSize.Width,
                        $"Backup action '{button.Text}' must fit its container: {button.Bounds}, container {button.Parent.ClientSize}, window {recovery.Size}.");
            }
            recovery.Hide();
        }
        using (var setup = new SetupForm(preview: true))
        {
            setup.Show(); Application.DoEvents();
            var sync = Descendants(setup).OfType<CheckBox>().Single(b => b.Text.StartsWith("Não sincronizo"));
            var folder = Descendants(setup).OfType<Button>().Single(b => b.Text == "Escolher pasta local…");
            Require(sync.Checked && !folder.Visible, "New setup must not require a folder and must collapse its optional controls.");
            sync.Checked = false; Application.DoEvents(); Require(folder.Visible, "Choosing synchronized folders must reveal the folder controls.");
            var folderRow = folder.Parent as TableLayoutPanel;
            Require(folderRow is not null && folderRow.ColumnCount == 2, "Folder selector and path must share a two-column row.");
            var folderPath = folderRow!.Controls.OfType<TextBox>().Single();
            Require(Math.Abs(folder.Width - folderPath.Width) <= 2 && folder.Left < folderPath.Left, "Folder selector and path must each occupy half the row.");
            var folderInfo = folderRow.Parent!.Controls.OfType<Label>().Single();
            Require(folderInfo.Width >= folderRow.Parent.ClientSize.Width * .8, "Synchronization instructions must span the folder panel instead of collapsing to a narrow column.");
            sync.Checked = true; Application.DoEvents(); Require(!folder.Visible, "Disabling folder restrictions must collapse their controls again.");
            Require(Descendants(setup).OfType<Label>().Any(l => l.AccessibleName == "Estado da autorização Google" && !string.IsNullOrWhiteSpace(l.Text)), "Google authorization must have a visible accessible status.");
            Require(Descendants(setup).OfType<Label>().Any(l => l.Text.Contains("drive.file", StringComparison.Ordinal) && l.Text.Contains("diretamente ao Google", StringComparison.Ordinal)), "Setup must show the OAuth data-use disclosure before authorization.");
            Require(Descendants(setup).OfType<LinkLabel>().Any(l => l.Text == "Política de Privacidade"), "Setup must expose the privacy policy link.");
            Require(Descendants(setup).OfType<LinkLabel>().Any(l => l.Text == "Termos de Uso"), "Setup must expose the terms link.");
            var sections = Descendants(setup).OfType<Label>().Where(l => l.Text.StartsWith("1. ") || l.Text.StartsWith("2. ") || l.Text.StartsWith("3. ")).ToArray();
            Require(sections.Length == 3 && sections[0].Text.Contains("Google") && sections[2].Text.Contains("sincronizadas"), "Setup must order Google, Windows defaults and optional sync folders.");
            var advanced = Descendants(setup).OfType<Button>().Single(b => b.Text == "Opções avançadas…");
            var dialog = setup.AdvancedDialog;
            var close = dialog.Controls.OfType<Button>().Single(b => b.Text == "Fechar");
            var checkedModal = false;
            dialog.Shown += (_, _) => dialog.BeginInvoke((Action)(() =>
            {
                checkedModal = dialog.Modal && dialog.Owner == setup && close.Visible && close.Bottom <= dialog.ClientSize.Height;
                close.PerformClick();
            }));
            advanced.PerformClick();
            Require(checkedModal && setup.Enabled && !dialog.Visible, "Advanced settings must be modal, keep Close visible and restore the owner after closing.");
            advanced.PerformClick();
            Require(!dialog.IsDisposed && setup.Enabled, "Advanced settings must reopen after closing.");
            setup.Hide();
        }
        using (var tutorial = new TutorialForm())
        {
            tutorial.Show(); Application.DoEvents();
            Require(!Descendants(tutorial).OfType<CheckBox>().Single().Checked, "Tutorial dismissal must require explicit opt-in.");
            var next = Descendants(tutorial).OfType<Button>().Single(b => b.Text == "Próximo");
            for (var i = 0; i < 2; i++) next.PerformClick();
            Application.DoEvents();
            var standard = Descendants(tutorial).OfType<Label>().Single(l => l.Text == "Defina o ZagoSheetsWin como aplicativo padrão de planilhas (opcional):");
            Require(!standard.Font.Bold, "Tutorial optional defaults instruction must use regular sentence text.");
            Require(!Descendants(tutorial).OfType<Label>().Any(l => l.Text.Contains("ODS experimental")), "Tutorial step three must omit the previous format sentence.");
            next.PerformClick();
            Application.DoEvents();
            foreach (var size in new[] { tutorial.Size, tutorial.MinimumSize })
            {
                tutorial.Size = size; tutorial.PerformLayout(); Application.DoEvents();
                var content = Descendants(tutorial).OfType<FlowLayoutPanel>().Single(p => p.FlowDirection == FlowDirection.TopDown);
                Require(!content.AutoScroll && !content.VerticalScroll.Visible, "Tutorial must have no vertical scrollbar.");
                foreach (Control control in content.Controls)
                    Require(control.Bottom <= content.ClientSize.Height && control.Right <= content.ClientSize.Width, "Tutorial text and actions must remain fully visible without clipping.");
            }
            Require(next.Text == "Começar", "Tutorial must reach its final page.");
            Descendants(tutorial).OfType<Button>().Single(b => b.Text == "Pular tutorial").PerformClick();
            Require(!tutorial.Visible, "Tutorial must be skippable on the final page.");
        }
        Require(home.Font.Name == "Segoe UI", "Native interface must use the shared readable font.");
        Require(Descendants(home).OfType<Button>().Any(b => b.Tag as string == "primary" && b.Text.Contains("Abrir planilha")), "Home must emphasize its primary action.");
        var light = home.BackColor;
        Branding.PreviewTheme(ApplicationTheme.Dark);
        Require(home.BackColor != light && home.BackColor == preview.BackColor, "Theme must update open forms together.");
        var toggle = Descendants(home).OfType<ThemeToggle>().Single();
        Require(toggle.Dark && toggle.AccessibilityObject.Role == AccessibleRole.PushButton && toggle.AccessibilityObject.Value == "Escuro", "Theme action button must expose its dark mode without checkbox semantics.");
        Branding.PreviewTheme(ApplicationTheme.Light);
        Require(home.BackColor == light && !toggle.Dark && toggle.AccessibilityObject.Value == "Claro", "Theme must switch back without restarting.");
        // Switching must update existing instances, preserving checkbox values and identity.
        using (var settings = new SetupForm(preview: true))
        using (var backups = new RecoveryForm(preview: true))
        using (var tutorial = new TutorialForm(3))
        using (var picker = new LanguagePicker())
        {
            settings.Show(); backups.Show(); tutorial.Show(); picker.Show(); Application.DoEvents();
            var sync = Descendants(settings).OfType<CheckBox>().Single(c=>c.Text==UiText.Get("setup.noSync"));
            sync.Checked=false;
            var list = Descendants(backups).OfType<ListView>().Single();list.Items[0].Checked=true;
            var filename=list.Items[0].Text;
            UiText.Select("en"); Application.DoEvents();
            Require(Descendants(home).OfType<Button>().Any(b=>b.Text=="&Help"),"Open home must switch to English.");
            Require(settings.Text=="Settings - ZagoSheetsWin" && sync.Text.StartsWith("I do not sync") && !sync.Checked,"Language change must preserve unsaved settings.");
            Require(list.Items[0].Checked && list.Items[0].Text==filename && list.Columns[0].Text=="File","Language change must preserve recovery data and selection.");
            Require(Descendants(tutorial).OfType<Label>().Any(l=>l.Text.Contains("Keep the original")),"Existing tutorial must refresh its current page.");
            Require(Descendants(tutorial).OfType<FlowLayoutPanel>().All(p=>!p.VerticalScroll.Visible),"English tutorial must not gain vertical scrolling.");
            foreach(var field in Descendants(picker).Where(c=>c is TextBox or ListBox or EmphasisLabel))
                Require(field.Right<=field.Parent!.ClientSize.Width-field.Parent.Padding.Right,$"Language picker content must remain inside its modal: '{field.Text}'.");
            Require(UiText.Get("error.backupQuota").Contains("original was preserved"),"English errors must retain preservation guidance.");
            foreach(var form in new Form[]{home,settings,settings.AdvancedDialog,backups,tutorial,picker})
            {
                foreach(var factor in new[]{1f,1.25f,1.5f,2f})
                {
                    var before=form.Size;form.Scale(new SizeF(factor,factor));form.PerformLayout();Application.DoEvents();
                    foreach(var button in Descendants(form).OfType<AdaptiveButton>().Where(b=>b.Visible))
                    {
                        var room=Math.Max(20,button.Width-button.Padding.Horizontal-12);
                        var needed=TextRenderer.MeasureText(button.Text,button.Font,new Size(room,int.MaxValue),TextFormatFlags.WordBreak).Height+button.Padding.Vertical+10;
                        Require(button.Height>=needed,$"Button text must fit at scale {factor}: '{button.Text}', {button.Size}, needed {needed}.");
                    }
                    form.Scale(new SizeF(1/factor,1/factor));form.Size=before;
                }
            }
            UiText.Select("pt");Application.DoEvents();
            Require(Descendants(home).OfType<Button>().Any(b=>b.Text=="&Ajuda") && !sync.Checked && list.Items[0].Checked,"Switching back must preserve state.");
        }
        home.Hide(); preview.Hide();
        using (var success = new ProcessingForm(new(LauncherAction.Open, "preview.xlsx"), execute: async (_, _) => await Task.Yield(), recordDiagnostics: false))
        {
            success.ShowDialog(); measurements["OfflineSuccessUi"] = success.LastMetrics!; Require(success.ExitCode == 0 && !success.IsBusy, "Successful processing must close automatically.");
        }
        using (var observed = new ProcessingForm(new(LauncherAction.Open, "preview.xlsx"), execute: async (_, ct) => await Task.Delay(350, ct), recordDiagnostics: false))
        {
            observed.ShowDialog(); measurements["OfflineSampledCpuUi"] = observed.LastMetrics!;
            Require(observed.LastMetrics!.PeakCpuPercent is >= 0 and <= 100, "Processing must sample CPU without keeping the process resident.");
        }
        var cancelled = false;
        using (var cancel = new ProcessingForm(new(LauncherAction.Open, "preview.xlsx"), execute: async (_, ct) =>
        {
            try { await Task.Delay(Timeout.Infinite, ct); } catch (OperationCanceledException) { cancelled = true; throw; }
        }, recordDiagnostics: false))
        using (var timer = new System.Windows.Forms.Timer { Interval = 25 })
        {
            timer.Tick += (_, _) => { if (cancel.IsBusy) { timer.Stop(); cancel.Close(); } }; timer.Start();
            cancel.ShowDialog(); measurements["OfflineCancelledUi"] = cancel.LastMetrics!; Require(cancelled && cancel.ExitCode == 1 && !cancel.IsBusy, "Closing busy processing must cancel and await the worker.");
        }
        using (var failure = new ProcessingForm(new(LauncherAction.Copy, "preview.csv"), execute: (_, _) => Task.FromException(new ConversionMismatchException()), recordDiagnostics: false))
        using (var timer = new System.Windows.Forms.Timer { Interval = 25 })
        {
            var failureVisible = false;
            timer.Tick += (_, _) =>
            {
                if (failure.IsBusy || failure.ExitCode != 1) return;
                failureVisible = failure.Visible && Descendants(failure).OfType<Button>().Any(b => b.Text == "Exportar diagnóstico…")
                    && Descendants(failure).OfType<Button>().Any(b => b.Text == "Recuperação / backups");
                timer.Stop(); failure.Close();
            };
            timer.Start(); failure.ShowDialog(); measurements["OfflineFailureUi"] = failure.LastMetrics!; Require(failureVisible, "Failure must stay visible with diagnosis and recovery actions.");
        }
        // Synthetic CSV matching the user case by shape, without publishing their contents.
        var csv = new System.Text.StringBuilder();
        for (var row = 0; row < 5000; row++) { for (var column = 0; column < 40; column++) { if (column > 0) csv.Append(';'); csv.Append("cell_").Append(row).Append('_').Append(column); } csv.Append('\n'); }
        var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        var benchmark = new ProcessingTelemetry(); SpreadsheetPayload payload;
        using (benchmark.Begin(ProcessingPhase.Conversion)) payload = SpreadsheetFormats.Prepare("csv", bytes, new("auto", "semicolon"));
        using (benchmark.Begin(ProcessingPhase.Verification)) SpreadsheetFormats.VerifyValues(payload.Expected!, payload.Bytes);
        measurements["OfflineCsv5000x40RoundTrip"] = benchmark.Capture();
        LocaleVerification.Run(output);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(output))!);
        File.WriteAllText(output, System.Text.Json.JsonSerializer.Serialize(new { Measurements = measurements, InputBytes = bytes.Length, Rows = 5000, Columns = 40, GoogleNetwork = false }));
        return 0;
    }
    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach (Control child in parent.Controls) { yield return child; foreach (var descendant in Descendants(child)) yield return descendant; }
    }
}

