using AppOptions = DotNetToolbox.ConsoleApplication.ShellApplicationOptions;
using Shell = DotNetToolbox.ConsoleApplication.ShellApplication;

namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationTests {
    [Fact]
    public void Create_CreatesShellApplication() {
        // Arrange & Act
        var app = Shell.Create();

        // Assert
        app.Should().BeOfType<Shell>();
        app.Name.Should().Be("testhost");
        app.Environment.Should().Be("");
        app.Arguments.Should().BeEmpty();
        app.AssemblyName.Should().Be("testhost");
        app.Children.Should().HaveCount(3);
        app.Configuration.Should().NotBeNull();
        app.Options.Should().NotBeNull();
        app.Options.ClearScreenOnStart.Should().BeFalse();
        app.Options.Environment.Should().Be("");
        app.Options.Prompt.Should().Be("> ");
        app.Data.Should().BeEmpty();
        app.DateTime.Should().BeOfType<DateTimeProvider>();
        app.Guid.Should().BeOfType<GuidProvider>();
        app.FileSystem.Should().BeOfType<FileSystem>();
        app.Output.Should().BeOfType<Output>();
        app.Input.Should().BeOfType<Input>();
    }

    [Fact]
    public void Create_AddEnvironmentVariables_CreatesShellApplication() {
        // Act
        var app = Shell.Create(b => b.AddEnvironmentVariables("MYAPP_"));

        // Assert
        app.Should().BeOfType<Shell>();
    }

    [Fact]
    public void Create_AddUserSecrets_CreatesShellApplication() {
        // Act
        var app = Shell.Create(b => b.AddUserSecrets<Shell>());

        // Assert
        app.Should().BeOfType<Shell>();
    }

    [Fact]
    public void Create_WithArgs_CreatesShellApplication() {
        // Arrange
        string[] args = ["arg1", "arg2"];

        // Act
        var app = Shell.Create(args);

        // Assert
        app.Should().BeOfType<Shell>();
        app.Arguments.Should().HaveCount(2);
    }

    [Fact]
    public void Create_WithConfig_CreatesShellApplication() {
        // Arrange & Act
        var wasCalled = false;
        var app = Shell.Create(_ => wasCalled = true);

        // Assert
        app.Should().BeOfType<Shell>();
        app.Arguments.Should().BeEmpty();
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public void Create_WithArgsAndConfig_CreatesShellApplication() {
        // Arrange
        var wasCalled = false;
        string[] args = ["arg1", "arg2"];

        // Act
        var app = Shell.Create(args, _ => wasCalled = true);

        // Assert
        app.Should().BeOfType<Shell>();
        app.Arguments.Should().HaveCount(2);
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public void Create_SetEnvironment_CreatesShellApplication() {
        // Arrange & Act
        var app = Shell.Create(b => b.SetEnvironment("Development"));

        // Assert
        app.Should().BeOfType<Shell>();
    }

    [Fact]
    public void Create_AddSettings_CreatesShellApplication() {
        // Arrange
        var fileProvider = new TestFileProvider();

        // Act
        var app = Shell.Create(b => b.AddSettings(fileProvider));

        // Assert
        app.Should().BeOfType<Shell>();
    }

    [Fact]
    public void Create_AddSettings_WithEnvironmentSet_CreatesShellApplication() {
        // Arrange
        var fileProvider = new TestFileProvider();

        // Act
        var app = Shell.Create(b => {
            b.SetEnvironment("Development");
            b.AddSettings(fileProvider);
        });

        // Assert
        app.Should().BeOfType<Shell>();
    }

    [Fact]
    public void ReplaceInput_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output);

        // Act
        var app = Shell.Create(b => b.ReplaceInput(input));

        // Assert
        app.Should().BeOfType<Shell>();
    }

    [Fact]
    public void Run_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > exit

            """;
        var fileSystem = new TestFileSystem();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        var app = Shell.Create(b => {
            b.ReplaceDateTimeProvider(dateTimeProvider);
            b.ReplaceGuidProvider(guidProvider);
            b.ReplaceFileSystem(fileSystem);
            b.ReplaceOutput(output);
            b.ReplaceInput(input);
        });

        // Act
        app.Run();

        // Assert
        app.Should().BeOfType<Shell>();
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public async Task RunAsync_WithSetOptionsAndProvidersReplaced_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > exit

            """;
        var fileSystem = new TestFileSystem();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        await using var app = Shell.Create(b => {
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
        app.Should().BeOfType<Shell>();
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public async Task RunAsync_WithHelp_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "help", "exit");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > help
            testhost v15.0.0.0

            Commands:
              ClearScreen | cls         Clear the screen.
              Exit                      Exit the application.
              Help | ?                  Display help information.

            > exit

            """;
        await using var app = Shell.Create(b => {
            b.SetOptions(o => o.ClearScreenOnStart = true);
            b.ReplaceOutput(output);
            b.ReplaceInput(input);
        });

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<Shell>();
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public async Task RunAsync_WithExceptionDuringExecution_ReturnsResultWithException() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "crash");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > crash
            Exception: Some error.

            """;
        var app = Shell.Create(b => {
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });
        app.AddCommand("Crash", _ => Result.ErrorTask(new Exception("Some error.")));

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(Application.DefaultErrorCode);
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public async Task RunAsync_WithConsoleExceptionDuringExecution_ReturnsResultWithException() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "crash");
        const int expectedErrorCode = 13;
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > crash
            ConsoleException: Some error.

            """;
        var app = Shell.Create(b => {
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });
        app.AddCommand("Crash", _ => Result.ErrorTask(new ConsoleException(expectedErrorCode, "Some error.")));

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(expectedErrorCode);
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public async Task RunAsync_WithErrorDuringExecution_ReturnsResultWithErrors() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "crash", "exit");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > crash
            Validation error: Some error.
            > exit

            """;
        var app = Shell.Create(b => {
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });
        app.AddCommand("Crash", _ => Result.InvalidTask("Some error."));

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(Application.DefaultExitCode);
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public async Task RunAsync_WithErrorDuringArgumentRead_ReturnsResultWithErrors() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            Validation error: Unknown option: '--invalid'.

            """;
        var app = Shell.Create(["--invalid"], b => {
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
    public void Create_AddConfiguration_CreatesShellApplication() {
        // Arrange & Act
        var options = new ShellApplicationOptions {
            ClearScreenOnStart = true,
            Environment = "Development",
            Prompt = "$ ",
        };
        var app = Shell.Create(b => b.AddConfiguration("ShellApplication", options));

        // Assert
        app.Should().BeOfType<Shell>();
        app.Options.ClearScreenOnStart.Should().BeTrue();
        app.Options.Environment.Should().Be("Development");
        app.Options.Prompt.Should().Be("$ ");
        app.Environment.Should().Be("Development");
    }

    [Fact]
    public void Create_SetLogging_CreatesShellApplication() {
        // Arrange & Act
        var app = Shell.Create(b
            => b.SetLogging(l
                => l.SetMinimumLevel(LogLevel.Debug)));

        // Assert
        app.Should().BeOfType<Shell>();
    }
}
