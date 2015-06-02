@echo off

rem Show current git branch.
git status

rem Confirm continue with rebase.
echo.
choice Are you sure you want to rebase master into the current branch?
if "%errorlevel%" neq "1" goto exit

rem Changes to master branch
git checkout master
if "%errorlevel%" neq "0" goto error

rem Update master branch with changes on origin
git pull
if "%errorlevel%" neq "0" goto error

rem Change back to the feature branch
git checkout -
if "%errorlevel%" neq "0" goto error

rem Rebase master into feature branch
git rebase master
if "%errorlevel%" neq "0" goto error

goto exit

:error
echo.
echo.
echo ********************************************
echo * ERROR '%ERRORLEVEL%' WAS THROWN.
echo ********************************************

:exit
echo.
echo.
pause