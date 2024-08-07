namespace DotNetToolbox.Graph;

public class RunnerTests {
    private readonly Context _context;
    private readonly ServiceProvider _provider;

    public RunnerTests() {
        var services = new ServiceCollection();
        _provider = services.BuildServiceProvider();
        _context = new(_provider);
    }

    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode, _context);
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
        var workflow = new Workflow(startingNode, _context);

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
        runner.ElapsedTime.Should().BeCloseTo(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(20));
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
        var workflow = new Workflow(startingNode, _context);
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
        var context = new Context(_provider);
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
        var workflow = new Workflow(startingNode, _context);
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
        var context = new Context(_provider);
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
        var context = new Context(_provider);
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
        var workflow = new Workflow(startingNode, _context);
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
        var workflow = new Workflow(startingNode, _context);
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
        var workflow = new Workflow(startingNode, _context);
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
        var workflow = new Workflow(startingNode, _context);
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
    public void Run_OnRunStartingEvent_IsRaised() {
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var eventRaised = false;

        runner.OnRunStarting += (_, args) => {
            eventRaised = true;
            args.Workflow.Should().BeSameAs(workflow);
        };

        runner.Run();

        eventRaised.Should().BeTrue();
    }

    [Fact]
    public void Run_OnNodeExecutingEvent_IsRaisedForEachNode() {
        var startingNode = Substitute.For<INode>();
        var secondNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var executingCount = 0;

        runner.OnNodeExecuting += (_, _) => executingCount++;

        runner.Run();

        executingCount.Should().Be(2);
    }

    [Fact]
    public void Run_OnNodeExecutingEvent_CanCancelExecution() {
        var startingNode = Substitute.For<INode>();
        var secondNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var context = new Context(_provider);
        var workflow = new Workflow(startingNode, context);
        var runner = new Runner(workflow);

        runner.OnNodeExecuting += (_, args) => {
            if (args.Node == secondNode) args.Cancel = true;
            args.Context.Should().BeSameAs(context);
        };

        runner.Run();

        startingNode.Received(1).Run(Arg.Any<Context>());
        secondNode.DidNotReceive().Run(Arg.Any<Context>());
    }

    [Fact]
    public void Run_OnNodeExecutedEvent_IsRaisedForEachExecutedNode() {
        var startingNode = Substitute.For<INode>();
        var secondNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var executedCount = 0;

        runner.OnNodeExecuted += (_, _) => executedCount++;

        runner.Run();

        executedCount.Should().Be(2);
    }

    [Fact]
    public void Run_OnNodeExecutedEvent_CanCancelExecution() {
        var startingNode = Substitute.For<INode>();
        var secondNode = Substitute.For<INode>();
        var thirdNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns(thirdNode);
        var context = new Context(_provider);
        var workflow = new Workflow(startingNode, context);
        var runner = new Runner(workflow);

        runner.OnNodeExecuted += (_, args) => {
            if (args.Node == secondNode) args.Cancel = true;
            args.Context.Should().BeSameAs(context);
        };

        runner.Run();

        startingNode.Received(1).Run(Arg.Any<Context>());
        secondNode.Received(1).Run(Arg.Any<Context>());
        thirdNode.DidNotReceive().Run(Arg.Any<Context>());
    }

    [Fact]
    public void Run_OnRunEndedEvent_IsRaisedAfterExecution() {
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var eventRaised = false;

        runner.OnRunEnded += (_, args) => {
            eventRaised = true;
            args.Workflow.Should().BeSameAs(workflow);
        };

        runner.Run();

        eventRaised.Should().BeTrue();
    }

    [Fact]
    public void Run_AllEvents_AreRaisedInCorrectOrder() {
        var startingNode = Substitute.For<INode>();
        startingNode.Id.Returns(1u);
        var secondNode = Substitute.For<INode>();
        secondNode.Id.Returns(2u);
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var eventOrder = new List<string>();

        runner.OnRunStarting += (_, _) => eventOrder.Add("RunStarting");
        runner.OnNodeExecuting += (_, args) => eventOrder.Add($"NodeExecuting: {args.Node.Id}");
        runner.OnNodeExecuted += (_, args) => eventOrder.Add($"NodeExecuted: {args.Node.Id}");
        runner.OnRunEnded += (_, _) => eventOrder.Add("RunEnded");

        runner.Run();

        eventOrder.Should().Equal(new List<string>
        {
            "RunStarting",
            "NodeExecuting: 1",
            "NodeExecuted: 1",
            "NodeExecuting: 2",
            "NodeExecuted: 2",
            "RunEnded",
        });
    }
}
