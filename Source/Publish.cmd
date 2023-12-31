@echo off

if [%1]==[] goto USAGE
if [%2]==[] goto USAGE
if [%3]==[] goto USAGE
set target=%1
set project=%2
set version=%3
set packageName=%4
if [%packageName%]==[] set packageName=%project%

@echo [92m%project% package...[0m

cd %project%

if [%target%]==[local] (
	@echo [93mBuilding and packing project...[0m
	dotnet build -c Debug
	dotnet pack -c Debug --no-build

	@echo [93mRemove existing packages locally...[0m
	rmdir /Q /S %userprofile%\.nuget\packages\DotNetToolbox.%project%\%version%
	nuget delete DotNetToolbox.%project% %version% -source c:\nuget\packages -noninteractive

	@echo [93mPublish package locally...[0m
	nuget add pkgs\Debug\DotNetToolbox.%packageName%.%version%.nupkg -source c:\nuget\packages
)
if [%target%]==[remote] (
	@echo [93mBuilding and packing project...[0m
	dotnet build -c Release
	dotnet pack -c Release --no-build

	@echo [93mPublish package remotely...[0m
	nuget push pkgs\Release\DotNetToolbox.%packageName%.%version%.nupkg %NuGetApiKey% -Source https://api.nuget.org/v3/index.json
)

cd ..

@echo [92mDone...[0m
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^> ^<project^> ^<version^>
echo;
