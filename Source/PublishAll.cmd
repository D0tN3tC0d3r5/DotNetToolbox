@echo off

if [%1]==[] goto USAGE
set target=%1

call Publish %target% Core 8.0.2
call Publish %target% ObjectDumper 8.0.1
call Publish %target% Azure 8.0.1
call Publish %target% Http 8.0.1
call Publish %target% Security 8.0.1
call Publish %target% ValidationBuilder 8.0.1
call Publish %target% CommandLineBuilder 8.0.1
call Publish %target% TestUtilities 8.0.1
goto :eof

:USAGE
echo Usage:
echo PublishAll ^<local^|remote^>
echo;
