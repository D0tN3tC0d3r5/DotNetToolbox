@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% OpenAI 8.0.4-rc1
cd OpenAI
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

