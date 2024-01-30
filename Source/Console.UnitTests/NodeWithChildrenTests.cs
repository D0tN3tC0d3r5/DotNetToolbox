namespace DotNetToolbox.ConsoleApplication;

public class NodeWithChildrenTests {
    [Fact]
    public void Constructor_CreatesNode() {
        // Arrange & Act
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);

        // Act
        var node = new TestNode(app, "node", ["n"]) {
            Description = "Some description.",
        };

        // Assert
        node.Name.Should().Be("node");
        node.Description.Should().Be("Some description.");
        node.Aliases.Should().BeEquivalentTo("n");
        node.Parent.Should().Be(app);
        node.Application.Should().Be(app);
        node.Children.Should().BeEmpty();
        node.Options.Should().BeEmpty();
        node.Parameters.Should().BeEmpty();
        node.Commands.Should().BeEmpty();
    }

    [Fact]
    public void ToString_ReturnsExpectedFormat() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]) {
            Description = "Some description.",
        };
        var expectedToString = $"TestNode: {node.Name}, {node.Aliases[0]} => {node.Description}";

        // Act
        var actualToString = node.ToString();

        // Assert
        actualToString.Should().Be(expectedToString);
    }

    private class InvalidCommandDelegates : TheoryData<Delegate> {
        public InvalidCommandDelegates() {
            Add(() => 13);
            Add((Command _) => "Invalid");
            Add((string _) => { });
        }
    }
    [Theory]
    [ClassData(typeof(InvalidCommandDelegates))]
    public void AddChildCommand_WithInvalidDelegate_AddsCommand(Delegate action) {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        var result = () => node.AddChildCommand("command", action);

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task AddChildCommand_WithException_AddsCommandThatThrows() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);
        var command = (Command)node.AddChildCommand("command", (Action)(() => throw new()));

        // Act
        var result = () => command.Execute();

        // Assert
        await result.Should().ThrowAsync<Exception>();
    }

    private class CommandDelegates : TheoryData<Delegate> {
        public CommandDelegates() {
            Add(null!);
            Add(() => { });
            Add((Command _) => { });
            Add(() => Result.Success());
            Add((Command _) => Result.Success());
            Add(() => Result.SuccessTask());
            Add((Command _) => Result.SuccessTask());
            Add(() => Task.CompletedTask);
            Add((Command _) => Task.CompletedTask);
            Add((CancellationToken _) => Result.SuccessTask());
            Add((Command _, CancellationToken _) => Result.SuccessTask());
            Add((CancellationToken _) => Task.CompletedTask);
            Add((Command _, CancellationToken _) => Task.CompletedTask);
        }
    }
    [Theory]
    [ClassData(typeof(CommandDelegates))]
    public async Task AddChildCommand_AddsCommand(Delegate action) {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        var subject = node.AddChildCommand("command", action);

        // Assert
        node.Children.Should().ContainSingle(x => x.Name == "command");
        node.Options.Should().BeEmpty();
        node.Parameters.Should().BeEmpty();
        node.Commands.Should().ContainSingle();
        var command = subject.Should().BeOfType<Command>().Subject;
        command.Aliases.Should().BeEmpty();
        var result = await command.Execute();
        result.Should().Be(Result.Success());
    }

    [Theory]
    [ClassData(typeof(CommandDelegates))]
    public void AddChildCommand_WithAlias_AddsCommand(Delegate action) {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        node.AddChildCommand("command", "c", action);

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == "command").Subject;
        child.Aliases.Should().BeEquivalentTo("c");
    }

    [Fact]
    public async Task AddChildCommand_OfType_AddsCommandOfType() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        node.AddChildCommand<TestCommand>();

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == "Command").Subject;
        var command = child.Should().BeOfType<TestCommand>().Subject;
        command.Aliases.Should().BeEquivalentTo("c");
        var text = command.ToString();
        text.Should().Be("TestCommand: Command, c => Test command.");
        var result = () => command.Execute();
        await result.Should().NotThrowAsync();
    }

    [Fact]
    public void AddOption_AddsOption() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        node.AddOption("option");

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == "option").Subject;
        node.Options.Should().ContainSingle();
        node.Parameters.Should().BeEmpty();
        node.Commands.Should().BeEmpty();
        var option = child.Should().BeOfType<Option>().Subject;
        option.Aliases.Should().BeEmpty();
    }

    [Fact]
    public void AddOption_WithAlias_AddsOption() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        node.AddOption("option", "o");

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == "option").Subject;
        var option = child.Should().BeOfType<Option>().Subject;
        option.Aliases.Should().BeEquivalentTo("o");
    }

    [Fact]
    public void AddOption_OfType_AddsOptionOfType() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        node.AddOption<TestOption>();

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == "Option").Subject;
        var option = child.Should().BeOfType<TestOption>().Subject;
        option.Aliases.Should().BeEquivalentTo("o");
    }

    [Fact]
    public void AddParameter_AddsParameter() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);
        var parameterName = "param1";

        // Act
        node.AddParameter(parameterName);

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == parameterName).Subject;
        node.Options.Should().BeEmpty();
        node.Parameters.Should().ContainSingle();
        node.Commands.Should().BeEmpty();
        var parameter = child.Should().BeOfType<Parameter>().Subject;
        parameter.Order.Should().Be(0);
        parameter.IsRequired.Should().BeTrue();
        parameter.IsSet.Should().BeFalse();
    }

    [Fact]
    public void AddParameter_WithDefaultValue_AddsParameter() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);
        var parameterName = "param1";
        var defaultValue = "defaultValue";

        // Act
        node.AddParameter(parameterName, defaultValue);

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == parameterName).Subject;
        var parameter = child.Should().BeOfType<Parameter>().Subject;
        parameter.Order.Should().Be(0);
        parameter.IsRequired.Should().BeFalse();
        parameter.IsSet.Should().BeFalse();
    }

    [Fact]
    public void AddParameter_OfType_AddsParameterOfType() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        node.AddParameter<TestParameter>();

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == "Age").Subject;
        var parameter = child.Should().BeOfType<TestParameter>().Subject;
        parameter.Aliases.Should().BeEmpty();
        parameter.Order.Should().Be(0);
    }

    private class InvalidFlagDelegates : TheoryData<Delegate> {
        public InvalidFlagDelegates() {
            Add(() => 13);
            Add((Flag _) => "Invalid");
            Add((string _) => { });
        }
    }
    [Theory]
    [ClassData(typeof(InvalidFlagDelegates))]
    public void AddFlag_WithInvalidDelegate_AddsFlag(Delegate action) {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        var result = () => node.AddFlag("flag", action);

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public async Task AddFlag_WithException_AddsFlagThatThrows() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);
        var flag = node.AddFlag("flag", (Action)(() => throw new()));

        // Act
        var result = () => flag.Read();

        // Assert
        await result.Should().ThrowAsync<Exception>();
    }

    private class FlagDelegates : TheoryData<Delegate> {
        public FlagDelegates() {
            Add(null!);
            Add(() => { });
            Add((Flag _) => { });
            Add(() => Result.Success());
            Add((Flag _) => Result.Success());
            Add(() => Result.SuccessTask());
            Add((Flag _) => Result.SuccessTask());
            Add(() => Task.CompletedTask);
            Add((Flag _) => Task.CompletedTask);
            Add((CancellationToken _) => Result.SuccessTask());
            Add((Flag _, CancellationToken _) => Result.SuccessTask());
            Add((CancellationToken _) => Task.CompletedTask);
            Add((Flag _, CancellationToken _) => Task.CompletedTask);
        }
    }
    [Theory]
    [ClassData(typeof(FlagDelegates))]
    public async Task AddFlag_AddsFlag(Delegate action) {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        var subject = node.AddFlag("flag", action);

        // Assert
        node.Children.Should().ContainSingle(x => x.Name == "flag");
        node.Options.Should().ContainSingle();
        node.Parameters.Should().BeEmpty();
        node.Commands.Should().BeEmpty();
        var flag = subject.Should().BeOfType<Flag>().Subject;
        flag.Aliases.Should().BeEmpty();
        var result = await subject.Read();
        result.Should().Be(Result.Success());
    }

    [Theory]
    [ClassData(typeof(FlagDelegates))]
    public void AddFlag_WithAlias_AddsFlag(Delegate action) {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        node.AddFlag("flag", "c", action);

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == "flag").Subject;
        child.Aliases.Should().BeEquivalentTo("c");
    }

    [Fact]
    public void AddFlag_OfType_AddsFlagOfType() {
        // Arrange
        var app = Substitute.For<IApplication>();
        var serviceProvider = CreateFakeServiceProvider();
        app.Services.Returns(serviceProvider);
        var node = new TestNode(app, "node", ["n"]);

        // Act
        node.AddFlag<TestFlag>();

        // Assert
        var child = node.Children.Should().ContainSingle(x => x.Name == "Flag").Subject;
        var flag = child.Should().BeOfType<TestFlag>().Subject;
        flag.Aliases.Should().BeEquivalentTo("f");
    }

    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestCommand
        : Command<TestCommand> {
        public TestCommand(IHasChildren app)
            : base(app, "Command", ["c"]) {
            Description = "Test command.";
        }

        public override Task<Result> Execute(CancellationToken ct = default) {
            Logger.LogInformation("Some logger.");
            return base.Execute(ct);
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestOption(IHasChildren app) : Option<TestOption>(app, "Option", ["o"]);
    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestParameter(IHasChildren app) : Parameter<TestParameter>(app, "Age", "18");
    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestFlag(IHasChildren app) : Flag<TestFlag>(app, "Flag", ["f"]);

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

    // ReSharper disable once ClassNeverInstantiated.Local - Used for tests.
    private class TestNode(IHasChildren parent, string name, params string[] aliases)
        : NodeWithChildren<TestNode>(parent, name, aliases) {
    }
}
