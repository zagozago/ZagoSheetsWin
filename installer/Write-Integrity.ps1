$ErrorActionPreference = 'Stop'
[xml]$project = Get-Content (Join-Path $PSScriptRoot '../src/SheetsWindows.Windows/SheetsWindows.Windows.csproj') -Raw
$version = [string]$project.Project.PropertyGroup.Version
$root = Join-Path $PSScriptRoot '../artifacts/installer'
$installer = Join-Path $root 'ZagoSheetsWin-Setup-win-x64.exe'
$binary = Join-Path $PSScriptRoot '../artifacts/SheetsWindows-win-x64/SheetsWindows.exe'
$actual = [Diagnostics.FileVersionInfo]::GetVersionInfo($binary).FileVersion
if (([version]$actual).ToString(3) -ne $version) { throw 'Executable and project versions differ' }
$hash = (Get-FileHash -LiteralPath $installer -Algorithm SHA256).Hash.ToLowerInvariant()
[IO.File]::WriteAllText((Join-Path $root 'SHA256SUMS.txt'), "$hash  ZagoSheetsWin-Setup-win-x64.exe" + [Environment]::NewLine)
$signature = (Get-AuthenticodeSignature -LiteralPath $installer).Status.ToString()
$report = @{ version=$version; commit=$env:GITHUB_SHA; workflowRun=$env:GITHUB_RUN_ID; installerSha256=$hash; authenticodeStatus=$signature } | ConvertTo-Json
[IO.File]::WriteAllText((Join-Path $root 'build-info.json'), $report)
Write-Host "Version $version; Authenticode: $signature; installer SHA-256: $hash"
