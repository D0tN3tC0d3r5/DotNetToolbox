@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% DotNetToolbox.Azure 8.0.0-rc1
cd DotNetToolbox.Azure
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

