@echo off

if [%1]==[] goto USAGE
set target=%1

call Publish %target% Core 8.0.4
call Publish %target% ObjectDumper 8.0.4
call Publish %target% TestUtilities 8.0.4
call Publish %target% FluentAssertions 8.0.4
call Publish %target% Console 8.0.4
call Publish %target% ValidationBuilder 8.0.4-rc1
call Publish %target% Azure 8.0.4-rc1
call Publish %target% Http 8.0.4-rc1
call Publish %target% Security 8.0.4-rc1
call Publish %target% OpenAI 8.0.4-rc1
goto :eof

:USAGE
echo Usage:
echo PublishAll ^<local^|remote^>
echo;
