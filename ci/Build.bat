@ECHO off

@REM === 0. Read User Settings from ini file ===
setlocal
FOR /F "usebackq delims== tokens=1,2" %%i IN ("%~dp0.env") do SET %%i=%%j

@REM === 1. Build applicetion with unity ===
set "BUILD_NAME=%APP_NAME%.%APP_VERSION%.%ANDROID_BUNDLE_VERSION_CODE%"
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
echo + Build Target: %BUILD_TARGET%
echo + Build Path: %BUILD_PATH%
echo + Build Name: %BUILD_NAME%

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

@REM === 2. Copy build to ci/Builds ===
"%OVR_PLATFORM_TOOL%"^
 upload-quest-build^
 --app-id "%OVR_APP_ID%"^
 --app-secret "%OVR_APP_SECRET%"^
 --apk "%BUILD_PATH%%BUILD_NAME%.apk"^
 --channel "%OVR_RELEASE_CHANNEL%"^
 --notes "%OVR_RELEASE_NOTE%"

echo Completed!
endlocal
