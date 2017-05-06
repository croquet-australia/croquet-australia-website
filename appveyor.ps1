Write-Host "Starting appveyor.ps1..."

Write-Host "Configuring powershell environment..."
$ErrorActionPreference = "Stop"

try {  
throw x  
    Write-Host "Successfully completed appveyor.ps1."
}
catch {
    Write-Error $_.Exception
}
