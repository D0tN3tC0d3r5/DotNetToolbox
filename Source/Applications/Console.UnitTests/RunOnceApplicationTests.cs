namespace DotNetToolbox.ConsoleApplication;

public class RunOnceApplicationTests {
    [Fact]
    public void Create_CreatesCliApplication() {
        // Arrange & Act
        var app = RunOnceApplication.Create();

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
        app.Name.Should().Be("testhost");
        app.Environment.Name.Should().Be("");
        app.AssemblyName.Should().Be("testhost");
        app.Children.Should().HaveCount(3);
        app.Context.Should().BeEmpty();
        app.Logger.Should().NotBeNull();
    }

    [Fact]
    public void Create_AddEnvironmentVariables_CreatesRunOnceApplication() {
        // Act
        var app = RunOnceApplication.Create(b => b.AddEnvironmentVariables("MYAPP_"));

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
    }

    [Fact]
    public void Create_WithVersionFlag_CreatesRunOnceApplication() {
        // Arrange
        var output = new TestOutput();
        var app = RunOnceApplication.Create(["--version"], b => b.SetOutputHandler(output));

        // Act
        var action = () => app.Run();

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Create_AddUserSecrets_CreatesRunOnceApplication() {
        // Act
        var app = RunOnceApplication.Create(b => b.AddUserSecrets<RunOnceApplication>());

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
    }

    [Fact]
    public void Create_WithArgs_CreatesRunOnceApplication() {
        // Arrange
        string[] args = ["arg1", "arg2"];

        // Act
        var app = RunOnceApplication.Create(args);

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
    }

    [Fact]
    public void Create_WithConfig_CreatesRunOnceApplication() {
        // Arrange & Act
        var setConfigCalled = false;
        var configBuilderCalled = false;
        var app = RunOnceApplication.Create(_ => setConfigCalled = true, _ => configBuilderCalled = true);

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
        setConfigCalled.Should().BeTrue();
        configBuilderCalled.Should().BeTrue();
    }

    [Fact]
    public void Create_WithArgsAndConfig_CreatesRunOnceApplication() {
        // Arrange
        var setConfigCalled = false;
        var configBuilderCalled = false;
        string[] args = ["arg1", "arg2"];

        // Act
        var app = RunOnceApplication.Create(args, _ => setConfigCalled = true, _ => configBuilderCalled = true);

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
        setConfigCalled.Should().BeTrue();
        configBuilderCalled.Should().BeTrue();
    }

    [Fact]
    public void Create_SetEnvironment_CreatesRunOnceApplication() {
        // Arrange & Act
        var app = RunOnceApplication.Create(["--environment", "Development"]);

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
    }

    [Fact]
    public void Create_AddSettings_CreatesRunOnceApplication() {
        // Arrange
        var fileProvider = new TestFileProvider();

        // Act
        var app = RunOnceApplication.Create(b => b.AddAppSettings(fileProvider));

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
    }

    [Fact]
    public void Create_AddSettings_WithEnvironmentSet_CreatesRunOnceApplication() {
        // Arrange
        var fileProvider = new TestFileProvider();

        // Act
        var app = RunOnceApplication.Create(["-env", "Development"], b => b.AddAppSettings(fileProvider));

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
    }

    [Fact]
    public void ReplaceInput_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output);

        // Act
        var app = RunOnceApplication.Create(b => b.SetInputHandler(input));

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
    }

    [Fact]
    public void Run_WithNoArgs_DisplayHelp() {
        // Arrange
        var output = new TestOutput();
        const string expectedOutput =
            """
            testhost v15.0.0.0

            Usage:
                testhost [Options]

            Options:
                --clear-screen, -cls      Clear the screen.
                --help, -h, -?            Display this help information.
                --version                 Display the application's version.


            """;
        var app = RunOnceApplication.Create(b => b.SetOutputHandler(output));

        // Act
        app.Run();

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
        output.ToString().Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Run_WithCommandAndNoArgs_DisplayHelp() {
        // Arrange
        var output = new TestOutput();
        const string expectedOutput =
            """
            testhost v15.0.0.0

            Usage:
                testhost [Options] [Commands]

            Options:
                --clear-screen, -cls      Clear the screen.
                --help, -h, -?            Display this help information.
                --version                 Display the application's version.

            Commands:
                magic
                say-it


            """;
        var app = RunOnceApplication.Create(b => b.SetOutputHandler(output));
        app.AddCommand("say-it", (Command c) => c.Environment.ConsoleOutput.WriteLine("Hello world!"));
        app.AddCommand("magic", (Command c) => c.Environment.ConsoleOutput.WriteLine("Please..."));

        // Act
        app.Run();

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
        output.ToString().Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Run_WithParameterAndNoArgs_DisplayHelp() {
        // Arrange
        var output = new TestOutput();
        const string expectedOutput =
            """
            testhost v15.0.0.0

            Usage:
                testhost [Options] [<Project>]

            Options:
                --clear-screen, -cls      Clear the screen.
                --help, -h, -?            Display this help information.
                --version                 Display the application's version.

            Parameters:
                Project


            """;
        var app = RunOnceApplication.Create(b => b.SetOutputHandler(output));
        app.AddParameter("Project", "");

        // Act
        app.Run();

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
        output.ToString().Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public void Run_WithCommand_ExecutesCommand() {
        // Arrange
        var output = new TestOutput();
        const string expectedOutput =
            """
            Hello world!

            """;
        var app = RunOnceApplication.Create([ "say-it" ], b => b.SetOutputHandler(output));
        app.AddCommand("say-it", (Command c) => c.Environment.ConsoleOutput.WriteLine("Hello world!"));

        // Act
        app.Run();

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
        output.ToString().Should().BeEquivalentTo(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithSetOptionsAndProvidersReplaced_ExecutesUntilExit() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output);
        var fileSystem = new TestFileSystemAccessor();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        const string expectedOutput =
            """
            testhost v15.0.0.0

            Usage:
                testhost [Options]

            Options:
                --clear-screen, -cls      Clear the screen.
                --help, -h, -?            Display this help information.
                --version                 Display the application's version.


            """;
        await using var app = RunOnceApplication.Create(b => {
            b.SetDateTimeProvider(dateTimeProvider);
            b.SetGuidProvider(guidProvider);
            b.SetFileSystem(fileSystem);
            b.SetOutputHandler(output);
            b.SetInputHandler(input);
        });

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithHelpOption_DisplaysHelp() {
        // Arrange
        var output = new TestOutput();
        const string expectedOutput =
            """
            testhost v15.0.0.0

            Usage:
                testhost [Options]

            Options:
                --clear-screen, -cls      Clear the screen.
                --help, -h, -?            Display this help information.
                --version                 Display the application's version.


            """;
        await using var app = RunOnceApplication.Create(["--help"], b => b.SetOutputHandler(output));

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithErrorDuringArgumentRead_ReturnsResultWithErrors() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output);
        const string expectedOutput =
            """
            Validation error: Unknown argument '--invalid'. For a list of available arguments use '--help'.


            """;
        var app = RunOnceApplication.Create(["--invalid"], b => {
            b.SetInputHandler(input);
            b.SetOutputHandler(output);
        });

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(IApplication.DefaultErrorCode);
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public void Create_SetLogging_CreatesRunOnceApplication() {
        // Arrange & Act
        var app = RunOnceApplication.Create(b => b.ConfigureLogging(l => l.SetMinimumLevel(LogLevel.Debug)));

        // Assert
        app.Should().BeOfType<RunOnceApplication>();
    }
}
