@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% DotNetToolbox.TestUtilities 7.0.0
cd DotNetToolbox.TestUtilities
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;

