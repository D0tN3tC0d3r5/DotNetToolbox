@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% Azure 8.0.5-rc1
cd Azure
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

