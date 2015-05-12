$framework = "4.5.1"

Properties {
    # Following properties are solution specific
    $solutionName = "CroquetAustraliaWebsite"
    $mainProject = "CroquetAustraliaWebsite.Application"

    # Following properties are generic
    $solutionFolder = (Resolve-Path ..\).ToString()
    $solutionFile = "$solutionFolder\$solutionName.sln"
    $specFlowVersion = "1.9.0"
    $configuration = "Release"
    $packages = "$solutionFolder\packages"
    $nuGet = "$packages\NuGet.exe"
    $nuGetConfig = "$solutionFolder\NuGet.config"
    $specFlowToolsFolder = "$packages\SpecFlow.$specFlowVersion\tools"
    $specFlow = "$specFlowToolsFolder\specflow.exe"
    $tests = "$solutionFolder\tests"
    $xunit = "$packages\xunit.runners\tools\xunit.console.clr4.exe"
    $artifacts = "$solutionFolder\artifacts"
    $nuGetArtifacts = "$artifacts\NuGet"
    $binArtifacts = "$artifacts\bin"
}

Task default -depends Validate-Properties, Delete-Artifacts, Build

Task Validate-Properties {

    Assert (Test-Path $solutionFolder) "solutionFolder '$solutionFolder' does not exist."
    Assert (Test-Path $solutionFile) "sln '$solutionFile' does not exist."
    Assert (-not [string]::IsNullOrWhitespace($configuration)) "configuration is required."
    Assert (Test-Path $packages) "packages '$packages' does not exist."
    Assert (Test-Path $nuGet) "nuGet '$nuGet' does not exist."
    Assert (Test-Path $tests) "tests '$tests' does not exist."

    Write-Host "Successfully validated properties."
    Write-Host
}

Task Build -depends Compile {
}

Task Compile -depends Restore-NuGet-Packages, Create-SpecFlow-Tests {

    Compile-Solution $solutionFile $configuration
}

Task Create-Bin-Artifacts {
    
    Create-Bin-Artifacts -bin $solutionFolder\source\$mainProject\bin\$configuration -artifacts $binArtifacts
}

Task Create-SpecFlow-Config {
    
    Create-SpecFlow-Config -specFlowToolsFolder $specFlowToolsFolder
}

Task Create-SpecFlow-Tests -depends Create-SpecFlow-Config {

    Create-SpecFlow-Tests -specFlow $specFlow -testsFolder $tests
}

Task Delete-Artifacts  {

    If (Test-Path $artifacts)
    {
        Remove-Item -Path $artifacts -Recurse -Force
    }
    Else
    {
        Write-Host
    }
}

Task Restore-NuGet-Packages {

    Restore-NuGet-Packages -nuGet $nuGet -nuGetConfig $nuGetConfig -sln $solutionFile -packages $packages
}

FormatTaskName {
   param($taskName)

   Write-Host $taskName -ForegroundColor Yellow
   Write-Host "----------------------------------------------------------------------" -ForegroundColor Yellow
}
