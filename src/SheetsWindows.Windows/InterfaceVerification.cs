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
                var actionCaptions = new[] { "home.open", "home.settings", "home.backups", "home.help" }.Select(UiText.Get).ToHashSet();
                foreach (var action in Descendants(languageOwner).OfType<Button>().Where(b => actionCaptions.Contains(b.Text)))
                    Require(Math.Abs(action.Font.SizeInPoints - 9f) < 0.01f && action.Font.FontFamily.Name == languageOwner.Font.FontFamily.Name,
                        "Home actions must use the original 9-point font and the selected language family.");
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
        using (var partial = new ProcessingForm(new(LauncherAction.Open, "partial.ods"), preview: true))
        {
            partial.ShowImportNotice(UiText.Format("import.notice.partialConversion", ("backups", UiText.Get("home.backups").Replace("&", "", StringComparison.Ordinal))));
            partial.Show(); Application.DoEvents();
            Require(!Descendants(partial).OfType<CheckBox>().Single(b => b.Name == "importNoticeDontShowAgain").Checked, "Import confirmation opt-out must start unchecked.");
            Require(partial.Text == Branding.Name && Descendants(partial).OfType<Button>().Any(b => b.Name == "importNoticeBackups" && b.Enabled),
                "A successful partial import must offer backup recovery without a copy-only title.");
            partial.Hide();
        }
        Console.Error.WriteLine("VERIFY: disconnected file startup and disabled buttons");
        foreach (var extension in new[] { "csv", "tsv", "xlsx", "xls", "ods" })
        {
            var request = LauncherRequest.Parse([Path.Combine(Path.GetTempPath(), "disconnected." + extension)]);
            using var missingAuthorization = Program.StartupConfiguration(request.Action, false, true, preview: true);
            using var missingSetup = Program.StartupConfiguration(request.Action, true, true, preview: true);
            Require(missingAuthorization is SetupForm && missingSetup is SetupForm, "Opening a local spreadsheet without connection must show Settings.");
            Require(Program.StartupConfiguration(request.Action, false, false, preview: true) is null, "Connected file openings must continue directly to processing.");
        }
        using (var firstUse = Program.StartupConfiguration(LauncherAction.FirstUse, false, false, preview: true))
            Require(firstUse is FirstUseWizard, "Explicit post-install first use must retain its wizard.");
        using (var owner = new Form())
        {
            var normal = new AdaptiveButton { Text = "Action", Width = 150 };
            var primary = new AdaptiveButton { Text = "Primary", Width = 150, Top = 60 }; Ui.Primary(primary);
            owner.Controls.Add(normal); owner.Controls.Add(primary); Branding.Apply(owner); owner.Show(); Application.DoEvents();
            foreach (var theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark })
            {
                Branding.PreviewTheme(theme); Application.DoEvents();
                foreach (var button in new[] { normal, primary })
                {
                    var enabledText = button.ForeColor; var enabledBorder = button.FlatAppearance.BorderColor;
                    button.Enabled = false; Application.DoEvents();
                    Require(button.ForeColor != enabledText && button.FlatAppearance.BorderColor != enabledBorder, "Disabled actions must visibly fade text and border.");
                    Branding.Refresh(owner); Application.DoEvents();
                    Require(button.ForeColor != enabledText, "Theme refresh must not restore active styling to disabled actions.");
                    button.Enabled = true; Application.DoEvents();
                    Require(button.ForeColor == enabledText && button.FlatAppearance.BorderColor == enabledBorder, "Enabled actions must restore their original theme colors.");
                }
            }
            owner.Hide();
        }
        Branding.PreviewTheme(ApplicationTheme.Light);
        using (var waiting = new SetupForm(preview: true, previewBusy: true))
        {
            waiting.Show(); Application.DoEvents();
            var accountActions = Descendants(waiting).OfType<TableLayoutPanel>().Single(p => p.ColumnCount == 3).Controls.OfType<Button>().ToArray();
            var cancelConnection = accountActions.Single(b => b.Text == UiText.Get("setup.cancelConnection"));
            var finishSetup = Descendants(waiting).OfType<Button>().Single(b => b.Text == UiText.Get("action.close"));
            Require(cancelConnection.Enabled && accountActions.Where(b => b != cancelConnection).All(b => !b.Enabled) && !finishSetup.Enabled,
                "Waiting for authorization must disable actual actions, including Close, while keeping Cancel available.");
            cancelConnection.PerformClick(); Application.DoEvents();
            Require(finishSetup.Enabled && accountActions[0].Enabled, "Cancelling authorization must restore the settings actions.");
            waiting.Hide();
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
            wizard.NextButton.PerformClick(); Application.DoEvents();
            Require(wizard.CurrentPage == 2 && wizard.NextButton.Enabled && wizard.NextButton.Text == UiText.Get("tutorial.start") && !wizard.Completed, "The third page must finish setup with Start after authorization.");
            Require(!Descendants(wizard).OfType<CheckBox>().Any(), "The simplified final step must contain no switches or old acknowledgement controls.");
            Require(!Descendants(wizard).OfType<NumericUpDown>().Any(), "The removed backup page must not expose retention or quota settings.");
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
        using (var firstNotice = new ProcessingForm(new(LauncherAction.Open, "first.xlsx"), preview: true))
        using (var nextNotice = new ProcessingForm(new(LauncherAction.Open, "next.csv"), preview: true))
        {
            Require(firstNotice.AcquireNotice(), "First import notice must acquire the per-session gate.");
            Require(!nextNotice.AcquireNotice(), "Concurrent imports must not stack confirmation notices.");
            firstNotice.Dispose();
            Require(nextNotice.AcquireNotice(), "Closing a notice must allow a later import notice.");
        }
        Console.Error.WriteLine("VERIFY: backup selection");
        var policyRoot = Path.Combine(Path.GetTempPath(), "zsw-policy-ui-" + Guid.NewGuid().ToString("N"));
        try
        {
            var policyStorage = new LocalStorage(policyRoot);
            using (var autosave = new RecoveryForm(preview: true, policyStorage: policyStorage))
            {
                autosave.Shown += (_, _) =>
                {
                    var fields = Descendants(autosave).OfType<NumericUpDown>().ToArray();
                    fields.Single(n => n.Maximum == 365).Value = 45;
                    fields.Single(n => n.Maximum == 1000).Value = 321;
                    Require(!Descendants(autosave).OfType<CheckBox>().Any(), "Backup cleanup is automatic and has no toggle.");
                    autosave.Close();
                };
                autosave.ShowDialog();
            }
            Require(BackupPolicy.Load(policyStorage) == new BackupPolicy(45, 321, true), "Every backup rule edit must auto-save and survive immediate close.");
        }
        finally { if (Directory.Exists(policyRoot)) Directory.Delete(policyRoot, true); }
        using (var recovery = new RecoveryForm(preview: true, previewBusy: true))
        {
            recovery.Show(); Application.DoEvents();
            var stop = Descendants(recovery).OfType<Button>().Single(b => b.Text == "Cancelar retomada");
            var list = Descendants(recovery).OfType<ListView>().Single();
            Require(stop.Visible && stop.Enabled && !stop.Bounds.IntersectsWith(list.Bounds) && stop.Bottom <= recovery.ClientSize.Height, "Recovery cancellation must remain visible outside the list.");
            var restoreAction = Descendants(recovery).OfType<Button>().Single(b => b.Text == UiText.Get("action.restore"));
            var selectionAction = Descendants(recovery).OfType<Button>().Single(b => b.Text == UiText.Get("recovery.selectAll"));
            Require(restoreAction.Parent == selectionAction.Parent && restoreAction.Parent!.Controls.GetChildIndex(restoreAction) == 0
                && restoreAction.Parent is TableLayoutPanel { ColumnCount: 5 } && restoreAction.Tag as string == "primary",
                "Restore must be the first regular primary action beside selection controls.");
            var numbers = Descendants(recovery).OfType<NumericUpDown>().ToArray();
            Require(numbers.Any(n => n.Value == 30 && n.Maximum == 365) && numbers.Any(n => n.Value == 200 && n.Maximum == 1000), "Backup defaults and quota ceiling must be visible.");
            Require(!Descendants(recovery).OfType<CheckBox>().Single().Checked, "Automatic cleanup requires informed opt-in.");
            Require(list.Columns.Count == 4 && list.Items.Cast<ListViewItem>().All(i => i.Tag is RecoveryEntry), "Backup table must preserve recovery identity and expose four readable columns.");
            Require(list.Items.Count == 3 && list.Items.Cast<ListViewItem>().All(i => ((RecoveryEntry)i.Tag!).CleanupState != 2), "Deleted backup history must be absent from the recovery list.");
            Require(list.MultiSelect && !list.CheckBoxes, "Backups must support multiple selection.");
            Descendants(recovery).OfType<Button>().Single(b => b.Text == "Selecionar todos").PerformClick();
            Require(list.CheckedItems.Count == 0 && list.SelectedItems.Count == 3, "Select all must include pending backups for explicit confirmed discard.");
            Require(Descendants(recovery).OfType<Button>().Single(b => b.Text == UiText.Get("recovery.deleteSelected")).Enabled, "Pending backups must allow the confirmed manual delete action.");
            Descendants(recovery).OfType<Button>().Single(b => b.Text == "Limpar seleção").PerformClick();
            Require(list.SelectedItems.Count == 0, "Clear selection must remove all marks.");
            Require(!Descendants(recovery).OfType<Button>().Any(b => b.Text == UiText.Get("recovery.savePolicy")), "Backup policy must auto-save without a save button.");
            Require(!Descendants(recovery).OfType<CheckBox>().Any(), "Automatic backup cleanup must not expose a switch.");
            var statistics = Descendants(recovery).OfType<Label>().Single(l => l.AccessibleName == "Backup statistics");
            Require(statistics.Font.Bold && statistics.Parent is FlowLayoutPanel { BorderStyle: BorderStyle.FixedSingle, Margin.Top: >= 16 }, "Backup statistics must have bold text, a border and vertical separation.");
            for (var column = 0; column < 4; column++)
            {
                recovery.SortByColumn(column);
                var ascending = list.Items.Cast<ListViewItem>().Select(i => ((RecoveryEntry)i.Tag!).Id).ToArray();
                recovery.SortByColumn(column);
                var descending = list.Items.Cast<ListViewItem>().Select(i => ((RecoveryEntry)i.Tag!).Id).ToArray();
                Require(ascending.SequenceEqual(descending.Reverse()), "Each backup header must reverse its sorting direction.");
            }
            recovery.SortByColumn(2);
            Require(list.Items.Cast<ListViewItem>().Select(i => ((RecoveryEntry)i.Tag!).Bytes).SequenceEqual(list.Items.Cast<ListViewItem>().Select(i => ((RecoveryEntry)i.Tag!).Bytes).Order()), "Backup sizes must sort numerically.");
            list.Items[0].Selected = list.Items[1].Selected = true; Application.DoEvents();
            Require(!Descendants(recovery).OfType<Button>().Single(b => b.Text == UiText.Get("action.restore")).Enabled, "Restoring must require exactly one selected file.");
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
                Require(Descendants(advanced).OfType<InformationButton>().Count() == 3, "Advanced help covers the two remaining groups plus header information.");
                Require(Descendants(advanced).OfType<Button>().Count(b => b.Text == UiText.Get("setup.chooseClient")) == 1, "Advanced options must provide the only JSON selector.");
                Require(!Descendants(advanced).OfType<ComboBox>().Any()
                    && !Descendants(advanced).OfType<Label>().Any(l => l.Text == UiText.Get("setup.formatsHeading") || l.Text == UiText.Get("setup.extendedFormats")),
                    "Advanced options must omit the read-only formats section and its unavailable selectors.");
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
        Console.Error.WriteLine("VERIFY: account email and permanent disconnect action");
        var longEmail = new string('a', 64) + "@" + new string('b', 120) + ".example";
        using (var accountSetup = new SetupForm(preview: true, previewEmail: longEmail))
        {
            accountSetup.Show(); Application.DoEvents();
            foreach (var language in LanguageSettings.Available)
            {
                UiText.Select(language.Code); Application.DoEvents();
                var accountLabel = Descendants(accountSetup).OfType<Label>().Single(l => l.Text.Contains(longEmail, StringComparison.Ordinal));
                Require(TextRenderer.MeasureText(accountLabel.Text, accountLabel.Font, Size.Empty, TextFormatFlags.SingleLine | TextFormatFlags.NoPadding).Width <= accountLabel.Width - 12,
                    "The complete account email must fit without wrapping or clipping.");
                var disconnect = Descendants(accountSetup).OfType<Button>().Single(b => b.Text == UiText.Get("setup.cancelConnection"));
                Require(disconnect.Visible && disconnect.Parent is TableLayoutPanel { ColumnCount: 3 }, "Disconnect must remain in the three account actions.");
            }
            UiText.Select("pt"); Application.DoEvents();
            Descendants(accountSetup).OfType<Button>().Single(b => b.Text == UiText.Get("setup.cancelConnection")).PerformClick(); Application.DoEvents();
            Require(!Descendants(accountSetup).OfType<Label>().Any(l => l.Text.Contains(longEmail, StringComparison.Ordinal)), "Disconnect must clear the displayed account email.");
            accountSetup.Close();
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
        var retryInput = Path.Combine(Path.GetTempPath(), "zsw-retry-" + Guid.NewGuid().ToString("N") + ".csv");
        File.WriteAllText(retryInput, "invalid");
        try
        {
            var count = 0; var blocked = false; var changed = false; var watch = System.Diagnostics.Stopwatch.StartNew();
            using var permanent = new ProcessingForm(new(LauncherAction.Open, retryInput), execute: (_, _) =>
                ++count == 1 ? Task.FromException(new InvalidDataException("Irregular delimited table.")) : Task.CompletedTask, recordDiagnostics: false);
            using var gateTimer = new System.Windows.Forms.Timer { Interval = 100 };
            gateTimer.Tick += (_, _) =>
            {
                if (permanent.IsBusy) return;
                if (watch.Elapsed > TimeSpan.FromSeconds(8)) { gateTimer.Stop(); permanent.Close(); return; }
                var retry = Descendants(permanent).OfType<Button>().SingleOrDefault(b => b.Text == UiText.Get("action.retry"));
                if (retry is null) return;
                if (!changed) { blocked = !retry.Enabled && permanent.StatusText.Contains(UiText.Get("retry.editInput"), StringComparison.Ordinal); File.AppendAllText(retryInput, " corrected"); changed = true; }
                else if (retry.Enabled) { gateTimer.Stop(); retry.PerformClick(); }
            };
            gateTimer.Start(); permanent.ShowDialog();
            Require(blocked && count == 2 && permanent.ExitCode == 0, "Permanent input failures must wait for an edit and then allow revalidation.");
        }
        finally { File.Delete(retryInput); }
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
        IProgress<string>? oldProgress = null;
        using (var failure = new ProcessingForm(new(LauncherAction.Copy, "preview.csv"), execute: (progress, _) =>
        { oldProgress = progress; return Task.FromException(new ConversionMismatchException()); }, recordDiagnostics: false))
        using (var timer = new System.Windows.Forms.Timer { Interval = 25 })
        {
            var failureVisible = false;
            timer.Tick += (_, _) =>
            {
                if (failure.IsBusy || failure.ExitCode != 1) return;
                timer.Stop();
                var before = failure.StatusText;
                var failureLabel = Descendants(failure).OfType<Label>().Single(l => l.AccessibleName == UiText.Get("accessibility.processingStatus"));
                var beforeInk = HasTextInk(failureLabel);
                SaveFrame(failure, "failure-before");
                oldProgress!.Report("STALE PROGRESS MUST NOT HIDE THE ERROR");
                Application.DoEvents();
                failure.WindowState = FormWindowState.Minimized; Application.DoEvents();
                failure.WindowState = FormWindowState.Normal; failure.Invalidate(true); Application.DoEvents();
                var afterInk = HasTextInk(failureLabel);
                SaveFrame(failure, "failure-after");
                Console.Error.WriteLine($"FAILURE UI: beforeInk={beforeInk}; afterInk={afterInk}; retained={failure.StatusText == before}; message={before == LauncherErrors.Message(new ConversionMismatchException())}; visible={failure.Visible}; label={failureLabel.Bounds}; foreground={failureLabel.ForeColor}; background={failureLabel.BackColor}; export={Descendants(failure).OfType<Button>().Any(b => b.Text == UiText.Get("action.exportDiagnostics"))}; recovery={Descendants(failure).OfType<Button>().Any(b => b.Text == UiText.Get("action.recovery"))}");
                failureVisible = beforeInk && afterInk && failure.StatusText == before && before == LauncherErrors.Message(new ConversionMismatchException()) && failure.Visible && Descendants(failure).OfType<Button>().Any(b => b.Text == "Exportar diagnóstico…")
                    && Descendants(failure).OfType<Button>().Any(b => b.Text == "Recuperação / backups");
                var previousTheme = Branding.Current;
                UiText.Select("en"); Application.DoEvents();
                var switched = failure.StatusText == LauncherErrors.Message(new ConversionMismatchException()) && HasTextInk(failureLabel);
                Branding.PreviewTheme(ApplicationTheme.Dark); Application.DoEvents(); switched &= HasTextInk(failureLabel);
                UiText.Select("pt"); Branding.PreviewTheme(previousTheme); Application.DoEvents();
                failureVisible &= switched && failure.StatusText == before;
                failure.SetDiagnosticFeedback("diagnostics.exported");
                Console.Error.WriteLine($"FAILURE UI feedback: retained={failure.StatusText.StartsWith(before, StringComparison.Ordinal)}; feedback={failure.StatusText.Contains(UiText.Get("diagnostics.exported"), StringComparison.Ordinal)}");
                failureVisible &= failure.StatusText.StartsWith(before, StringComparison.Ordinal) && failure.StatusText.Contains(UiText.Get("diagnostics.exported"), StringComparison.Ordinal);
                timer.Stop(); failure.Close();
            };
            timer.Start(); failure.ShowDialog(); measurements["OfflineFailureUi"] = failure.LastMetrics!; Require(failureVisible, "Failure must stay visible with diagnosis and recovery actions.");
        }
        foreach (var ambiguous in new[] { false, true })
        {
            var error = ambiguous ? new InvalidDataException("Ambiguous CSV delimiter; configure it explicitly.") : new InvalidDataException("Irregular delimited table.");
            using var errorForm = new ProcessingForm(new(LauncherAction.Open, "preview.csv"), execute: (_, _) => Task.FromException(error), recordDiagnostics: false);
            using var watcher = new System.Windows.Forms.Timer { Interval = 25 };
            var verified = false;
            watcher.Tick += (_, _) =>
            {
                if (errorForm.IsBusy || errorForm.ExitCode != 1) return;
                watcher.Stop();
                var button = Descendants(errorForm).OfType<Button>().Single(b => b.Text == UiText.Get(ambiguous ? "action.chooseSeparator" : "action.retry"));
                verified = ambiguous ? button.Tag as string == "primary" && button.Font.Bold && button.ForeColor == Color.White && button.BackColor == Color.FromArgb(18, 126, 69)
                    : button.Tag as string != "primary" && !Descendants(errorForm).OfType<Button>().Any(b => b.Text == UiText.Get("action.chooseSeparator"));
                SaveFrame(errorForm, ambiguous ? "choose-separator-action" : "ordinary-retry-action");
                errorForm.Close();
            };
            watcher.Start(); errorForm.ShowDialog(); Require(verified, "Only an ambiguous delimiter error gets the highlighted Choose separator action.");
        }
        using (var selector = new TextInterpretationForm())
        { Require(((Button)selector.AcceptButton!).Text == UiText.Get("action.retry"), "The interpretation modal keeps its Retry action."); }
        void SaveFrame(Form form, string name)
        {
            using var bitmap = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
            var directory = Path.GetDirectoryName(Path.GetFullPath(output))!; Directory.CreateDirectory(directory);
            bitmap.Save(Path.Combine(directory, name + ".png"));
        }
        // Synthetic CSV matching the user case by shape, without publishing their contents.
        var csv = new System.Text.StringBuilder();
        for (var row = 0; row < 5000; row++) { for (var column = 0; column < 40; column++) { if (column > 0) csv.Append(';'); csv.Append("cell_").Append(row).Append('_').Append(column); } csv.Append('\n'); }
        var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        var benchmark = new ProcessingTelemetry(); SpreadsheetPayload payload;
        using (benchmark.Begin(ProcessingPhase.Conversion)) payload = SpreadsheetFormats.Prepare("csv", bytes, new("auto", "semicolon"));
        using (benchmark.Begin(ProcessingPhase.Verification)) SpreadsheetFormats.VerifyValues(payload.Expected!, payload.Bytes);
        measurements["OfflineCsv5000x40RoundTrip"] = benchmark.Capture();
        var largeCsv = new System.Text.StringBuilder();
        for (var row = 0; row < 100000; row++) { for (var column = 0; column < 10; column++) { if (column > 0) largeCsv.Append('|'); largeCsv.Append(row).Append('_').Append(column); } largeCsv.Append('\n'); }
        var expanded = new ProcessingTelemetry();
        using (expanded.Begin(ProcessingPhase.Conversion)) payload = SpreadsheetFormats.Prepare("csv", System.Text.Encoding.UTF8.GetBytes(largeCsv.ToString()));
        using (expanded.Begin(ProcessingPhase.Verification)) SpreadsheetFormats.VerifyValues(payload.Expected!, payload.Bytes, textImport: true);
        measurements["OfflineCsv100000x10RoundTrip"] = expanded.Capture();
        Require(expanded.Capture().PeakWorkingSetBytes < 512L * 1024 * 1024, "CSV capacity benchmark exceeded the 512 MiB working-set budget.");
        using (var interpretation = new TextInterpretationForm(repair: true))
        { interpretation.Show(); Application.DoEvents(); Require(interpretation.Options == new TextImportOptions(), "Repair defaults must use the current automatic interpretation."); interpretation.Close(); }
        var previewInput = Path.Combine(Path.GetTempPath(), "zsw-preview-" + Guid.NewGuid().ToString("N") + ".csv");
        try
        {
            File.WriteAllText(previewInput, "a,b;c\n1,2;3\n4,5;6");
            foreach (var code in new[] { "en", "pt", "ar" })
            {
                UiText.Select(code);
                using var choice = new TextInterpretationForm(sourcePath: previewInput);
                choice.Show(); Application.DoEvents();
                var picker = Descendants(choice).OfType<ComboBox>().Single(c => c.AccessibleName == UiText.Get("setup.delimiterAuto"));
                picker.SelectedIndex = 2;
                var grid = Descendants(choice).OfType<DataGridView>().Single();
                var wait = System.Diagnostics.Stopwatch.StartNew();
                while (grid.Rows.Count == 0 && wait.Elapsed < TimeSpan.FromSeconds(10)) { Application.DoEvents(); Thread.Sleep(10); }
                Require(grid.Rows.Count == 3 && grid.Columns.Count == 2, "Per-file delimiter preview must render the selected interpretation.");
                Require((string?)grid.Rows[0].Cells[0].Value == "a,b", "Preview must preserve the competing separator inside the value.");
                Require(((Button)choice.AcceptButton!).Enabled, "A valid explicit preview must permit retry.");
                choice.Close();
            }
        }
        finally { if (File.Exists(previewInput)) File.Delete(previewInput); UiText.Select("pt"); }
        Console.Error.WriteLine("VERIFY: 51 locale layouts");
        LocaleVerification.Run(output);
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(output))!);
        File.WriteAllText(output, System.Text.Json.JsonSerializer.Serialize(new { Measurements = measurements, InputBytes = bytes.Length, Rows = 5000, Columns = 40, GoogleNetwork = false }));
        return 0;
    }
    private static bool HasTextInk(Label label)
    {
        using var bitmap = new Bitmap(label.Width, label.Height);
        label.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
        var background = label.BackColor; var ink = 0;
        for (var y = label.Padding.Top; y < bitmap.Height - label.Padding.Bottom; y += 2)
        for (var x = label.Padding.Left; x < bitmap.Width - label.Padding.Right; x += 2)
        {
            var pixel = bitmap.GetPixel(x, y);
            if (Math.Abs(pixel.R - background.R) + Math.Abs(pixel.G - background.G) + Math.Abs(pixel.B - background.B) > 80) ink++;
        }
        return ink > 20;
    }
    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach (Control child in parent.Controls) { yield return child; foreach (var descendant in Descendants(child)) yield return descendant; }
    }
}



