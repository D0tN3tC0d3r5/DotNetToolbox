@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% FluentAssertions 8.0.3
cd FluentAssertions
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

