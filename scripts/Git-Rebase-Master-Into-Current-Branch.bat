@echo off

rem Show current git branch.
git status

rem Confirm continue with rebase.
echo.
choice /m "Are you sure you want to rebase master into the current branch?"
if "%errorlevel%" neq "1" goto exit

echo.
echo Checking out master branch...
echo.
git checkout master
if not "%errorlevel%" == "0" goto error

echo.
echo Pulling master origin changes...
echo.
git pull
if not "%errorlevel%" == "0" goto error

echo.
echo Changing back to the feature branch...
echo.
git checkout -
if not "%errorlevel%" == "0" goto error

echo.
echo Rebasing master into feature branch...
echo.
git rebase master
if not "%errorlevel%" == "0" goto error

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