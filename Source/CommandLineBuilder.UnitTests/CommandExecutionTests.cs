using DotNetToolbox.CommandLineBuilder.Extensions;

namespace DotNetToolbox.CommandLineBuilder;

public class CommandExecutionTests {
    private readonly InMemoryOutputWriter _writer = new();

    [Fact]
    public async Task Command_Execute_WithException_AndVerboseFlagAndNoColor_ShowError() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(() => throw new("Some exception."))
            .Build();

        await subject.Execute("-v", "2", "--no-color");

        _writer.Output.Should().Be("An error occurred while executing command 'testhost'.\n");
    }

    [Fact]
    public async Task Command_Execute_WithException_AndVerboseLevel_Detailed_ShowError() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(() => throw new("Some exception."))
            .Build();

        await subject.Execute("-v", "2");

        _writer.Output.Should().Be("An error occurred while executing command 'testhost'.\n");
    }

    [Fact]
    public async Task Command_Execute_WithException_AndVerboseLevel_Silent_ShowNotShowError() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(() => throw new("Some exception."))
            .Build();

        await subject.Execute("-v", "6");

        _writer.Output.Should().BeEmpty();
    }

    [Fact]
    public async Task Command_Execute_WithException_AndVerboseLevel_Debug_ShowErrorWithException() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(() => throw new("Some exception."))
            .Build();

        await subject.Execute("-v", "1");

        _writer.Output.Should().Contain("An error occurred while executing command 'testhost'.\nSystem.Exception: Some exception.\n");
    }

    [Fact]
    public async Task Command_Execute_WriteError_WithoutException_AndVerboseLevel_Silent_ShowNoError() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(c => Task.Run(() => c.Writer.WriteError("Some error.")))
            .Build();

        await subject.Execute("-v", "6");

        _writer.Output.Should().BeEmpty();
    }

    [Fact]
    public async Task Command_Execute_ExecutesDelegate() {
        var subject = new Command("Command", "Command description.") {
            Writer = _writer,
        };
        subject.OnExecute += (cmd, ct) => Task.Run(() => cmd.Writer.WriteLine("Executing command..."), ct);

        await subject.Execute();

        _writer.Output.Should().Be("Executing command...\n");
    }

    [Fact]
    public async Task Command_Execute_WithTerminalOption_ExecutesDelegate() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(r => Task.Run(() => r.Writer.WriteLine("You should not be here!")))
            .AddFlag("option", onRead: c => c.Writer.WriteLine("Stop here!"), existsIfSet: true)
            .Build();

        await subject.Execute("--option");

        _writer.Output.Should().Be("Stop here!\n");
    }

    [Fact]
    public async Task Command_Execute_WithUnknownOption_ExecutesDelegate() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(r => Task.Run(() => r.Writer.WriteLine("Executing command...")))
            .Build();

        await subject.Execute("--option");

        _writer.Output.Should().Be("Executing command...\n");
    }

    [Fact]
    public async Task Command_Execute_WithEmptyArgument_ExecutesDelegate() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(r => Task.Run(() => r.Writer.WriteLine("Executing command...")))
            .Build();

        await subject.Execute("  ");

        _writer.Output.Should().Be("Executing command...\n");
    }

    [Fact]
    public async Task Command_Execute_WithEmptyName_ExecutesDelegate() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(r => Task.Run(() => r.Writer.WriteLine("Executing command...")))
            .Build();

        await subject.Execute("--");

        _writer.Output.Should().Be("Executing command...\n");
    }

    [Fact]
    public async Task Command_Execute_WithEmptyAlias_ExecutesDelegate() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(r => Task.Run(() => r.Writer.WriteLine("Executing command...")))
            .Build();

        await subject.Execute("-");

        _writer.Output.Should().Be("Executing command...\n");
    }

    [Fact]
    public async Task Command_Execute_WithChildCommand_ExecutesDelegate() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(r => Task.Run(() => r.Writer.WriteLine("You should not be here!")))
            .AddChildCommand("sub", build: b =>
                b.OnExecute(c => Task.Run(() => c.Writer.WriteLine("Executing sub-Command..."))))
            .Build();

        await subject.Execute("sub");

        _writer.Output.Should().Be("Executing sub-Command...\n");
    }

    [Fact]
    public async Task Command_Execute_WithExceptionDuringExecution_ShowError() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(() => throw new("Some exception."))
            .Build();

        await subject.Execute();

        _writer.Output.Should().Be("An error occurred while executing command 'testhost'.\n");
    }

    [Fact]
    public async Task Command_Execute_WithExceptionDuringRead_ShowError() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(r => Task.Run(() => r.Writer.WriteLine("You should not be here!")))
            .AddOption<string>("option", onRead: _ => throw new("Some exception."))
            .Build();

        await subject.Execute("--option", "abc");

        _writer.Output.Should().Be("An error occurred while reading option 'option'.\nAn error occurred while executing command 'testhost'.\n");
    }

    [Fact]
    public async Task Command_Execute_WithRootHelp_ShowsHelp() {
        var subject = CommandBuilder.FromDefaultRoot(_writer).Build();

        await subject.Execute();

        _writer.Output.Should().Be("""

                                        DotNetToolbox.CommandLineBuilder 7.0.0

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
    public async Task Command_Execute_WithChildCommand_ShowsHelp() {
        var subject = CommandBuilder.FromDefaultRoot(_writer)
            .OnExecute(r => Task.Run(() => r.Writer.WriteLine("You should not be here!")))
            .AddChildCommand("sub-Command")
            .Build();

        await subject.Execute("sub-Command");

        _writer.Output.Should().Be("""

                                        Usage: testhost sub-Command

                                        Options:
                                          -h, --help                    Show this help information and exit.


                                        """.Replace("\r", ""));
    }

    [Fact]
    public async Task Command_Execute_DefaultRoot_ShowsHelp() {
        var subject = CommandBuilder.FromDefaultRoot(_writer).Build();

        await subject.Execute("--version");

        _writer.Output.Should().Be("DotNetToolbox.CommandLineBuilder\n7.0.0\n");
    }

    [Fact]
    public async Task Command_Execute_WithHelpLongCommandName_ShowsHelp() {
        var subject = new Command("command", "Command description.");
        subject.Add(new Option<string>("options"));
        subject.Add(new Option<string>("very-long-name", 'v', "Some description"));
        var childCommand = new Command("sub-Command");
        childCommand.OnExecute += (cmd, ct) => Task.Run(() => cmd.Writer.WriteLine("Executing sub-Command..."), ct);
        subject.Add(childCommand);
        subject.Add(new Parameter<string>("param"));
        subject.Writer = _writer;
        subject.OnExecute += (cmd, ct) => Task.Run(() => cmd.Writer.WriteLine("You should not be here!"), ct);

        await subject.Execute("-h");

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
