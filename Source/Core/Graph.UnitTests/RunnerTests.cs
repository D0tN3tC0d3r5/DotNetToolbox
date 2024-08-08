namespace DotNetToolbox.Graph;

public sealed class RunnerTests : IDisposable {
    private readonly Context _context;
    private readonly ServiceProvider _provider;

    public RunnerTests() {
        var services = new ServiceCollection();
        _provider = services.BuildServiceProvider();
        _context = new(_provider);
    }

    public void Dispose()
        => _context.Dispose();

    [Fact]
    public void Constructor_WithValidParameters_CreatesInstance() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode, _context);
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var loggerFactory = Substitute.For<ILoggerFactory>();

        // Act
        var runner = new Runner("42", workflow, dateTimeProvider, loggerFactory);

        // Assert
        runner.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullStartingNode_Throws() {
        // Act
        var action = () => new Runner(null!);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithMessage("*workflow*");
    }

    [Fact]
    public async Task Run_WithValidNode_ExecutesWithoutError() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode, _context);

        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>())
                    .Returns(Task.Run(() => {
                        Thread.Sleep(100);
                        return default(INode?);
                    }));
        var runner = new Runner(workflow);

        // Act
        var action = () => runner.Run();

        // Assert
        await action.Should().NotThrowAsync();
        runner.ElapsedTime.Should().BeCloseTo(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(20));
    }

    [Fact]
    public async Task Run_WithNullContext_ReturnsNonNullContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode, _context);
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).Returns(default(INode?));
        var runner = new Runner(workflow);

        // Act
        var action = () => runner.Run();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Run_WithEmptyContext_ReturnsSameContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var context = new Context(_provider);
        var workflow = new Workflow(startingNode, context);
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).Returns(default(INode?));
        var runner = new Runner(workflow);

        // Act
        var action = () => runner.Run();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Run_WithRunningRunner_ThrowsInvalidOperationException() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode, _context);
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).Returns(default(INode?));
        var runner = new Runner(workflow);

        // Set the Start property using reflection
        var propertyInfo = typeof(Runner).GetProperty("Start");
        propertyInfo?.SetValue(runner, DateTimeOffset.Now);

        // Act
        var action = () => runner.Run();

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("*already being executed*");
    }

    [Fact]
    public async Task Run_WithSingleNode_ReturnsSameContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var context = new Context(_provider);
        var workflow = new Workflow(startingNode, context);
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).Returns(default(INode?));
        var runner = new Runner(workflow);

        // Act
        var action = () => runner.Run();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Run_WithMultipleNodes_ReturnsSameContext() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var context = new Context(_provider);
        var workflow = new Workflow(startingNode, context);
        var nextNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).Returns(nextNode);
        nextNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).Returns(default(INode?));

        var loggerFactory = new TrackedLoggerFactory();
        var runner = new Runner(workflow, loggerFactory: loggerFactory);
        var logger = loggerFactory.Loggers[typeof(Runner).FullName!];

        // Act
        var action = () => runner.Run();

        // Assert
        await action.Should().NotThrowAsync();
        logger.Should().Have(2).Logs();
    }

    [Fact]
    public async Task Run_WithExceptionInNode_ThrowsException() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode, _context);
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());
        var runner = new Runner(workflow);

        // Act
        var action = () => runner.Run();

        // Assert
        await action.Should().ThrowAsync<Exception>();
    }

    [Fact]
    public async Task Run_WithExceptionInNode_LogsError() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode, _context);
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());
        var loggerFactory = new TrackedLoggerFactory();
        var runner = new Runner(workflow, loggerFactory: loggerFactory);
        var logger = loggerFactory.Loggers[typeof(Runner).FullName!];

        // Act
        var action = () => runner.Run();

        // Assert
        await action.Should().ThrowAsync<Exception>();
        logger.Should().Have(2).LogsWith(LogLevel.Information);
    }

    [Fact]
    public async Task Run_WithExceptionInNode_RethrowsException() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode, _context);
        var exception = new Exception();
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).ThrowsAsync(exception);
        var runner = new Runner(workflow);

        // Act
        var action = () => runner.Run();

        // Assert
        (await action.Should().ThrowAsync<Exception>())
            .Which.Should().BeSameAs(exception);
    }

    [Fact]
    public async Task Run_WithExceptionInNode_LogsEndOfWorkflow() {
        // Arrange
        var startingNode = Substitute.For<INode>();
        var workflow = new Workflow(startingNode, _context);
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).ThrowsAsync(new Exception());
        var loggerFactory = new TrackedLoggerFactory();
        var runner = new Runner(workflow, loggerFactory: loggerFactory);
        var logger = loggerFactory.Loggers[typeof(Runner).FullName!];

        // Act
        var action = () => runner.Run();

        // Assert
        await action.Should().ThrowAsync<Exception>();
        logger.Should().Have(2).LogsWith(LogLevel.Information);
    }

    [Fact]
    public async Task Run_OnRunStartingEvent_IsRaised() {
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>(), Arg.Any<CancellationToken>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var eventRaised = false;

        runner.OnStartingWorkflow = (_, wf, _) => {
            eventRaised = true;
            wf.Should().BeSameAs(workflow);
            return Task.CompletedTask;
        };

        await runner.Run();

        eventRaised.Should().BeTrue();
    }

    [Fact]
    public async Task Run_OnNodeExecutingEvent_IsRaisedForEachNode() {
        var startingNode = Substitute.For<INode>();
        var secondNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var executingCount = 0;

        runner.OnExecutingNode += (_, _, _, _) => {
            executingCount++;
            return Task.FromResult(true);
        };

        await runner.Run();

        executingCount.Should().Be(2);
    }

    [Fact]
    public async Task Run_OnNodeExecutingEvent_CanCancelExecution() {
        var startingNode = Substitute.For<INode>();
        var secondNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var context = new Context(_provider);
        var workflow = new Workflow(startingNode, context);
        var runner = new Runner(workflow);

        runner.OnExecutingNode += (_, wf, node, _) => {
            wf.Context.Should().BeSameAs(context);
            return Task.FromResult(node == secondNode);
        };

        await runner.Run();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        startingNode.Received(1).Run(Arg.Any<Context>(), Arg.Any<CancellationToken>());
        secondNode.DidNotReceive().Run(Arg.Any<Context>(), Arg.Any<CancellationToken>());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [Fact]
    public async Task Run_OnNodeExecutedEvent_IsRaisedForEachExecutedNode() {
        var startingNode = Substitute.For<INode>();
        var secondNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var executedCount = 0;

        runner.OnNodeExecuted += (_, _, _, _, _) => {
            executedCount++;
            return Task.FromResult(true);
        };

        await runner.Run();

        executedCount.Should().Be(2);
    }

    [Fact]
    public async Task Run_OnNodeExecutedEvent_CanCancelExecution() {
        var startingNode = Substitute.For<INode>();
        var secondNode = Substitute.For<INode>();
        var thirdNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns(thirdNode);
        var context = new Context(_provider);
        var workflow = new Workflow(startingNode, context);
        var runner = new Runner(workflow);

        runner.OnNodeExecuted += (_, wf, node, _, _) => {
            wf.Context.Should().BeSameAs(context);
            return Task.FromResult(node == secondNode);
        };

        await runner.Run();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        startingNode.Received(1).Run(Arg.Any<Context>(), Arg.Any<CancellationToken>());
        secondNode.Received(1).Run(Arg.Any<Context>(), Arg.Any<CancellationToken>());
        thirdNode.DidNotReceive().Run(Arg.Any<Context>(), Arg.Any<CancellationToken>());
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    [Fact]
    public async Task Run_OnRunEndedEvent_IsRaisedAfterExecution() {
        var startingNode = Substitute.For<INode>();
        startingNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var eventRaised = false;

        runner.OnWorkflowEnded += (_, wf, _) => {
            eventRaised = true;
            wf.Should().BeSameAs(workflow);
            return Task.CompletedTask;
        };

        await runner.Run();

        eventRaised.Should().BeTrue();
    }

    [Fact]
    public async Task Run_AllEvents_AreRaisedInCorrectOrder() {
        var startingNode = Substitute.For<INode>();
        startingNode.Id.Returns(1u);
        var secondNode = Substitute.For<INode>();
        secondNode.Id.Returns(2u);
        startingNode.Run(Arg.Any<Context>()).Returns(secondNode);
        secondNode.Run(Arg.Any<Context>()).Returns((INode?)null);
        var workflow = new Workflow(startingNode, _context);
        var runner = new Runner(workflow);
        var eventOrder = new List<string>();

        runner.OnStartingWorkflow = (_, _, _) => {
            eventOrder.Add("RunStarting");
            return Task.CompletedTask;
        };
        runner.OnExecutingNode = (_, _, node, _) => {
            eventOrder.Add($"NodeExecuting: {node.Id}");
            return Task.FromResult(true);
        };
        runner.OnNodeExecuted = (_, _, node, _, _) => {
            eventOrder.Add($"NodeExecuted: {node.Id}");
            return Task.FromResult(true);
        };
        runner.OnWorkflowEnded = (_, _, _) => {
            eventOrder.Add("RunEnded");
            return Task.CompletedTask;
        };

        await runner.Run();

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
