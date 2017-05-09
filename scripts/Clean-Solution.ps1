Function Main()
{
    Push-Location $PSScriptRoot\..

    Write-Host "Cleaning $(Get-Location)..."

    try
    {
        Delete-Folder .\.nuget
        Delete-Folder .\packages
        Delete-Folder .\source\CroquetAustraliaWebsite.Application\bower_packages
        Delete-Folder .\source\CroquetAustraliaWebsite.Application\node_modules
        Delete-Folders bin
        Delete-Folders obj
    }
    finally
    {
        Pop-Location
    }
}

Function Delete-Folders([string] $folderName)
{
    Write-Host "Deleting $folderName folders..."
    Get-ChildItem -Directory -Recurse |
        Where-Object { $_.Name -eq $folderName } |
        ForEach-Object { Remove-Item $_.FullName -Recurse -Force }
}

Function Delete-Folder([string] $folder)
{
    if (Test-Path $folder)
    {
        Write-Host "Deleting $folder..."
        &rimraf $folder
    }
}

$ErrorActionPreference = "Stop"
$WarningPreference = "Continue"
$VerbosePreference = "SilentlyContinue"

Main