using AppOptions = DotNetToolbox.ConsoleApplication.CommandLineInterfaceApplicationOptions;
using CLI = DotNetToolbox.ConsoleApplication.CommandLineInterfaceApplication;

namespace DotNetToolbox.ConsoleApplication;

public class CommandLineInterfaceApplicationTests {
    [Fact]
    public void Create_CreatesCliApplication() {
        // Arrange & Act
        var app = CLI.Create();

        // Assert
        app.Should().BeOfType<CLI>();
        app.Name.Should().Be("testhost");
        app.Environment.Should().Be("");
        app.Arguments.Should().BeEmpty();
        app.AssemblyName.Should().Be("testhost");
        app.Children.Should().HaveCount(2);
        app.Configuration.Should().NotBeNull();
        app.Options.Should().NotBeNull();
        app.Options.ClearScreenOnStart.Should().BeFalse();
        app.Options.Environment.Should().Be("");
        app.Data.Should().BeEmpty();
        app.DateTime.Should().BeOfType<DateTimeProvider>();
        app.Guid.Should().BeOfType<GuidProvider>();
        app.FileSystem.Should().BeOfType<FileSystem>();
        app.Output.Should().BeOfType<Output>();
        app.Input.Should().BeOfType<Input>();
    }

    [Fact]
    public void Create_AddEnvironmentVariables_CreatesCommandLineInterfaceApplication() {
        // Act
        var app = CLI.Create(b => b.AddEnvironmentVariables("MYAPP_"));

        // Assert
        app.Should().BeOfType<CLI>();
    }

    [Fact]
    public void Create_AddUserSecrets_CreatesCommandLineInterfaceApplication() {
        // Act
        var app = CLI.Create(b => b.AddUserSecrets<CLI>());

        // Assert
        app.Should().BeOfType<CLI>();
    }

    [Fact]
    public void Create_WithArgs_CreatesCommandLineInterfaceApplication() {
        // Arrange
        string[] args = ["arg1", "arg2"];

        // Act
        var app = CLI.Create(args);

        // Assert
        app.Should().BeOfType<CLI>();
        app.Arguments.Should().HaveCount(2);
    }

    [Fact]
    public void Create_WithConfig_CreatesCommandLineInterfaceApplication() {
        // Arrange & Act
        var wasCalled = false;
        var app = CLI.Create(_ => wasCalled = true);

        // Assert
        app.Should().BeOfType<CLI>();
        app.Arguments.Should().BeEmpty();
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public void Create_WithArgsAndConfig_CreatesCommandLineInterfaceApplication() {
        // Arrange
        var wasCalled = false;
        string[] args = ["arg1", "arg2"];

        // Act
        var app = CLI.Create(args, _ => wasCalled = true);

        // Assert
        app.Should().BeOfType<CLI>();
        app.Arguments.Should().HaveCount(2);
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public void Create_SetEnvironment_CreatesCommandLineInterfaceApplication() {
        // Arrange & Act
        var app = CLI.Create(b => b.SetEnvironment("Development"));

        // Assert
        app.Should().BeOfType<CLI>();
    }

    [Fact]
    public void Create_AddSettings_CreatesCommandLineInterfaceApplication() {
        // Arrange
        var fileProvider = new TestFileProvider();

        // Act
        var app = CLI.Create(b => b.AddSettings(fileProvider));

        // Assert
        app.Should().BeOfType<CLI>();
    }

    [Fact]
    public void Create_AddSettings_WithEnvironmentSet_CreatesCommandLineInterfaceApplication() {
        // Arrange
        var fileProvider = new TestFileProvider();

        // Act
        var app = CLI.Create(b => {
            b.SetEnvironment("Development");
            b.AddSettings(fileProvider);
        });

        // Assert
        app.Should().BeOfType<CLI>();
    }

    [Fact]
    public void ReplaceInput_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output);

        // Act
        var app = CLI.Create(b => b.ReplaceInput(input));

        // Assert
        app.Should().BeOfType<CLI>();
    }

