Write-Host "Starting appveyor.ps1..."

Write-Host "Configuring powershell environment..."
$ErrorActionPreference = "Stop"

try {  
    Write-Host "todo: Successfully completed appveyor.ps1."
    Restore-Dependencies
}
catch {
    Write-Error $_.Exception
}

function Restore-Dependencies() {
    .where nuget
}