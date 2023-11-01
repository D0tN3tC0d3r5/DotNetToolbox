@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% DotNetToolbox.Security 8.0.0-rc1
cd DotNetToolbox.Security
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

