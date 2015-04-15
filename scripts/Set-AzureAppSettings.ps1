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
    [Parameter(Mandatory = $true)] 
    [string] $ElmahLogId,
    [Parameter(Mandatory = $false)] 
    [string] $WebSiteName = "croquet-australia",
    [Parameter(Mandatory = $false)] 
    [string] $ContentRepositoryUrl = "https://github.com/croquet-australia/website-content.git",
    [Parameter(Mandatory = $false)] 
    [string] $ContentRepositoryFullDirectoryPath = "~/App_Data/Content/Git",
    [Parameter(Mandatory = $false)] 
    [string] $ContentPublishedRepositoryFullDirectoryPath = "~/App_Data/Content/Published",
    [Parameter(Mandatory = $false)] 
    [string] $ContentBlogDirectoryName = "news",
    [Parameter(Mandatory = $false)] 
    [string] $ElmahErrorLogType = "Elmah.Io"
)

$appSettings = @{ ` 
    "Content:BlogDirectoryName" = $ContentBlogDirectoryName; `
    "Content:Repository:Url" = $ContentRepositoryUrl; `
    "Content:Repository:FullDirectoryPath" = $ContentRepositoryFullDirectoryPath; `
    "Content:Repository:UserName" = $ContentRepositoryUserName; `
    "Content:Repository:Password" = $ContentRepositoryPassword; `
    "Content:PublishedRepository:FullDirectoryPath" = $ContentPublishedRepositoryFullDirectoryPath; `
    "Elmah:ErrorLogType" = $ElmahErrorLogType; `
    "Elmah:LogId" = $ElmahLogId; `
    "OAuth:Google:ClientId" = $OAuthGoogleClientId; `
    "OAuth:Google:ClientSecret" = $OAuthGoogleClientSecret;
}

Set-AzureWebsite -Name "$WebSiteName" -AppSettings $appSettings 