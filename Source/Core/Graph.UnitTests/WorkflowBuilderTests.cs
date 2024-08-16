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
        var start = _builder.Do(_ => { }).Build().Value!;

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
                            .Build().Value!;

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
        var start = _builder.If(_ => true, b => b
                                .IsTrue(t => t
                                    .Do(_ => { }))
                                .IsFalse(f => f
                                    .Do(_ => { })))
                            .Build().Value!;

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
        var start = _builder.Case(_ => "key1", k => k
                                                   .Is("key1", b => b.Do(_ => { }))
                                                   .Is("key2", b => b.Do(_ => { }))
                                                   .Is("key3", b => b.Do(_ => { })))
                            .Build().Value!;

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
                            .If(_ => true, b => b
                                .IsTrue(t => t
                                    .Do(_ => { })
                                    .Case(_ => "key1", k => k
                                        .Is("key1", c => c
                                            .Do(_ => { }))
                                        .Is("key2", c => c
                                            .Do(_ => { }))))
                                .IsFalse(f => f
                                    .Do(_ => { })))
                            .Build().Value!;

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
                            .If("Decision", _ => true, b => b
                                .IsTrue(t => t.Do("Success", _ => { }))
                                .IsFalse(f => f.Do("Fail", _ => { })))
                            .Build().Value!;

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
                            .If("LoopCondition", _ => true, b => b
                                .IsTrue(t => t
                                    .Do("LoopAction", _ => { })
                                    .JumpTo("LoopCondition"))
                                .IsFalse(f => f
                                    .Do("Exit", _ => { })))
                            .Build().Value!;
        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_WithGenericAction_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      flowchart TD
                                      1["CustomAction"]

                                      """;
        var start = _builder.Do<CustomAction>().Build().Value!;

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
        var start = _builder.If(_ => true, b1 => b1
                                .IsTrue(t1 => t1
                                    .If(_ => false, b2 => b2
                                        .IsTrue(t2 => t2
                                            .Do(_ => { }))
                                        .IsFalse(f2 => f2
                                            .Do(_ => { }))))
                                .IsFalse(f1 => f1
                                    .Do(_ => { })))
                            .Build().Value!;

        var graph = GraphBuilder.BuildFrom(start);

        graph.Should().Be(expectedResult);
    }

    // ReSharper disable once ClassNeverInstantiated.Local - Test class
    private sealed class CustomAction(IServiceProvider services, uint id, string? tag = null, string? label = null)
                : ActionNode<CustomAction>(services, id, tag, label) {
        protected override Task Execute(Context context, CancellationToken ct)
            => Task.CompletedTask;
    }
}
