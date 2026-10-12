using SheetsWindows.Core;
using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

// Offline native regression checks on the same WinForms message loop used by the app.
internal static class InterfaceVerification
{
    public static int Run(string output)
    {
        Console.Error.WriteLine("VERIFY: language selection");
        var measurements = new Dictionary<string, ProcessingMetrics>();
        void Require(bool value, string detail) { if (!value) throw new InvalidOperationException(detail); }
        using (var languageOwner = new LauncherForm(new(LauncherAction.Home)))
        {
            languageOwner.Show(); Application.DoEvents();
            var openButton = Descendants(languageOwner).OfType<Button>().Single(b => b.Text == UiText.Get("home.open"));
            var container = openButton.Parent!;
            Require(Math.Abs((openButton.Left - container.Padding.Left) - (container.ClientSize.Width - container.Padding.Right - openButton.Right)) <= 1,
                "The main action must retain symmetric margins without reserving an absent scrollbar.");

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
        using (var picker = new LanguagePicker())
        {
            picker.Show(); Application.DoEvents();
            var list = Descendants(picker).OfType<ListBox>().Single();
            var apply = (Button)picker.AcceptButton!;
            var originalLanguage = UiText.Language;
            var originalPreference = ApplicationLanguages.Preference;
            var originalTitle = picker.Text;
            for (var i = 0; i < LanguageSettings.Available.Count; i++)
            {
                var language = LanguageSettings.Available[i];
                list.SelectedIndex = i + 1; Application.DoEvents();
                Require(apply.Text == UiText.GetForLanguage(language.Code, "language.apply"), $"Apply caption must preview {language.Code}.");
                Require(apply.RightToLeft == (language.Direction == "rtl" ? RightToLeft.Yes : RightToLeft.No), "Only the apply caption follows the selected reading direction.");
                Require(UiText.Language == originalLanguage && ApplicationLanguages.Preference == originalPreference && picker.Text == originalTitle,
                    "Previewing a language must not change the app, preference or dialog title.");
            }
            list.SelectedIndex = 0; Application.DoEvents();
            var automatic = LanguageSettings.Resolve(LanguageSettings.Automatic, LanguageSettings.WindowsPreferredLanguages());
            Require(apply.Text == UiText.GetForLanguage(automatic, "language.apply"), "Automatic preview follows the resolved Windows language.");
            picker.Close();
        }
        using (var action = new MenuActionButton())
        {
            var invoked = 0; action.Click += (_, _) => invoked++;
            action.InvokeAction(); action.Enabled = false; action.InvokeAction();
            Require(invoked == 1, "Recovery menu must invoke enabled handlers and refuse disabled actions.");
        }
        Console.Error.WriteLine("VERIFY: first use");
        var wizardRoot = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(output))!, "wizard-fixture-" + Guid.NewGuid().ToString("N"));
        var wizardStorage = new LocalStorage(wizardRoot);
        var connectionAttempts = 0; var saved = 0;
        using (var wizard = new FirstUseWizard(storage: wizardStorage,
            authorize: _ => ++connectionAttempts == 1 ? Task.FromException(new AuthorizationRequiredException()) : Task.CompletedTask,
            save: _ => { saved++; return Task.CompletedTask; }))
        {
            wizard.Show(); Application.DoEvents();
            Require(!Descendants(wizard).OfType<CheckBox>().Any(c => c.Text == UiText.Get("tutorial.hide")), "Post-install wizard must not expose the help hide preference.");
            Require(!Descendants(wizard).OfType<Button>().Any(b => b.Text == UiText.Get("tutorial.skip")), "Post-install wizard must not expose skip tutorial.");
            wizard.NextButton.PerformClick(); Application.DoEvents();
            Require(wizard.CurrentPage == 1 && !wizard.NextButton.Enabled && wizard.PreviousButton.Enabled, "Google step must require successful authorization before navigation.");
            wizard.PreviousButton.PerformClick(); Require(wizard.CurrentPage == 0, "An unauthenticated user must be able to go back."); wizard.NextButton.PerformClick();
            wizard.ConnectButton.PerformClick(); Application.DoEvents();
            Require(!wizard.AuthorizationConfirmed && !wizard.NextButton.Enabled, "Failed authorization must leave the wizard on the Google step.");
            wizard.ConnectButton.PerformClick(); Application.DoEvents();
            Require(wizard.AuthorizationConfirmed && wizard.NextButton.Enabled, "Successful authorization must unlock navigation.");
            wizard.NextButton.PerformClick(); wizard.NextButton.PerformClick(); Application.DoEvents();
            Require(wizard.CurrentPage == 3 && wizard.NextButton.Enabled, "Defaults are optional; replacement acknowledgement is required before completion.");
            Require(!Descendants(wizard).OfType<CheckBox>().Any(), "The simplified final step must contain no switches or old acknowledgement controls.");
            Require(Descendants(wizard).OfType<NumericUpDown>().Count() == 2, "The final step must expose only retention and quota settings.");
            wizard.NextButton.PerformClick(); Application.DoEvents();
            Require(wizard.Completed && saved == 1, "Finishing must save once and complete without opening a second setup dialog.");
        }
        Require(!Directory.Exists(wizardRoot), "Offline wizard verification must not write user settings or OAuth data.");
        using (var closable = new FirstUseWizard(storage: wizardStorage, authorize: _ => Task.CompletedTask))
        {
            closable.Show(); closable.NextButton.PerformClick(); Application.DoEvents(); closable.Close();
            Require(closable.IsDisposed && !closable.Completed, "Closing without authorization must cancel first use without marking it complete.");
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
        Console.Error.WriteLine("VERIFY: backup selection");
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
            Require(list.Items.Count == 3 && list.Items.Cast<ListViewItem>().All(i => ((RecoveryEntry)i.Tag!).CleanupState != 2), "Deleted backup history must be absent from the recovery list.");
            Require(list.MultiSelect && list.CheckBoxes, "Backups must support multiple selection.");
            Descendants(recovery).OfType<Button>().Single(b => b.Text == "Selecionar todos").PerformClick();
            Require(list.CheckedItems.Count == 3 && list.SelectedItems.Count == 3, "Select all must include pending backups for explicit confirmed discard.");
            Require(Descendants(recovery).OfType<Button>().Single(b => b.Text == "Apagar selecionados…").Enabled, "Pending backups must allow the confirmed manual delete action.");
            Descendants(recovery).OfType<Button>().Single(b => b.Text == "Limpar seleção").PerformClick();
            Require(list.CheckedItems.Count == 0, "Clear selection must remove all marks.");
            list.Items[0].Selected = list.Items[1].Selected = true; Application.DoEvents();
            Require(!Descendants(recovery).OfType<Button>().Single(b => b.Text == "Restaurar em…").Enabled, "Restoring must require exactly one selected file.");
            foreach (var size in new[] { new Size(980, 680), new Size(760, 560) })
            {
                recovery.Size = size; recovery.PerformLayout(); Application.DoEvents();
                foreach (var button in Descendants(recovery).OfType<Button>().Where(b => b.Visible))
                    Require((button.Parent is ScrollableControl { AutoScroll: true } || button.Bottom <= button.Parent!.ClientSize.Height) && button.Right <= button.Parent!.ClientSize.Width,
                        $"Backup action '{button.Text}' must fit its container: {button.Bounds}, container {button.Parent.ClientSize}, window {recovery.Size}.");
            }
            recovery.Hide();
        }
        Console.Error.WriteLine("VERIFY: settings and information");
        using (var setup = new SetupForm(preview: true))
        {
            setup.Show(); Application.DoEvents();
            Require(!Descendants(setup).OfType<CheckBox>().Any(b => b.Text == UiText.Get("setup.noSync")), "Synchronized folder preferences belong in the advanced modal.");
            Require(!Descendants(setup).OfType<Button>().Any(b => b.Text == UiText.Get("setup.save")), "Preferences save automatically without a Save button.");
            using (var advanced = new AdvancedPreferencesForm(LocalStorage.ForCurrentUser(), preview: true))
            {
                advanced.Show(setup); Application.DoEvents();
                var sync = Descendants(advanced).OfType<CheckBox>().Single(b => b.Text == UiText.Get("setup.noSync"));
                Require(sync.Checked, "Default folder policy must be preserved.");
                Require(Descendants(advanced).OfType<CheckBox>().Any(b => b.Text == UiText.Get("setup.replaceXls")), "Advanced options must preserve XLS preferences.");
                Require(Descendants(advanced).OfType<InformationButton>().Count() == 4, "Advanced help is divided into three groups plus header information.");
                Require(Descendants(advanced).OfType<Button>().Count(b => b.Text == UiText.Get("setup.chooseClient")) == 1, "Advanced options must provide the only JSON selector.");
                var selectors = Descendants(advanced).OfType<ComboBox>().ToArray();
                Require(selectors.Length == 2 && selectors[0].Parent == selectors[1].Parent && selectors[0].Top == selectors[1].Top,
                    "Encoding and delimiter selectors must be side by side.");
                Require(Descendants(advanced).OfType<FlowLayoutPanel>().Where(p => p.Dock == DockStyle.Fill).All(p => !p.VerticalScroll.Visible),
                    "Portuguese advanced options must fit without vertical scrolling at the reference size.");
                advanced.Close();
            }
            using (var bitmap = new Bitmap(setup.Width, setup.Height))
            {
                setup.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                bitmap.Save(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(output))!, "settings-reference.png"));
            }
            foreach (var panel in Descendants(setup).OfType<FlowLayoutPanel>().Where(p => p.Dock == DockStyle.Fill))
                Console.Error.WriteLine($"SETTINGS: viewport={panel.ClientSize}, content={panel.DisplayRectangle}, children={string.Join(';', panel.Controls.Cast<Control>().Select(c => $"{c.GetType().Name}:{c.Bounds}"))}");
            Require(Descendants(setup).OfType<FlowLayoutPanel>().Where(p => p.Dock == DockStyle.Fill).All(p => !p.VerticalScroll.Visible), "Portuguese settings must fit without vertical scrolling at the reference size.");
            Require(Descendants(setup).OfType<Label>().Any(l => l.AccessibleName == "Estado da autorização Google" && !string.IsNullOrWhiteSpace(l.Text)), "Google authorization must have a visible accessible status.");
            Require(Descendants(setup).OfType<Button>().Any(b => b.Text == UiText.Get("setup.advanced")), "Advanced navigation must be available.");
            var informationButtons = Descendants(setup).OfType<InformationButton>().ToArray();
            Require(informationButtons.Length == 4, "Settings must have global information and three section pop-ups.");
            informationButtons[0].OpenInformation(); Application.DoEvents();
            Require(informationButtons[0].PopupVisible && informationButtons[0].PopupContent is not null, "Contextual information must open without a second settings modal.");
            setup.Hide();
        }
        Console.Error.WriteLine("VERIFY: advanced JSON selection");
        var clientTestRoot = Path.Combine(Path.GetTempPath(), "zagosheets-client-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(clientTestRoot);
        try
        {
            var clientStorage = new LocalStorage(Path.Combine(clientTestRoot, "state"));
            var invalidPath = Path.Combine(clientTestRoot, "invalid.json"); File.WriteAllText(invalidPath, "{}");
            var validPath = Path.Combine(clientTestRoot, "desktop.json");
            File.WriteAllText(validPath, System.Text.Json.JsonSerializer.Serialize(new { installed = new { client_id = "preview.apps.googleusercontent.com", client_secret = "test-only" } }));
            using (var advanced = new AdvancedPreferencesForm(clientStorage))
            {
                advanced.Show(); Application.DoEvents();
                var choose = Descendants(advanced).OfType<Button>().Single(b => b.Text == UiText.Get("setup.chooseClient"));
                Require(choose.Enabled, "An unconfigured client must allow JSON selection.");
                void Complete(Task task)
                {
                    var deadline = System.Diagnostics.Stopwatch.StartNew();
                    while (!task.IsCompleted && deadline.Elapsed < TimeSpan.FromSeconds(5)) Application.DoEvents();
                    Require(task.IsCompletedSuccessfully, "Advanced client selection must finish on the UI message loop.");
                }
                Complete(advanced.ApplyClientAsync(invalidPath));
                Require(!File.Exists(LauncherConfiguration.ClientPath(clientStorage)) && choose.Enabled, "Invalid JSON must leave configuration untouched and selection available.");
                Complete(advanced.ApplyClientAsync(validPath));
                Require(File.ReadAllText(LauncherConfiguration.ClientPath(clientStorage)) == File.ReadAllText(validPath) && !choose.Enabled,
                    "The JSON chosen in advanced options must be validated and persisted using the existing configuration rules.");
                advanced.Close();
            }
            using (var reopened = new AdvancedPreferencesForm(clientStorage))
            {
                reopened.Show(); Application.DoEvents();
                Require(!Descendants(reopened).OfType<Button>().Single(b => b.Text == UiText.Get("setup.chooseClient")).Enabled,
                    "Reopening must preserve the configured client and the existing migration restriction.");
                reopened.Close();
            }
        }
        finally { Directory.Delete(clientTestRoot, recursive: true); }
        Console.Error.WriteLine("VERIFY: preference autosave");
        using (var owner = new Form())
        {
            var content = new Panel(); var status = new Label(); owner.Controls.Add(content); owner.Controls.Add(status);
            owner.Show(); Application.DoEvents();
            var saver = new PreferenceAutosave(owner, content, status, preview: false);
            var writes = new List<int>();
            var first = saver.SaveAsync(async () => { await Task.Yield(); writes.Add(1); }, () => { });
            var second = saver.SaveAsync(async () => { await Task.Yield(); writes.Add(2); }, () => { });
            var completed = Task.WhenAll(first, second);
            var deadline = System.Diagnostics.Stopwatch.StartNew();
            while (!completed.IsCompleted && deadline.Elapsed < TimeSpan.FromSeconds(5)) Application.DoEvents();
            Require(completed.IsCompletedSuccessfully && writes.SequenceEqual(new[] { 1, 2 }) && content.Enabled,
                "Rapid preference saves must commit in order and restore input after the last write.");
            var restored = false;
            var failure = saver.SaveAsync(() => Task.FromException(new IOException("offline verification")), () => restored = true);
            Require(failure.IsCompletedSuccessfully && restored && content.Enabled && status.Text.Length > 0,
                "Failed preference writes must restore the saved value and display an error.");
            restored = false;
            var failedEarlier = saver.SaveAsync(async () => { await Task.Yield(); throw new IOException("offline verification"); }, () => restored = true);
            var latest = saver.SaveAsync(() => { writes.Add(3); return Task.CompletedTask; }, () => { });
            completed = Task.WhenAll(failedEarlier, latest); deadline.Restart();
            while (!completed.IsCompleted && deadline.Elapsed < TimeSpan.FromSeconds(5)) Application.DoEvents();
            Require(completed.IsCompletedSuccessfully && !restored && writes.Last() == 3,
                "An earlier failed save must not restore over a later queued preference.");
            owner.Close();
        }
        Console.Error.WriteLine("VERIFY: help");
        using (var tutorial = new TutorialForm())
        {
            tutorial.Show(); Application.DoEvents();
            Require(!Descendants(tutorial).OfType<CheckBox>().Single().Checked, "Showing the tutorial at startup requires explicit opt-in.");
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
        Console.Error.WriteLine("VERIFY: live language switching");
        // Switching must update existing instances, preserving checkbox values and identity.
        using (var settings = new SetupForm(preview: true))
        using (var advancedSettings = new AdvancedPreferencesForm(LocalStorage.ForCurrentUser(), preview: true))
        using (var backups = new RecoveryForm(preview: true))
        using (var tutorial = new TutorialForm(3))
        using (var picker = new LanguagePicker())
        {
            settings.Show(); advancedSettings.Show(); backups.Show(); tutorial.Show(); picker.Show(); Application.DoEvents();
            var sync = Descendants(advancedSettings).OfType<CheckBox>().Single(c=>c.Text==UiText.Get("setup.noSync"));
            sync.Checked=false;
            var list = Descendants(backups).OfType<ListView>().Single();list.Items[0].Checked=true;
            var filename=list.Items[0].Text;
            UiText.Select("en"); Application.DoEvents();
            Require(Descendants(home).OfType<Button>().Any(b=>b.Text=="&Help"),"Open home must switch to English.");
            Require(settings.Text=="Settings - ZagoSheetsWin" && sync.Text.StartsWith("I do not sync") && !sync.Checked,"Language change must preserve unsaved settings.");
            var googleInfo = Descendants(settings).OfType<InformationButton>().First(); googleInfo.OpenInformation(); Application.DoEvents();
            Require(!Descendants(googleInfo.PopupContent!).OfType<Button>().Any(b => b.Text == UiText.Get("setup.chooseClient")), "Google information must contain explanations without client configuration controls.");
            Require(Descendants(advancedSettings).OfType<Button>().Single(b => b.Text == UiText.Get("setup.chooseClient")).Text.Contains("JSON"), "Advanced client selection must use the current language.");
            Require(list.Items[0].Checked && list.Items[0].Text==filename && list.Columns[0].Text=="File","Language change must preserve recovery data and selection.");
            Require(Descendants(tutorial).OfType<Label>().Any(l=>l.Text.Contains("Keep the original")),"Existing tutorial must refresh its current page.");
            Require(Descendants(tutorial).OfType<FlowLayoutPanel>().All(p=>!p.VerticalScroll.Visible),"English tutorial must not gain vertical scrolling.");
            foreach(var field in Descendants(picker).Where(c=>c is TextBox or ListBox or EmphasisLabel))
                Require(field.Right<=field.Parent!.ClientSize.Width-field.Parent.Padding.Right,$"Language picker content must remain inside its modal: '{field.Text}'.");
            Require(UiText.Get("error.backupQuota").Contains("original was preserved"),"English errors must retain preservation guidance.");
            foreach(var form in new Form[]{home,settings,advancedSettings,backups,tutorial,picker})
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
        Console.Error.WriteLine("VERIFY: processing retry");
        var retryAttempts = 0;
        using (var automaticRetry = new ProcessingForm(new(LauncherAction.Open, "preview.xlsx"), execute: async (_, _) =>
        { await Task.Yield(); if (++retryAttempts == 1) throw new HttpRequestException(); }, recordDiagnostics:false,
            retryDelay: (_, ct) => { ct.ThrowIfCancellationRequested(); return Task.CompletedTask; }))
        {
            automaticRetry.ShowDialog(); Require(retryAttempts == 2 && automaticRetry.ExitCode == 0, "Transient import failure must retry and close on success.");
        }
        var manualAttempts = 0;
        using (var manualRetry = new ProcessingForm(new(LauncherAction.Open, "preview.xlsx"), execute: async (_, _) =>
        { await Task.Yield(); if (++manualAttempts == 1) throw new InvalidOperationException(); }, recordDiagnostics:false))
        using (var timer = new System.Windows.Forms.Timer { Interval = 25 })
        {
            timer.Tick += (_, _) => { if (manualRetry.IsBusy) return; var retry = Descendants(manualRetry).OfType<Button>().SingleOrDefault(b => b.Text == UiText.Get("action.retry")); if (retry is null) return; timer.Stop(); retry.PerformClick(); };
            timer.Start(); manualRetry.ShowDialog(); Require(manualAttempts == 2 && manualRetry.ExitCode == 0, "Manual retry must rerun the same request and close after success.");
        }
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
        Console.Error.WriteLine("VERIFY: 51 locale layouts");
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

