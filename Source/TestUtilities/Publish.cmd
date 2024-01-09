@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% TestUtilities 8.0.2-rc3
cd TestUtilities
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

