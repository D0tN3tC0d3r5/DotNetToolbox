@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% Console 8.0.3-rc1
cd ConsoleApplication
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

