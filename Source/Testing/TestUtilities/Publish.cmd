@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..\..
call Publish %target% Testing\TestUtilities TestUtilities 8.1.0
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

