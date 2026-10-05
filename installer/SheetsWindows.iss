#ifndef PilotVersion
  #define PilotVersion "0.9.21"
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
ShowLanguageDialog=yes
LanguageDetectionMethod=uilanguage

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl,i18n\pt-standard.isl,i18n\pt.isl"

Name: "english"; MessagesFile: "compiler:Default.isl,i18n\en-standard.isl,i18n\en.isl"

[Files]
Source: "..\artifacts\SheetsWindows-win-x64\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\ZagoSheetsWin"; Filename: "{app}\SheetsWindows.exe"
Name: "{group}\{cm:Zago_restoreBackups}"; Filename: "{app}\SheetsWindows.exe"; Parameters: "--recovery"
Name: "{group}\{cm:Zago_uninstall}"; Filename: "{uninstallexe}"

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
      Result := CustomMessage('Zago_invalidInstalledVersion')
    else if StrToVersion('{#PilotVersion}', PackagePacked) then
      if ComparePackedVersion(InstalledPacked, PackagePacked) > 0 then
        Result := CustomMessage('Zago_downgradeBlocked');
  end;
  if CompareText(RemoveBackslashUnlessRoot(WizardDirValue),
    ExpandConstant('{localappdata}\Programs\SheetsWindows')) <> 0 then
    Result := CustomMessage('Zago_dedicatedDirectory');
end;

procedure MaintainAssociation(const Argument: String);
var
  ExitCode: Integer;
begin
  if not Exec(ExpandConstant('{app}\SheetsWindows.exe'), Argument, '', SW_HIDE,
    ewWaitUntilTerminated, ExitCode) then
    RaiseException(CustomMessage('Zago_associationRunFailed'));
  if ExitCode <> 0 then
    RaiseException(CustomMessage('Zago_associationConflict'));
end;

procedure CurStepChanged(CurStep: TSetupStep);
begin
  if CurStep = ssPostInstall then
  begin
    MaintainAssociation('--register');
    if ActiveLanguage = 'brazilianportuguese' then MaintainAssociation('--installer-language pt')
    else MaintainAssociation('--installer-language en');
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  if CurUninstallStep = usUninstall then MaintainAssociation('--unregister');
end;
// No UninstallDelete: state, OAuth, backups, user .url and Google files are never installed here.

var
  Credit: TNewStaticText;

procedure FitWizardButtons;
var
  BackWidth, NextWidth, CancelWidth, Gap, Needed: Integer;
begin
  Gap := ScaleX(8);
  BackWidth := WizardForm.CalculateButtonWidth([WizardForm.BackButton.Caption]);
  NextWidth := WizardForm.CalculateButtonWidth([WizardForm.NextButton.Caption]);
  CancelWidth := WizardForm.CalculateButtonWidth([WizardForm.CancelButton.Caption]);
  Needed := BackWidth + NextWidth + CancelWidth + Gap * 4;
  if Needed > WizardForm.ClientWidth then WizardForm.ClientWidth := Needed;
  WizardForm.CancelButton.Width := CancelWidth;
  WizardForm.CancelButton.Left := WizardForm.ClientWidth - Gap - CancelWidth;
  WizardForm.NextButton.Width := NextWidth;
  WizardForm.NextButton.Left := WizardForm.CancelButton.Left - Gap - NextWidth;
  WizardForm.BackButton.Width := BackWidth;
  WizardForm.BackButton.Left := WizardForm.NextButton.Left - Gap - BackWidth;
  if Credit <> nil then Credit.Width := WizardForm.ClientWidth - ScaleX(16);
end;

procedure InitializeWizard;
begin
  WizardForm.ClientHeight := WizardForm.ClientHeight + ScaleY(28);
  WizardForm.OuterNotebook.Height := WizardForm.OuterNotebook.Height - ScaleY(28);
  WizardForm.Bevel.Top := WizardForm.Bevel.Top - ScaleY(28);
  WizardForm.BackButton.Top := WizardForm.BackButton.Top - ScaleY(28);
  WizardForm.NextButton.Top := WizardForm.NextButton.Top - ScaleY(28);
  WizardForm.CancelButton.Top := WizardForm.CancelButton.Top - ScaleY(28);
  Credit := TNewStaticText.Create(WizardForm);
  Credit.Parent := WizardForm;
  Credit.Caption := CustomMessage('Zago_credit');
  Credit.Left := ScaleX(8);
  Credit.Top := WizardForm.ClientHeight - ScaleY(24);
  Credit.Width := WizardForm.ClientWidth - ScaleX(16);
  Credit.WordWrap := True;
  Credit.Height := ScaleY(24);
  Credit.Font.Size := 7;
  FitWizardButtons;
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  if CurPageID = wpFinished then
  begin
    WizardForm.NextButton.Caption := CustomMessage('Zago_runSetup');
    WizardForm.RunList.Checked[0] := True;
    WizardForm.RunList.Visible := False;
  end;
  FitWizardButtons;
end;

procedure InitializeUninstallProgressForm;
begin
  UninstallProgressForm.CancelButton.Width := UninstallProgressForm.CalculateButtonWidth([UninstallProgressForm.CancelButton.Caption]);
  UninstallProgressForm.CancelButton.Left := UninstallProgressForm.ClientWidth - ScaleX(8) - UninstallProgressForm.CancelButton.Width;
end;
