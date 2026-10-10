$ErrorActionPreference = 'Stop'
# Exercises the real installer dialog in the disposable Windows CI desktop.
Add-Type -AssemblyName System.Drawing
Add-Type -TypeDefinition @'
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
public static class InstallerSelectorNative {
  public delegate bool Callback(IntPtr hwnd, IntPtr data);
  [DllImport("user32.dll")] public static extern bool EnumWindows(Callback callback, IntPtr data);
  [DllImport("user32.dll")] public static extern bool EnumChildWindows(IntPtr hwnd, Callback callback, IntPtr data);
  [DllImport("user32.dll")] public static extern uint GetWindowThreadProcessId(IntPtr hwnd, out uint id);
  [DllImport("user32.dll", CharSet=CharSet.Unicode)] public static extern IntPtr SendMessage(IntPtr hwnd, int message, IntPtr index, StringBuilder text);
  [DllImport("user32.dll")] public static extern bool PostMessage(IntPtr hwnd, int message, IntPtr wparam, IntPtr lparam);
  [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr hwnd);
  public static IntPtr[] Find(uint[] ids) {
    IntPtr form=IntPtr.Zero, combo=IntPtr.Zero;
    EnumWindows((hwnd, data) => {
      uint id; GetWindowThreadProcessId(hwnd, out id);
      if (Array.IndexOf(ids,id) < 0 || !IsWindowVisible(hwnd)) return true;
      EnumChildWindows(hwnd, (child, state) => {
        if (SendMessage(child, 0x0146, IntPtr.Zero, null).ToInt64() == 51) { form=hwnd; combo=child; return false; }
        return true;
      }, IntPtr.Zero);
      return combo==IntPtr.Zero;
    }, IntPtr.Zero);
    return new[]{form,combo};
  }
}
'@
$baseline = @(Get-Process | Select-Object -ExpandProperty Id)
$setup = (Resolve-Path 'artifacts/installer/ZagoSheetsWin-Setup-win-x64.exe').Path
$process = Start-Process $setup -PassThru
try {
    $found = @([IntPtr]::Zero, [IntPtr]::Zero)
    $deadline = [DateTime]::UtcNow.AddSeconds(40)
    do {
        $newIds = [uint[]]@(Get-Process | Where-Object { $_.Id -notin $baseline } | Select-Object -ExpandProperty Id)
        $found = [InstallerSelectorNative]::Find($newIds)
        if ($found[1] -ne [IntPtr]::Zero) { break }
        Start-Sleep -Milliseconds 200
    } while ([DateTime]::UtcNow -lt $deadline)
    if ($found[1] -eq [IntPtr]::Zero) { throw 'Custom installer selector with all 51 entries did not appear' }
    $languages = (Get-Content 'i18n/locales.json' -Raw -Encoding UTF8 | ConvertFrom-Json).languages
    $actual = @()
    for ($i=0; $i -lt $languages.Count; $i++) {
        $language = $languages[$i]
        $expected = if ($language.name -eq $language.englishName) { "$($language.displayCode) - $($language.name)" } else { "$($language.displayCode) - $($language.name) / $($language.englishName) ($($language.code))" }
        $text = New-Object Text.StringBuilder 1024
        [void][InstallerSelectorNative]::SendMessage($found[1],0x0148,[IntPtr]$i,$text)
        if ($text.ToString() -cne $expected) { throw "Installer entry $i differs from application caption or order" }
        $actual += $text.ToString()
    }
    New-Item -ItemType Directory -Force 'artifacts/branding-preview' | Out-Null
    $actual | ConvertTo-Json | Set-Content 'artifacts/branding-preview/installer-languages.json' -Encoding UTF8
    [void][InstallerSelectorNative]::PostMessage($found[0],0x0010,[IntPtr]::Zero,[IntPtr]::Zero)
    if (!$process.WaitForExit(30000)) { throw 'Cancelling language selector did not close the installer' }
    if (Test-Path (Join-Path $env:LOCALAPPDATA 'Programs/SheetsWindows/SheetsWindows.exe')) { throw 'Cancelling selector installed the application' }
} finally {
    if (!$process.HasExited) { $process.Kill() }
}
