@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..\..
call Publish %target% Graph\Core Graph 8.1.1-rc1
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

