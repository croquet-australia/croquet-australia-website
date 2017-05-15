function Build() {
    Restore-Dependencies
    Compile-Solution
}

function Compile-Solution() {
    Run-Command {
        & msbuild
    }
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

function Restore-Dependencies() {
    Write-Host "Restoring dependencies..."

    Install-NuGet
    Restore-NuGetPackages
    Restore-NpmPackages
}

function Restore-NuGetPackages {
    Write-Host "Restoring NuGet packages..."
    Run-Command { & $($settings.NuGetPath) restore }
}

function Restore-NpmPackages {
    Write-Host "Restoring NPM packages..."
    
    Push-Location $settings.WebSiteProjectFolder
    Run-Command { 
        Write-Host "node version: "
        & node --version
        Write-Host "npm version: "
        & npm --version
        Write-Host "Installing npm@3..."
        & npm -g install npm@3
        Write-Host "Installing packages..."
        & npm install --no-optional
    }
    Pop-Location
}

function Run-Command([scriptblock] $command) {
    & $command
    if ($lastexitcode -ne 0) {
        throw "An error occurred while running '$command'. `$lastexitcode: $lastexitcode"
    }
}

Write-Host "Starting appveyor-build.ps1..."

Write-Host "Configuring powershell environment..."

$settings = @{}
$settings.SolutionFolder = Resolve-Path $PSScriptRoot\..\..
$settings.NuGetVersion = "v4.1.0"
$settings.NuGetUrl = "https://dist.nuget.org/win-x86-commandline/$($settings.NuGetVersion)/nuget.exe"
$settings.NuGetFolder = "$($settings.SolutionFolder)\.nuget\$($settings.NuGetVersion)"
$settings.NuGetPath = "$($settings.NuGetFolder)\nuget.exe"
$settings.WebSiteProjectFolder = "$($settings.SolutionFolder)\source\CroquetAustralia.Website"

Write-Host "Changing to solution's root folder '$($settings.SolutionFolder)'..."
Push-Location $settings.SolutionFolder

try {  
    Build
    Write-Host "todo: Successfully completed appveyor-build.ps1."
}
catch {
    Write-Error "`n`nappveyor-build.ps1 failed!!!`n`n$($_.Exception)`n`n"
    throw 
}
finally {
    Pop-Location
}
