# Adds application settings to Azure.
#
# This script needs to be run before first publishing of the website to Azure or
# if any of the settings change.

param(
    [CmdletBinding(SupportsShouldProcess=$true)]
         
    [Parameter(Mandatory = $true)] 
    [string] $ContentRepositoryUserName,
    [Parameter(Mandatory = $true)] 
    [string] $ContentRepositoryPassword,
    [Parameter(Mandatory = $true)] 
    [string] $OAuthGoogleClientID,
    [Parameter(Mandatory = $true)] 
    [string] $OAuthGoogleClientSecret,
    [Parameter(Mandatory = $false)] 
    [string] $WebSiteName = "croquet-australia",
    [Parameter(Mandatory = $false)] 
    [string] $ContentRepositoryUrl = "https://github.com/croquet-australia/website-content.git",
    [Parameter(Mandatory = $false)] 
    [string] $ContentRepositoryFullDirectoryPath = "~/App_Data/Content/Git",
    [Parameter(Mandatory = $false)] 
    [string] $ContentPublishedRepositoryFullDirectoryPath = "~/App_Data/Content/Published"
)

$appSettings = @{ ` 
    "Content:Repository:Url" = $ContentRepositoryUrl; `
    "Content:Repository:FullDirectoryPath" = $ContentRepositoryFullDirectoryPath; `
    "Content:Repository:UserName" = $ContentRepositoryUserName; `
    "Content:Repository:Password" = $ContentRepositoryPassword; `
    "Content:PublishedRepository:FullDirectoryPath" = $ContentPublishedRepositoryFullDirectoryPath; `
    "OAuth:Google:ClientId" = $OAuthGoogleClientId; `
    "OAuth:Google:ClientSecret" = $OAuthGoogleClientSecret;
}

Set-AzureWebsite -Name "$WebSiteName" -AppSettings $appSettings 