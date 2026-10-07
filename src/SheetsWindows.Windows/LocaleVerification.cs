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
                    form.Show(); Application.DoEvents(); form.PerformLayout(); Application.DoEvents();
                    if (form.RightToLeftLayout != UiText.RightToLeft) throw new InvalidOperationException($"{language.Code}/{name}: RTL not applied.");
                    foreach (var button in Descendants(form).OfType<Button>().Where(b => b.Visible && b is not ThemeToggle))
                    {
                        var flags = TextFormatFlags.WordBreak;
                        var text = TextRenderer.MeasureText(button.Text, button.Font, new Size(Math.Max(20,button.Width-button.Padding.Horizontal-8),int.MaxValue),flags);
                        if (text.Height > button.Height-button.Padding.Vertical+2)
                            throw new InvalidOperationException($"{language.Code}/{name}: button text clipped: {button.Text}; text {text}, bounds {button.Size}.");
                        var scroll = button.Parent is ScrollableControl { AutoScroll: true };
                        if (!scroll && (button.Bottom > button.Parent!.ClientSize.Height+2 || button.Right > button.Parent.ClientSize.Width+2 || button.Left < -2))
                            throw new InvalidOperationException($"{language.Code}/{name}: button outside parent: {button.Text}; {button.Bounds}, parent {button.Parent.ClientSize}.");
                        buttons++;
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
                using (var home = new LauncherForm(new(LauncherAction.Home))) Check(home,"home",true);
                using (var settings = new SetupForm(preview:true)) { Check(settings,"setup",true); Check(settings.AdvancedDialog,"advanced"); }
                using (var recovery = new RecoveryForm(preview:true)) Check(recovery,"backups");
                using (var picker = new LanguagePicker())
                {
                    Check(picker,"language");
                    var list = Descendants(picker).OfType<ListBox>().Single();
                    if (list.Items.Count != 52) throw new InvalidOperationException("Language picker must contain automatic and all 51 languages.");
                }
                for (var page=0;page<4;page++) using (var tutorial=new TutorialForm(page)) Check(tutorial,$"help-{page+1}",true);
                using (var error = new ProcessingForm(new(LauncherAction.Open,"preview.xlsx"),preview:true,previewError:true)) Check(error,"error");
                // Font-size stress at the four requested scales, with native GDI measurement.
                foreach (var scale in new[] {1f,1.25f,1.5f,2f})
                foreach (var key in new[] {"tutorial.skip","tutorial.start","tutorial.defineDefaults","action.next","language.apply","language.button"})
                {
                    using var panel = new FlowLayoutPanel { Width = (int)(360*scale) };
                    using var button = new AdaptiveButton { Text=UiText.Get(key), AutoSize=true, Font=new Font(LanguageVisuals.Family,10*scale) };
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
