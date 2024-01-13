using Microsoft.Extensions.Logging;

namespace DotNetToolbox.ConsoleApplication;

public class ShellApplicationTests {
    [Fact]
    public void Create_CreatesShellApplication() {
        // Arrange & Act
        var app = ShellApplication.Create();

        // Assert
        app.Should().BeOfType<ShellApplication>();
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
        var app = ShellApplication.Create(b => b.AddEnvironmentVariables("MYAPP_"));

        // Assert
        app.Should().BeOfType<ShellApplication>();
    }

    [Fact]
    public void Create_AddUserSecrets_CreatesShellApplication() {
        // Act
        var app = ShellApplication.Create(b => b.AddUserSecrets<ShellApplication>());

        // Assert
        app.Should().BeOfType<ShellApplication>();
    }

    [Fact]
    public void Create_WithArgs_CreatesShellApplication() {
        // Arrange
        string[] args = ["arg1", "arg2"];

        // Act
        var app = ShellApplication.Create(args);

        // Assert
        app.Should().BeOfType<ShellApplication>();
        app.Arguments.Should().HaveCount(2);
    }

    [Fact]
    public void Create_WithConfig_CreatesShellApplication() {
        // Arrange & Act
        var wasCalled = false;
        var app = ShellApplication.Create(_ => wasCalled = true);

        // Assert
        app.Should().BeOfType<ShellApplication>();
        app.Arguments.Should().BeEmpty();
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public void Create_WithArgsAndConfig_CreatesShellApplication() {
        // Arrange
        var wasCalled = false;
        string[] args = ["arg1", "arg2"];

        // Act
        var app = ShellApplication.Create(args, _ => wasCalled = true);

        // Assert
        app.Should().BeOfType<ShellApplication>();
        app.Arguments.Should().HaveCount(2);
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public void Create_SetEnvironment_CreatesShellApplication() {
        // Arrange & Act
        var app = ShellApplication.Create(b => b.SetEnvironment("Development"));

        // Assert
        app.Should().BeOfType<ShellApplication>();
    }

    [Fact]
    public void Create_AddSettings_CreatesShellApplication() {
        // Arrange
        var fileProvider = new TestFileProvider();

        // Act
        var app = ShellApplication.Create(b => b.AddSettings(fileProvider));

        // Assert
        app.Should().BeOfType<ShellApplication>();
    }

    [Fact]
    public void Create_AddSettings_WithEnvironmentSet_CreatesShellApplication() {
        // Arrange
        var fileProvider = new TestFileProvider();

        // Act
        var app = ShellApplication.Create(b => {
            b.SetEnvironment("Development");
            b.AddSettings(fileProvider);
        });

        // Assert
        app.Should().BeOfType<ShellApplication>();
    }

    [Fact]
    public void ReplaceInput_ExecutesUntilExit() {
        // Arrange
        var input = new TestInput();

        // Act
        var app = ShellApplication.Create(b => b.ReplaceInput(input));

        // Assert
        app.Should().BeOfType<ShellApplication>();
    }

    [Fact]
    public async Task RunAsync_ExecutesUntilExit() {
        // Arrange
        var input = new TestInput("exit");
        var output = new TestOutput();
        var fileSystem = new TestFileSystem();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        var app = ShellApplication.Create(b => {
            b.ReplaceDateTimeProvider(dateTimeProvider);
            b.ReplaceGuidProvider(guidProvider);
            b.ReplaceFileSystem(fileSystem);
            b.ReplaceOutput(output);
            b.ReplaceInput(input);
        });

        // Act
        await app.RunAsync();

        // Assert
        app.Should().BeOfType<ShellApplication>();
        output.Lines.Should().AllBeEquivalentTo("> ");
    }

    [Fact]
    public async Task RunAsync_WithClearScreenOnStartSet_ExecutesUntilExit() {
        // Arrange
        var input = new TestInput("exit");
        var output = new TestOutput();
        var fileSystem = new TestFileSystem();
        var guidProvider = new TestGuidProvider();
        var dateTimeProvider = new TestDateTimeProvider();
        var app = ShellApplication.Create(b => {
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
        app.Should().BeOfType<ShellApplication>();
        output.Lines.Should().AllBeEquivalentTo("> ");
    }

    [Fact]
    public void Create_AddConfiguration_CreatesShellApplication() {
        // Arrange & Act
        var options = new ShellApplicationOptions {
            ClearScreenOnStart = true,
            Environment = "Development",
            Prompt = "$ ",
        };
        var app = ShellApplication.Create(b => b.AddConfiguration("ShellApplication", options));

        // Assert
        app.Should().BeOfType<ShellApplication>();
        app.Options.ClearScreenOnStart.Should().BeTrue();
        app.Options.Environment.Should().Be("Development");
        app.Options.Prompt.Should().Be("$ ");
        app.Environment.Should().Be("Development");
    }

    [Fact]
    public void Create_SetLogging_CreatesShellApplication() {
        // Arrange & Act
        var app = ShellApplication.Create(b
            => b.SetLogging(l
                => l.SetMinimumLevel(LogLevel.Debug)));

        // Assert
        app.Should().BeOfType<ShellApplication>();
    }
}
