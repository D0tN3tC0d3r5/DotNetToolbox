namespace DotNetToolbox.Graph.Nodes;

public class GenericNodeTests {
    private readonly INodeFactory _factory;

    public GenericNodeTests() {
        var services = new ServiceCollection();
        services.AddTransient<INodeFactory>(p => new NodeFactory(p));
        var provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<INodeFactory>();
    }

    [Fact]
    public void CreateNode_WithGenericType_ReturnsCustomActionNode() {
        var node = _factory.Create<GenericNode>("1");

        node.Should().NotBeNull();
        node.Should().BeOfType<GenericNode>();
    }

    public sealed class GenericNode(string tag)
        : ActionNode<GenericNode>(tag, null, null) {
        protected override Task Execute(Context context, CancellationToken ct) => Task.CompletedTask;
    }
}
