﻿using Shell = DotNetToolbox.ConsoleApplication.ShellApplication;

namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationTests {
    [Fact]
    public void Create_CreatesShellApplication() {
        // Arrange & Act
        var app = Shell.Create();

        // Assert
        app.Should().BeOfType<Shell>();
        app.Name.Should().Be("testhost");
        app.Version.Should().Be("15.0.0.0");
        app.Environment.Name.Should().Be("");
        app.AssemblyName.Should().Be("testhost");
        app.Children.Should().HaveCount(6);
        app.Settings.Should().NotBeNull();
        app.Settings.ClearScreenOnStart.Should().BeFalse();
        app.Settings.Prompt.Should().Be("> ");
        app.Context.Should().BeEmpty();
        app.Logger.Should().NotBeNull();
    }

    [Fact]
    public void ReplaceInput_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output);

        // Act
        var app = Shell.Create(b => b.SetInputHandler(input));

        // Assert
        app.Should().BeOfType<Shell>();
    }

    [Fact]
    public void Run_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "", "--exit", "\"exit\"", "exit");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > 
            > --exit
            Validation error: Command '--exit' not found. For a list of available commands use 'help'.

            > "exit"
            Validation error: Command '"exit"' not found. For a list of available commands use 'help'.

            > exit

            """;
        var fileSystem = new TestFileSystem();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        var app = Shell.Create(b => {
            b.SetDateTimeProvider(dateTimeProvider);
            b.SetGuidProvider(guidProvider);
            b.SetFileSystem(fileSystem);
            b.SetOutputHandler(output);
            b.SetInputHandler(input);
        });

        // Act
        app.Run();

        // Assert
        app.Should().BeOfType<Shell>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithSetOptionsAndProvidersReplaced_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            $ exit

            """;
        var fileSystem = new TestFileSystem();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        await using var app = Shell.Create(b => {
            b.ConfigureOptions(o => {
                o.ClearScreenOnStart = true;
                o.Prompt = "$ ";
            });
            b.SetDateTimeProvider(dateTimeProvider);
            b.SetGuidProvider(guidProvider);
            b.SetFileSystem(fileSystem);
            b.SetOutputHandler(output);
            b.SetInputHandler(input);
        });

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<Shell>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithHelp_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "help", "exit");
        var assemblyDescriptor = Substitute.For<IAssemblyDescriptor>();
        assemblyDescriptor.Name.Returns("tsa");
        assemblyDescriptor.Version.Returns(new Version(1, 0));
        assemblyDescriptor.GetCustomAttribute<AssemblyTitleAttribute>().Returns(new AssemblyTitleAttribute("Test Shell Application"));
        assemblyDescriptor.GetCustomAttribute<AssemblyDescriptionAttribute>().Returns(new AssemblyDescriptionAttribute("This is a test application."));
        const string expectedOutput =
            """
            Test Shell Application v1.0
            > help
            Test Shell Application v1.0
            This is a test application.

            Usage:
                tsa [Options] [Commands]
                tsa [Options] [<Timeout>]

            Options:
                --clear-screen, -cls      Clear the screen.
                --help, -h, -?            Display this help information.
                --version                 Display the application's version.

            Parameters:
                Timeout

            Commands:
                ClearScreen, cls          Clear the screen.
                Exit                      Exit the application.
                Help, ?                   Display this help information.

            > exit

            """;
        await using var app = Shell.Create(b => {
            b.ConfigureOptions(o => o.ClearScreenOnStart = true);
            b.SetAssemblyInformation(assemblyDescriptor);
            b.SetOutputHandler(output);
            b.SetInputHandler(input);
        });
        app.AddParameter("Timeout", "5000");

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<Shell>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithHelpAlias_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "?", "exit");
        var assemblyDescriptor = Substitute.For<IAssemblyDescriptor>();
        assemblyDescriptor.Name.Returns("tsa");
        assemblyDescriptor.Version.Returns(new Version(1, 0));
        assemblyDescriptor.GetCustomAttribute<AssemblyTitleAttribute>().Returns(new AssemblyTitleAttribute("Test Shell Application"));
        assemblyDescriptor.GetCustomAttribute<AssemblyDescriptionAttribute>().Returns(new AssemblyDescriptionAttribute("This is a test application."));
        const string expectedOutput =
            """
            Test Shell Application v1.0
            > ?
            Test Shell Application v1.0
            This is a test application.

            Usage:
                tsa [Options] [Commands]
                tsa [Options] [<Timeout>]

            Options:
                --clear-screen, -cls      Clear the screen.
                --help, -h, -?            Display this help information.
                --version                 Display the application's version.

            Parameters:
                Timeout

            Commands:
                ClearScreen, cls          Clear the screen.
                Exit                      Exit the application.
                Help, ?                   Display this help information.

            > exit

            """;
        await using var app = Shell.Create(b => {
            b.ConfigureOptions(o => o.ClearScreenOnStart = true);
            b.SetAssemblyInformation(assemblyDescriptor);
            b.SetOutputHandler(output);
            b.SetInputHandler(input);
        });
        app.AddParameter("Timeout", "5000");

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<Shell>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithHelpCommand_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "help exit", "exit");
        var assemblyDescriptor = Substitute.For<IAssemblyDescriptor>();
        assemblyDescriptor.Name.Returns("tsa");
        assemblyDescriptor.Version.Returns(new Version(1, 0));
        assemblyDescriptor.GetCustomAttribute<AssemblyTitleAttribute>().Returns(new AssemblyTitleAttribute("Test Shell Application"));
        assemblyDescriptor.GetCustomAttribute<AssemblyDescriptionAttribute>().Returns(new AssemblyDescriptionAttribute("This is a test application."));
        const string expectedOutput =
            """
            Test Shell Application v1.0
            > help exit
            Exit the application.

            Usage:
                Exit

            > exit

            """;
        await using var app = Shell.Create(b => {
            b.SetAssemblyInformation(assemblyDescriptor);
            b.SetOutputHandler(output);
            b.SetInputHandler(input);
        });
        app.AddParameter("Timeout", "5000");

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<Shell>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithHelpFlag_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output);
        var assemblyDescriptor = Substitute.For<IAssemblyDescriptor>();
        assemblyDescriptor.Name.Returns("tsa");
        assemblyDescriptor.Version.Returns(new Version(1, 0));
        assemblyDescriptor.GetCustomAttribute<AssemblyTitleAttribute>().Returns(new AssemblyTitleAttribute("Test Shell Application"));
        assemblyDescriptor.GetCustomAttribute<AssemblyDescriptionAttribute>().Returns(new AssemblyDescriptionAttribute("This is a test application."));
        const string expectedOutput =
            """
            Test Shell Application v1.0

            """;
        await using var app = Shell.Create(["--version"], b => {
            b.SetAssemblyInformation(assemblyDescriptor);
            b.SetOutputHandler(output);
            b.SetInputHandler(input);
        });
        app.AddParameter("Timeout", "5000");

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<Shell>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithHelpCommand_ShowsCommandHelp() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "help ClearScreen", "exit");
        var assemblyDescriptor = Substitute.For<IAssemblyDescriptor>();
        assemblyDescriptor.Name.Returns("tsa");
        assemblyDescriptor.Version.Returns(new Version(1, 0));
        assemblyDescriptor.GetCustomAttribute<AssemblyTitleAttribute>().Returns(new AssemblyTitleAttribute("Test Shell Application"));
        assemblyDescriptor.GetCustomAttribute<AssemblyDescriptionAttribute>().Returns(new AssemblyDescriptionAttribute("This is a test application."));
        const string expectedOutput =
            """
            Test Shell Application v1.0
            > help ClearScreen
            Clear the screen.

            Usage:
                ClearScreen

            Aliases: cls

            > exit

            """;
        var app = Shell.Create(b => {
            b.SetAssemblyInformation(assemblyDescriptor);
            b.SetOutputHandler(output);
            b.SetInputHandler(input);
        });
        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<Shell>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithExceptionDuringExecution_ReturnsResultWithException() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "crash");

        static Result CommandAction() => throw new("Some error.");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > crash
            Exception: Some error.
                Stack Trace:
                       at DotNetToolbox.ConsoleApplication.ShellApplicationTests.*
                       at DotNetToolbox.ConsoleApplication.Utilities.NodeFactory*
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1*
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*


            """;
        var app = Shell.Create(b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });
        app.AddCommand("Crash", CommandAction);

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(ApplicationBase.DefaultErrorCode);
        output.ToString().Should().Match(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithConsoleExceptionDuringExecution_ReturnsResultWithException() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "crash");
        const int expectedErrorCode = 13;

        static Result CommandAction()
            => throw new ConsoleException(expectedErrorCode, "Some error.");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > crash
            ConsoleException: Some error.
                Stack Trace:
                       at DotNetToolbox.ConsoleApplication.ShellApplicationTests*
                       at DotNetToolbox.ConsoleApplication.Utilities.NodeFactory*
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1*
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.ProcessUserInput(String[] input, CancellationToken ct)*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.ProcessCommandLine(CancellationToken ct)*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.Run(CancellationToken ct)*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.RunAsync()*


            """;
        var app = Shell.Create(b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });
        app.AddCommand("Crash", CommandAction);

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(expectedErrorCode);
        output.ToString().Should().Match(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithConsoleExceptionDuringExecution_AndInnerException_ReturnsResultWithException() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "crash");
        const int expectedErrorCode = 13;

        static Result CommandAction() {
            try {
                throw new InvalidOperationException("Some error.");
            }
            catch (Exception ex) {
                throw new ConsoleException(expectedErrorCode, "Some error.", ex);
            }
        }
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > crash
            ConsoleException: Some error.
                Stack Trace:
                       at DotNetToolbox.ConsoleApplication.ShellApplicationTests*
                       at DotNetToolbox.ConsoleApplication.Utilities.NodeFactory*
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1*
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.ProcessCommandLine(CancellationToken ct)*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.Run(CancellationToken ct)*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.RunAsync()*
                Inner Exception => InvalidOperationException: Some error.
                    Stack Trace:
                           at DotNetToolbox.ConsoleApplication.ShellApplicationTests*


            """;
        var app = Shell.Create(b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });
        app.AddCommand("Crash", CommandAction);

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(expectedErrorCode);
        output.ToString().Should().Match(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithErrorDuringExecution_ReturnsResultWithErrors() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "crash", "exit");

        static Result CommandAction() => Result.Invalid("Some error.");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > crash
            Validation error: Some error.

            > exit

            """;
        var app = Shell.Create(b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });
        app.AddCommand("Crash", CommandAction);

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(ApplicationBase.DefaultExitCode);
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithErrorDuringArgumentRead_ReturnsResultWithErrors() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        const string expectedOutput =
            $"""
             Validation error: Unknown argument '--invalid'. For a list of available arguments use '--help'.


             """;
        var app = Shell.Create(["--invalid"], b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(ApplicationBase.DefaultErrorCode);
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_InvalidCommand_ReturnsResultWithErrors() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "invalid", "exit");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > invalid
            Validation error: Command 'invalid' not found. For a list of available commands use 'help'.

            > exit

            """;
        var app = Shell.Create(b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(ApplicationBase.DefaultExitCode);
        output.ToString().Should().Be(expectedOutput);
    }

    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestShellApp(string[] args, IServiceProvider serviceProvider)
        : ShellApplication<TestShellApp>(args, serviceProvider) {
        protected override Task<Result> Execute(CancellationToken ct) => Result.InvalidTask("Some error.");
    }

    [Fact]
    public async Task RunAsync_WithInvalidAction_ExecutesApp() {
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        var app = TestShellApp.Create(b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(1);
    }

    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestFaultyShellApp(string[] args, IServiceProvider serviceProvider)
        : ShellApplication<TestFaultyShellApp>(args, serviceProvider) {
        protected override Task<Result> Execute(CancellationToken ct) => Result.ErrorTask(new ConsoleException(13));
    }

    [Fact]
    public async Task RunAsync_WithFaultyAction_ExecutesApp() {
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        var app = TestFaultyShellApp.Create(b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(13);
    }

    [Fact]
    public async Task RunAsync_WithArguments_ExecutesApp() {
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        var app = Shell.Create(["Option", "o", "20"], b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });
        app.AddOption<TestOption>();
        app.AddFlag<TestFlag>();
        app.AddParameter<TestParameter>();

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(0);
    }

    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestOption(IHasChildren app) : Option<TestOption>(app, "Option", ["o"]);
    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestParameter(IHasChildren app) : Parameter<TestParameter>(app, "Age", "18");
    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestFlag(IHasChildren app) : Flag<TestFlag>(app, "Flag", ["f"]);
}
