@echo off

if [%1]==[] goto USAGE
set target=%1

call Publish %target% Core\Core Core 8.1.1
call Publish %target% Core\Linq Linq 8.1.1
call Publish %target% Core\Graph Graph 8.1.1
call Publish %target% Data\Storage Data 8.1.1
call Publish %target% Utilities\ObjectDumper ObjectDumper 8.1.1
REM call Publish %target% Utilities\ValidationBuilder ValidationBuilder 8.1.1
call Publish %target% Testing\TestUtilities TestUtilities 8.1.1
call Publish %target% Testing\FluentAssertions FluentAssertions 8.1.1
REM call Publish %target% Cloud\Azure Azure 8.1.1
call Publish %target% Cloud\Http Http 8.1.1
REM call Publish %target% Utilities\Security Security 8.1.1
call Publish %target% Applications\Console Console 8.1.1
REM call Publish %target% AI\Core AI 8.1.1
REM call Publish %target% AI\Anthropic AI.Anthropic 8.1.1
REM call Publish %target% AI\OpenAI AI.OpenAI 8.1.1
goto :eof

:USAGE
echo Usage:
echo PublishAll ^<local^|remote^>
echo;
