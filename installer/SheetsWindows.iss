#ifndef PilotVersion
  #define PilotVersion "0.9.22"
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
Name: "english"; MessagesFile: "compiler:Default.isl,i18n\en-options.isl,i18n\en-standard.isl,i18n\en.isl"
Name: "zh"; MessagesFile: "compiler:Default.isl,i18n\zh-options.isl,i18n\zh-standard.isl,i18n\zh.isl"
Name: "hi"; MessagesFile: "compiler:Default.isl,i18n\hi-options.isl,i18n\hi-standard.isl,i18n\hi.isl"
Name: "es"; MessagesFile: "compiler:Default.isl,i18n\es-options.isl,i18n\es-standard.isl,i18n\es.isl"
Name: "ar"; MessagesFile: "compiler:Default.isl,i18n\ar-options.isl,i18n\ar-standard.isl,i18n\ar.isl"
Name: "fr"; MessagesFile: "compiler:Default.isl,i18n\fr-options.isl,i18n\fr-standard.isl,i18n\fr.isl"
Name: "bn"; MessagesFile: "compiler:Default.isl,i18n\bn-options.isl,i18n\bn-standard.isl,i18n\bn.isl"
Name: "brazilianportuguese"; MessagesFile: "compiler:Default.isl,i18n\pt-options.isl,i18n\pt-standard.isl,i18n\pt.isl"
Name: "id"; MessagesFile: "compiler:Default.isl,i18n\id-options.isl,i18n\id-standard.isl,i18n\id.isl"
Name: "ur"; MessagesFile: "compiler:Default.isl,i18n\ur-options.isl,i18n\ur-standard.isl,i18n\ur.isl"
Name: "ru"; MessagesFile: "compiler:Default.isl,i18n\ru-options.isl,i18n\ru-standard.isl,i18n\ru.isl"
Name: "de"; MessagesFile: "compiler:Default.isl,i18n\de-options.isl,i18n\de-standard.isl,i18n\de.isl"
Name: "ja"; MessagesFile: "compiler:Default.isl,i18n\ja-options.isl,i18n\ja-standard.isl,i18n\ja.isl"
Name: "vi"; MessagesFile: "compiler:Default.isl,i18n\vi-options.isl,i18n\vi-standard.isl,i18n\vi.isl"
Name: "tr"; MessagesFile: "compiler:Default.isl,i18n\tr-options.isl,i18n\tr-standard.isl,i18n\tr.isl"
Name: "ko"; MessagesFile: "compiler:Default.isl,i18n\ko-options.isl,i18n\ko-standard.isl,i18n\ko.isl"
Name: "it"; MessagesFile: "compiler:Default.isl,i18n\it-options.isl,i18n\it-standard.isl,i18n\it.isl"
Name: "th"; MessagesFile: "compiler:Default.isl,i18n\th-options.isl,i18n\th-standard.isl,i18n\th.isl"
Name: "fil"; MessagesFile: "compiler:Default.isl,i18n\fil-options.isl,i18n\fil-standard.isl,i18n\fil.isl"
Name: "ms"; MessagesFile: "compiler:Default.isl,i18n\ms-options.isl,i18n\ms-standard.isl,i18n\ms.isl"
Name: "sw"; MessagesFile: "compiler:Default.isl,i18n\sw-options.isl,i18n\sw-standard.isl,i18n\sw.isl"
Name: "pcm"; MessagesFile: "compiler:Default.isl,i18n\pcm-options.isl,i18n\pcm-standard.isl,i18n\pcm.isl"
Name: "mr"; MessagesFile: "compiler:Default.isl,i18n\mr-options.isl,i18n\mr-standard.isl,i18n\mr.isl"
Name: "te"; MessagesFile: "compiler:Default.isl,i18n\te-options.isl,i18n\te-standard.isl,i18n\te.isl"
Name: "ha"; MessagesFile: "compiler:Default.isl,i18n\ha-options.isl,i18n\ha-standard.isl,i18n\ha.isl"
Name: "pa"; MessagesFile: "compiler:Default.isl,i18n\pa-options.isl,i18n\pa-standard.isl,i18n\pa.isl"
Name: "ta"; MessagesFile: "compiler:Default.isl,i18n\ta-options.isl,i18n\ta-standard.isl,i18n\ta.isl"
Name: "yue"; MessagesFile: "compiler:Default.isl,i18n\yue-options.isl,i18n\yue-standard.isl,i18n\yue.isl"
Name: "fa"; MessagesFile: "compiler:Default.isl,i18n\fa-options.isl,i18n\fa-standard.isl,i18n\fa.isl"
Name: "am"; MessagesFile: "compiler:Default.isl,i18n\am-options.isl,i18n\am-standard.isl,i18n\am.isl"
Name: "jv"; MessagesFile: "compiler:Default.isl,i18n\jv-options.isl,i18n\jv-standard.isl,i18n\jv.isl"
Name: "gu"; MessagesFile: "compiler:Default.isl,i18n\gu-options.isl,i18n\gu-standard.isl,i18n\gu.isl"
Name: "kn"; MessagesFile: "compiler:Default.isl,i18n\kn-options.isl,i18n\kn-standard.isl,i18n\kn.isl"
Name: "yo"; MessagesFile: "compiler:Default.isl,i18n\yo-options.isl,i18n\yo-standard.isl,i18n\yo.isl"
Name: "bho"; MessagesFile: "compiler:Default.isl,i18n\bho-options.isl,i18n\bho-standard.isl,i18n\bho.isl"
Name: "ps"; MessagesFile: "compiler:Default.isl,i18n\ps-options.isl,i18n\ps-standard.isl,i18n\ps.isl"
Name: "odia"; MessagesFile: "compiler:Default.isl,i18n\or-options.isl,i18n\or-standard.isl,i18n\or.isl"
Name: "my"; MessagesFile: "compiler:Default.isl,i18n\my-options.isl,i18n\my-standard.isl,i18n\my.isl"
Name: "ml"; MessagesFile: "compiler:Default.isl,i18n\ml-options.isl,i18n\ml-standard.isl,i18n\ml.isl"
Name: "pl"; MessagesFile: "compiler:Default.isl,i18n\pl-options.isl,i18n\pl-standard.isl,i18n\pl.isl"
Name: "su"; MessagesFile: "compiler:Default.isl,i18n\su-options.isl,i18n\su-standard.isl,i18n\su.isl"
Name: "mai"; MessagesFile: "compiler:Default.isl,i18n\mai-options.isl,i18n\mai-standard.isl,i18n\mai.isl"
Name: "uk"; MessagesFile: "compiler:Default.isl,i18n\uk-options.isl,i18n\uk-standard.isl,i18n\uk.isl"
Name: "om"; MessagesFile: "compiler:Default.isl,i18n\om-options.isl,i18n\om-standard.isl,i18n\om.isl"
Name: "uz"; MessagesFile: "compiler:Default.isl,i18n\uz-options.isl,i18n\uz-standard.isl,i18n\uz.isl"
Name: "sd"; MessagesFile: "compiler:Default.isl,i18n\sd-options.isl,i18n\sd-standard.isl,i18n\sd.isl"
Name: "ne"; MessagesFile: "compiler:Default.isl,i18n\ne-options.isl,i18n\ne-standard.isl,i18n\ne.isl"
Name: "ig"; MessagesFile: "compiler:Default.isl,i18n\ig-options.isl,i18n\ig-standard.isl,i18n\ig.isl"
Name: "az"; MessagesFile: "compiler:Default.isl,i18n\az-options.isl,i18n\az-standard.isl,i18n\az.isl"
Name: "nl"; MessagesFile: "compiler:Default.isl,i18n\nl-options.isl,i18n\nl-standard.isl,i18n\nl.isl"
Name: "gn"; MessagesFile: "compiler:Default.isl,i18n\gn-options.isl,i18n\gn-standard.isl,i18n\gn.isl"

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
    MaintainAssociation('--installer-language ' + CustomMessage('Zago_languageCode'));
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

