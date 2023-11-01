@echo off

if [%1]==[] goto USAGE
set target=%1

call Publish %target% DotNetToolbox.Core 7.0.0
call Publish %target% DotNetToolbox.Azure 7.0.0
call Publish %target% DotNetToolbox.Http 7.0.0
call Publish %target% DotNetToolbox.Security 7.0.0
call Publish %target% DotNetToolbox.ValidationBuilder 7.0.0
goto :eof

:USAGE
echo Usage:
echo PublishAll ^<local^|remote^>
echo;
