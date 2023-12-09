@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% TestUtilities 7.0.1
cd TestUtilities
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

