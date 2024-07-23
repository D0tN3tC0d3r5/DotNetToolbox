using System.Text.RegularExpressions;

namespace DotNetToolbox.Graph;

public class WorkflowBuilderTests {
    private readonly INodeFactory _nodeFactory = new NodeFactory();
    private readonly WorkflowBuilder _builder;

    public WorkflowBuilderTests() {
        _builder = new(_nodeFactory);
    }


    [Fact]
    public void BuildGraph_SingleAction_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      graph TD
                                          1["Action"]

                                      """;
        _builder.Do(_ => { });

        var graph = _builder.BuildGraph();

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_TwoConnectedActions_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      graph TD
                                          1["Action"]
                                          1 --> 2
                                          2["Action"]

                                      """;

        _builder.Do(_ => { })
                .Do(_ => { });

        var graph = _builder.BuildGraph();

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_IfNode_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      graph TD
                                          1["If"]
                                          1 --> |True| 2
                                          2["Action"]
                                          1 --> |False| 3
                                          3["Action"]

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
                                      graph TD
                                          1["Map"]
                                          1 --> |key1| 2
                                          2["Action"]
                                          1 --> |key2| 3
                                          3["Action"]

                                      """;
        _builder.Select(_ => "key1", k => k
                    .Case("key1", b => b.Do(_ => { }))
                    .Case("key2", b => b.Do(_ => { })));

        var graph = _builder.BuildGraph();

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_ComplexWorkflow_ReturnsCorrectMermaidChart() {
        const string expectedResult = """
                                      graph TD
                                          1["Action"]
                                          1 --> 2
                                          2["If"]
                                          2 --> |True| 3
                                          3["Action"]
                                          3 --> 4
                                          4["Map"]
                                          4 --> |key1| 5
                                          5["Action"]
                                          4 --> |key2| 6
                                          6["Action"]
                                          2 --> |False| 7
                                          7["Action"]

                                      """;

        _builder.Do(_ => { })
                .If(_ => true,
                    t => t.Do(_ => { })
                          .Select(_ => "key1", k => k
                              .Case("key1", branch => branch.Do(_ => { }))
                              .Case("key2", branch => branch.Do(_ => { }))),
                    f => f.Do(_ => { }));

        var graph = _builder.BuildGraph();

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_WithLabels_IncludesLabelsInMermaidChart() {
        const string expectedResult = """
                                      graph TD
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
                    setTrueBranch: b => b.Do("Success", _ => { }),
                    setFalseBranch: b => b.Do("Fail", _ => { }));

        var graph = _builder.BuildGraph();

        graph.Should().Be(expectedResult);
    }

    [Fact]
    public void BuildGraph_WithLoop_HandlesLoopCorrectly() {
        var builder = new WorkflowBuilder(_nodeFactory)
            .Do("Start", _ => { })
            .If("LoopCondition", _ => true,
                setTrueBranch: b => b
                    .Do("LoopAction", _ => { })
                    .JumpTo("LoopCondition"),
                setFalseBranch: b => b.Do("Exit", _ => { }));

        var graph = builder.BuildGraph();

        graph.Should().Contain("Start");
        graph.Should().Contain("LoopCondition");
        graph.Should().Contain("LoopAction");
        graph.Should().Contain("Exit");
        graph.Should().MatchRegex(@"node\w+ --> node\w+"); // Check for connection back to LoopCondition
    }

    [Fact]
    public void BuildGraph_WithGenericAction_ReturnsCorrectMermaidChart() {
        var builder = new WorkflowBuilder(_nodeFactory)
            .Do<CustomAction>();

        var graph = builder.BuildGraph();

        graph.Should().MatchRegex(@"graph TD\s+node\w+\[""Action""\]");
    }

    [Fact]
    public void BuildGraph_WithMultipleBranches_ReturnsCorrectMermaidChart() {
        var builder = new WorkflowBuilder(_nodeFactory)
            .Select(_ => _["key"] as string,
                options => options
                    .Case("A", b => b.Do(_ => { }))
                    .Case("B", b => b.Do(_ => { }))
                    .Case("C", b => b.Do(_ => { })));

        var graph = builder.BuildGraph();

        graph.Should().MatchRegex(@"graph TD\s+node\w+\[""Map""\]");
        graph.Should().Contain("|A|");
        graph.Should().Contain("|B|");
        graph.Should().Contain("|C|");
    }

    [Fact]
    public void BuildGraph_WithNestedConditions_ReturnsCorrectMermaidChart() {
        var builder = new WorkflowBuilder(_nodeFactory)
            .If(_ => true,
                setTrueBranch: b => b
                    .If(_ => false,
                        setTrueBranch: inner => inner.Do(_ => { }),
                        setFalseBranch: inner => inner.Do(_ => { })),
                setFalseBranch: b => b.Do(_ => { }));

        var graph = builder.BuildGraph();

        var ifNodeCount = Regex.Matches(graph, @"\[""If""\]").Count;
        ifNodeCount.Should().Be(2);
    }

    private class CustomAction(INodeFactory? factory = null)
        : ActionNode<CustomAction>(factory) {
        protected override void Execute(Context context) { }
    }
}
