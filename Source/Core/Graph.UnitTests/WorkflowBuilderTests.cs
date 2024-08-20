namespace DotNetToolbox.Graph;

public sealed class WorkflowBuilderTests {
    private readonly IWorkflowBuilder _builder;

    public WorkflowBuilderTests() {
        var services = new ServiceCollection();
        services.AddScoped<INodeSequence, NodeSequence>();
        services.AddTransient<INodeFactory, NodeFactory>();
        services.AddTransient<IRetryPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();
        _builder = new WorkflowBuilder(provider);
    }

    [Fact]
    public void BuildGraph_SingleAction_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["action"]

                                      """;
        var start = _builder.Do(_ => { }).Build();

        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_TwoConnectedActions_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["action"]
                                      1 --> 2
                                      2["action"]

                                      """;

        var start = _builder.Do(_ => { })
                            .Do(_ => { })
                            .Build();

        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_IfNode_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["if"]
                                      1 --> |True| 2
                                      2["action"]
                                      1 --> |False| 3
                                      3["action"]

                                      """;
        var start = _builder.If(_ => true)
                            .Then(t => t.Do(_ => { }))
                            .Else(f => f.Do(_ => { }))
                            .Build();

        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_SelectNode_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["case"]
                                      1 --> |key1| 2
                                      2["action"]
                                      1 --> |key2| 3
                                      3["action"]
                                      1 --> |key3| 4
                                      4["action"]

                                      """;
        var start = _builder.Case(_ => "key1")
                            .Is("key1", b => b.Do(_ => { }))
                            .Is("key2", b => b.Do(_ => { }))
                            .Is("key3", b => b.Do(_ => { }))
                            .Build();

        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_ComplexWorkflow_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["action"]
                                      1 --> 2
                                      2["if"]
                                      2 --> |True| 3
                                      3["action"]
                                      3 --> 4
                                      4["case"]
                                      4 --> |key1| 5
                                      5["action"]
                                      4 --> |key2| 6
                                      6["action"]
                                      2 --> |False| 7
                                      7["action"]

                                      """;

        var start = _builder.Do(_ => { })
                            .If(_ => true)
                            .Then(t => t.Do(_ => { })
                                        .Case(_ => "key1")
                                        .Is("key1", b => b.Do(_ => { }))
                                        .Is("key2", b => b.Do(_ => { })))
                            .Else(f => f.Do(_ => { }))
                            .Build();

        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_WithLabels_IncludesLabelsInMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["Start"]
                                      1 --> 2
                                      2["Decision"]
                                      2 --> |True| 3
                                      3["Success"]
                                      2 --> |False| 4
                                      4["Fail"]

                                      """;
        var start = _builder.Do("Start", _ => { })
                            .If("Decision", _ => true)
                            .Then(t => t.Do("Success", _ => { }))
                            .Else(f => f.Do("Fail", _ => { }))
                            .Build();

        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_WithLoop_HandlesLoopCorrectly() {
        const string expectedResult = """
                                      flowchart TD
                                      1["Start"]
                                      1 --> 2
                                      2["LoopCondition"]
                                      2 --> |True| 3
                                      3["LoopAction"]
                                      3 --> 4
                                      4["goto"]
                                      2 --> |False| 5
                                      5["Exit"]

                                      """;
        var start = _builder.Do("Start", _ => { })
                            .If("LoopCondition", _ => true)
                            .Then(t => t.Do("LoopAction", _ => { })
                                        .GoTo("LoopCondition"))
                            .Else(f => f.Do("Exit", _ => { }))
                            .Build();
        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_WithGenericAction_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["CustomAction"]

                                      """;
        var start = _builder.Do<CustomAction>().Build();

        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_WithNestedConditions_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["if"]
                                      1 --> |True| 2
                                      2["if"]
                                      2 --> |True| 3
                                      3["action"]
                                      2 --> |False| 4
                                      4["action"]
                                      1 --> |False| 5
                                      5["action"]

                                      """;
        var start = _builder.If(_ => true)
                            .Then(t1 => t1.If(_ => false)
                                          .Then(t2 => t2.Do(_ => { }))
                                          .Else(f2 => f2.Do(_ => { })))
                            .Else(f1 => f1.Do(_ => { }))
                            .Build();

        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    // ReSharper disable once ClassNeverInstantiated.Local - Test class
    private sealed class CustomAction(IServiceProvider services)
                : ActionNode<CustomAction>(services) {
        protected override Task Execute(Context context, CancellationToken ct)
            => Task.CompletedTask;
    }
}
