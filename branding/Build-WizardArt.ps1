$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$root = $PSScriptRoot
$bitmap = New-Object Drawing.Bitmap 164,314
$graphics = [Drawing.Graphics]::FromImage($bitmap)
$logo = [Drawing.Image]::FromFile((Join-Path $root 'zonly.png'))
$brand = New-Object Drawing.Font 'Segoe UI',12,([Drawing.FontStyle]::Regular),([Drawing.GraphicsUnit]::Pixel)
$company = New-Object Drawing.Font 'Segoe UI',12,([Drawing.FontStyle]::Regular),([Drawing.GraphicsUnit]::Pixel)
$green = New-Object Drawing.SolidBrush ([Drawing.ColorTranslator]::FromHtml('#62d98b'))
$white = New-Object Drawing.SolidBrush ([Drawing.ColorTranslator]::FromHtml('#edf8f0'))
try {
    $graphics.Clear([Drawing.ColorTranslator]::FromHtml('#07100b'))
    $graphics.InterpolationMode = [Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $ratio = [Math]::Min(140.0/$logo.Width,140.0/$logo.Height)
    $width = [single]($logo.Width*$ratio); $height=[single]($logo.Height*$ratio)
    $graphics.DrawImage($logo,[single]((164-$width)/2),[single]24,$width,$height)
    $graphics.DrawString('ZagoSheetsWin',$brand,$green,[single]12,[single]174)
    $graphics.DrawString('Zagotools',$company,$white,[single]12,[single]198)
    $bitmap.Save((Join-Path $root 'wizard.bmp'),[Drawing.Imaging.ImageFormat]::Bmp)
    $output = Join-Path $root '../artifacts/installer-verification'
    New-Item -ItemType Directory -Force $output | Out-Null
    Copy-Item (Join-Path $root 'wizard.bmp') (Join-Path $output 'wizard.bmp') -Force
    $bitmap.Save((Join-Path $output 'wizard-art.png'),[Drawing.Imaging.ImageFormat]::Png)
} finally { $green.Dispose(); $white.Dispose(); $company.Dispose(); $brand.Dispose(); $logo.Dispose(); $graphics.Dispose(); $bitmap.Dispose() }
