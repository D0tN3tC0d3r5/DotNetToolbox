@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% ConsoleApplication 8.0.1
cd ConsoleApplication
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

