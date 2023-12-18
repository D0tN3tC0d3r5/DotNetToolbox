@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% ObjectDumper 8.0.1
cd ObjectDumper
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

