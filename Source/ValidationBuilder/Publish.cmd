@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% ValidationBuilder 7.0.0
cd ValidationBuilder
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

