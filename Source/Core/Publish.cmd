@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% Core 8.0.4
cd Core
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

