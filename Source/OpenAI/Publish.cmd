@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% OpenAI 8.0.2-rc2
cd OpenAI
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

