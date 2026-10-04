using SheetsWindows.Infrastructure;
using SheetsWindows.Core;

namespace SheetsWindows.Windows;

internal sealed class SetupForm : Form
{
    public int ExitCode { get; private set; }
    private readonly TextBox client = new() { Width = 570, ReadOnly = true };
    private readonly TextBox folder = new() { Width = 570, ReadOnly = true };
    private readonly Label status = new EmphasisLabel() { AutoSize = true, MaximumSize = new Size(570, 0) };
    private CancellationTokenSource cancellation = new();
    private bool busy;
    internal Form AdvancedDialog { get; private set; } = null!;
    public SetupForm(bool firstUse = false, bool preview = false)
    {
        var storage = LocalStorage.ForCurrentUser();
        Text = firstUse ? "Primeiro uso - ZagoSheetsWin" : "Configurações - ZagoSheetsWin";
        ClientSize = new Size(560, 700); MinimumSize = new Size(480, 520); AutoScaleMode = AutoScaleMode.Dpi; StartPosition = FormStartPosition.CenterScreen;
        var layout = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(18), FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true };
        Label Info(string text) => Ui.Text(text);
        layout.Controls.Add(Info("Abra suas planilhas no Google Sheets com sua conta Google. O original é guardado em backup antes da substituição por um atalho."));
        var configured = !FirstUseState.NeedsSetup(storage);
        var opening = OpeningPolicy.Load(storage);
        var noSync = new CheckBox { AutoSize = true, Checked = true, Text = "Não sincronizo minhas planilhas com Google Drive para Windows, OneDrive ou similares." };
        var folderOptions = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        folder.Text = opening.Folder ?? "";
        var chooseFolder = new Button { AutoSize = true, Text = "Escolher pasta local…" };
        chooseFolder.Click += (_, _) => { using var dialog = new FolderBrowserDialog(); if (dialog.ShowDialog(this) == DialogResult.OK) folder.Text = dialog.SelectedPath; };
        folderOptions.Controls.Add(Info("Usa sincronização? Escolha uma pasta local fora desses aplicativos. Retirar um arquivo de uma pasta sincronizada pode apagá-lo também na nuvem e em outros dispositivos."));
        var folderRow = new TableLayoutPanel { AutoSize = false, Height = 38, ColumnCount = 2, RowCount = 1, Margin = new Padding(0, 4, 0, 4) };
        folderRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); folderRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        chooseFolder.AutoSize = false; chooseFolder.Dock = DockStyle.Fill; chooseFolder.Height = 34;
        folder.Dock = DockStyle.Fill; folder.Margin = new Padding(3, 7, 3, 3);
        folderRow.Controls.Add(chooseFolder, 0, 0); folderRow.Controls.Add(folder, 1, 0);
        folderOptions.Controls.Add(folderRow);
        var consent = new CheckBox { AutoSize = true, Checked = configured, Text = "Entendi: após a importação confirmada, o original é substituído por um atalho; um backup local é guardado." };
        void ShowFolderOptions() { folderOptions.Visible = !noSync.Checked; layout.PerformLayout(); }
        noSync.CheckedChanged += (_, _) => ShowFolderOptions();
        var advanced = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(18), FlowDirection = FlowDirection.TopDown, WrapContents = false };
        var advancedDialog = new Form { Text = "Opções avançadas - ZagoSheetsWin", ClientSize = new Size(550, 640), MinimumSize = new Size(480, 480), StartPosition = FormStartPosition.CenterParent, AutoScaleMode = AutoScaleMode.Dpi, ShowInTaskbar = false, MinimizeBox = false };
        var closeAdvanced = new Button { Text = "Fechar", Dock = DockStyle.Bottom, Height = 44 };
        advancedDialog.Controls.Add(advanced); advancedDialog.Controls.Add(closeAdvanced);
        closeAdvanced.Click += (_, _) => advancedDialog.Close();
        AdvancedDialog = advancedDialog;
        advancedDialog.CancelButton = closeAdvanced;
        Disposed += (_, _) => advancedDialog.Dispose();
        var toggle = new Button { AutoSize = true, Text = "Opções avançadas…" };
        toggle.Click += (_, _) => advancedDialog.ShowDialog(this);

        advanced.Controls.Add(Info("Conexão Google\nO aplicativo já inclui a configuração de conexão. Cada pessoa entra com sua própria conta. Um JSON próprio é opcional. A configuração existente é mantida."));
        var chooseClient = new Button { AutoSize = true, Text = "Escolher arquivo JSON de conexão…", Enabled = !File.Exists(LauncherConfiguration.ClientPath(storage)) };
        chooseClient.Click += (_, _) => { using var dialog = new OpenFileDialog { Filter = "JSON OAuth|*.json", CheckFileExists = true }; if (dialog.ShowDialog(this) == DialogResult.OK)  { client.Text = dialog.FileName;  status.Text = "JSON selecionado. Clique em Autorizar Google."; } };
        advanced.Controls.Add(Info("Privacidade da conexão Google\nO ZagoSheetsWin usa somente a permissão drive.file para criar e gerenciar os arquivos usados com o aplicativo. As credenciais de acesso ficam neste computador, protegidos pelo Windows, e suas planilhas são enviadas diretamente às APIs do Google - não passam por servidor do Zagotools."));
        advanced.Controls.Add(chooseClient); advanced.Controls.Add(client);
        if (File.Exists(LauncherConfiguration.ClientPath(storage))) client.Text = "Configuração de conexão existente mantida.";
        var extended = new CheckBox { AutoSize = true, Checked = true, Text = "Habilitar CSV, TSV, XLS e ODS (experimental)." };
        var encoding = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 570 };
        encoding.Items.AddRange(["Texto: UTF-8 / UTF-16 com BOM", "Texto: Windows-1252"]); encoding.SelectedIndex = 0;
        var delimiter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 570 };
        delimiter.Items.AddRange(["CSV: detectar separador", "CSV: vírgula", "CSV: ponto e vírgula"]); delimiter.SelectedIndex = 0;
        advanced.Controls.Add(Ui.Separator()); advanced.Controls.Add(Ui.Text("Formatos e texto", true)); advanced.Controls.Add(extended); advanced.Controls.Add(encoding); advanced.Controls.Add(delimiter);
        advanced.Controls.Add(Info("Até 20 MiB por arquivo.\nCSV/TSV: 500 mil células, 50 mil linhas e mil colunas.\n\nO conteúdo é tratado como texto literal. Se a conversão de ODS não puder ser conferida, o original é mantido. Suas preferências de texto são preservadas."));
        try { var previous = ExtendedConfiguration.Load(storage); extended.Enabled = false; encoding.Enabled = false; delimiter.Enabled = false; encoding.SelectedIndex = previous.Encoding == "auto" ? 0 : 1; delimiter.SelectedIndex = previous.Delimiter switch { "comma" => 1, "semicolon" => 2, _ => 0 }; }
        catch (LauncherNotConfiguredException) { }
        var xls = new CheckBox { AutoSize = true, Checked = XlsReplacementSettings.Load(storage), Text = "Substituir XLS por atalho após as verificações." };
        advanced.Controls.Add(Ui.Separator()); advanced.Controls.Add(Ui.Text("Arquivos XLS", true)); advanced.Controls.Add(xls);
        advanced.Controls.Add(Info("Macros não funcionam no Sheets. Fórmulas, vínculos e formatação podem mudar. O backup conserva o original completo. Desmarque para importar XLS como cópia."));
        var saveXls = new Button { AutoSize = true, Text = "Salvar escolha para arquivos XLS" };
        saveXls.Click += async (_, _) => { saveXls.Enabled = false; try { await XlsReplacementSettings.SaveAsync(storage, xls.Checked); status.Text = "Preferência XLS salva."; } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = LauncherErrors.Message(ex); } finally { saveXls.Enabled = true; } };
        advanced.Controls.Add(saveXls);
        var advancedStatus = Ui.Text(""); advanced.Controls.Add(advancedStatus);
        status.TextChanged += (_, _) => advancedStatus.Text = status.Text;
        layout.Controls.Add(Ui.Separator()); layout.Controls.Add(Ui.Text("1. Conta Google", true));
        layout.Controls.Add(Info("Entre com sua própria conta. As planilhas ficam no seu Google Drive."));
        layout.Controls.Add(Info("Privacidade: usamos somente drive.file para os arquivos usados com o aplicativo. As credenciais de acesso ficam protegidos neste computador; as planilhas vão diretamente ao Google, sem passar por servidor do Zagotools."));
        var connectionState = Ui.Text(FirstUseState.NeedsAuthorization(storage) ? "○ Google ainda não autorizado" : "◉ Autorização salva · verificando conexão…", true);
        connectionState.AccessibleName = "Estado da autorização Google";
        layout.Controls.Add(connectionState);
        var legalLinks = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, FlowDirection = FlowDirection.LeftToRight, WrapContents = true };
        var privacyLink = new LinkLabel { Text = "Política de Privacidade", AutoSize = true, Margin = new Padding(0, 4, 18, 4) };
        privacyLink.LinkClicked += (_, _) => { try { new BrowserLauncher().Open(new Uri("https://zagotools.top/legal.html#privacidade")); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { MessageBox.Show("https://zagotools.top/legal.html#privacidade", "Política de Privacidade"); } };
        var termsLink = new LinkLabel { Text = "Termos de Uso", AutoSize = true, Margin = new Padding(0, 4, 0, 4) };
        termsLink.LinkClicked += (_, _) => { try { new BrowserLauncher().Open(new Uri("https://zagotools.top/legal.html#termos")); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { MessageBox.Show("https://zagotools.top/legal.html#termos", "Termos de Uso"); } };
        legalLinks.Controls.Add(privacyLink); legalLinks.Controls.Add(termsLink); layout.Controls.Add(legalLinks);
        var connect = new Button { AutoSize = true, Text = FirstUseState.NeedsAuthorization(storage) ? "Autorizar Google…" : "Trocar conta Google…" };
        var verify = new Button { Text = "Verificar conexão Google", AutoSize = true, Enabled = !FirstUseState.NeedsAuthorization(storage) };
        var save = new Button { AutoSize = true, Text = "Salvar configurações" };
        var defaults = new Button { AutoSize = true, Text = "Abrir Aplicativos padrão do Windows" };
        var finish = new Button { AutoSize = true, Text = firstUse ? "Salvar e concluir" : "Fechar" };
        var cancel = new Button { AutoSize = true, Text = "Cancelar conexão", Visible = false };
        cancel.Click += (_, _) => cancellation.Cancel();
        var authorizationValid = !FirstUseState.NeedsAuthorization(storage);
        bool Ready() => authorizationValid && !FirstUseState.NeedsAuthorization(storage);
        if (firstUse) finish.Enabled = Ready();
        async Task Save(bool authorize)
        {
            if (busy) return;
            busy = true; layout.Enabled = false; cancel.Visible = authorize; cancel.Enabled = true; status.Text = authorize ? "Salvando e aguardando autorização no navegador…" : "Salvando…";
            try
            {
                await using (var held = await new FileOperationLock(storage.LocksPath).AcquireAsync("windows-registration", cancellation.Token))
                {
                    var json = await LauncherConfiguration.SetupClientJsonAsync(storage, client.Text, cancellation.Token);
                    if (!authorize)
                    {
                        await OpeningPolicy.SaveAsync(storage, !noSync.Checked, folder.Text, consent.Checked, cancellation.Token);
                        ExtendedConfiguration.Save(storage, new(encoding.SelectedIndex == 0 ? "auto" : "windows-1252", delimiter.SelectedIndex switch { 1 => "comma", 2 => "semicolon", _ => "auto" }), extended.Checked);
                        await XlsReplacementSettings.SaveAsync(storage, xls.Checked, cancellation.Token);
                    }
                    LauncherConfiguration.SaveClient(storage, json);
                    new WindowsAssociationRegistration(Microsoft.Win32.Registry.CurrentUser).Register(Environment.ProcessPath!); WindowsAssociationRegistration.NotifyShell();
                }
                if (authorize)
                {
                    using var handler = new HttpClientHandler { AllowAutoRedirect = false }; using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(90) };
                    await Task.Run(() => new WindowsLauncher(storage, http, new BrowserLauncher()).LoginAsync(cancellation.Token));
                }
                if (authorize) { authorizationValid = true; connectionState.Text = "✓ Google conectado · autorização confirmada"; connect.Text = "Trocar conta Google…"; }
                ExitCode = 0; status.Text = authorize ? "Google conectado. Revise as opções abaixo e salve as configurações." : "Configurações salvas.";
                if (firstUse) finish.Enabled = Ready();
            }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { ExitCode = 1; if (authorize) { authorizationValid = false; connectionState.Text = "○ Conexão não concluída · tente autorizar novamente"; connect.Text = "Autorizar Google…"; if (firstUse) finish.Enabled = false; }
                status.Text = ex is ArgumentException ? "Confirme a substituição com backup e, se usar sincronização, escolha uma pasta local." : LauncherErrors.Message(ex); }
            finally { busy = false; layout.Enabled = true; verify.Enabled = !FirstUseState.NeedsAuthorization(storage); cancel.Visible = false; if (cancellation.IsCancellationRequested) { cancellation.Dispose(); cancellation = new CancellationTokenSource(); } }
        }
        connect.Click += async (_, _) => await Save(true); save.Click += async (_, _) => await Save(false);
        Ui.Primary(FirstUseState.NeedsAuthorization(storage) ? connect : save); layout.Controls.Add(Ui.Separator()); layout.Controls.Add(connect);
        layout.Controls.Add(Ui.Separator()); layout.Controls.Add(Ui.Text("2. Quer abrir com dois cliques?", true)); layout.Controls.Add(Info("Escolha ZagoSheetsWin nos Aplicativos padrão do Windows. É opcional: você também pode usar Abrir com → ZagoSheetsWin."));
        defaults.Click += (_, _) => { try { new BrowserLauncher().Open(WindowsAssociationPlan.DefaultsUri); } catch (Exception ex) when (LauncherErrors.Expected(ex)) { status.Text = "Abra Configurações > Aplicativos > Aplicativos padrão e procure ZagoSheetsWin."; } };
        layout.Controls.Add(defaults);
        layout.Controls.Add(Ui.Separator()); layout.Controls.Add(Ui.Text("3. Pastas sincronizadas (opcional)", true));
        layout.Controls.Add(noSync); layout.Controls.Add(folderOptions); layout.Controls.Add(consent);
        layout.Controls.Add(Info("Depois da importação confirmada: backup do original + atalho para o Sheets. Pastas não são apagadas. Em pastas sincronizadas identificadas, o original é mantido."));
        layout.Controls.Add(save);
        ShowFolderOptions(); Ui.Adapt(folderOptions);
        async Task CheckConnection()
        {
            if (busy || FirstUseState.NeedsAuthorization(storage)) return;
            busy = true; verify.Enabled = connect.Enabled = false; connectionState.Text = "◉ Verificando conexão Google…";
            try
            {
                using var handler = new HttpClientHandler { AllowAutoRedirect = false };
                using var http = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(30) };
                await new WindowsLauncher(storage, http, new BrowserLauncher()).CheckConnectionAsync(cancellation.Token);
                authorizationValid = true; connectionState.Text = "✓ Google conectado · autorização válida"; connect.Text = "Trocar conta Google…"; if (firstUse) finish.Enabled = Ready();
            }
            catch (AuthorizationRequiredException) { authorizationValid = false; connectionState.Text = "○ Google precisa de uma nova autorização"; connect.Text = "Autorizar Google…"; if (firstUse) finish.Enabled = false; }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { connectionState.Text = "◉ Autorização salva · não foi possível verificar a conexão agora"; }
            finally { busy = false; verify.Enabled = connect.Enabled = true; if (cancellation.IsCancellationRequested) { cancellation.Dispose(); cancellation = new CancellationTokenSource(); } }
        }
        verify.Click += async (_, _) => await CheckConnection();
        layout.Controls.Add(verify); layout.Controls.SetChildIndex(verify, layout.Controls.GetChildIndex(connect) + 1);
        if (!preview) Shown += async (_, _) => await CheckConnection();
        layout.Controls.Add(Ui.Separator()); layout.Controls.Add(toggle); layout.Controls.Add(status); 
        finish.Click += async (_, _) => { if (busy) return; if (firstUse) { await Save(false); if (ExitCode != 0 || !Ready()) return; } Close(); };
        Controls.Add(layout); Controls.Add(finish); finish.Dock = DockStyle.Bottom; finish.Height = 44; Controls.Add(cancel); cancel.Dock = DockStyle.Bottom;
        FormClosing += (_, e) => { if (busy) { e.Cancel = true; cancellation.Cancel(); } };
        Branding.Apply(this); Branding.Apply(advancedDialog); Ui.Adapt(layout); Ui.Adapt(advanced);
    }
    protected override void Dispose(bool disposing) { if (disposing) cancellation.Dispose(); base.Dispose(disposing); }
}
