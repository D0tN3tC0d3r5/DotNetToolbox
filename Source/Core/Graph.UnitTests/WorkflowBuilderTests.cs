namespace DotNetToolbox.Graph;

public sealed class WorkflowBuilderTests {
    private readonly WorkflowBuilder _builder;

    public WorkflowBuilderTests() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();
        _builder = new(provider);
    }

    [Fact]
    public void BuildGraph_SingleAction_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["action"]

                                      """;
        _builder.Do(_ => { });

        var graph = _builder.BuildGraph();

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

        _builder.Do(_ => { })
                .Do(_ => { });

        var graph = _builder.BuildGraph();

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
        _builder.If(_ => true,
                    t => t.Do(_ => { }),
                    f => f.Do(_ => { }));

        var graph = _builder.BuildGraph();

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
        _builder.When(_ => "key1", k => k
                    .Is("key1", b => b.Do(_ => { }))
                    .Is("key2", b => b.Do(_ => { }))
                    .Is("key3", b => b.Do(_ => { })));

        var graph = _builder.BuildGraph();

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

        _builder.Do(_ => { })
                .If(_ => true,
                    t => t.Do(_ => { })
                          .When(_ => "key1", k => k
                              .Is("key1", b => b.Do(_ => { }))
                              .Is("key2", b => b.Do(_ => { }))),
                    f => f.Do(_ => { }));

        var graph = _builder.BuildGraph();

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
        _builder.Do("Start", _ => { })
                .If("Decision", _ => true,
                    t => t.Do("Success", _ => { }),
                    f => f.Do("Fail", _ => { }));

        var graph = _builder.BuildGraph();

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
                                      3 --> 2
                                      2 --> |False| 4
                                      4["Exit"]

                                      """;
        var builder = _builder
            .Do("Start", _ => { })
            .If("LoopCondition", _ => true,
                t => t.Do("LoopAction", _ => { })
                      .JumpTo("LoopCondition"),
                f => f.Do("Exit", _ => { }));

        var graph = builder.BuildGraph();

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_WithGenericAction_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["CustomAction"]

                                      """;

        var builder = _builder
            .Do<CustomAction>();

        var graph = builder.BuildGraph();

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

        var builder = _builder
            .If(_ => true,
                t1 => t1.If(_ => false,
                    t2 => t2.Do(_ => { }),
                    f2 => f2.Do(_ => { })),
                f1 => f1.Do(_ => { }));

        var graph = builder.BuildGraph();

        graph.Should().Be(expectedResult);
    }

    // ReSharper disable once ClassNeverInstantiated.Local - Test class
    private sealed class CustomAction
        : ActionNode<CustomAction> {
        public CustomAction(uint id, string label, IServiceProvider services)
            : base(id, label, services) {
        }

        public CustomAction(uint id, IServiceProvider services)
            : base(id, services) { }

        protected override Task Execute(Context context, CancellationToken ct)
            => Task.CompletedTask;
    }
}
