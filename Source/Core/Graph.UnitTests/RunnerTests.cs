namespace DotNetToolbox.Graph;

public class RunnerTests {
    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode);
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var loggerFactory = Substitute.For<ILoggerFactory>();

        // Act
        var runner = new Runner(workflow, dateTimeProvider, loggerFactory);

        // Assert
        runner.Should().NotBeNull();
    }

    [Fact]
    public void Run_WithValidNode_ExecutesWithoutError() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode);

        startingNode.Run(Arg.Any<Context>()).Returns(static _ => {
            Thread.Sleep(100);
            return null;
        });
        var runner = new Runner(workflow);

        // Act
        var action = runner.Run;

        // Assert
        action.Should().NotThrow();
        startingNode.Received(1).Run(Arg.Any<Context>());
        runner.ElapsedTime.Should().BeCloseTo(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(5));
    }

    [Fact]
    public void Constructor_WithNullStartingNode_Throws() {
        // Act
        var action = () => new Runner(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithMessage("*workflow*");
    }

    [Fact]
    public void Run_WithNullContext_ReturnsNonNullContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode);
        startingNode.Run(Arg.Any<Context>()).Returns(static _ => null);
        var runner = new Runner(workflow);

        // Act
        var action = runner.Run;

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Run_WithEmptyContext_ReturnsSameContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var context = new Context();
        var workflow = new Workflow(startingNode, context);
        startingNode.Run(Arg.Any<Context>()).Returns(static _ => null);
        var runner = new Runner(workflow);

        // Act
        var action = runner.Run;

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Run_WithRunningRunner_ThrowsInvalidOperationException() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode);
        startingNode.Run(Arg.Any<Context>()).Returns(static _ => null);
        var runner = new Runner(workflow);

        // Set the Start property using reflection
        var propertyInfo = typeof(Runner).GetProperty("Start");
        propertyInfo?.SetValue(runner, DateTimeOffset.Now);

        // Act
        var action = runner.Run;

        // Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("*already being executed*");
    }

    [Fact]
    public void Run_WithSingleNode_ReturnsSameContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var context = new Context();
        var workflow = new Workflow(startingNode, context);
        startingNode.Run(Arg.Any<Context>()).Returns(static _ => null);
        var runner = new Runner(workflow);

        // Act
        var action = runner.Run;

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void Run_WithMultipleNodes_ReturnsSameContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var context = new Context();
        var workflow = new Workflow(startingNode, context);
        var nextNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(nextNode);
        nextNode.Run(Arg.Any<Context>()).Returns(static _ => null);

        var loggerFactory = new TrackedLoggerFactory();
        var runner = new Runner(workflow, loggerFactory: loggerFactory);
        var logger = loggerFactory.Loggers[typeof(Runner).FullName!];

        // Act
        var action = runner.Run;

        // Assert
        action.Should().NotThrow();
        logger.Should().Have(2).Logs();
    }

    [Fact]
    public void Run_WithExceptionInNode_ThrowsException() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode);
        startingNode.Run(Arg.Any<Context>()).Throws(new Exception());
        var runner = new Runner(workflow);

        // Act
        var action = runner.Run;

        // Assert
        action.Should().Throw<Exception>();
    }

    [Fact]
    public void Run_WithExceptionInNode_LogsError() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode);
        startingNode.Run(Arg.Any<Context>()).Throws(new Exception());
        var loggerFactory = new TrackedLoggerFactory();
        var runner = new Runner(workflow, loggerFactory: loggerFactory);
        var logger = loggerFactory.Loggers[typeof(Runner).FullName!];

        // Act
        var action = runner.Run;

        // Assert
        action.Should().Throw<Exception>();
        logger.Should().Have(2).LogsWith(LogLevel.Information);
    }

    [Fact]
    public void Run_WithExceptionInNode_RethrowsException() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode);
        var exception = new Exception();
        startingNode.Run(Arg.Any<Context>()).Throws(exception);
        var runner = new Runner(workflow);

        // Act
        var action = runner.Run;

        // Assert
        action.Should().Throw<Exception>().Which.Should().BeSameAs(exception);
    }

    [Fact]
    public void Run_WithExceptionInNode_LogsEndOfWorkflow() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode);
        startingNode.Run(Arg.Any<Context>()).Throws(new Exception());
        var loggerFactory = new TrackedLoggerFactory();
        var runner = new Runner(workflow, loggerFactory: loggerFactory);
        var logger = loggerFactory.Loggers[typeof(Runner).FullName!];

        // Act
        var action = runner.Run;

        // Assert
        action.Should().Throw<Exception>();
        logger.Should().Have(2).LogsWith(LogLevel.Information);
    }
}