    [Fact]
    public void Run_WithNoArgs_DisplayHelp() {
        // Arrange
        var output = new TestOutput();
        const string expectedOutput =
            """
            testhost v15.0.0.0

            Usage: testhost [Options] [Commands]
            Options:
              --help | -h | -?          Display help information.
              --version                 Display version information.

            Commands:
              --help | -h | -?          Display help information.
              --version                 Display version information.


            """;
        var fileSystem = new TestFileSystem();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        var app = CLI.Create(b => {
            b.ReplaceDateTimeProvider(dateTimeProvider);
            b.ReplaceGuidProvider(guidProvider);
            b.ReplaceFileSystem(fileSystem);
            b.ReplaceOutput(output);
        });

        // Act
        app.Run();

        // Assert
        app.Should().BeOfType<CLI>();
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public async Task RunAsync_WithSetOptionsAndProvidersReplaced_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        var fileSystem = new TestFileSystem();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        const string expectedOutput =
            """
            testhost v15.0.0.0

            Usage: testhost [Options] [Commands]
            Options:
              --help | -h | -?          Display help information.
              --version                 Display version information.

            Commands:
              --help | -h | -?          Display help information.
              --version                 Display version information.


            """;
        await using var app = CLI.Create(b => {
                                             b.SetOptions(o => o.ClearScreenOnStart = true);
                                             b.ReplaceDateTimeProvider(dateTimeProvider);
                                             b.ReplaceGuidProvider(guidProvider);
                                             b.ReplaceFileSystem(fileSystem);
                                             b.ReplaceOutput(output);
                                             b.ReplaceInput(input);
                                         });

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<CLI>();
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public async Task RunAsync_WithHelpOption_DisplaysHelp() {
        // Arrange
        var output = new TestOutput();
        const string expectedOutput =
            """
            testhost v15.0.0.0

            Usage: testhost [Options] [Commands]
            Options:
              --help | -h | -?          Display help information.
              --version                 Display version information.

            Commands:
              --help | -h | -?          Display help information.
              --version                 Display version information.


            """;
        await using var app = CLI.Create(["--help"], b => {
            b.SetOptions(o => o.ClearScreenOnStart = true);
            b.ReplaceOutput(output);
        });

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<CLI>();
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    //[Fact]
    //public async Task RunAsync_WithExceptionDuringExecution_ReturnsResultWithException() {
    //    // Arrange
    //    var output = new TestOutput();
    //    var input = new TestInput(output, "crash");
    //    const string expectedOutput =
    //        """
    //        testhost v15.0.0.0
    //        > crash
    //        Exception: Some error.

    //        """;
    //    var app = CLI.Create(b => {
    //        b.ReplaceInput(input);
    //        b.ReplaceOutput(output);
    //    });
    //    app.AddCommand("Crash", _ => Result.ErrorTask(new Exception("Some error.")));

    //    // Act
    //    var actualResult = await app.RunAsync();

    //    // Assert
    //    actualResult.Should().Be(Application.DefaultErrorCode);
    //    output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    //}

    //[Fact]
    //public async Task RunAsync_WithConsoleExceptionDuringExecution_ReturnsResultWithException() {
    //    // Arrange
    //    var output = new TestOutput();
    //    var input = new TestInput(output, "crash");
    //    const int expectedErrorCode = 13;
    //    const string expectedOutput =
    //        """
    //        testhost v15.0.0.0
    //        > crash
    //        ConsoleException: Some error.

    //        """;
    //    var app = CLI.Create(b => {
    //        b.ReplaceInput(input);
    //        b.ReplaceOutput(output);
    //    });
    //    app.AddCommand("Crash", _ => Result.ErrorTask(new ConsoleException(expectedErrorCode, "Some error.")));

    //    // Act
    //    var actualResult = await app.RunAsync();

    //    // Assert
    //    actualResult.Should().Be(expectedErrorCode);
    //    output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    //}

    //[Fact]
    //public async Task RunAsync_WithErrorDuringExecution_ReturnsResultWithErrors() {
    //    // Arrange
    //    var output = new TestOutput();
    //    var input = new TestInput(output, "crash", "exit");
    //    const string expectedOutput =
    //        """
    //        testhost v15.0.0.0
    //        > crash
    //        Validation error: Some error.
    //        > exit

    //        """;
    //    var app = CLI.Create(b => {
    //        b.ReplaceInput(input);
    //        b.ReplaceOutput(output);
    //    });
    //    app.AddCommand("Crash", _ => Result.InvalidTask("Some error."));

    //    // Act
    //    var actualResult = await app.RunAsync();

    //    // Assert
    //    actualResult.Should().Be(Application.DefaultExitCode);
    //    output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    //}

    [Fact]
    public async Task RunAsync_WithErrorDuringArgumentRead_ReturnsResultWithErrors() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        const string expectedOutput =
            """
            Validation error: Unknown option: '--invalid'.

            """;
        var app = CLI.Create(["--invalid"], b => {
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(Application.DefaultErrorCode);
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public void Create_AddConfiguration_CreatesCommandLineInterfaceApplication() {
        // Arrange & Act
        var options = new AppOptions {
            ClearScreenOnStart = true,
            Environment = "Development",
        };
        var app = CLI.Create(b => b.AddConfiguration("CommandLineInterfaceApplication", options));

        // Assert
        app.Should().BeOfType<CLI>();
        app.Options.ClearScreenOnStart.Should().BeTrue();
        app.Options.Environment.Should().Be("Development");
        app.Environment.Should().Be("Development");
    }

    [Fact]
    public void Create_SetLogging_CreatesCommandLineInterfaceApplication() {
        // Arrange & Act
        var app = CLI.Create(b => b.SetLogging(l => l.SetMinimumLevel(LogLevel.Debug)));

        // Assert
        app.Should().BeOfType<CLI>();
    }
}
