@echo off

if [%1]==[] goto USAGE
set target=%1

call Publish %target% Core 7.0.1
call Publish %target% Azure 7.0.1
call Publish %target% Http 7.0.1
call Publish %target% Security 7.0.1
call Publish %target% ValidationBuilder 7.0.1
call Publish %target% CommandLineBuilder 7.0.3
call Publish %target% TestUtilities 7.0.1
goto :eof

:USAGE
echo Usage:
echo PublishAll ^<local^|remote^>
echo;
