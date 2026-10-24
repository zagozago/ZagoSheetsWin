using SheetsWindows.Infrastructure;

namespace SheetsWindows.Windows;

internal static class Program
{
    internal static Form? StartupConfiguration(LauncherAction action, bool needsSetup, bool needsAuthorization, bool preview = false)
    {
        if (action != LauncherAction.FirstUse && !needsSetup && !needsAuthorization) return null;
        return action is LauncherAction.Open or LauncherAction.Copy
            ? new SetupForm(preview: preview)
            : new FirstUseWizard(preview: preview);
    }
    [STAThread]
    private static int Main(string[] args)
    {
        var startedAt = System.Diagnostics.Stopwatch.GetTimestamp();
        try
        {
            if (args.Length == 2 && args[0] == "--preview-branding")
            {
                ApplicationConfiguration.Initialize();
                var folder = Path.GetFullPath(args[1]); Directory.CreateDirectory(folder);
                Branding.PreviewTheme(ApplicationTheme.Light);
                using var home = new LauncherForm(new LauncherRequest(LauncherAction.Home));
                using var processingPreview = new ProcessingForm(new(LauncherAction.Open, "preview.xlsx"), preview: true); using var failurePreview = new ProcessingForm(new(LauncherAction.Open, "preview.xlsx"), preview: true, previewError: true);
                using var setupPreview = new SetupForm(preview: true); using var firstUsePreview = new FirstUseWizard(preview: true); using var recoveryPreview = new RecoveryForm(preview: true); using var recoveryBusyPreview = new RecoveryForm(preview: true, previewBusy: true); using var aboutPreview = new AboutForm();
                foreach (var entry in new[] { ("home", (Form)home), ("setup", (Form)setupPreview), ("first-use", (Form)firstUsePreview), ("recovery", (Form)recoveryPreview), ("recovery-busy", (Form)recoveryBusyPreview), ("about", (Form)aboutPreview), ("processing", (Form)processingPreview), ("processing-error", (Form)failurePreview) })
                {
                    entry.Item2.Show(); Application.DoEvents(); entry.Item2.PerformLayout();
                    using var bitmap = new Bitmap(entry.Item2.Width, entry.Item2.Height);
                    entry.Item2.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                    bitmap.Save(Path.Combine(folder, entry.Item1 + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                    entry.Item2.Hide();
                }
                Branding.PreviewTheme(ApplicationTheme.Dark);
                foreach (var entry in new[] { ("home-dark", (Form)home), ("setup-dark", (Form)setupPreview), ("recovery-dark", (Form)recoveryPreview), ("recovery-busy-dark", (Form)recoveryBusyPreview), ("processing-dark", (Form)processingPreview), ("processing-error-dark", (Form)failurePreview) })
                {
                    entry.Item2.Show(); Application.DoEvents(); using var bitmap = new Bitmap(entry.Item2.Width, entry.Item2.Height);
                    entry.Item2.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size)); bitmap.Save(Path.Combine(folder, entry.Item1 + ".png"), System.Drawing.Imaging.ImageFormat.Png); entry.Item2.Hide();
                }
                static IEnumerable<Control> All(Control root) { foreach(Control child in root.Controls) { yield return child; foreach(var nested in All(child)) yield return nested; } }
                foreach (var theme in new[] { ApplicationTheme.Light, ApplicationTheme.Dark })
                {
                    Branding.PreviewTheme(theme);
                    using (var expandedBackups = new RecoveryForm(preview: true))
                    {
                        expandedBackups.Show(); foreach(var panel in All(expandedBackups).OfType<DisclosureCard>()) panel.SetExpanded(true); Application.DoEvents();
                        using var bitmap = new Bitmap(expandedBackups.Width, expandedBackups.Height); expandedBackups.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                        bitmap.Save(Path.Combine(folder, $"recovery-expanded-{theme}.png"), System.Drawing.Imaging.ImageFormat.Png); expandedBackups.Hide();
                    }
                    using (var advanced = new AdvancedPreferencesForm(LocalStorage.ForCurrentUser(), preview: true))
                    {
                        advanced.Show(); Application.DoEvents();
                        using var bitmap = new Bitmap(advanced.Width, advanced.Height); advanced.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                        bitmap.Save(Path.Combine(folder, $"advanced-{theme}.png"), System.Drawing.Imaging.ImageFormat.Png); advanced.Hide();
                    }
                    for (var page = 0; page < FirstUseWizard.PageCount; page++)
                    {
                        using var wizard = new FirstUseWizard(preview: true, initialPage: page); wizard.Show(); Application.DoEvents();
                        using var bitmap = new Bitmap(wizard.Width, wizard.Height); wizard.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                        bitmap.Save(Path.Combine(folder, $"first-use-{page+1}-{theme}.png"), System.Drawing.Imaging.ImageFormat.Png); wizard.Hide();
                    }
                    for (var page = 0; page < 4; page++)
                    {
                        using var tutorial = new TutorialForm(page); tutorial.Show(); Application.DoEvents();
                        using var bitmap = new Bitmap(tutorial.Width, tutorial.Height); tutorial.DrawToBitmap(bitmap, new Rectangle(Point.Empty, bitmap.Size));
                        bitmap.Save(Path.Combine(folder, $"tutorial-{page + 1}-{theme}.png"), System.Drawing.Imaging.ImageFormat.Png); tutorial.Hide();
                    }
                }
                UiText.Select("en");
                Branding.PreviewTheme(ApplicationTheme.Light);
                using (var language = new LanguagePicker())
                {
                    foreach (var entry in new[] { ("home-en", (Form)home), ("setup-en", (Form)setupPreview), ("recovery-en", (Form)recoveryPreview), ("about-en", (Form)aboutPreview), ("language-en", (Form)language) })
                    {
                        entry.Item2.Show(); Application.DoEvents(); using var bitmap = new Bitmap(entry.Item2.Width, entry.Item2.Height);
                        entry.Item2.DrawToBitmap(bitmap,new Rectangle(Point.Empty,bitmap.Size)); bitmap.Save(Path.Combine(folder,entry.Item1+".png"),System.Drawing.Imaging.ImageFormat.Png);entry.Item2.Hide();
                    }
                    for (var page = 0; page < 4; page++)
                    {
                        using var tutorial = new TutorialForm(page); tutorial.Show(); Application.DoEvents();
                        using var bitmap = new Bitmap(tutorial.Width,tutorial.Height); tutorial.DrawToBitmap(bitmap,new Rectangle(Point.Empty,bitmap.Size));bitmap.Save(Path.Combine(folder,$"tutorial-{page+1}-en.png"),System.Drawing.Imaging.ImageFormat.Png);tutorial.Hide();
                    }
                }
                UiText.Select("pt");
                return 0;
            }
            if (args.Length == 2 && args[0] == "--verify-interface") { ApplicationConfiguration.Initialize(); Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException); return InterfaceVerification.Run(args[1]); }
            // Maintenance saves only an absent preference; no OAuth or UI initialization.
            if (args.Length == 2 && args[0] == "--installer-language")
            { LanguageSettings.InitializeFromInstallerAsync(LocalStorage.ForCurrentUser(), args[1]).GetAwaiter().GetResult(); return 0; }
            if (args.Length == 2 && args[0] == "--installer-language-selected")
            { LanguageSettings.SelectFromInstallerAsync(LocalStorage.ForCurrentUser(), args[1]).GetAwaiter().GetResult(); return 0; }
            ApplicationLanguages.Initialize();
            var request = LauncherRequest.Parse(args);
            if (request.Action == LauncherAction.Version) { Console.WriteLine("ZagoSheetsWin " + UiText.ProductVersion); return 0; }
            if (request.Action is LauncherAction.Register or LauncherAction.Unregister)
            {
                var held = new FileOperationLock(LocalStorage.ForCurrentUser().LocksPath).AcquireAsync("windows-registration").AsTask().GetAwaiter().GetResult();
                try
                {
                    var registration = new WindowsAssociationRegistration(Microsoft.Win32.Registry.CurrentUser);
                    if (request.Action == LauncherAction.Register) registration.Register(Environment.ProcessPath!);
                    else registration.Unregister();
                    WindowsAssociationRegistration.NotifyShell(); return 0;
                }
                finally { held.DisposeAsync().AsTask().GetAwaiter().GetResult(); }
            }
            ApplicationConfiguration.Initialize();
            if (request.Action is LauncherAction.FirstUse or LauncherAction.Home or LauncherAction.Open or LauncherAction.Copy)
            {
                var storage = LocalStorage.ForCurrentUser();
                var needsSetup = FirstUseState.NeedsSetup(storage);
                using var initial = StartupConfiguration(request.Action, needsSetup, needsSetup || FirstUseState.NeedsAuthorization(storage));
                if (initial is SetupForm settings)
                {
                    // Opening settings does not authorize importing the pending file. Leave it untouched until reopened.
                    Application.Run(settings); return settings.ExitCode;
                }
                if (initial is FirstUseWizard wizard)
                {
                    Application.Run(wizard);
                    if (!wizard.Completed) return 1;
                    if (request.Action is LauncherAction.Home or LauncherAction.FirstUse)
                    { using var home = new LauncherForm(new(LauncherAction.Home)); Application.Run(home); return home.ExitCode; }
                }
                else if (request.Action == LauncherAction.Home && !TutorialSettings.Load(storage))
                { using var tutorial = new TutorialForm(); tutorial.ShowDialog(); }
            }
            if (request.Action == LauncherAction.Setup) { using var setup = new SetupForm(); Application.Run(setup); return setup.ExitCode; }
            if (request.Action == LauncherAction.Recovery) { using var recovery = new RecoveryForm(); Application.Run(recovery); return 0; }
            if (request.Action == LauncherAction.Defaults) { new BrowserLauncher().Open(WindowsAssociationPlan.DefaultsUri); return 0; }
            if (request.Action == LauncherAction.Home) { using var home = new LauncherForm(request); Application.Run(home); return home.ExitCode; }
            using var form = new ProcessingForm(request, startedAt); Application.Run(form); return form.ExitCode;
        }
        catch (Exception ex) when (LauncherErrors.Expected(ex))
        {
            if (args.Length == 2 && args[0] == "--verify-interface") { Console.Error.WriteLine(ex.Message); return 1; }
            if (args.Length == 1 && args[0] is "--register" or "--unregister") { Console.Error.WriteLine("Association maintenance failed; existing state preserved."); return 1; }
            MessageBox.Show(UiText.Format("error.launcherContext", ("reason", LauncherErrors.Message(ex))), "ZagoSheetsWin", MessageBoxButtons.OK, MessageBoxIcon.Warning); return 1;
        }
    }
}


