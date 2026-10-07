using SheetsWindows.Infrastructure;
using System.Drawing.Text;

namespace SheetsWindows.Windows;

internal static class LanguageVisuals
{
    private static readonly HashSet<string> Installed = ReadFonts();
    private static HashSet<string> ReadFonts()
    {
        using var fonts = new InstalledFontCollection();
        return fonts.Families.Select(f => f.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
    internal static string Family => Installed.Contains(LanguageSettings.Details(UiText.Language).FontFamily)
        ? LanguageSettings.Details(UiText.Language).FontFamily : "Segoe UI";
    internal static void Attach(Form form)
    {
        var fonts = new Dictionary<(string Family, float Size, FontStyle Style), Font>();
        void Refresh()
        {
            if (form.IsDisposed) return;
            var family = Family;
            var rtl = UiText.RightToLeft;
            form.SuspendLayout();
            try
            {
                void Apply(Control control)
                {
                    var key = (family, control.Font.Size, control.Font.Style);
                    if (!fonts.TryGetValue(key, out var font)) fonts[key] = font = new Font(key.Item1, key.Item2, key.Item3);
                    control.Font = font;
                    control.RightToLeft = rtl ? RightToLeft.Yes : RightToLeft.No;
                    // Local paths, filenames and legal originals retain their own reading order.
                    if (control is TextBoxBase && control is not RichTextBox) control.RightToLeft = RightToLeft.No;
                    if (control is ListView list) list.RightToLeftLayout = rtl;
                    foreach (Control child in control.Controls) Apply(child);
                }
                Apply(form);
                form.RightToLeftLayout = rtl;
            }
            finally { form.ResumeLayout(true); }
        }
        UiText.Changed += Refresh;
        form.Disposed += (_, _) => { UiText.Changed -= Refresh; foreach (var font in fonts.Values) font.Dispose(); };
        Refresh();
    }
}
