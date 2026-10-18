<#
.SYNOPSIS
Copy subscribed Steam Workshop mods (tModLoader app 1281930) and the in-repo
MIT packages into a tModLoader Mods directory.

.NOTES
This script does not download anything. It only reads the local Steam Workshop
cache. Workshop content remains subject to each author's license; see
catalog.json and workshop-links.md before redistributing anything.
#>
[CmdletBinding()]
param(
    [string]$Destination = $null,
    [string]$WorkshopRoot = $null,
    [switch]$ListOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

if (-not $Destination) {
    $Destination = Join-Path $PSScriptRoot 'downloads'
}

function Find-WorkshopRoot {
    $steamDirs = [System.Collections.Generic.List[string]]::new()
    if ($env:SteamPath) { $steamDirs.Add($env:SteamPath) }
    try {
        $steam = (Get-ItemProperty -Path 'HKCU:\Software\Valve\Steam' -Name SteamPath -ErrorAction Stop).SteamPath
        if ($steam) { $steamDirs.Add($steam) }
    }
    catch { }
    $programFilesX86 = ${env:ProgramFiles(x86)}
    if ($programFilesX86) { $steamDirs.Add((Join-Path $programFilesX86 'Steam')) }
    if ($env:ProgramFiles) { $steamDirs.Add((Join-Path $env:ProgramFiles 'Steam')) }

    foreach ($steamDir in $steamDirs) {
        $vdf = Join-Path $steamDir 'steamapps\libraryfolders.vdf'
        if (Test-Path -LiteralPath $vdf) {
            $text = Get-Content -LiteralPath $vdf -Raw
            foreach ($match in [regex]::Matches($text, '"path"\s+"([^"]+)"')) {
                $library = $match.Groups[1].Value.Replace('\\', '\')
                $root = Join-Path $library 'steamapps\workshop\content\1281930'
                if (Test-Path -LiteralPath $root) { return $root }
            }
        }
        $root = Join-Path $steamDir 'steamapps\workshop\content\1281930'
        if (Test-Path -LiteralPath $root) { return $root }
    }
    return $null
}

if (-not $WorkshopRoot) {
    $WorkshopRoot = Find-WorkshopRoot
}
if (-not $WorkshopRoot) {
    $programFilesX86 = ${env:ProgramFiles(x86)}
    if (-not $programFilesX86) { $programFilesX86 = $env:ProgramFiles }
    $WorkshopRoot = Join-Path $programFilesX86 'Steam\steamapps\workshop\content\1281930'
    Write-Warning "Steam Workshop root not found; pass -WorkshopRoot <path> if it is elsewhere."
}
$workshopAvailable = Test-Path -LiteralPath $WorkshopRoot
if ($workshopAvailable) {
    Write-Host "Workshop: $WorkshopRoot"
}
else {
    Write-Warning "Workshop root does not exist: $WorkshopRoot"
}
Write-Host 'Workshop copies use the newest subscribed build; localPortVersion in catalog.json is what this branch tested.'
Write-Host ''

$catalog = Get-Content -LiteralPath (Join-Path $PSScriptRoot 'catalog.json') -Raw -Encoding UTF8 |
    ConvertFrom-Json

if (-not $ListOnly) {
    New-Item -ItemType Directory -Force -Path $Destination | Out-Null
}

$copied = 0
$missing = [System.Collections.Generic.List[string]]::new()

foreach ($mod in $catalog.mods) {
    $label = "$($mod.name) $($mod.localPortVersion)"

    if ($mod.package) {
        $source = Join-Path $PSScriptRoot $mod.package
        if (-not (Test-Path -LiteralPath $source)) {
            $missing.Add("$label (repo package missing: $($mod.package))")
            continue
        }
        if ($ListOnly) {
            Write-Host "[repo]     $label -> $(Split-Path -Leaf $source)"
        }
        else {
            Copy-Item -LiteralPath $source -Destination $Destination -Force
            Write-Host "[copy]     $label (repo package)"
        }
        $copied++
        continue
    }

    if (-not $mod.workshopId) {
        $where = if ($mod.homepage) { $mod.homepage } else { 'no public link' }
        Write-Host "[link]     $label -> $where"
        continue
    }

    $dir = Join-Path $WorkshopRoot ([string]$mod.workshopId)
    if (-not (Test-Path -LiteralPath $dir)) {
        $missing.Add("$label (workshop $($mod.workshopId) not subscribed)")
        continue
    }

    $tmod = Get-ChildItem -LiteralPath $dir -Recurse -File -Filter '*.tmod' |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1
    if (-not $tmod) {
        $missing.Add("$label (workshop $($mod.workshopId) has no .tmod)")
        continue
    }

    if ($ListOnly) {
        Write-Host "[workshop] $label -> $($tmod.FullName)"
    }
    else {
        Copy-Item -LiteralPath $tmod.FullName -Destination $Destination -Force
        Write-Host "[copy]     $label (workshop $($mod.workshopId))"
    }
    $copied++
}

Write-Host ''
Write-Host "Copied: $copied; missing: $($missing.Count); destination: $Destination"
foreach ($entry in $missing) {
    Write-Host "  - $entry"
}
