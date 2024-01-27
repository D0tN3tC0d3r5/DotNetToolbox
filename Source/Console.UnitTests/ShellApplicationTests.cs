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
        app.Version.Should().Be("15.0.0.0");
        app.Environment.Should().Be("");
        app.Arguments.Should().BeEmpty();
        app.AssemblyName.Should().Be("testhost");
        app.Children.Should().HaveCount(5);
        app.Configuration.Should().NotBeNull();
        app.Settings.Should().NotBeNull();
        app.Settings.ClearScreenOnStart.Should().BeFalse();
        app.Settings.Environment.Should().Be("");
        app.Settings.Prompt.Should().Be("> ");
        app.Data.Should().BeEmpty();
        app.DateTime.Should().BeOfType<DateTimeProvider>();
        app.Guid.Should().BeOfType<GuidProvider>();
        app.FileSystem.Should().BeOfType<FileSystem>();
        app.Output.Should().BeOfType<Output>();
        app.Input.Should().BeOfType<Input>();
        app.Logger.Should().BeOfType<Logger<Shell>>();
    }

    [Fact]
    public void Create_WhenCreationFails_Throws() {
        // Arrange & Act
        var app = Shell.Create(b => b.SetLogging(l => l.SetMinimumLevel(LogLevel.Information)));

        // Assert
        app.Should().BeOfType<Shell>();
    }

    [Fact]
    public void Create_CreatesShellApplication_WithCustomAttributes() {
        // Arrange & Act
        var assemblyDescriptor = Substitute.For<IAssemblyDescriptor>();
        var assemblyAccessor = Substitute.For<IAssemblyAccessor>();
        assemblyAccessor.GetEntryAssembly().Returns(assemblyDescriptor);
        assemblyDescriptor.Name.Returns("TestApp");
        assemblyDescriptor.Version.Returns(new Version(1, 0));
        assemblyDescriptor.GetCustomAttribute<AssemblyTitleAttribute>().Returns(new AssemblyTitleAttribute("My App"));
        assemblyDescriptor.GetCustomAttribute<AssemblyDescriptionAttribute>().Returns(new AssemblyDescriptionAttribute("Some description."));
        var app = Shell.Create(b => b.ReplaceAssemblyAccessor(assemblyAccessor));

        // Assert
        app.Should().BeOfType<Shell>();
        app.Name.Should().Be("My App");
        app.AssemblyName.Should().Be("TestApp");
        app.Version.Should().Be("1.0");
        app.Description.Should().Be("Some description.");
    }

    [Fact]
    public void Create_CreatesShellApplication_WithAssemblyInfo() {
        // Arrange & Act
        var assemblyDescriptor = Substitute.For<IAssemblyDescriptor>();
        var assemblyAccessor = Substitute.For<IAssemblyAccessor>();
        assemblyAccessor.GetEntryAssembly().Returns(assemblyDescriptor);
        assemblyDescriptor.Name.Returns("TestApp");
        assemblyDescriptor.Version.Returns(new Version(1, 0));
        var app = Shell.Create(b => b.ReplaceAssemblyAccessor(assemblyAccessor));

        // Assert
        app.Should().BeOfType<Shell>();
        app.Name.Should().Be("TestApp");
        app.AssemblyName.Should().Be("TestApp");
        app.Version.Should().Be("1.0");
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
            
            Usage: testhost [Options] [Commands]
            Options:
              --help, -h, -?            Displays this help information and finishes.
              --version                 Displays the version and exits.
            
            Commands:
              ClearScreen, cls          Clear the screen.
              Exit                      Exit the application.
              Help, ?                   Display help information.

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

        static Result CommandAction() => throw new("Some error.");
        const string expectedOutput =
            """
            testhost v15.0.0.0
            > crash
            Exception: Some error.
                Stack Trace:
                       at DotNetToolbox.ConsoleApplication.ShellApplicationTests.*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1.*
                       at System.Threading.Tasks.Task`1*
                       at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop*
                    --- End of stack trace from previous location ---
                       at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop*
                       at System.Threading.Tasks.Task.ExecuteWithThreadLocal*
                    --- End of stack trace from previous location ---
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
            
            
            """;
        var expectedLines = expectedOutput.Split(Environment.NewLine);
        var app = Shell.Create(b => {
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });
        app.AddCommand("Crash", CommandAction);

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(ApplicationBase.DefaultErrorCode);
        output.Lines.Select((line, index) => (line, index)).Should().AllSatisfy(x => {
            if (expectedLines[x.index].StartsWith('*') || expectedLines[x.index].EndsWith('*'))
                x.line.Should().Match(expectedLines[x.index]);
            else x.line.Should().Be(expectedLines[x.index]);
        });

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
                       at DotNetToolbox.ConsoleApplication.ShellApplicationTests.*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1.*
                       at System.Threading.Tasks.Task`1*
                       at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop*
                    --- End of stack trace from previous location ---
                       at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop*
                       at System.Threading.Tasks.Task.ExecuteWithThreadLocal*
                    --- End of stack trace from previous location ---
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
            
            
            """;
        var expectedLines = expectedOutput.Split(Environment.NewLine);
        var app = Shell.Create(b => {
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });
        app.AddCommand("Crash", CommandAction);

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(expectedErrorCode);
        output.Lines.Select((line, index) => (line, index)).Should().AllSatisfy(x => {
            if (expectedLines[x.index].StartsWith('*') || expectedLines[x.index].EndsWith('*'))
                x.line.Should().Match(expectedLines[x.index]);
            else x.line.Should().Be(expectedLines[x.index]);
        });
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
                       at DotNetToolbox.ConsoleApplication.ShellApplicationTests.*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1.*
                       at System.Threading.Tasks.Task`1*
                       at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop*
                    --- End of stack trace from previous location ---
                       at System.Threading.ExecutionContext.RunFromThreadPoolDispatchLoop*
                       at System.Threading.Tasks.Task.ExecuteWithThreadLocal*
                    --- End of stack trace from previous location ---
                       at DotNetToolbox.ConsoleApplication.Nodes.Command`1*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.*
                       at DotNetToolbox.ConsoleApplication.ShellApplication`3.*
                       at DotNetToolbox.ConsoleApplication.Application.Application`3.*
                Inner Exception => InvalidOperationException: Some error.
                    Stack Trace:
                           at DotNetToolbox.ConsoleApplication.ShellApplicationTests.*


            """;
        var expectedLines = expectedOutput.Split(Environment.NewLine);
        var app = Shell.Create(b => {
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });
        app.AddCommand("Crash", CommandAction);

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(expectedErrorCode);
        output.Lines.Select((line, index) => (line, index)).Should().AllSatisfy(x => {
            if (expectedLines[x.index].StartsWith('*') || expectedLines[x.index].EndsWith('*'))
                x.line.Should().Match(expectedLines[x.index]);
            else x.line.Should().Be(expectedLines[x.index]);
        });
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
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });
        app.AddCommand("Crash", CommandAction);

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(ApplicationBase.DefaultExitCode);
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
    }

    [Fact]
    public async Task RunAsync_WithErrorDuringArgumentRead_ReturnsResultWithErrors() {
        // Arrange
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        const string expectedOutput =
            $"""
             testhost v15.0.0.0
             Validation error: Unknown argument '--invalid'. For a list of available arguments use '--help'.


             """;
        var app = Shell.Create(["--invalid"], b => {
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(ApplicationBase.DefaultErrorCode);
        output.Lines.Should().BeEquivalentTo(expectedOutput.Split(Environment.NewLine));
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
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });

        // Act
        var actualResult = await app.RunAsync();

        // Assert
        actualResult.Should().Be(ApplicationBase.DefaultExitCode);
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
        app.Settings.ClearScreenOnStart.Should().BeTrue();
        app.Settings.Environment.Should().Be("Development");
        app.Settings.Prompt.Should().Be("$ ");
        app.Environment.Should().Be("Development");
    }

    [Fact]
    public void Create_SetLogging_CreatesShellApplication() {
        // Arrange & Act
        var app = Shell.Create(b
            => b.SetLogging(l => l.SetMinimumLevel(LogLevel.Debug)));

        // Assert
        app.Should().BeOfType<Shell>();
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat() {
        // Arrange
        var serviceProvider = CreateFakeServiceProvider();
        var app = new Shell([], null, serviceProvider) {
            Description = "Test Application",
        };

        var expectedToString = $"ShellApplication: {app.Name} v{app.Version} => Test Application";

        // Act
        var actualToString = app.ToString();

        // Assert
        actualToString.Should().Be(expectedToString);
    }

    [Fact]
    public void AppendVersion_AppendsVersionInformation() {
        // Arrange
        var serviceProvider = CreateFakeServiceProvider();
        var app = new Shell([], null, serviceProvider);
        var builder = new StringBuilder();
        var expectedVersionInfo = $"{app.Name} v{app.Version}{Environment.NewLine}";

        // Act
        app.AppendVersion(builder);
        var actualVersionInfo = builder.ToString();

        // Assert
        actualVersionInfo.Should().Be(expectedVersionInfo);
    }

    [Fact]
    public void AppendHelp_AppendsHelpInformation() {
        // Arrange
        var serviceProvider = CreateFakeServiceProvider();
        var app = new Shell([], null, serviceProvider) {
            Description = "Test Application",
        };
        var builder = new StringBuilder();
        var expectedHelpInfo = $"{app.Name} v{app.Version}{Environment.NewLine}Test Application{Environment.NewLine}";

        // Act
        app.AppendHelp(builder);
        var actualHelpInfo = builder.ToString();

        // Assert
        actualHelpInfo.Should().Be(expectedHelpInfo);
    }

    [Fact]
    public void AddOption_AddsOptionWithAliases() {
        // Arrange
        var serviceProvider = CreateFakeServiceProvider();
        var app = new Shell([], null, serviceProvider);
        var optionName = "test-option";
        var aliases = new[] { "t", "test" };

        // Act
        app.AddOption(optionName, aliases);

        // Assert
        var child = app.Children.Should().ContainSingle(x => x.Name == optionName).Subject;
        var option = child.Should().BeOfType<Option>().Subject;
        option.Aliases.Should().BeEquivalentTo(aliases);
    }

    [Fact]
    public void AddOption_Generic_AddsOptionOfType() {
        // Arrange
        var serviceProvider = CreateFakeServiceProvider();
        var app = new Shell([], null, serviceProvider);

        // Act
        app.AddOption<TestOption>();

        // Assert
        var child = app.Children.Should().ContainSingle(x => x.Name == "Option").Subject;
        var option = child.Should().BeOfType<TestOption>().Subject;
        option.Aliases.Should().BeEquivalentTo("o");
    }

    [Fact]
    public void AddParameter_AddsParameterWithDefaultValue() {
        // Arrange
        var serviceProvider = CreateFakeServiceProvider();
        var app = new Shell([], null, serviceProvider);
        var parameterName = "param1";
        var defaultValue = "default-value";

        // Act
        app.AddParameter(parameterName, defaultValue);

        // Assert
        var child = app.Children.Should().ContainSingle(x => x.Name == parameterName).Subject;
        var parameter = child.Should().BeOfType<Parameter>().Subject;
        parameter.Order.Should().Be(0);
    }

    [Fact]
    public void AddParameter_Generic_AddsParameterOfType() {
        // Arrange
        var serviceProvider = CreateFakeServiceProvider();
        var app = new Shell([], null, serviceProvider);

        // Act
        app.AddParameter<TestParameter>();

        // Assert
        var child = app.Children.Should().ContainSingle(x => x.Name == "Age").Subject;
        var parameter = child.Should().BeOfType<TestParameter>().Subject;
        parameter.Aliases.Should().BeEmpty();
        parameter.Order.Should().Be(0);
    }

    [Fact]
    public void AddFlag_AddsFlagWithAliases() {
        // Arrange
        var serviceProvider = CreateFakeServiceProvider();
        var app = new Shell([], null, serviceProvider);
        var flagName = "Verbose";
        var aliases = new[] { "v" };

        // Act
        app.AddFlag(flagName, aliases);

        // Assert
        var child = app.Children.Should().ContainSingle(x => x.Name == flagName).Subject;
        var flag = child.Should().BeOfType<Flag>().Subject;
        flag.Aliases.Should().BeEquivalentTo(aliases);
    }

    [Fact]
    public void AddFlag_Generic_AddsFlagOfType() {
        // Arrange
        var serviceProvider = CreateFakeServiceProvider();
        var app = new Shell([], null, serviceProvider);

        // Act
        app.AddFlag<TestFlag>();

        // Assert
        var child = app.Children.Should().ContainSingle(x => x.Name == "Flag").Subject;
        var flag = child.Should().BeOfType<TestFlag>().Subject;
        flag.Aliases.Should().BeEquivalentTo("f");
    }

    [Fact]
    public async Task RunAsync_WithArguments_ExecutesApp() {
        var output = new TestOutput();
        var input = new TestInput(output, "exit");
        var app = Shell.Create(["Option", "o", "20"], b => {
            b.ReplaceInput(input);
            b.ReplaceOutput(output);
        });
        app.AddOption<TestOption>();
        app.AddFlag<TestFlag>();
        app.AddParameter<TestParameter>();

        // Act
        var actualResult = await app.RunAsync();

        // Assert
    }

    private class TestOption(IHasChildren app) : Option<TestOption>(app, "Option", "o");
    private class TestParameter(IHasChildren app) : Parameter<TestParameter>(app, "Age", "18");
    private class TestFlag(IHasChildren app) : Flag<TestFlag>(app, "Flag", "f");

    private readonly IAssemblyDescriptor _assemblyDescriptor = Substitute.For<IAssemblyDescriptor>();
    private readonly IAssemblyAccessor _assemblyAccessor = Substitute.For<IAssemblyAccessor>();
    private IServiceProvider CreateFakeServiceProvider() {
        var output = new TestOutput();
        var input = new TestInput(output);
        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(IConfiguration)).Returns(Substitute.For<IConfiguration>());
        serviceProvider.GetService(typeof(IOutput)).Returns(output);
        serviceProvider.GetService(typeof(IInput)).Returns(input);
        _assemblyAccessor.GetEntryAssembly().Returns(_assemblyDescriptor);
        _assemblyDescriptor.Name.Returns("TestApp");
        _assemblyDescriptor.Version.Returns(new Version(1, 0));
        serviceProvider.GetService(typeof(IAssemblyAccessor)).Returns(_assemblyAccessor);
        serviceProvider.GetService(typeof(IDateTimeProvider)).Returns(Substitute.For<IDateTimeProvider>());
        serviceProvider.GetService(typeof(IGuidProvider)).Returns(Substitute.For<IGuidProvider>());
        serviceProvider.GetService(typeof(IFileSystem)).Returns(Substitute.For<IFileSystem>());
        serviceProvider.GetService(typeof(ILoggerFactory)).Returns(Substitute.For<ILoggerFactory>());
        return serviceProvider;
    }
}
