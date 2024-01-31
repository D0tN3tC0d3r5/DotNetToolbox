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
        app.Environment.Name.Should().Be("");
        app.AssemblyName.Should().Be("testhost");
        app.Children.Should().HaveCount(3);
        app.Settings.Should().NotBeNull();
        app.Settings.ClearScreenOnStart.Should().BeFalse();
        app.Context.Should().BeEmpty();
        app.Logger.Should().BeOfType<Logger<CLI>>();
    }

    [Fact]
    public void Create_AddEnvironmentVariables_CreatesCommandLineInterfaceApplication() {
        // Act
        var app = CLI.Create(b => b.AddEnvironmentVariables("MYAPP_"));

        // Assert
        app.Should().BeOfType<CLI>();
    }

    [Fact]
    public void Create_WithVersionFlag_CreatesCommandLineInterfaceApplication() {
        // Arrange
        var output = new TestOutput();
        var app = CLI.Create([ "--version" ], b => b.SetOutputHandler(output));

        // Act
        var action = () => app.Run();

        // Assert
        action.Should().NotThrow();
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
    }

    [Fact]
    public void Create_WithConfig_CreatesCommandLineInterfaceApplication() {
        // Arrange & Act
        var wasCalled = false;
        var app = CLI.Create(_ => wasCalled = true);

        // Assert
        app.Should().BeOfType<CLI>();
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
        var app = CLI.Create(b => b.SetInputHandler(input));

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

            Usage:
                testhost [Options]

            Options:
                --clear-screen, -cls      Clear the screen.
                --help, -h, -?            Display this help information.
                --version                 Display the application's version.


            """;
        var fileSystem = new TestFileSystem();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        var app = CLI.Create(b => {
            b.SetDateTimeProvider(dateTimeProvider);
            b.SetGuidProvider(guidProvider);
            b.SetFileSystem(fileSystem);
            b.SetOutputHandler(output);
        });

        // Act
        app.Run();

        // Assert
        app.Should().BeOfType<CLI>();
        output.ToString().Should().BeEquivalentTo(expectedOutput);
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

            Usage:
                testhost [Options]

            Options:
                --clear-screen, -cls      Clear the screen.
                --help, -h, -?            Display this help information.
                --version                 Display the application's version.


            """;
        await using var app = CLI.Create(b => {
            b.SetOptions(o => o.ClearScreenOnStart = true);
            b.SetDateTimeProvider(dateTimeProvider);
            b.SetGuidProvider(guidProvider);
            b.SetFileSystem(fileSystem);
            b.SetOutputHandler(output);
            b.SetInputHandler(input);
        });

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<CLI>();
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
        await using var app = CLI.Create(["--help"], b => {
            b.SetOptions(o => o.ClearScreenOnStart = true);
            b.SetOutputHandler(output);
        });

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<CLI>();
        output.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public async Task RunAsync_WithErrorDuringArgumentRead_ReturnsResultWithErrors() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        const string expectedOutput =
            """
            Validation error: Unknown argument '--invalid'. For a list of available arguments use '--help'.


            """;
        var app = CLI.Create(["--invalid"], b => {
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
    public void Create_AddConfiguration_CreatesCommandLineInterfaceApplication() {
        // Arrange & Act
        var options = new AppOptions {
            ClearScreenOnStart = true,
        };
        var app = CLI.Create(b => b.AddConfiguration("CommandLineInterfaceApplication", options));

        // Assert
        app.Should().BeOfType<CLI>();
        app.Settings.ClearScreenOnStart.Should().BeTrue();
    }

    [Fact]
    public void Create_SetLogging_CreatesCommandLineInterfaceApplication() {
        // Arrange & Act
        var app = CLI.Create(b => b.SetLogging(l => l.SetMinimumLevel(LogLevel.Debug)));

        // Assert
        app.Should().BeOfType<CLI>();
    }
}
