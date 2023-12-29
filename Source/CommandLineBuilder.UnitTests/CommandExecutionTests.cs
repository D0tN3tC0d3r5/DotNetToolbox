namespace DotNetToolbox.CommandLineBuilder;

public class CommandExecutionTests {
    private readonly InMemoryOutputWriter _writer = new();

    [Fact]
    public void Command_Execute_WithException_AndVerboseFlagAndNoColor_ShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithStaticAction(() => throw new("Some exception."))
                     .Build();

        subject.Execute("-v", "2", "--no-color");

        _writer.Output.Should().StartWith("An error occurred while executing command 'testhost'.\n");
    }

    [Fact]
    public void Command_Execute_WithException_AndVerboseLevel_Detailed_ShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithStaticAction(() => throw new("Some exception."))
                     .Build();

        subject.Execute("-v", "2");

        _writer.Output.Should().StartWith("An error occurred while executing command 'testhost'.\n");
    }

    [Fact]
    public void Command_Execute_WithException_AndVerboseLevel_Silent_ShowsNotShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithStaticAction(() => throw new("Some exception."))
                     .Build();

        subject.Execute("-v", "6");

        _writer.Output.Should().BeEmpty();
    }

    [Fact]
    public void Command_Execute_WithException_AndVerboseLevel_Debug_ShowsErrorWithException() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithStaticAction(() => throw new("Some exception."))
                     .Build();

        subject.Execute("-v", "1");

        _writer.Output.Should().Contain("An error occurred while executing command 'testhost'.\nSystem.Exception: Some exception.");
    }

    [Fact]
    public void Command_Execute_WithDelegate_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithStaticAction(() => throw new("Some exception."))
                     .Build();

        subject.Execute("-v", "1");

        _writer.Output.Should().Contain("An error occurred while executing command 'testhost'.\nSystem.Exception: Some exception.");
    }

    [Fact]
    public void Command_Execute_WriteError_WithoutException_AndVerboseLevel_Silent_ShowsNoError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithInstanceAction(cmd => cmd.Writer.WriteError("Some error."))
                     .Build();

        subject.Execute("-v", "6");

        _writer.Output.Should().BeEmpty();
    }

    [Fact]
    public void Command_Execute_ExecutesDelegate() {
        var subject = new Command("Command", "Command description.") {
            Writer = _writer,
        };
        subject.SetStaticAction(() => subject.Writer.WriteLine("Executing command..."));

        subject.Execute();

        _writer.Output.Should().Be("Executing command...\n");
    }

    [Fact]
    public void Command_Execute_WithTerminalOption_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithInstanceAction(r => r.Writer.WriteLine("You should not be here!"))
                     .AddFlag("option", onRead: c => c.Writer.WriteLine("Stop here!"), existsIfSet: true)
            .Build();

        subject.Execute("--option");

        _writer.Output.Should().Be("Stop here!\n");
    }

    [Fact]
    public void Command_Execute_WithUnknownOption_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithInstanceAction(r => r.Writer.WriteLine("Executing command..."))
                     .Build();

        subject.Execute("--option");

        _writer.Output.Should().Be("Executing command...\n");
    }

    [Fact]
    public void Command_Execute_Action_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithStaticAction(() => { })
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_Action_WithArgs_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithStaticAction(_ => { })
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_Action_WithCommand_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithInstanceAction(_ => { })
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_Action_WithCommandAndArgs_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithInstanceAction((_, _) => { })
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithAsyncStaticAction(() => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithArgs_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithAsyncStaticAction((string[] _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCommand_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithAsyncInstanceAction(_ => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCancellationToken_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithAsyncStaticAction((CancellationToken _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCommandAndArgs_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithAsyncInstanceAction((RootCommand _, string[] _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCommandAndCancellationToken_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithAsyncInstanceAction((RootCommand _, CancellationToken _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithStringAndCancellationToken_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithAsyncStaticAction((_, _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_AsyncFunc_WithCommandAndStringAndCancellationToken_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithAsyncInstanceAction((_, _, _) => Task.CompletedTask)
                     .Build();

        subject.Execute("");

        _writer.Output.Should().Be("");
    }

    [Fact]
    public void Command_Execute_WithEmptyName_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithInstanceAction(r => r.Writer.WriteLine("Executing command..."))
                     .Build();

        subject.Execute("--");

        _writer.Output.Should().Be("Executing command...\n");
    }

    [Fact]
    public void Command_Execute_WithEmptyAlias_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithInstanceAction(r => r.Writer.WriteLine("Executing command..."))
                     .Build();

        subject.Execute("-");

        _writer.Output.Should().Be("Executing command...\n");
    }

    [Fact]
    public void Command_Execute_WithChildCommand_ExecutesDelegate() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithInstanceAction(r => r.Writer.WriteLine("You should not be here!"))
                     .AddChild("sub", b => b.WithInstanceAction(c => c.Writer.WriteLine("Executing sub-Command...")))
                     .Build();

        subject.Execute("sub");

        _writer.Output.Should().Be("Executing sub-Command...\n");
    }

    [Fact]
    public void Command_Execute_WithExceptionDuringExecution_ShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithStaticAction(() => throw new("Some exception."))
                     .Build();

        subject.Execute();

        _writer.Output.Should().StartWith("An error occurred while executing command 'testhost'.\n");
    }

    [Fact]
    public void Command_Execute_WithExceptionDuringRead_ShowsError() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .AddChild("sub", b => b.WithInstanceAction(c => c.Writer.WriteLine("Executing sub-Command...")))
                     .WithInstanceAction(r => r.Writer.WriteLine("Executing command..."))
                     .AddOption<string>("option", onRead: _ => throw new("Some exception."))
                     .Build();

        subject.Execute("--option", "abc");

        _writer.Output.Should().Contain("An error occurred while reading option 'option'.\n");
        _writer.Output.Should().Contain("An error occurred while executing command 'testhost'.\n");
    }

    [Fact]
    public void Command_Execute_WithRootHelp_ShowsHelp() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .Build();

        subject.Execute();

        _writer.Output.Should().Be("""

                                   DotNetToolbox.CommandLineBuilder 8.0.2

                                   This package provides tools for creating a simple CLI (Command-Line Interface) console application.

                                   Usage: testhost [options]

                                   Options:
                                     -h, --help                    Show this help information and exit.
                                     --no-color                    Don't colorize output.
                                     -v, --verbose <verbose>       Show verbose output.
                                     --version                     Show version information and exit.


                                   """.Replace("\r", ""));
    }

    [Fact]
    public void Command_Execute_WithChildCommand_ShowsHelp() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .WithInstanceAction(r => r.Writer.WriteLine("You should not be here!"))
                     .AddChild("sub-Command")
                     .Build();

        subject.Execute("sub-Command");

        _writer.Output.Should().Be("""

                                   Usage: testhost sub-Command

                                   Options:
                                     -h, --help                    Show this help information and exit.


                                   """.Replace("\r", ""));
    }

    [Fact]
    public void Command_Execute_DefaultRoot_ShowsHelp() {
        var subject = CommandBuilder
                     .FromDefaultRoot()
                     .WithWriter(_writer)
                     .Build();

        subject.Execute("--version");

        _writer.Output.Should().Be("DotNetToolbox.CommandLineBuilder\n8.0.2\n");
    }

    [Fact]
    public void Command_Execute_WithHelpLongCommandName_ShowsHelp() {
        var subject = new Command("command", "Command description.");
        subject.Add(new Option<string>("options"));
        subject.Add(new Option<string>("very-long-name", 'v', "Some description"));
        var childCommand = new Command("sub-Command");
        childCommand.SetStaticAction(() => childCommand.Writer.WriteLine("Executing sub-Command..."));
        subject.Add(childCommand);
        subject.Add(new Parameter<string>("param"));
        subject.Writer = _writer;
        subject.SetStaticAction(() => subject.Writer.WriteLine("You should not be here!"));

        subject.Execute("-h");

        _writer.Output.Should().Be("""

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


                                   """.Replace("\r", ""));
    }
}
