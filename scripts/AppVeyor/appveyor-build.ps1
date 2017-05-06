Write-Host "Starting appveyor-build.ps1..."

Write-Host "Configuring powershell environment..."
$ErrorActionPreference = "Stop"

function Build() {
    Restore-Dependencies
}

function Restore-Dependencies() {
    Write-Host "Restoring dependencies..."
    nuget
}

try {  
    Build
    Write-Host "todo: Successfully completed appveyor-build.ps1."
}
catch {
    Write-Error $_.Exception
}

