@echo off
powershell -noprofile -ExecutionPolicy Unrestricted -file "%~dp0\Clean-Solution.ps1"

:exit
echo.
echo.
pause
