@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% Security 8.0.1
cd Security
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

