@echo off

if [%1]==[] goto USAGE
set target=%1

cd ..
call Publish %target% DotNetToolbox.Core.Abstractions 1.0.0
cd DotNetToolbox.Core.Abstractions
goto :eof

:USAGE
echo Usage:
echo Publish ^<local^|remote^>
echo;
