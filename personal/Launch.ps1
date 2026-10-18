param(
    [string]$SaveDirectory,
    [Parameter(ValueFromRemainingArguments = $true)][string[]]$GameArguments
)
$ErrorActionPreference = 'Stop'
if (-not $SaveDirectory) { $SaveDirectory = Join-Path $PSScriptRoot 'UserData' }
$SaveDirectory = [System.IO.Path]::GetFullPath($SaveDirectory)
New-Item -ItemType Directory -Force -Path $SaveDirectory | Out-Null
$env:DOTNET_ROOT = Join-Path $PSScriptRoot 'dotnet'
$env:DOTNET_ROOT_X64 = $env:DOTNET_ROOT
$env:DOTNET_MULTILEVEL_LOOKUP = '0'
$env:SteamAppId = '1281930'
$env:SteamGameId = '1281930'
Set-Location $PSScriptRoot
& (Join-Path $env:DOTNET_ROOT 'dotnet.exe') (Join-Path $PSScriptRoot 'tModLoader.dll') -steam -tmlsavedirectory $SaveDirectory @GameArguments
exit $LASTEXITCODE
