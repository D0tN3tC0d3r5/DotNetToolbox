namespace DotNetToolbox.CommandLineBuilder;

public class CommandExecutionTests {
    private readonly InMemoryOutput _writer = new();

    [Fact]
    public void Command_Execute_WithException_AndVerboseFlagAndNoColor_ShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer).SetAction((Action)(() => throw new("Some exception.")))
                       .Build();

        subject.Execute("-v", "2", "--no-color");

        _writer.Output.ToString().Should().StartWith("An error occurred while executing command 'testhost'.\r\n");
    }

    [Fact]
    public void Command_Execute_WithException_AndVerboseLevel_Detailed_ShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer).SetAction((Action)(() => throw new("Some exception.")))
                       .Build();

        subject.Execute("-v", "2");

        _writer.Output.ToString().Should().StartWith("An error occurred while executing command 'testhost'.\r\n");
    }

    [Fact]
    public void Command_Execute_WithException_AndVerboseLevel_Silent_ShowsNotShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer).SetAction((Action)(() => throw new("Some exception.")))
                       .Build();

        subject.Execute("-v", "5");

        _writer.Output.ToString().Should().BeEmpty();
    }

    [Fact]
    public void Command_Execute_WithException_AndVerboseLevel_Debug_ShowsErrorWithException() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer).SetAction((Action)(() => throw new("Some exception.")))
                       .Build();

        subject.Execute("-v", "1");

        _writer.Output.ToString().Should().Contain("An error occurred while executing command 'testhost'.\r\nSystem.Exception: Some exception.");
    }

    [Fact]
    public void Command_Execute_WithDelegate_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer).SetAction((Action)(() => throw new("Some exception.")))
                       .Build();

        subject.Execute("-v", "1");

        _writer.Output.ToString().Should().Contain("An error occurred while executing command 'testhost'.\r\nSystem.Exception: Some exception.");
    }

    [Fact]
    public void Command_Execute_WriteError_WithoutException_AndVerboseLevel_Silent_ShowsNoError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction(cmd => cmd.Writer.WriteError("Some error."))
                     .Build();

        subject.Execute("-v", "5");

        _writer.Output.ToString().Should().BeEmpty();
    }

    [Fact]
    public void Command_Execute_ExecutesDelegate() {
        var subject = new Command("Command", "Command description.") {
            Writer = _writer,
        };
        subject.SetAction(() => subject.Writer.WriteLine("Executing command..."));

        subject.Execute();

        _writer.Output.ToString().Should().Be("Executing command...\r\n");
    }

    [Fact]
    public void Command_Execute_WithTerminalOption_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction(r => r.Writer.WriteLine("You should not be here!"))
                     .AddFlag("option", onRead: c => c.Writer.WriteLine("Stop here!"), existsIfSet: true)
            .Build();

        subject.Execute("--option");

        _writer.Output.ToString().Should().Be("Stop here!\r\n");
    }

    [Fact]
    public void Command_Execute_WithUnknownOption_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction(r => r.Writer.WriteLine("Executing command..."))
                     .Build();

        subject.Execute("--option");

        _writer.Output.ToString().Should().Be("Executing command...\r\n");
    }

    [Fact]
    public void Command_Execute_Action_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction(() => { })
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_Action_WithArgs_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer).SetAction((string[] _) => { })
                       .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_Action_WithCommand_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction((RootCommand _) => { })
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_Action_WithCommandAndArgs_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction((_, _) => { })
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction(() => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithArgs_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction((string[] _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCommand_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction((RootCommand _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCancellationToken_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction((CancellationToken _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCommandAndArgs_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction((RootCommand _, string[] _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCommandAndCancellationToken_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction((RootCommand _, CancellationToken _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithStringAndCancellationToken_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer).SetAction((string[] _, CancellationToken _) => Task.CompletedTask)
                       .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCommandAndStringAndCancellationToken_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction((_, _, _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.ToString().Should().Be("");
    }

    [Fact]
    public void Command_Execute_WithEmptyName_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction(r => r.Writer.WriteLine("Executing command..."))
                     .Build();

        subject.Execute("--");

        _writer.Output.ToString().Should().Be("Executing command...\r\n");
    }

    [Fact]
    public void Command_Execute_WithEmptyAlias_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction(r => r.Writer.WriteLine("Executing command..."))
                     .Build();

        subject.Execute("-");

        _writer.Output.ToString().Should().Be("Executing command...\r\n");
    }

    [Fact]
    public void Command_Execute_WithChildCommand_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction(r => r.Writer.WriteLine("You should not be here!"))
                     .AddChild("sub", b => b.SetAction(c => c.Writer.WriteLine("Executing sub-Command...")))
                     .Build();

        subject.Execute("sub");

        _writer.Output.ToString().Should().Be("Executing sub-Command...\r\n");
    }

    [Fact]
    public void Command_Execute_WithExceptionDuringExecution_ShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer).SetAction((Action)(() => throw new("Some exception.")))
                       .Build();

        subject.Execute();

        _writer.Output.ToString().Should().StartWith("An error occurred while executing command 'testhost'.\r\n");
    }

    [Fact]
    public void Command_Execute_WithExceptionDuringRead_ShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .AddChild("sub", b => b.SetAction(c => c.Writer.WriteLine("Executing sub-Command...")))
                     .SetAction(r => r.Writer.WriteLine("Executing command..."))
                     .AddOption<string>("option", onRead: _ => throw new("Some exception."))
                     .Build();

        subject.Execute("--option", "abc");

        _writer.Output.ToString().Should().Contain("An error occurred while reading option 'option'.\r\n");
        _writer.Output.ToString().Should().Contain("An error occurred while executing command 'testhost'.\r\n");
    }

    [Fact]
    public void Command_Execute_WithRootHelp_ShowsHelp() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .Build();

        subject.Execute();

        _writer.Output.ToString().Should().Be("""

                                   DotNetToolbox.CommandLineBuilder 8.0.2

                                   This package provides tools for creating a simple CLI (Command-Line Interface) console application.

                                   Usage: testhost [options]

                                   Options:
                                     -h, --help                    Show this help information and exit.
                                     --no-color                    Don't colorize output.
                                     -v, --verbose <verbose>       Show verbose output.
                                     --version                     Show version information and exit.


                                   """);
    }

    [Fact]
    public void Command_Execute_WithChildCommand_ShowsHelp() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .SetAction(r => r.Writer.WriteLine("You should not be here!"))
                     .AddChild("sub-Command")
                     .Build();

        subject.Execute("sub-Command");

        _writer.Output
               .ToString()
               .Should().Be("""

                            Usage: testhost sub-Command

                            Options:
                              -h, --help                    Show this help information and exit.


                            """);
    }

    [Fact]
    public void Command_Execute_DefaultRoot_ShowsHelp() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .Build();

        subject.Execute("--version");

        _writer.Output.ToString().Should().Be("DotNetToolbox.CommandLineBuilder\r\n8.0.2\r\n");
    }

    [Fact]
    public void Command_Execute_WithHelpLongCommandName_ShowsHelp() {
        var subject = new Command("command", "Command description.");
        subject.Add(new Option<string>("options"));
        subject.Add(new Option<string>("very-long-name", 'v', "Some description"));
        var childCommand = new Command("sub-Command");
        childCommand.SetAction(() => childCommand.Writer.WriteLine("Executing sub-Command..."));
        subject.Add(childCommand);
        subject.Add(new Parameter<string>("param"));
        subject.Writer = _writer;
        subject.SetAction(() => subject.Writer.WriteLine("You should not be here!"));

        subject.Execute("-h");

        _writer.Output.ToString().Should().Be("""

                                   Usage: command [parameters] [options]
                                   Usage: command [options] [command]

                                   Options:
                                     -h, --help                    Show this help information and exit.
                                     --options <options>
                                     -v, --very-long-name <very-long-name> Some description

                                   Parameters:
                                     <param>

                                   Commands:
                                     sub-Command

                                   Use "command [command] --help" for more information about a command.


                                   """);
    }
}
