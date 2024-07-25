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
        var actionExecuted = false;
        var policy = new TestPolicy();
        var node = _factory.CreateAction(1, _ => actionExecuted = true, policy);
        var context = new Context();

        node.Run(context);

        policy.TryCount.Should().Be(1);
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void CreateAction_RunWithSuccessfulRetry_ExecutesPolicyAndAction() {
        var actionExecuted = false;
        var policy = new TestPolicy(failedTries: 2);
        var node = _factory.CreateAction(1, _ => actionExecuted = true, policy);
        var context = new Context();

        node.Run(context);

        policy.TryCount.Should().Be(3);
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void CreateAction_RunWithTooManyRetries_ExecutesPolicyAndAction() {
        var actionExecuted = false;
        var policy = new TestPolicy(failedTries: RetryPolicy.DefaultMaximumRetries + 1);
        var node = _factory.CreateAction(1, _ => actionExecuted = true, policy);
        var context = new Context();

        var action = () => node.Run(context);

        action.Should().Throw<PolicyException>();
        policy.TryCount.Should().Be(RetryPolicy.DefaultMaximumRetries);
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void CreateAction_RunWithCustomRetries_ExecutesPolicyAndAction() {
        var actionExecuted = false;
        var policy = new TestPolicy(maxRetries: 10, failedTries: 11);
        var node = _factory.CreateAction(1, _ => actionExecuted = true, policy);
        var context = new Context();

        var action = () => node.Run(context);

        action.Should().Throw<PolicyException>();
        policy.TryCount.Should().Be(10);
        actionExecuted.Should().BeTrue();
    }

    [Fact]
    public void CreateAction_RetryOnException_ExecutesPolicyAndAction() {
        var policy = new TestPolicy(failedTries: RetryPolicy.DefaultMaximumRetries + 1);
        var node = _factory.CreateAction(1, _ => throw new(), policy);
        var context = new Context();

        var action = () => node.Run(context);

        action.Should().Throw<PolicyException>();
        policy.TryCount.Should().Be(RetryPolicy.DefaultMaximumRetries);
    }

    [Fact]
    public void CreateAction_WithRetryAsMax_IgnoresPolicyAndTryAsManyTimesAsNeeded() {
        var policy = new TestPolicy(maxRetries: byte.MaxValue);
        var node = _factory.CreateAction(1, _ => {
            if (policy.TryCount < 10) throw new();
        }, policy);
        var context = new Context();

        var action = () => node.Run(context);

        action.Should().NotThrow<PolicyException>();
        policy.TryCount.Should().Be(10);
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

    private class TestPolicy(byte maxRetries = RetryPolicy.DefaultMaximumRetries, uint failedTries = 0)
        : RetryPolicy(maxRetries) {
        protected override bool TryExecute(Action action) {
            TryCount++;
            action();
            return TryCount > failedTries;
        }
        public uint TryCount { get; private set; }
    }
}
