@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% CommandLineBuilder 7.0.3
cd CommandLineBuilder
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

