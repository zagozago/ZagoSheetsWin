param([string]$ExpectedVersion = "")
if ([string]::IsNullOrEmpty($ExpectedVersion)) {
    [xml]$project = Get-Content (Join-Path $PSScriptRoot '../src/SheetsWindows.Windows/SheetsWindows.Windows.csproj') -Raw
    $ExpectedVersion = [string]$project.Project.PropertyGroup.Version
}
$ErrorActionPreference = 'Stop'
$script:LifecycleRun = 0
$lifecycleLogs = Join-Path (Get-Location) 'artifacts/installer-verification/lifecycle'
New-Item -ItemType Directory -Force $lifecycleLogs | Out-Null
# Inno's temporary clone continues cleanup after the registered EXE returns.
# Wait for the final log record, not just the bootstrap process; never retry an install
# over a still-running uninstall or remove its directory on behalf of the test.
function Wait-UninstallFinished([string]$log, [Diagnostics.Stopwatch]$timer) {
    do {
        $text = ''
        if (Test-Path -LiteralPath $log) {
            $stream = [IO.File]::Open($log, [IO.FileMode]::Open, [IO.FileAccess]::Read,
                [IO.FileShare]::ReadWrite -bor [IO.FileShare]::Delete)
            $reader = [IO.StreamReader]::new($stream)
            try { $text = $reader.ReadToEnd() } finally { $reader.Dispose() }
            if ($text -match '(?m) Log closed\.\r?$') {
                if ($text -notmatch 'Uninstallation process succeeded\.') { throw 'Uninstaller closed without confirming success' }
                Write-Host 'Uninstaller final cleanup and log closure confirmed'
                return
            }
        }
        if ($timer.ElapsedMilliseconds -ge 120000) {
            if ($text) { $text -split "`n" | Select-Object -Last 100 | Write-Host }
            throw 'Timed out waiting for the uninstall clone to finish cleanup'
        }
        Start-Sleep -Milliseconds 100
    } while ($true)
}
function Run-Checked($file, $arguments) {
    $script:LifecycleRun++
    $log = $null
    $name = [IO.Path]::GetFileName($file)
    if ($name -like '*Setup*.exe' -or $name -like 'unins*.exe') {
        $log = Join-Path $lifecycleLogs ('{0:d2}-{1}.log' -f $script:LifecycleRun, [IO.Path]::GetFileNameWithoutExtension($file))
        $arguments += " /LOG=`"$log`""
    }
    $timer = [Diagnostics.Stopwatch]::StartNew()
    $process = Start-Process -FilePath $file -ArgumentList $arguments -PassThru
    if (!$process.WaitForExit(120000)) { $process.Kill(); throw "Timeout: $file" }
    if ($process.ExitCode -ne 0) {
        if ($log -and (Test-Path -LiteralPath $log)) { Get-Content -LiteralPath $log -Tail 100 | Write-Host }
        throw "Nonzero exit $($process.ExitCode): $file"
    }
    if ($name -like 'unins*.exe') { Wait-UninstallFinished $log $timer }
}
# Disposable Windows CI account only. Never run against a user's existing installation.
$state = Join-Path $env:LOCALAPPDATA 'SheetsWindows'
$installed = Join-Path $env:LOCALAPPDATA 'Programs/SheetsWindows'
if ((Test-Path $state) -or (Test-Path $installed) -or (Test-Path 'HKCU:/Software/SheetsWindows/Integration')) { throw 'Lifecycle test requires a clean disposable profile' }
$extensions = @('.xlsx', '.ods', '.xls', '.csv', '.tsv')
function Defaults-Snapshot {
    foreach ($extension in $extensions) {
        $defaultKey = "HKCU:/Software/Classes/$extension"
        $choiceKey = "HKCU:/Software/Microsoft/Windows/CurrentVersion/Explorer/FileExts/$extension/UserChoice"
        $default = if (Test-Path $defaultKey) { (Get-Item $defaultKey).GetValue('') } else { $null }
        $choice = if (Test-Path $choiceKey) { (Get-ItemProperty $choiceKey | Select-Object ProgId, Hash) | ConvertTo-Json -Compress } else { '<absent>' }
        "$extension-default:$default"; "$extension-choice:$choice"
    }
}
function Assert-NoLauncherUI {
    if (@(Get-Process -Name SheetsWindows -ErrorAction SilentlyContinue | Where-Object { $_.MainWindowHandle -ne 0 }).Count -gt 0) { throw 'Silent installation launched interactive UI' }
}
function Current-Uninstaller {
    $key = 'HKCU:/Software/Microsoft/Windows/CurrentVersion/Uninstall/{D970FA65-0364-4F10-A6AA-D4302F31B607}_is1'
    $command = (Get-ItemProperty $key).UninstallString
    $path = $command.Trim('"')
    if ([IO.Path]::GetDirectoryName($path) -ne $installed -or [IO.Path]::GetFileName($path) -notmatch '^unins[0-9]+\.exe$' -or !(Test-Path $path)) { throw 'Unexpected uninstall command' }
    return $path
}
$before = Defaults-Snapshot
$oldSetup = (Resolve-Path 'artifacts/installer-old/ZagoSheetsWin-Setup-win-x64.exe').Path
$setup = (Resolve-Path 'artifacts/installer/ZagoSheetsWin-Setup-win-x64.exe').Path
Write-Host 'Running installer'
Run-Checked $oldSetup '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART'
Assert-NoLauncherUI
$exe = Join-Path $installed 'SheetsWindows.exe'
if ((Get-ItemProperty 'HKCU:/Software/Microsoft/Windows/CurrentVersion/Uninstall/{D970FA65-0364-4F10-A6AA-D4302F31B607}_is1').DisplayName -ne 'ZagoSheetsWin') { throw 'Product name incorrect' }
if (!(Test-Path $exe)) { throw 'Executable not installed' }
Run-Checked $exe '--version'
if (!(Test-Path 'HKCU:/Software/SheetsWindows/Integration')) { throw 'Association registration missing' }
foreach ($extension in $extensions) {
    if ((Get-ItemProperty 'HKCU:/Software/SheetsWindows/Integration/Capabilities/FileAssociations').$extension -ne 'SheetsWindows.Xlsx') { throw "Missing association: $extension" }
}
# Data sentinels are outside the installation manifest; Google network access is never needed.
foreach ($directory in @('backups', 'auth', 'shortcuts', 'uploads', 'logs')) { New-Item (Join-Path $state $directory) -ItemType Directory -Force | Out-Null }
$sentinels = @('registry.db', 'google.db', 'replacement.db', 'launcher-client.json', 'replacement-root.txt', 'backups/preserved.snapshot', 'formats.json', 'auth/preserved.dat', 'shortcuts/preserved.url', 'uploads/preserved.session', 'shortcut-icon-v1.ico', 'xls-replacement.json', 'theme.json', 'language.json', 'backup-policy.json', 'backup-lifecycle.db', 'logs/events.jsonl')
foreach ($name in $sentinels) { $value = if ($name -eq 'language.json') { '"pt"' } else { "preserve:$name" }; [IO.File]::WriteAllText((Join-Path $state $name), $value) }
$shortcut = Join-Path $env:RUNNER_TEMP 'preserved.url'
[IO.File]::WriteAllText($shortcut, "[InternetShortcut]`r`nURL=https://docs.google.com/spreadsheets/d/test/edit`r`n")
$shortcutBefore = [IO.File]::ReadAllText($shortcut)
Write-Host 'Running installer'
Run-Checked $setup '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /LANG=english'
Assert-NoLauncherUI
# The previous-version package uses the same payload to exercise installer version policy.
$key = 'HKCU:/Software/Microsoft/Windows/CurrentVersion/Uninstall/{D970FA65-0364-4F10-A6AA-D4302F31B607}_is1'
if ((Get-ItemProperty $key).DisplayVersion -ne $ExpectedVersion) { throw 'Upgrade version missing' }
$reject = Start-Process -FilePath $oldSetup -ArgumentList '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART' -PassThru
if (!$reject.WaitForExit(120000)) { $reject.Kill(); throw 'Downgrade rejection timed out' }
if ($reject.ExitCode -eq 0 -or (Get-ItemProperty $key).DisplayVersion -ne $ExpectedVersion) { throw 'Downgrade was not blocked' }
Write-Host 'Running registered uninstaller'
Run-Checked (Current-Uninstaller) '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART'
if (Test-Path $exe) { throw 'Installed executable remains' }
if (Test-Path 'HKCU:/Software/SheetsWindows/Integration') { throw 'Owned registration remains' }
foreach ($name in $sentinels) { $expected = if ($name -eq 'language.json') { '"pt"' } else { "preserve:$name" }; if ([IO.File]::ReadAllText((Join-Path $state $name)) -ne $expected) { throw "Data changed: $name" } }
if ([IO.File]::ReadAllText($shortcut) -ne $shortcutBefore) { throw 'Shortcut changed' }
if (Compare-Object $before (Defaults-Snapshot)) { throw 'Windows defaults changed' }
# Reinstall and remove again demonstrates retained state does not block maintenance.
Write-Host 'Running installer'
Run-Checked $setup '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /LANG=english'
Run-Checked $setup '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /LANG=ar /ZagoSelectedLanguage=ar'
if ([IO.File]::ReadAllText((Join-Path $state 'language.json')) -ne '"ar"') { throw 'Explicit installer selection was not passed to the app' }
Run-Checked $exe '--installer-language-selected pt'

Assert-NoLauncherUI
Write-Host 'Running registered uninstaller'
Run-Checked (Current-Uninstaller) '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART'
# Repeat the immediate reinstall scenario: no arbitrary grace period may hide a race.
foreach ($cycle in 1..2) {
    Write-Host "Immediate reinstall/uninstall regression cycle $cycle"
    Run-Checked $setup '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART /LANG=english'
    Assert-NoLauncherUI
    Run-Checked (Current-Uninstaller) '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART'
    if (Test-Path $exe) { throw 'Installed executable remains after repeated uninstall' }
    if (Test-Path 'HKCU:/Software/SheetsWindows/Integration') { throw 'Owned registration remains after repeated uninstall' }
    foreach ($name in $sentinels) { $expected = if ($name -eq 'language.json') { '"pt"' } else { "preserve:$name" }; if ([IO.File]::ReadAllText((Join-Path $state $name)) -ne $expected) { throw "Data changed during repeated uninstall: $name" } }
    if ([IO.File]::ReadAllText($shortcut) -ne $shortcutBefore) { throw 'Shortcut changed during repeated uninstall' }
    if (Compare-Object $before (Defaults-Snapshot)) { throw 'Windows defaults changed during repeated uninstall' }
}
Write-Host 'Per-user install, upgrade, uninstall and reinstall passed; backups, state, shortcut and Windows defaults preserved.'
