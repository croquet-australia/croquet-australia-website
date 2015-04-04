@echo off
pushd "%~dp0\..\"

choice -m "Are you sure you want to merge 'develop' branch in 'master' branch as a single commit?"

if "%errorlevel%" neq "1" goto Exit

git checkout master
if "%errorlevel%" neq "0" goto Error

git merge --squash develop
if "%errorlevel%" neq "0" goto Error

git commit
if "%errorlevel%" neq "0" goto Error

:Error
echo.
echo Error '%errorlevel%'.

:Exit
popd
echo.
echo.
git status
echo.
echo.
pause