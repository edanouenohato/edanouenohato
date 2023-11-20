@ECHO off

@REM === 0. User Settings ===
set "ANDROID_BUNDLE_VERSION_CODE="
set "UNITY_EDITOR_ROOT=%PROGRAMFILES%/Unity/Hub/Editor/"
set "UNITY_KEYSTORE_PASS=XXXXX"
set "UNITY_KEYALIAS_PASS=XXXXX"

@REM === 1. Build applicetion with unity ===
@REM === Settings ===
set "BUILD_NAME=branch"
set "BUILD_TARGET=Android"

set "PROJECT_PATH=%~dp0.."
set "BUILD_PATH=%PROJECT_PATH%/Builds/"

@REM === Read Unity Version from ProjectVersion.txt ===
set "PROJECT_VERSION_TXT_PATH=%PROJECT_PATH%/ProjectSettings/ProjectVersion.txt"
set /P FIRSTLINE=<"%PROJECT_VERSION_TXT_PATH%"
set "FIRSTLINE=%FIRSTLINE%"
for /F "tokens=1-2" %%A in ("%FIRSTLINE%") do ( set "UNITY_VERSION=%%B" )

@REM === Show Info ===
echo Start to Player Build ...
echo + Unity Editor root directory: %UNITY_EDITOR_ROOT%
echo + Unity Version: %UNITY_VERSION%
echo + Project Path: %PROJECT_PATH%
echo + Build Path: %BUILD_PATH%
echo + Build Target: %BUILD_TARGET%

@REM === Execute Player Build ===
set "UNITY=%UNITY_EDITOR_ROOT%%UNITY_VERSION%/Editor/Unity.exe"

"%UNITY%"^
 -quit -batchmode -nographics^
 -projectPath "%PROJECT_PATH%"^
 -logFile "%~dp0../Logs/%~n0.log"^
 -executeMethod BuildCommand.PerformBuild^
 -buildTarget %BUILD_TARGET%^
 -customBuildName %BUILD_NAME%^
 -customBuildPath "%BUILD_PATH%"^
 -customBuildTarget %BUILD_TARGET%

echo Completed!
pause
