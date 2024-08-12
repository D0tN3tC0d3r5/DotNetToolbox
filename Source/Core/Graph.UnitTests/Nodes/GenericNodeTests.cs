namespace DotNetToolbox.Graph.Nodes;

public class GenericNodeTests {
    private readonly NodeFactory _factory;

    public GenericNodeTests() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();
        _factory = new(provider);
    }

    [Fact]
    public void CreateNode_WithGenericType_ReturnsCustomActionNode() {
        var node = _factory.Create<GenericNode>(1);

        node.Should().NotBeNull();
        node.Should().BeOfType<GenericNode>();
    }

    private sealed class GenericNode(uint id, IServiceProvider services, string? tag = null, string? label = null)
        : ActionNode<GenericNode>(id, services, tag, label) {
        protected override Task Execute(Context context, CancellationToken ct) => Task.CompletedTask;
    }
}
