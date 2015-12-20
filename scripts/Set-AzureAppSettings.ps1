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
    [string] $ElmahErrorLogType = "MemoryErrorLog", # todo: add logging
    [Parameter(Mandatory = $false)] 
    [string] $SupportEmail = "tim@26tp.com",
    [Parameter(Mandatory = $false)] 
    [string] $SupportName = "Tim Murphy",
    [Parameter(Mandatory = $false)]
    [string] $WebApiBaseUri = "https://croquet-australia-api.azurewebsites.net/"
)

$appSettings = @{ ` 
    "Content:BlogDirectoryName" = $ContentBlogDirectoryName; `
    "Content:Repository:Url" = $ContentRepositoryUrl; `
    "Content:Repository:FullDirectoryPath" = $ContentRepositoryFullDirectoryPath; `
    "Content:Repository:UserName" = $ContentRepositoryUserName; `
    "Content:Repository:Password" = $ContentRepositoryPassword; `
    "Content:PublishedRepository:FullDirectoryPath" = $ContentPublishedRepositoryFullDirectoryPath; `
    "Elmah:ErrorLogType" = $ElmahErrorLogType; `
    "OAuth:Google:ClientId" = $OAuthGoogleClientId; `
    "OAuth:Google:ClientSecret" = $OAuthGoogleClientSecret; `
    "Support:Email" = $SupportEmail; `
    "Support:Name" = $SupportName; `
    "WebApi:BaseUri" = $WebApiBaseUri;
}

Set-AzureWebsite -Name "$WebSiteName" -AppSettings $appSettings 