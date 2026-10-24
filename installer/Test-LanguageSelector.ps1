$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
# Drive the real shipped installer: choose each language and enter its welcome page.
Add-Type -TypeDefinition @'
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
public static class InstallerSelectorNative {
  [StructLayout(LayoutKind.Sequential)] public struct Rect { public int Left, Top, Right, Bottom; }
  [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr hwnd, out Rect rect);
  [DllImport("user32.dll")] public static extern bool GetClientRect(IntPtr hwnd, out Rect rect);
  [DllImport("user32.dll")] public static extern bool PrintWindow(IntPtr hwnd, IntPtr dc, uint flags);
  public delegate bool Callback(IntPtr hwnd, IntPtr data);
  [DllImport("user32.dll")] public static extern bool EnumWindows(Callback callback, IntPtr data);
  [DllImport("user32.dll")] public static extern bool EnumChildWindows(IntPtr hwnd, Callback callback, IntPtr data);
  [DllImport("user32.dll")] public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint id);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] public static extern IntPtr SendMessage(IntPtr hwnd, int message, IntPtr index, StringBuilder text);
  [DllImport("user32.dll", CharSet=CharSet.Unicode, EntryPoint="SendMessageW")] public static extern IntPtr SendValue(IntPtr hwnd, int message, IntPtr wparam, IntPtr lparam);
  [DllImport("user32.dll")] public static extern bool PostMessage(IntPtr hwnd, int message, IntPtr wparam, IntPtr lparam);
  [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr hwnd);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern int GetClassName(IntPtr hwnd, StringBuilder text, int size);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] static extern int GetWindowText(IntPtr hwnd, StringBuilder text, int size);
  [DllImport("user32.dll", EntryPoint="GetWindowLongW")] static extern int GetStyle(IntPtr hwnd, int index);
  public static string Class(IntPtr hwnd) { var s=new StringBuilder(256); GetClassName(hwnd,s,s.Capacity); return s.ToString(); }
  public static string Text(IntPtr hwnd) { var s=new StringBuilder(4096); GetWindowText(hwnd,s,s.Capacity); return s.ToString(); }
  public static IntPtr[] Windows(uint[] ids) {
    var found=new List<IntPtr>();
    EnumWindows((hwnd,data) => { uint id; GetWindowThreadProcessId(hwnd,out id);
      if (Array.IndexOf(ids,id)>=0 && IsWindowVisible(hwnd)) found.Add(hwnd); return true; },IntPtr.Zero);
    return found.ToArray();
  }
  public static IntPtr[] Children(IntPtr hwnd) {
    var found=new List<IntPtr>(); EnumChildWindows(hwnd,(child,data)=>{found.Add(child);return true;},IntPtr.Zero); return found.ToArray();
  }
  public static IntPtr[] Find(uint[] ids) {
    foreach (var form in Windows(ids)) foreach (var child in Children(form))
      if (Class(child).Contains("Combo") && SendValue(child,0x0146,IntPtr.Zero,IntPtr.Zero).ToInt64()==51) return new[]{form,child};
    return new[]{IntPtr.Zero,IntPtr.Zero};
  }
  public static IntPtr DefaultButton(IntPtr form) {
    foreach(var child in Children(form)) if(Class(child).Contains("Button") && (GetStyle(child,-16)&15)==1) return child;
    return IntPtr.Zero;
  }
}
'@
$setup = (Resolve-Path 'artifacts/installer/ZagoSheetsWin-Setup-win-x64.exe').Path
$languages = (Get-Content 'i18n/locales.json' -Raw -Encoding UTF8 | ConvertFrom-Json).languages
$output = 'artifacts/installer-verification'
New-Item -ItemType Directory -Force $output | Out-Null
$results = @()
function New-InstallerIds($baseline) {
    [uint[]]@(Get-Process | Where-Object { $_.Id -notin $baseline -and ($_.ProcessName -like 'ZagoSheetsWin-Setup-win-x64*' -or $_.ProcessName -like 'is-*') } | Select-Object -ExpandProperty Id)
}
function Stop-InstallerProcesses($baseline) {
    foreach ($idToStop in @(New-InstallerIds $baseline)) { Stop-Process -Id $idToStop -Force -ErrorAction SilentlyContinue }
}
function Wait-Selector($baseline) {
    $deadline = [DateTime]::UtcNow.AddSeconds(30)
    do {
        $found = [InstallerSelectorNative]::Find([uint[]]@(New-InstallerIds $baseline))
        if ($found[1] -ne [IntPtr]::Zero) { return $found }
        Start-Sleep -Milliseconds 100
    } while ([DateTime]::UtcNow -lt $deadline)
    throw 'Custom installer selector with all 51 entries did not appear'
}
function Save-InstallerFrame([IntPtr]$window, [string]$name) {
    $bounds = New-Object InstallerSelectorNative+Rect
    [void][InstallerSelectorNative]::GetWindowRect($window,[ref]$bounds)
    $bitmap = New-Object Drawing.Bitmap ($bounds.Right-$bounds.Left),($bounds.Bottom-$bounds.Top)
    $graphics = [Drawing.Graphics]::FromImage($bitmap)
    $dc = $graphics.GetHdc()
    try { [void][InstallerSelectorNative]::PrintWindow($window,$dc,2) }
    finally { $graphics.ReleaseHdc($dc) }
    try { $bitmap.Save((Join-Path (Get-Location).Path "$output/$name.png"),[Drawing.Imaging.ImageFormat]::Png) }
    finally { $graphics.Dispose(); $bitmap.Dispose() }
}
for ($index=0; $index -lt $languages.Count; $index++) {
    $language = $languages[$index]
    $code = $language.code
    $baseline = @(Get-Process | Select-Object -ExpandProperty Id)
    $log = Join-Path (Get-Location).Path "$output/$code.log"
    $process = Start-Process $setup -ArgumentList @('/SP-', "/LOG=`"$log`"") -PassThru
    try {
        $found = Wait-Selector $baseline
        if ($index -eq 0) {
            Save-InstallerFrame $found[0] 'language-selector-closed'
            [void][InstallerSelectorNative]::SendValue($found[1],0x014F,[IntPtr]1,[IntPtr]::Zero)
            Start-Sleep -Milliseconds 100
            if ([InstallerSelectorNative]::SendValue($found[1],0x0157,[IntPtr]::Zero,[IntPtr]::Zero).ToInt32() -ne 1) { throw 'Installer language list must open' }
            foreach($popup in [InstallerSelectorNative]::Windows([uint[]]@(New-InstallerIds $baseline))) {
                if ([InstallerSelectorNative]::Class($popup) -eq 'ComboLBox') { Save-InstallerFrame $popup 'language-selector-expanded-list' }
            }
            [void][InstallerSelectorNative]::SendValue($found[1],0x014F,[IntPtr]::Zero,[IntPtr]::Zero)
        }
        # The list must retain exact application captions and canonical order on every launch.
        for ($i=0; $i -lt $languages.Count; $i++) {
            $item = $languages[$i]
            $expected = if ($item.name -eq $item.englishName) { "$($item.displayCode) - $($item.name)" } else { "$($item.displayCode) - $($item.name) / $($item.englishName) ($($item.code))" }
            $text = New-Object Text.StringBuilder 1024
            [void][InstallerSelectorNative]::SendMessage($found[1],0x0148,[IntPtr]$i,$text)
            if ($text.ToString() -cne $expected) { throw "Installer entry $i differs from application caption or order" }
        }
        [void][InstallerSelectorNative]::SendValue($found[1],0x014E,[IntPtr]$index,[IntPtr]::Zero)
        if ([InstallerSelectorNative]::SendValue($found[1],0x0147,[IntPtr]::Zero,[IntPtr]::Zero).ToInt32() -ne $index) { throw "Could not select $code" }
        $accept = [InstallerSelectorNative]::DefaultButton($found[0])
        if ($accept -eq [IntPtr]::Zero) { throw 'Selector OK button not found' }
        [void][InstallerSelectorNative]::PostMessage($accept,0x00F5,[IntPtr]::Zero,[IntPtr]::Zero)
        $line = Get-Content "installer/i18n/$code-standard.isl" -Encoding UTF8 | Where-Object { $_.StartsWith('WelcomeLabel1=') } | Select-Object -First 1
        $welcome = $line.Substring('WelcomeLabel1='.Length).Replace('[name]','ZagoSheetsWin').Replace('%1','ZagoSheetsWin')
        $deadline = [DateTime]::UtcNow.AddSeconds(30)
        $wizard = [IntPtr]::Zero
        do {
            $windows = [InstallerSelectorNative]::Windows([uint[]]@(New-InstallerIds $baseline))
            foreach ($window in $windows) {
                if ([InstallerSelectorNative]::Class($window).StartsWith('TWizardForm')) {
                    $texts = @([InstallerSelectorNative]::Children($window) | Where-Object { [InstallerSelectorNative]::IsWindowVisible($_) } | ForEach-Object { [InstallerSelectorNative]::Text($_).Trim() })
                    if ($welcome.Trim() -cin $texts) { $wizard = $window; break }
                }
            }
            if ($wizard -ne [IntPtr]::Zero) { break }
            Start-Sleep -Milliseconds 100
        } while ([DateTime]::UtcNow -lt $deadline)
        if ($wizard -eq [IntPtr]::Zero) {
            $details = @($windows | ForEach-Object { [InstallerSelectorNative]::Class($_) + ': ' + [InstallerSelectorNative]::Text($_); [InstallerSelectorNative]::Children($_) | ForEach-Object { [InstallerSelectorNative]::Text($_) } }) -join ' | '
            throw "Language $code did not reach its translated welcome page. Windows: $details"
        }
        if ($code -in @('en','pt','ru','ar')) { Save-InstallerFrame $wizard "welcome-$code" }
        if (!(Test-Path $log) -or !(Select-String -Path $log -SimpleMatch "Zago installer language: $code" -Quiet)) { throw "Installer did not activate requested language $code" }
        if (Test-Path (Join-Path $env:LOCALAPPDATA 'Programs/SheetsWindows/SheetsWindows.exe')) { throw "Installer started installing before welcome confirmation for $code" }
        $results += [PSCustomObject]@{ language=$code; caption=$welcome; selectorAccepted=$true; wizardVisible=$true; activeLanguageVerified=$true }
        $results | ConvertTo-Json -Depth 4 | Set-Content "$output/languages.json" -Encoding UTF8
        Write-Host "PASS $code`: selector -> translated installer welcome page"
    } finally { Stop-InstallerProcesses $baseline }
}
if ($results.Count -ne 51) { throw 'All 51 languages must pass' }
# Cancellation must still abort without installing.
$baseline = @(Get-Process | Select-Object -ExpandProperty Id)
$process = Start-Process $setup -PassThru
try {
    $found = Wait-Selector $baseline
    [void][InstallerSelectorNative]::PostMessage($found[0],0x0010,[IntPtr]::Zero,[IntPtr]::Zero)
    if (!$process.WaitForExit(30000)) { throw 'Cancelling selector did not close installer' }
    if (Test-Path (Join-Path $env:LOCALAPPDATA 'Programs/SheetsWindows/SheetsWindows.exe')) { throw 'Cancelling selector installed application' }
} finally { Stop-InstallerProcesses $baseline }
