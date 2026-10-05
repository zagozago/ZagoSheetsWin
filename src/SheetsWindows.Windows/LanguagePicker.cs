using SheetsWindows.Infrastructure;
using System.Globalization;

namespace SheetsWindows.Windows;

internal sealed class LanguagePicker : Form
{
    private readonly TextBox search = new() { Dock = DockStyle.Top, PlaceholderText = UiText.Get("language.search") };
    private readonly ListBox choices = new() { Dock = DockStyle.Fill, IntegralHeight = false };
    private readonly AdaptiveButton apply = new() { Text = UiText.Get("language.apply"), Dock = DockStyle.Bottom };
    private sealed record Choice(string Code, string Caption) { public override string ToString() => Caption; }
    public LanguagePicker()
    {
        Text = UiText.Get("language.title"); ClientSize = new Size(420, 340); MinimumSize = new Size(360, 320);
        AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterParent; ShowInTaskbar = false; MinimizeBox = false; MaximizeBox = false;
        var body = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), ColumnCount = 1, RowCount = 4 };
        body.RowStyles.Add(new(SizeType.AutoSize)); body.RowStyles.Add(new(SizeType.AutoSize)); body.RowStyles.Add(new(SizeType.Percent, 100)); body.RowStyles.Add(new(SizeType.AutoSize));
        body.Controls.Add(Ui.Text(UiText.Get("language.choose"), true),0,0);
        search.AccessibleName = UiText.Get("language.search"); body.Controls.Add(search,0,1); body.Controls.Add(choices,0,2);
        var note = Ui.Text(UiText.Get("language.available"));note.Dock=DockStyle.Fill;body.Controls.Add(note,0,3);
        choices.AccessibleName=UiText.Get("language.choose");
        Controls.Add(body);Controls.Add(apply);Ui.Primary(apply);Branding.Apply(this,aboutButton:false,compact:true,languageButton:false);
        void Filter()
        {
            var selected = (choices.SelectedItem as Choice)?.Code ?? ApplicationLanguages.Preference;
            var query = search.Text.Trim(); choices.Items.Clear();
            var all = new[] { new Choice(LanguageSettings.Automatic,UiText.Get("language.automatic")) }.Concat(LanguageSettings.Available.Select(l => new Choice(l.Code,$"{l.DisplayCode} - {l.NativeName} / {l.EnglishName}")));
            foreach(var item in all.Where(l => CultureInfo.InvariantCulture.CompareInfo.IndexOf(l.Caption, query, CompareOptions.IgnoreCase|CompareOptions.IgnoreNonSpace)>=0)) choices.Items.Add(item);
            choices.SelectedIndex = choices.Items.Cast<Choice>().ToList().FindIndex(l=>l.Code==selected);
            if(choices.SelectedIndex<0 && choices.Items.Count>0) choices.SelectedIndex=0;
            apply.Enabled=choices.SelectedItem is Choice;
        }
        search.TextChanged+=(_,_)=>Filter();choices.SelectedIndexChanged+=(_,_)=>apply.Enabled=choices.SelectedItem is Choice;
        async Task ApplyChoice()
        {
            if(choices.SelectedItem is not Choice choice) return;
            apply.Enabled=false;
            try { await ApplicationLanguages.SetAsync(choice.Code); DialogResult=DialogResult.OK; Close(); }
            catch(Exception ex) when(LauncherErrors.Expected(ex)) { MessageBox.Show(this,UiText.Get("language.saveFailed"),Branding.Name,MessageBoxButtons.OK,MessageBoxIcon.Warning);apply.Enabled=true; }
        }
        apply.Click+=async(_,_)=>await ApplyChoice();choices.DoubleClick+=async(_,_)=>await ApplyChoice();AcceptButton=apply;
        KeyPreview=true;KeyDown+=(_,e)=> {if(e.KeyCode==Keys.Escape){e.Handled=true;Close();}};
        Filter();Shown+=(_,_)=>search.Focus();
    }
}
internal static class ApplicationLanguages
{
    internal static string Preference {get;private set;}=LanguageSettings.Automatic;
    internal static void Initialize()
    {
        try{Preference=LanguageSettings.Load(LocalStorage.ForCurrentUser());}catch(Exception ex) when(LauncherErrors.Expected(ex)){Preference=LanguageSettings.Automatic;}
        UiText.Select(LanguageSettings.Resolve(Preference,LanguageSettings.WindowsPreferredLanguages()));
    }
    internal static async Task SetAsync(string preference)
    {
        var language=LanguageSettings.Resolve(preference,LanguageSettings.WindowsPreferredLanguages());
        await LanguageSettings.SaveAsync(LocalStorage.ForCurrentUser(),preference);
        Preference=preference;UiText.Select(language);
    }
}
