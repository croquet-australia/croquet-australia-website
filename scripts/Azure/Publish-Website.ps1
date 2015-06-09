$ErrorActionPreference = "Stop"
$VerbosePreference = "Continue"

$websiteName = "publishwebsite"
$projectFile = $(Resolve-Path "..\..\source\CroquetAustraliaWebsite.Application\CroquetAustraliaWebsite.Application.csproj").ToString()
$configuration = "Release"

Write-Host "Publishing project file '$projectFile' to website '$websiteName'..."
Publish-AzureWebsiteProject -Name $websiteName -ProjectFile $projectFile -Configuration $configuration


Write-Host "Successfully published project file '$projectFile' to website '$websiteName'..."
