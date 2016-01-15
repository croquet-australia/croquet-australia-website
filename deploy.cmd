@if "%SCM_TRACE_LEVEL%" NEQ "4" @echo off

call :ShowEnvironment Environment at start of batch file.

:: Prerequisites
:: -------------

:: Verify node.js installed

where node 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
    echo.
    echo Missing node.js executable, please install node.js, if already installed make sure it can be reached from current environment.
    echo.
    goto error
)

:: Verify nuget.exe installed

IF DEFINED NUGET_CMD (
    goto NUGET_CMD_DEFINED
)

echo.
echo NUGET_CMD is undefined.
echo Searching known locations for nuget.exe
echo ---------------------------------------

:: Test if nuget.exe is in path
where nuget /q
    
IF "%ERRORLEVEL%" == "0" (
    echo Found nuget.exe in path.
    SET NUGET_CMD=nuget.exe
    GOTO NUGET_CMD_DEFINED
)
    
:: Test is nuget.exe is in AppData
IF EXIST "%AppData%\NuGet\nuget.exe" (
    echo Found nuget.exe in AppData. 
    SET NUGET_CMD="%AppData%\NuGet\nuget.exe"
    GOTO NUGET_CMD_DEFINED    
)    

echo.
echo Missing nuget.exe, please install nuget.exe, if already installed make sure it can be reached from current environment.
echo -----------------------------------------------------------------------------------------------------------------------
echo.
goto error

:NUGET_CMD_DEFINED
 
:: Setup
:: -----

setlocal enabledelayedexpansion

SET ARTIFACTS=%~dp0%..\artifacts

IF NOT DEFINED DEPLOYMENT_SOURCE (
  SET DEPLOYMENT_SOURCE=%~dp0%.
)

IF NOT DEFINED DEPLOYMENT_TARGET (
  SET DEPLOYMENT_TARGET=%ARTIFACTS%\wwwroot
)

IF NOT DEFINED NEXT_MANIFEST_PATH (
  SET NEXT_MANIFEST_PATH=%ARTIFACTS%\manifest

  IF NOT DEFINED PREVIOUS_MANIFEST_PATH (
    SET PREVIOUS_MANIFEST_PATH=%ARTIFACTS%\manifest
  )
)

IF NOT DEFINED KUDU_IGNORE_PATHS (
    SET KUDU_IGNORE_PATHS=.git;.hg;.deployment;deploy.cmd
)

IF NOT DEFINED KUDU_SYNC_CMD (

    IF NOT EXIST %appdata%\npm\kuduSync.cmd (
        echo.
        echo Installing Kudu Sync
        echo --------------------
        call npm install kudusync -g --silent
        IF !ERRORLEVEL! NEQ 0 goto error
    )
    
    SET KUDU_SYNC_CMD=%appdata%\npm\kuduSync.cmd
)
    
IF NOT DEFINED DEPLOYMENT_TEMP (
  SET DEPLOYMENT_TEMP=%temp%\___deployTemp%random%
  SET CLEAN_LOCAL_DEPLOYMENT_TEMP=true
)

IF DEFINED CLEAN_LOCAL_DEPLOYMENT_TEMP (
    IF EXIST "%DEPLOYMENT_TEMP%" (
        echo.
        echo Removing DEPLOYMENT_TEMP directory '%DEPLOYMENT_TEMP%'
        echo ----------------------------------------------------------------------------------
        rd /s /q "%DEPLOYMENT_TEMP%"
    )
    echo.
    echo Creating DEPLOYMENT_TEMP directory '%DEPLOYMENT_TEMP%'
    echo ---------------------------------------------------------------------------------------
    mkdir "%DEPLOYMENT_TEMP%"
)

echo.
echo Testing if tsd exists
echo ---------------------
where tsd /q
IF "%ERRORLEVEL%" == "0" (
    echo Found tsd.
) ELSE (
    echo.
    echo Installing tsd
    echo --------------
    call :ExecuteCmd npm install tsd --global --no-optional
    IF !ERRORLEVEL! NEQ 0 goto error  
)

:: Explicitly override MSBUILD_PATH to use C# 6.0 compatible compiler.
SET MSBUILD_PATH=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe

call :ShowEnvironment Environment after setup.

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------

echo.
echo Handling .NET Web Application deployment
echo ========================================

echo.
echo Running npm install
echo -------------------
pushd "%DEPLOYMENT_SOURCE%\source\CroquetAustralia.Website"
call :ExecuteCmd npm install --no-optional
IF !ERRORLEVEL! NEQ 0 goto error
popd

