@echo off
pushd "%~dp0\.."

if exist .\deploy.cmd (
	echo Cannot run get-azure-deployment-script.bat because %cd%\deploy.cmd already exists.
	goto exit
)

if exist .\.deployment (
	echo Cannot run get-azure-deployment-script.bat because %cd%\.deployment already exists.
	goto exit
)

call azure site deploymentscript --aspWAP ".\source\CroquetAustraliaWebsite.Application\CroquetAustraliaWebsite.Application.csproj" --solutionFile ".\CroquetAustraliaWebsite.sln"
goto exit

:exit
popd
echo.
echo.
pause
