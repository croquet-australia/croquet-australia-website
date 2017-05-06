Write-Host "Starting appveyor.ps1..."

Write-Host "Configuring powershell environment..."
$ErrorActionPreference = "Stop"

try {  
    Main
    Write-Host "todo: Successfully completed appveyor.ps1."
}
catch {
    Write-Error $_.Exception
}

function Main() {
    Restore-Dependencies
}

function Restore-Dependencies() {
    .where nuget
}