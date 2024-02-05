@echo off

if [%1]==[] goto USAGE
if [%2]==[] goto USAGE
if [%3]==[] goto USAGE
set target=%1
set project=%2
set version=%3
set packageName=%4
if [%packageName%]==[] set packageName=%project%

setlocal EnableDelayedExpansion
@echo [92m%project% v%version%[0m
@echo Starting... @[96m%date% %time%[0m

cd %project%

if [!target!]==[local] (
	set task=build
	@echo [96mBuilding and packing project...[0m
	dotnet build -c Debug --nologo
	if not [!errorlevel!]==[0] goto :ERROR

	set task=create
	dotnet pack -c Debug --no-build --nologo
	if not [!errorlevel!]==[0] goto :ERROR

	@echo [96mRemove existing packages locally...[0m
	set task=delete
	rmdir /Q /S %USERPROFILE%\.nuget\packages\DotNetToolbox.%project%\%version%
	nuget delete DotNetToolbox.%project% %version% -source c:\nuget\packages -noninteractive

	@echo [96mPublish package locally...[0m
	set task=publish
	nuget push pkgs\Debug\DotNetToolbox.%packageName%.%version%.nupkg -source c:\nuget\packages
	if not [!errorlevel!]==[0] goto :ERROR
)
if [!target!]==[remote] (
	@echo [96mBuilding and packing project...[0m
	set task=build
	dotnet build -c Release
	if not [!errorlevel!]==[0] goto :ERROR

	set task=create
	dotnet pack -c Release --no-build
	if not [!errorlevel!]==[0] goto :ERROR

	@echo [96mPublish package remotely...[0m
	set task=publish
	nuget push pkgs\Release\DotNetToolbox.%packageName%.%version%.nupkg %NuGetApiKey% -Source https://api.nuget.org/v3/index.json
	if not [!errorlevel!]==[0] goto :ERROR
)

@echo [92mDone...[0m
goto :END

:ERROR
@echo [91mFailed to !task! package %project% %version%...[0m
goto :END

:USAGE
echo Usage:
echo Publish ^<local^|remote^> ^<project^> ^<version^>
echo;

:END
cd ..