IF /I "croquet-australia.com.au.sln" NEQ "" (
    echo.
    echo Restoring NuGet packages
    echo ------------------------
    call :ExecuteCmd %NUGET_CMD% restore "%DEPLOYMENT_SOURCE%\croquet-australia.com.au.sln"
    IF !ERRORLEVEL! NEQ 0 goto error
)

IF /I "%IN_PLACE_DEPLOYMENT%" NEQ "1" (
    
    echo.
    echo Compiling the solution
    echo ----------------------
    call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\source\CroquetAustralia.Website\CroquetAustralia.Website.csproj" /nologo /verbosity:m /t:Build /t:pipelinePreDeployCopyAllFilesToOneFolder /p:_PackageTempDir="%DEPLOYMENT_TEMP%";AutoParameterizationWebConfigConnectionStrings=false;Configuration=Release /p:SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%
    IF !ERRORLEVEL! NEQ 0 goto error

    echo.
    echo Running 'gulp before-kudusync'
    echo ------------------------------
    pushd %DEPLOYMENT_SOURCE%\source\CroquetAustralia.Website
    IF !ERRORLEVEL! NEQ 0 goto error
    call :ExecuteCmd call gulp before-kudusync
    SET GULP_ERRORLEVEL=%ERRORLEVEL%
    popd
    IF "%GULP_ERRORLEVEL%" NEQ "" (
        IF "%GULP_ERRORLEVEL%" NEQ "0" (
            goto error
        )
    )
          
    echo.
    echo Deploying website with KuduSync
    echo -------------------------------
    call :ExecuteCmd "%KUDU_SYNC_CMD%" -v 50 -f "%DEPLOYMENT_TEMP%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i "%KUDU_IGNORE_PATHS%"
    IF !ERRORLEVEL! NEQ 0 goto error
    
) ELSE (
    
    echo IN_PLACE_DEPLOYMENT is not supported by this batch file because I can't imagine when you would want to!
    goto error
    rem call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\source\CroquetAustralia.Website\CroquetAustralia.Website.csproj" /nologo /verbosity:m /t:Build /p:AutoParameterizationWebConfigConnectionStrings=false;Configuration=Release /p:SolutionDir="%DEPLOYMENT_SOURCE%\.\\" %SCM_BUILD_ARGS%
)

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

:: Post deployment stub
IF DEFINED POST_DEPLOYMENT_ACTION call "%POST_DEPLOYMENT_ACTION%"
IF !ERRORLEVEL! NEQ 0 goto error

goto end

:ShowEnvironment
echo.
echo ----------------------------------------------------------
echo %*
echo.
echo BATCH FILE DIRECTORY: %~dp0%
echo CD: %CD%
echo.
echo ARTIFACTS: %ARTIFACTS%
echo CLEAN_LOCAL_DEPLOYMENT_TEMP: %CLEAN_LOCAL_DEPLOYMENT_TEMP% 
echo NUGET_CMD: %NUGET_CMD%
echo DEPLOYMENT_SOURCE: %DEPLOYMENT_SOURCE%
echo DEPLOYMENT_TARGET: %DEPLOYMENT_TARGET%
echo DEPLOYMENT_TEMP: %DEPLOYMENT_TEMP%
echo IN_PLACE_DEPLOYMENT: %IN_PLACE_DEPLOYMENT%
echo KUDU_IGNORE_PATHS: %KUDU_IGNORE_PATHS%
echo KUDU_SYNC_CMD: %KUDU_SYNC_CMD%
echo MSBUILD_PATH: %MSBUILD_PATH% 
echo NEXT_MANIFEST_PATH: %NEXT_MANIFEST_PATH%
echo POST_DEPLOYMENT_ACTION: %POST_DEPLOYMENT_ACTION% 
echo PREVIOUS_MANIFEST_PATH: %PREVIOUS_MANIFEST_PATH%
echo ----------------------------------------------------------
echo.
exit /b 0

:: Execute command routine that will echo out when error
:ExecuteCmd
setlocal
set _CMD_=%*
call %_CMD_%
if "%ERRORLEVEL%" NEQ "0" echo Failed exitCode=%ERRORLEVEL%, command=%_CMD_%
exit /b %ERRORLEVEL%

:error
endlocal
echo An error has occurred during web site deployment.
call :exitSetErrorLevel
call :exitFromFunction 2>nul

:exitSetErrorLevel
exit /b 1

:exitFromFunction
()

:end
endlocal
echo Finished successfully.
