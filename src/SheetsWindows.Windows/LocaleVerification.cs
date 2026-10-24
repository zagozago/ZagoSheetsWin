using SheetsWindows.Infrastructure;
using System.Text.Json;

namespace SheetsWindows.Windows;

// Exercises real controls on Windows; no Google calls or changes to user settings.
internal static class LocaleVerification
{
    internal static void Run(string output)
    {
        var results = new List<object>();
        var folder = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(output))!, "locales");
        Directory.CreateDirectory(folder);
        try
        {
            foreach (var language in LanguageSettings.Available)
            {
                UiText.Select(language.Code);
                var buttons = 0;
                void Check(Form form, string name, bool picture = false)
                {
                    Console.Error.WriteLine($"VERIFY: {language.Code}/{name}");
                    form.Show(); Application.DoEvents(); form.PerformLayout(); Application.DoEvents();
                    if (form.RightToLeftLayout != UiText.RightToLeft) throw new InvalidOperationException($"{language.Code}/{name}: RTL not applied.");
                    foreach (var button in Descendants(form).OfType<Button>().Where(b => b.Visible && b is not ThemeToggle))
                    {
                        if (button is AdaptiveButton adaptive && Math.Abs(button.Font.SizeInPoints - (adaptive.FontSizeOverride ?? Ui.ButtonFontSize)) > 0.01f)
                            throw new InvalidOperationException($"{language.Code}/{name}: action button must use the standard 9-point font.");
                        var flags = TextFormatFlags.WordBreak;
                        var text = TextRenderer.MeasureText(button.Text, button.Font, new Size(Math.Max(20,button.Width-button.Padding.Horizontal-8),int.MaxValue),flags);
                        if (text.Height > button.Height-button.Padding.Vertical+2)
                            throw new InvalidOperationException($"{language.Code}/{name}: button text clipped: {button.Text}; text {text}, bounds {button.Size}.");
                        var scroll = button.Parent is ScrollableControl { AutoScroll: true };
                        if (!scroll && (button.Bottom > button.Parent!.ClientSize.Height+2 || button.Right > button.Parent.ClientSize.Width+2 || button.Left < -2))
                            throw new InvalidOperationException($"{language.Code}/{name}: button outside parent: {button.Text}; {button.Bounds}, parent {button.Parent.ClientSize}.");
                        buttons++;
                    }
                    if (UiText.RightToLeft && form is (LauncherForm or SetupForm))
                    {
                        foreach (var host in Descendants(form).OfType<InsetFlowLayoutPanel>())
                        foreach (Control child in host.Controls)
                        {
                            if (!child.Visible || child is not (Button or FlowLayoutPanel or TableLayoutPanel)) continue;
                            if (child.Left < host.Padding.Left - 1 || child.Right > host.ClientSize.Width - host.Padding.Right + 1)
                                throw new InvalidOperationException($"{language.Code}/{name}: RTL content must retain both horizontal insets: {child.Text}; {child.Bounds}, padding {host.Padding}.");
                        }
                    }
                    if (form is TutorialForm)
                    {
                        var content = Descendants(form).OfType<FlowLayoutPanel>().Single(p => p.FlowDirection == FlowDirection.TopDown);
                        if (content.VerticalScroll.Visible || content.Controls.Cast<Control>().Any(c => c.Bottom > content.ClientSize.Height+2))
                            throw new InvalidOperationException($"{language.Code}/{name}: tutorial content clipped or scrolled.");
                    }
                    if (picture)
                    {
                        using var bitmap = new Bitmap(form.Width, form.Height);
                        form.DrawToBitmap(bitmap,new Rectangle(Point.Empty,bitmap.Size));
                        bitmap.Save(Path.Combine(folder,$"{language.Code}-{name}.png"));
                    }
                    form.Hide();
                }
                void CheckBoth(Form form, string name)
                {
                    Branding.PreviewTheme(ApplicationTheme.Light); Check(form,name,true);
                    Branding.PreviewTheme(ApplicationTheme.Dark); Check(form,name+"-dark",true);
                    Branding.PreviewTheme(ApplicationTheme.Light);
                }
                using (var home = new LauncherForm(new(LauncherAction.Home))) CheckBoth(home,"home");
                using (var about = new AboutForm()) CheckBoth(about,"about");
                using (var advanced = new AdvancedPreferencesForm(LocalStorage.ForCurrentUser(), preview:true)) { CheckBoth(advanced,"advanced"); foreach (var info in Descendants(advanced).OfType<InformationButton>()) { info.OpenInformation(); Application.DoEvents(); } }
                using (var settings = new SetupForm(preview:true)) { CheckBoth(settings,"setup"); foreach (var info in Descendants(settings).OfType<InformationButton>()) { info.OpenInformation(); Application.DoEvents(); } }
                using (var recovery = new RecoveryForm(preview:true))
                {
                    Check(recovery,"backups",true);
                    var panels = Descendants(recovery).OfType<DisclosureCard>().ToArray();
                    if (panels.Length != 3 || panels.Any(p => p.Expanded)) throw new InvalidOperationException("Backup panels must start collapsed in every locale.");
                    foreach(var panel in panels) panel.SetExpanded(true);
                    Check(recovery,"backups-expanded",true);
                    var rows = Descendants(recovery).OfType<BackupSelectionList>().Single();
                    rows.Items[0].Selected = true; Application.DoEvents();
                    if(!rows.Items[0].Checked) throw new InvalidOperationException("Selection and checkbox state diverged.");
                    foreach(var theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark })
                    {
                        Branding.PreviewTheme(theme);
                        foreach(var scale in new[] { 1f, 1.25f, 1.5f, 2f })
                        {
                            var before = recovery.Size; recovery.Scale(new SizeF(scale,scale)); Check(recovery,$"backups-expanded-{theme}-{scale}");
                            recovery.Scale(new SizeF(1/scale,1/scale)); recovery.Size = before;
                        }
                    }
                    Branding.PreviewTheme(ApplicationTheme.Light);
                }
                using (var repair = new TextInterpretationForm(repair:true)) Check(repair,"repair",true);
                using (var picker = new LanguagePicker())
                {
                    Check(picker,"language");
                    var list = Descendants(picker).OfType<ListBox>().Single();
                    if (list.Items.Count != 52) throw new InvalidOperationException("Language picker must contain automatic and all 51 languages.");
                }
                for (var page=0;page<4;page++)
                {
                    using var tutorial = new TutorialForm(page); Check(tutorial,$"help-{page+1}",true);
                    if (page < FirstUseWizard.PageCount)
                    { using var wizard = new FirstUseWizard(preview:true, initialPage:page); CheckBoth(wizard,$"first-use-{page+1}"); }
                }
                using (var error = new ProcessingForm(new(LauncherAction.Open,"preview.xlsx"),preview:true,previewError:true))
                {
                    var status = Descendants(error).OfType<Label>().Single(l => l.AccessibleName == UiText.Get("accessibility.processingStatus"));
                    status.Text = UiText.Get("error.generic");
                    Check(error,"error",true);
                    var text = TextRenderer.MeasureText(status.Text,status.Font,new Size(Math.Max(100,status.ClientSize.Width-status.Padding.Horizontal),int.MaxValue),TextFormatFlags.WordBreak);
                    if(text.Height+status.Padding.Vertical>status.ClientSize.Height+2)throw new InvalidOperationException($"{language.Code}: generic failure message is clipped.");
                }
                using (var editError = new ProcessingForm(new(LauncherAction.Open,"preview.csv"), preview:true, previewFailure:new InvalidDataException("Irregular delimited table.")))
                {
                    CheckBoth(editError,"error-edit-input");
                    editError.SetDiagnosticFeedback("error.generic"); CheckBoth(editError,"error-edit-input-long");
                    var message = Descendants(editError).OfType<Label>().Single(l => l.AccessibleName == UiText.Get("accessibility.processingStatus"));
                    if (!message.Text.Contains(UiText.Get("retry.editInput"),StringComparison.Ordinal) || message.Text.Contains('\n') || message.Bottom > message.Parent!.ClientSize.Height)
                        throw new InvalidOperationException($"{language.Code}: full continuous edit-input guidance must remain visible.");
                }
                using (var partial = new ProcessingForm(new(LauncherAction.Open,"preview.ods"),preview:true))
                {
                    partial.ShowImportNotice(UiText.Format("import.notice.partialConversion", ("backups", UiText.Get("home.backups").Replace("&", "", StringComparison.Ordinal))));
                    Check(partial,"partial-import",true);
                    var status = Descendants(partial).OfType<Label>().Single(l => l.AccessibleName == UiText.Get("accessibility.processingStatus"));
                    var text = TextRenderer.MeasureText(status.Text,status.Font,new Size(Math.Max(100,status.ClientSize.Width-status.Padding.Horizontal),int.MaxValue),TextFormatFlags.WordBreak);
                    if(text.Height+status.Padding.Vertical>status.ClientSize.Height+2)throw new InvalidOperationException($"{language.Code}: partial import notice is clipped.");
                }
                // Font-size stress at the four requested scales, with native GDI measurement.
                foreach (var scale in new[] {1f,1.25f,1.5f,2f})
                foreach (var key in new[] {"tutorial.skip","tutorial.start","tutorial.defineDefaults","action.next","language.apply","language.button"})
                {
                    using var panel = new FlowLayoutPanel { Width = (int)(360*scale) };
                    using var button = new AdaptiveButton { Text=UiText.Get(key), AutoSize=true, Font=new Font(LanguageVisuals.Family,Ui.ButtonFontSize*scale) };
                    panel.Controls.Add(button);panel.PerformLayout();
                    var preferred=button.GetPreferredSize(new Size(panel.Width,0));
                    var needed=TextRenderer.MeasureText(button.Text,button.Font,new Size(Math.Max(20,preferred.Width-button.Padding.Horizontal-12),int.MaxValue),TextFormatFlags.WordBreak);
                    if(preferred.Height<needed.Height+button.Padding.Vertical)throw new InvalidOperationException($"{language.Code}/{key}: scaled button clipped at {scale}.");
                }
                results.Add(new { language.Code, language.Locale, language.Direction, Font=LanguageVisuals.Family, Buttons=buttons, Scales=new[]{100,125,150,200} });
            }
            File.WriteAllText(Path.Combine(folder,"verification.json"),JsonSerializer.Serialize(results,new JsonSerializerOptions{WriteIndented=true}));
        }
        finally { UiText.Select("pt"); }
    }
    private static IEnumerable<Control> Descendants(Control parent)
    {
        foreach(Control child in parent.Controls) { yield return child;foreach(var nested in Descendants(child))yield return nested; }
    }
}

