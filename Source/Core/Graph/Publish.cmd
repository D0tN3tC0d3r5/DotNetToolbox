@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..\..
call Publish %target% Core\Graph Graph 9.0.1
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

