@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..\..
call Publish %target% AI\Core AI.Core 8.0.5-rc1
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;
