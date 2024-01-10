@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% Http 8.0.3-rc1
cd Http
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

