namespace DotNetToolbox.Graph.Nodes;

public class ActionNodeTests {
    private readonly NodeFactory _factory = new();

    [Fact]
    public void CreateAction_WithoutLabel_ReturnsActionNodeWithDefaultLabel() {
        var node = _factory.CreateAction(1, _ => { });

        node.Should().NotBeNull();
        node.Should().BeOfType<ActionNode>();
        node.Label.Should().Be("action");
    }

    [Fact]
    public void CreateAction_WithCustomLabel_ReturnsActionNodeWithCustomLabel() {
        const string customLabel = "CustomAction";
        var node = _factory.CreateAction(1, customLabel, _ => { });

        node.Should().NotBeNull();
        node.Should().BeOfType<ActionNode>();
        node.Label.Should().Be(customLabel);
    }

    [Fact]
    public void CreateAction_WithGenericType_ReturnsCustomActionNode() {
        var node = _factory.CreateAction<CustomActionNode>(1);

        node.Should().NotBeNull();
        node.Should().BeOfType<CustomActionNode>();
    }

    [Fact]
    public void CreateAction_WithPolicy_AppliesPolicyToNode() {
        var policy = new TestPolicy();
        var node = _factory.CreateAction(1, _ => { }, policy);

        node.Should().NotBeNull();
        node.Should().BeOfType<ActionNode>();
    }

    [Fact]
    public void CreateAction_RunWithPolicy_ExecutesPolicyAndAction() {
        var policyExecuted = false;
        var actionExecuted = false;
        var policy = new TestPolicy(() => policyExecuted = true);
        var node = _factory.CreateAction(1, _ => actionExecuted = true, policy);
        var context = new Context();

        node.Run(context);

        policyExecuted.Should().BeTrue();
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void CreateAction_RunMethod_UpdatesContextAndReturnsNextNode() {
        var context = new Context();
        var nextNode = _factory.CreateAction(1, _ => { });
        var node = _factory.CreateAction(2, ctx => ctx["key"] = "value");
        node.Next = nextNode;

        var result = node.Run(context);

        result.Should().BeSameAs(nextNode);
        context["key"].Should().Be("value");
    }

    private class CustomActionNode(uint id, string? label = null, IPolicy? policy = null)
        : ActionNode<CustomActionNode>(id, label, policy) {
        protected override void Execute(Context context) { }
    }

    private class TestPolicy(Action? onExecute = null)
        : IPolicy {

        public void Execute(Action action) {
            onExecute?.Invoke();
            action();
        }
    }
}
