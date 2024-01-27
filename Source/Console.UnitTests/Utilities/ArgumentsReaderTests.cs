namespace DotNetToolbox.ConsoleApplication.Utilities;

public class ArgumentsReaderTests
{
    private readonly IApplication _app = Substitute.For<IApplication>();
    private readonly IServiceProvider _serviceProvider = Substitute.For<IServiceProvider>();
    private readonly ILoggerFactory _loggerFactory = Substitute.For<ILoggerFactory>();
    private readonly IFlag _flag;
    private readonly IOption _option;
    private readonly IParameter _parameter;
    private readonly IParameter _requiredParameter;
    private readonly ICommand _command;
    private readonly ICommand _executableOption;

    public ArgumentsReaderTests() {
        _serviceProvider.GetService(typeof(ILoggerFactory)).Returns(_loggerFactory);
        _app.Services.Returns(_serviceProvider);
        _option = new Option(_app, "--option", "-o");
        _flag = new Flag(_app, "--flag", "-f");
        _requiredParameter = new Parameter(_app, "age");
        _parameter = new Parameter(_app, "age", "18");
        _command = new Command(_app, "say", _ => Result.Success());
        _executableOption = new Command(_app, "--list", "-s", _ => Result.Success());
    }

    [Fact]
    public async Task Read_WithNoArguments_ShouldReturnSuccess()
    {
        // Arrange & Act
        var result = await ArgumentsReader.Read([], [], default);

        // Assert
        result.Should().BeEquivalentTo(Result.Success());
    }

    [Fact]
    public async Task Read_WithInvalidArgument_ReturnsInvalid() {
        // Arrange
        var arguments = new[] { "unknown" };
        const string expectedMessage = "Unknown argument 'unknown'. For a list of arguments use '--help'.";

        // Act
        var result = await ArgumentsReader.Read(arguments, [], default);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedMessage);
    }

    [Fact]
    public async Task Read_WithInvalidOption_ReturnsInvalid() {
        // Arrange
        var arguments = new[] { "--unknown" };
        const string expectedMessage = "Unknown argument '--unknown'. For a list of arguments use '--help'.";

        // Act
        var result = await ArgumentsReader.Read(arguments, [], default);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedMessage);
    }

    [Fact]
    public async Task Read_WithInvalidAlias_ReturnsInvalid()
    {
        // Arrange
        var arguments = new[] { "-u" };
        const string expectedMessage = "Unknown argument '-u'. For a list of arguments use '--help'.";

        // Act
        var result = await ArgumentsReader.Read(arguments, [], default);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedMessage);
    }

    [Fact]
    public async Task Read_WithFlagName_AndValue_ReturnsSuccess() {
        // Arrange
        var arguments = new[] { "--flag" };
        var children = new List<INode> { _flag };

        // Act
        var result = await ArgumentsReader.Read(arguments, children, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Read_WithFlagAlias_AndValue_ReturnsSuccess() {
        // Arrange
        var arguments = new[] { "-f" };
        var children = new List<INode> { _flag };

        // Act
        var result = await ArgumentsReader.Read(arguments, children, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Read_WithOptionName_WithoutValue_ReturnsInvalid() {
        // Arrange
        var arguments = new[] { "--option" };
        var children = new List<INode> { _option };
        const string expectedMessage = "Missing value for option '--option'.";

        // Act
        var result = await ArgumentsReader.Read(arguments, children, default);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors[0].Message.Should().Be(expectedMessage);
    }

    [Fact]
    public async Task Read_WithOptionName_AndValue_ReturnsSuccess()
    {
        // Arrange
        var arguments = new[] { "--option", "32" };
        var children = new List<INode> { _option };

        // Act
        var result = await ArgumentsReader.Read(arguments, children, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Read_WithOptionAlias_AndValue_ReturnsSuccess() {
        // Arrange
        var arguments = new[] { "-o", "32" };
        var children = new List<INode> { _option };

        // Act
        var result = await ArgumentsReader.Read(arguments, children, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Read_WithCommandName_ReturnsInvalid() {
        // Arrange
        var arguments = new[] { "say" };
        var children = new List<INode> { _command };

        // Act
        var result = await ArgumentsReader.Read(arguments, children, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Read_WithCommandAlias_AndValue_ReturnsSuccess() {
        // Arrange
        var arguments = new[] { "-o", "32" };
        var children = new List<INode> { _option };

        // Act
        var result = await ArgumentsReader.Read(arguments, children, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
