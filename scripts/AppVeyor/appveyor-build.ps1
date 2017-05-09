function Build() {
    Restore-Dependencies
}

function Restore-Dependencies() {
    Write-Host "Restoring dependencies..."

    Install-NuGet
    Restore-NuGetPackages
    Restore-NpmPackages
}

function Install-NuGet() {
    Write-Host "Installing nuget.exe..."

    if (Test-Path $settings.NuGetPath) {
        Write-Host "NuGet already installed."
        return
    }

    if (!(Test-Path $settings.NuGetFolder)) {
        Write-Host "Creating NuGet folder '$($settings.NuGetFolder)'..."
        New-Item -Path $settings.NuGetFolder -ItemType Directory
    }

    $webClient = New-Object System.Net.WebClient
    $webClient.DownloadFile($settings.NuGetUrl, $settings.NuGetPath)
}

function Restore-NuGetPackages {
    Write-Host "Restoring NuGet packages..."
    Run-Command { & $($settings.NuGetPath) restore }
}

function Restore-NpmPackages {
    Write-Host "Restoring NPM packages..."

    Push-Location $settings.WebSiteProjectFolder
    Run-Command { 
        & npm --version
        & npm install --no-optional
    }
    Pop-Location
}

function Run-Command([scriptblock] $command) {
    & $command
    if ($lastexitcode -ne 0) {
        throw
    }
}

Write-Host "Starting appveyor-build.ps1..."

Write-Host "Configuring powershell environment..."

$ErrorActionPreference = "Stop"
$WarningPreference = "Continue"
$VerbosePreference = "SilentlyContinue"

$settings = @{}
$settings.SolutionFolder = Resolve-Path $PSScriptRoot\..\..
$settings.NuGetVersion = "v4.1.0"
$settings.NuGetUrl = "https://dist.nuget.org/win-x86-commandline/$($settings.NuGetVersion)/nuget.exe"
$settings.NuGetFolder = "$($settings.SolutionFolder)\.nuget\$($settings.NuGetVersion)"
$settings.NuGetPath = "$($settings.NuGetFolder)\nuget.exe"
$settings.WebSiteProjectFolder = "$($settings.SolutionFolder)\source\CroquetAustralia.Website"

Write-Host "Changing to solution's root folder '$solutionFolder'..."
Push-Location $solutionFolder


try {  
    Build
    Write-Host "todo: Successfully completed appveyor-build.ps1."
}
catch {
    Write-Error $_.Exception
}
finally {
    Pop-Location
}
