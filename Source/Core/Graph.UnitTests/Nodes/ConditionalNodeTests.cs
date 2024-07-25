namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNodeTests {
    private readonly NodeFactory _factory = new();

    [Fact]
    public void CreateFork_WithoutLabel_ReturnsConditionalNodeWithDefaultLabel() {
        var builder = new WorkflowBuilder();
        var node = _factory.CreateFork(1, _ => true, builder, _ => { });

        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
        node.Label.Should().Be("if");
    }

    [Fact]
    public void CreateFork_WithCustomLabel_ReturnsConditionalNodeWithCustomLabel() {
        const string customLabel = "CustomFork";
        var builder = new WorkflowBuilder();
        var node = _factory.CreateFork(1, customLabel, _ => true, builder, _ => { });

        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
        node.Label.Should().Be(customLabel);
    }

    [Fact]
    public void CreateFork_WithGenericType_ReturnsCustomConditionalNode() {
        var node = _factory.CreateFork<CustomConditionalNode>(1);

        node.Should().NotBeNull();
        node.Should().BeOfType<CustomConditionalNode>();
    }

    [Fact]
    public void CreateFork_WithTrueBranchOnly_SetsOnlyTrueBranch() {
        var builder = new WorkflowBuilder();
        var node = _factory.CreateFork(1, _ => true, builder, t => t.Do(_ => { }));

        node.Should().BeOfType<ConditionalNode>();
        node.IsTrue.Should().NotBeNull();
        node.IsFalse.Should().BeNull();
    }

    [Fact]
    public void CreateFork_WithBothBranches_SetsBothBranches() {
        var builder = new WorkflowBuilder();
        var node = _factory.CreateFork(1, _ => true, builder,
                                       t => t.Do(_ => { }),
                                       f => f.Do(_ => { }));

        node.Should().BeOfType<ConditionalNode>();
        node.IsTrue.Should().NotBeNull();
        node.IsFalse.Should().NotBeNull();
    }

    [Fact]
    public void CreateFork_RunMethodWithTrueCondition_ExecutesTrueBranch() {
        using var context = new Context();
        var builder = new WorkflowBuilder();
        var node = _factory.CreateFork(1, _ => true, builder,
                                       t => t.Do(ctx => ctx["branch"] = "true"),
                                       f => f.Do(ctx => ctx["branch"] = "false"));

        node.Run(context);

        context["branch"].Should().Be("true");
    }

    private class CustomContext : Context;

    [Fact]
    public void CreateFork_RunMethodWithFalseCondition_ExecutesFalseBranch() {
        var context = new CustomContext();
        context["Disposable"] = new CustomContext();
        var builder = new WorkflowBuilder();
        var node = _factory.CreateFork(1, _ => false, builder,
                                       t => t.Do(ctx => ctx["branch"] = "true"),
                                       f => f.Do(ctx => ctx["branch"] = "false"));

        node.Run(context);

        context["branch"].Should().Be("false");
        context.Dispose();
        context.Dispose();
    }

    [Fact]
    public void CreateFork_ValidateMethod_ValidatesBothBranches() {
        var builder = new WorkflowBuilder();
        var node = _factory.CreateFork(1, _ => true, builder,
                                       t => t.Do(_ => { }),
                                       f => f.Do(_ => { }));

        var result = node.Validate();

        result.IsSuccess.Should().BeTrue();
    }

    private class CustomConditionalNode(uint id, string? label = null)
        : ConditionalNode<CustomConditionalNode>(id, label) {
        protected override bool When(Context context) => true;
    }
}
