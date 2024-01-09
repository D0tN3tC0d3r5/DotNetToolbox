@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% ValidationBuilder 8.0.2-rc3
cd ValidationBuilder
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

