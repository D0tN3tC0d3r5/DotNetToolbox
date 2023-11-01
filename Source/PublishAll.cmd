@echo off

if [%1]==[] goto USAGE
set target=%1

call Publish %target% DotNetToolbox.Core 8.0.0-rc1
call Publish %target% DotNetToolbox.Azure 8.0.0-rc1
call Publish %target% DotNetToolbox.Http 8.0.0-rc1
call Publish %target% DotNetToolbox.Security 8.0.0-rc1
call Publish %target% DotNetToolbox.ValidationBuilder 8.0.0-rc1
goto :eof

:USAGE
echo Usage:
echo PublishAll ^<local^|remote^>
echo;
