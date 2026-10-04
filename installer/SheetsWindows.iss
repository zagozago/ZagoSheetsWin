#ifndef PilotVersion
  #define PilotVersion "0.9.19"
#endif
[Setup]
AppId={{D970FA65-0364-4F10-A6AA-D4302F31B607}
AppName=ZagoSheetsWin
UninstallDisplayName=ZagoSheetsWin
AppVersion={#PilotVersion}
AppPublisher=Zagotools
AppPublisherURL=https://zagotools.top/
AppSupportURL=https://github.com/zagozago/ZagoSheetsWin/issues
SetupIconFile=..\branding\zagosheetswin.ico
WizardImageFile=..\branding\wizard.bmp
WizardSmallImageFile=..\branding\wizard-small.bmp
WizardImageBackColor=$0B1007
DefaultDirName={localappdata}\Programs\SheetsWindows
DefaultGroupName=ZagoSheetsWin
PrivilegesRequired=lowest
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir=..\artifacts\installer
OutputBaseFilename=ZagoSheetsWin-Setup-win-x64
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
DisableWelcomePage=yes
DisableReadyPage=yes
UninstallDisplayIcon={app}\SheetsWindows.exe
CloseApplications=yes

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Files]
Source: "..\artifacts\SheetsWindows-win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\ZagoSheetsWin"; Filename: "{app}\SheetsWindows.exe"
Name: "{group}\Restaurar backups"; Filename: "{app}\SheetsWindows.exe"; Parameters: "--recovery"
Name: "{group}\Desinstalar ZagoSheetsWin"; Filename: "{uninstallexe}"

[Run]
Filename: "{app}\SheetsWindows.exe"; Parameters: "--first-use"; Flags: nowait postinstall skipifsilent

[Code]
function PrepareToInstall(var NeedsRestart: Boolean): String;
var
  InstalledVersion: String;
  InstalledPacked, PackagePacked: Int64;
begin
  Result := '';
  if RegQueryStringValue(HKCU, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{D970FA65-0364-4F10-A6AA-D4302F31B607}_is1', 'DisplayVersion', InstalledVersion) then
  begin
    if not StrToVersion(InstalledVersion, InstalledPacked) then
      Result := 'O registro da versão instalada é inválido. Corrija esse registro antes de atualizar.'
    else if StrToVersion('{#PilotVersion}', PackagePacked) then
      if ComparePackedVersion(InstalledPacked, PackagePacked) > 0 then
        Result := 'Uma versão mais recente já está instalada. A instalação de uma versão anterior está bloqueada e exige uma migração específica.';
  end;
  if CompareText(RemoveBackslashUnlessRoot(WizardDirValue),
    ExpandConstant('{localappdata}\Programs\SheetsWindows')) <> 0 then
    Result := 'Use a pasta de instalação exclusiva deste usuário. Outras pastas não são compatíveis.';
end;

procedure MaintainAssociation(const Argument: String);
var
  ExitCode: Integer;
begin
  if not Exec(ExpandConstant('{app}\SheetsWindows.exe'), Argument, '', SW_HIDE,
    ewWaitUntilTerminated, ExitCode) then
    RaiseException('Não foi possível atualizar as associações de arquivos. Os dados existentes foram preservados.');
  if ExitCode <> 0 then
    RaiseException('Há um conflito nas associações de arquivos. Remova o registro da versão portátil antes de instalar ou repare esta instalação antes de desinstalar. Os dados existentes foram preservados.');
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then MaintainAssociation('--register');
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then MaintainAssociation('--unregister');
end;
// No UninstallDelete: state, OAuth, backups, user .url and Google files are never installed here.

procedure InitializeWizard;
var
  Credit: TNewStaticText;
begin
  Credit := TNewStaticText.Create(WizardForm);
  Credit.Parent := WizardForm;
  Credit.Caption := 'Zagotools | Open in Google - Swati K (SwatiK425) | MIT';
  Credit.Left := ScaleX(8);
  Credit.Top := WizardForm.ClientHeight - ScaleY(31);
  Credit.Width := WizardForm.BackButton.Left - ScaleX(16);
  Credit.WordWrap := True;
  Credit.Height := ScaleY(24);
  Credit.Font.Size := 7;
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  if CurPageID = wpFinished then
  begin
    WizardForm.NextButton.Caption := 'Iniciar configuração';
    WizardForm.RunList.Checked[0] := True;
    WizardForm.RunList.Visible := False;
  end;
end;
