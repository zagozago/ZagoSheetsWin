using SheetsWindows.Infrastructure;
using System.Drawing.Drawing2D;

namespace SheetsWindows.Windows;

// Offline illustrations: no web content, tracking or Google credentials.
internal sealed class TutorialForm : Form
{
    private readonly CheckBox hide = new() { Text = "Não mostrar este tutorial ao abrir o aplicativo", AutoSize = true, Checked = false };
    private readonly Label heading = new() { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Segoe UI", 14, FontStyle.Bold) };
    private readonly FlowLayoutPanel explanation = new() { Dock = DockStyle.Fill, AutoScroll = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
    private readonly TutorialPicture picture = new() { Dock = DockStyle.Fill };
    private readonly Button previous = new() { Text = "Voltar", AutoSize = true };
    private readonly Button next = new() { Text = "Próximo", AutoSize = true };
    private readonly Label count = new() { AutoSize = true };
    private int page;
    private bool saving;
    private static readonly (string Title, string Text)[] Pages =
    [
        ("Abra suas planilhas no Google Sheets", "Abra um arquivo no computador. O ZagoSheetsWin importa e confere a planilha. Continue no navegador.|Após a conferência: backup do original + atalho para o Sheets. Se a conversão não puder ser confirmada, o original é mantido."),
        ("Conecte sua conta Google", "Escolha sua conta → autorize o acesso → volte ao aplicativo.|Nas configurações, ✓ Google conectado confirma a autorização. Suas planilhas ficam no Google Drive dessa conta. Não precisa instalar o Google Drive para Windows."),
        ("Escolha como abrir suas planilhas", "Dê dois cliques ou use Abrir com → ZagoSheetsWin no menu do Windows.|CSV · TSV · XLSX · XLS. ODS experimental. Definir como aplicativo padrão é opcional."),
        ("Guarde o original. Saiba como recuperar.", "Original → backup local → Google Sheets. Em Backups, escolha Restaurar em… para recuperar o arquivo.|30 dias · 200 MB (ajustável até 1 GB). Limpeza automática só quando ativada; operações pendentes são protegidas.|O backup guarda o original, sem alterações posteriores no Sheets. Fórmulas e formatação podem mudar; macros XLS não funcionam no Sheets.")
    ];
    public TutorialForm(int initialPage = 0)
    {
        page = Math.Clamp(initialPage, 0, Pages.Length - 1);
        Text = "Como funciona — ZagoSheetsWin"; ClientSize = new Size(760, 650); MinimumSize = new Size(620, 560); StartPosition = FormStartPosition.CenterParent; AutoScaleMode = AutoScaleMode.Dpi;
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), ColumnCount = 1, RowCount = 5 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 250)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        layout.Controls.Add(heading, 0, 0); layout.Controls.Add(picture, 0, 1); layout.Controls.Add(explanation, 0, 2); layout.Controls.Add(hide, 0, 3);
        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
        var skip = new Button { Text = "Pular tutorial", AutoSize = true };
        actions.Controls.Add(skip); actions.Controls.Add(previous); actions.Controls.Add(next); actions.Controls.Add(count); layout.Controls.Add(actions, 0, 4);
        previous.Click += (_, _) => { page--; RefreshPage(); }; next.Click += (_, _) => { if (page == Pages.Length - 1) Close(); else { page++; RefreshPage(); } }; skip.Click += (_, _) => Close();
        Ui.Primary(next); Controls.Add(layout); Branding.Apply(this); Ui.Adapt(explanation); RefreshPage();
        FormClosing += async (_, e) =>
        {
            if (saving) { e.Cancel = true; return; }
            if (!hide.Checked) return;
            e.Cancel = true; saving = true; layout.Enabled = false;
            try { await TutorialSettings.SaveAsync(LocalStorage.ForCurrentUser(), true); hide.Checked = false; saving = false; Close(); }
            catch (Exception ex) when (LauncherErrors.Expected(ex)) { saving = false; layout.Enabled = true; MessageBox.Show(this, LauncherErrors.Message(ex), "Preferência do tutorial", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        };
    }
    private void RefreshPage()
    {
        heading.Text = $"{page + 1}. {Pages[page].Title}";
        var old = explanation.Controls.Cast<Control>().ToArray(); explanation.Controls.Clear(); foreach (var control in old) control.Dispose();
        var groups = Pages[page].Text.Split('|');
        for (var i = 0; i < groups.Length; i++) { if (i > 0) explanation.Controls.Add(Ui.Separator()); var paragraph = Ui.Text(groups[i]); paragraph.MaximumSize = new Size(Math.Max(120, explanation.ClientSize.Width - 25), 0); explanation.Controls.Add(paragraph); }
        if (page is 2 or 3)
        {
            var action = new Button { AutoSize = true, Text = page == 2 ? "Definir como padrão…" : "Abrir backups…" };
            var backups = page == 3;
            action.Click += (_, _) =>
            {
                if (backups) { using var recovery = new RecoveryForm(); recovery.ShowDialog(this); }
                else try { new BrowserLauncher().Open(WindowsAssociationPlan.DefaultsUri); }
                catch (Exception ex) when (LauncherErrors.Expected(ex)) { MessageBox.Show(this, "Abra Configurações > Aplicativos > Aplicativos padrão e procure ZagoSheetsWin."); }
            };
            explanation.Controls.Add(action);
        }
        explanation.PerformLayout(); Branding.Refresh(this);
        count.Text = $"{page + 1} de {Pages.Length}";
        previous.Enabled = page > 0; next.Text = page == Pages.Length - 1 ? "Começar" : "Próximo"; picture.Page = page; picture.AccessibleName = Pages[page].Title; picture.Invalidate();
    }
}
internal sealed class TutorialPicture : Control
{
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int Page { get; set; }
    public TutorialPicture() { DoubleBuffered = true; }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
        var scale = Math.Min(Width / 700f, Height / 250f);
        g.TranslateTransform((Width - 700 * scale) / 2, (Height - 250 * scale) / 2); g.ScaleTransform(scale, scale);
        var dark = Branding.Current == ApplicationTheme.Dark;
        var accent = SystemInformation.HighContrast ? ForeColor : Color.FromArgb(25, 134, 74);
        using var pen = new Pen(accent, 3);
        using var green = new SolidBrush(accent);
        using var ink = new SolidBrush(ForeColor);
        using var pale = new SolidBrush(SystemInformation.HighContrast ? BackColor : dark ? Color.FromArgb(18, 37, 26) : Color.FromArgb(231, 246, 237));
        using var white = new SolidBrush(SystemInformation.HighContrast ? BackColor : Color.White);
        using var gold = new SolidBrush(Color.FromArgb(243, 189, 63));
        using var font = new Font("Segoe UI", 16, FontStyle.Bold, GraphicsUnit.Pixel);
        using var small = new Font("Segoe UI", 13, FontStyle.Regular, GraphicsUnit.Pixel);
        using var centered = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        void Round(float x, float y, float width, float height, Brush brush)
        {
            using var shape = new GraphicsPath(); const float radius = 20;
            shape.AddArc(x, y, radius, radius, 180, 90); shape.AddArc(x + width - radius, y, radius, radius, 270, 90);
            shape.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90); shape.AddArc(x, y + height - radius, radius, radius, 90, 90); shape.CloseFigure();
            g.FillPath(brush, shape);
        }
        void FileIcon(float x, float y, string label, bool sheet = false)
        {
            using var outline = new Pen(ForeColor, 1.5f);
            using var shape = new GraphicsPath();
            shape.AddPolygon([new PointF(x, y), new PointF(x + 51, y), new PointF(x + 72, y + 21), new PointF(x + 72, y + 91), new PointF(x, y + 91)]);
            g.FillPath(sheet ? green : pale, shape); g.DrawPath(outline, shape);
            g.DrawLines(outline, [new PointF(x + 51, y), new PointF(x + 51, y + 21), new PointF(x + 72, y + 21)]);
            for (var row = 0; row < 3; row++) for (var col = 0; col < 3; col++) g.FillRectangle(sheet ? white : green, x + 13 + col * 16, y + 31 + row * 14, 12, 10);
            if (label.Length > 0) { Round(x - 4, y + 77, 80, 28, green); g.DrawString(label, small, white, new RectangleF(x - 4, y + 77, 80, 28), centered); }
        }
        void AppIcon(float x, float y, bool authorize)
        {
            Round(x, y, 144, 107, pale); g.DrawRectangle(pen, x, y, 144, 107); g.DrawLine(pen, x, y + 18, x + 144, y + 18);
            using var source = typeof(Branding).Assembly.GetManifestResourceStream("Brand.z.png")!;
            using var logo = Image.FromStream(source); g.DrawImage(logo, x + 48, y + 29, 48, 44);
            g.DrawString("ZagoSheetsWin", small, ink, new RectangleF(x, y + 70, 144, 20), centered);
            Round(x + 12, y + 92, 120, 9, green);
            if (authorize) { Round(x + 12, y + 80, 120, 23, green); g.DrawString("Autorizar", small, white, new RectangleF(x + 12, y + 80, 120, 23), centered); }
        }
        void Folder(float x, float y)
        {
            Round(x, y + 18, 98, 65, gold); Round(x, y + 6, 46, 28, gold);
            g.FillEllipse(green, x + 67, y + 58, 38, 38); using var hands = new Pen(Color.White, 2);
            g.DrawLines(hands, [new PointF(x + 86, y + 66), new PointF(x + 86, y + 77), new PointF(x + 94, y + 83)]);
        }
        if (Page == 2)
        {
            Round(5, 10, 465, 151, pale);
            var formats = new[] { "CSV", "TSV", "XLSX", "XLS", "ODS" };
            for (var index = 0; index < formats.Length; index++) FileIcon(15 + index * 91, 32, formats[index]);
            AppIcon(540, 30, false);
            g.DrawLine(pen, 487, 83, 521, 83); g.DrawLines(pen, [new PointF(511, 73), new PointF(521, 83), new PointF(511, 93)]);
            g.DrawString("Escolha sua planilha", font, ink, new RectangleF(5, 185, 465, 45), centered);
            g.DrawString("Abrir com → ZagoSheetsWin", small, ink, new RectangleF(485, 178, 210, 60), centered);
            return;
        }
        string[] labels = Page switch
        {
            1 => ["Escolha sua conta", "Autorize o acesso", "Google conectado"],
            2 => ["Arquivo no computador", "Abrir com", "Google Sheets"],
            3 => ["Original", "Backup local", "Google Sheets"],
            _ => ["Abra o arquivo", "Importação conferida", "Continue no navegador"]
        };
        for (var step = 0; step < 3; step++)
        {
            var x = 5 + step * 245;
            Round(x, 10, 195, 151, pale);
            if (Page == 1 && step == 0)
            {
                g.DrawString("Google", font, ink, new RectangleF(x, 27, 195, 35), centered);
                g.FillEllipse(green, x + 78, 70, 38, 38);
                g.DrawString("Sua conta", small, ink, new RectangleF(x, 112, 195, 26), centered);
            }
            else if (step == 1 && Page == 3) Folder(x + 48, 39);
            else if (step == 1) AppIcon(x + 25, 30, Page == 1);
            else FileIcon(x + 61, 31, step == 0 ? "XLSX" : "", step == 2);
            g.FillEllipse(green, x + 5, 174, 26, 26);
            g.DrawString((step + 1).ToString(), small, white, new RectangleF(x + 5, 174, 26, 26), centered);
            g.DrawString(labels[step], font, ink, new RectangleF(x + 2, 207, 191, 42), centered);
        }
        foreach (var x in new[] { 211, 456 }) { g.DrawLine(pen, x, 83, x + 24, 83); g.DrawLines(pen, [new PointF(x + 15, 74), new PointF(x + 24, 83), new PointF(x + 15, 92)]); }
    }
}
