namespace DotNetToolbox.Graph;

public class RunnerTests {
    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var guidProvider = Substitute.For<IGuidProvider>();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var loggerFactory = Substitute.For<ILoggerFactory>();

        // Act
        var runner = new Runner(startingNode, guidProvider, dateTimeProvider, loggerFactory);

        // Assert
        runner.Should().NotBeNull();
    }

    [Fact]
    public void Run_WithValidNode_ExecutesWithoutError() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(_ => null);
        var runner = new Runner(startingNode);

        // Act
        var action = () => runner.Run();

        // Assert
        action.Should().NotThrow();
        startingNode.Received(1).Run(Arg.Any<Context>());
    }

    [Fact]
    public void Constructor_WithNullStartingNode_Throws() {
        // Act
        var action = () => new Runner(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithMessage("*startingNode*");
    }

    [Fact]
    public void Run_WithNullContext_ReturnsNonNullContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(_ => null);
        var runner = new Runner(startingNode);

        // Act
        var result = runner.Run();

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Run_WithEmptyContext_ReturnsSameContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(_ => null);
        var runner = new Runner(startingNode);
        var context = new Context();

        // Act
        var result = runner.Run(context);

        // Assert
        result.Should().BeSameAs(context);
    }

    [Fact]
    public void Run_WithRunningRunner_ThrowsInvalidOperationException() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(_ => null);
        var runner = new Runner(startingNode);

        // Set the Start property using reflection
        var propertyInfo = typeof(Runner).GetProperty("Start");
        propertyInfo?.SetValue(runner, DateTimeOffset.Now);

        // Act
        var action = () => runner.Run();

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("*already being executed*");
    }

    [Fact]
    public void Run_WithSingleNode_ReturnsSameContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(_ => null);
        var runner = new Runner(startingNode);
        var context = new Context();
        // Act
        var result = runner.Run(context);

        // Assert
        result.Should().BeSameAs(context);
    }

    [Fact]
    public void Run_WithMultipleNodes_ReturnsSameContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var nextNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(nextNode);
        nextNode.Run(Arg.Any<Context>()).Returns(_ => null);

        var logger = Substitute.For<ILogger>();
        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger<Runner>().Returns(logger);
        var runner = new Runner(startingNode, loggerFactory: loggerFactory);
        var context = new Context();

        // Act
        var result = runner.Run(context);

        // Assert
        result.Should().BeSameAs(context);
        logger.Received(2).Log(Arg.Any<LogLevel>(), Arg.Any<EventId>(), Arg.Any<string>(), Arg.Any<Exception?>(), Arg.Any<Func<string, Exception, string>>());
    }

    [Fact]
    public void Run_WithExceptionInNode_ThrowsRunnerException() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Throws(new RunnerException());
        var runner = new Runner(startingNode);

        // Act
        var action = () => runner.Run();

        // Assert
        action.Should().Throw<RunnerException>();
    }

    [Fact]
    public void Run_WithExceptionInNode_LogsError() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Throws(new RunnerException());
        var logger = Substitute.For<ILogger>();
        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger<Runner>().Returns(logger);
        var runner = new Runner(startingNode, loggerFactory: loggerFactory);

        // Act
        var action = () => runner.Run();

        // Assert
        action.Should().Throw<RunnerException>();
        logger.Received(1).LogError(Arg.Any<RunnerException>(), Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void Run_WithExceptionInNode_RethrowsException() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var exception = new RunnerException();
        startingNode.Run(Arg.Any<Context>()).Throws(exception);
        var runner = new Runner(startingNode);

        // Act
        var action = () => runner.Run();

        // Assert
        action.Should().Throw<RunnerException>().Which.Should().BeSameAs(exception);
    }

    [Fact]
    public void Run_WithExceptionInNode_LogsEndOfWorkflow() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Throws(new RunnerException());
        var logger = Substitute.For<ILogger>();
        var loggerFactory = Substitute.For<ILoggerFactory>();
        loggerFactory.CreateLogger<Runner>().Returns(logger);
        var runner = new Runner(startingNode, loggerFactory: loggerFactory);

        // Act
        var action = () => runner.Run();

        // Assert
        action.Should().Throw<RunnerException>();
        logger.Received(1).LogInformation(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void GetHashCode_ReturnsIdHashCode() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(_ => null);
        var runner = new Runner(startingNode);

        // Act
        var hashCode = runner.GetHashCode();

        // Assert
        hashCode.Should().Be(runner.Id.GetHashCode());
    }
}
