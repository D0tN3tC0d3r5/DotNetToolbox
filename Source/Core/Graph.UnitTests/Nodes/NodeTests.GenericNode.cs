namespace DotNetToolbox.Graph.Nodes;

public partial class NodeTests {
    public class GenericNodeTests : NodeTests {
        [Fact]
        public void CreateNode_WithGenericType_AndCustomProperty_ReturnsCustomActionNode() {
            var node = CreateFactory().Create<GenericNode>("Data");

            node.Should().NotBeNull();
            node.Should().BeOfType<GenericNode>();
            node.Data.Should().Be("Data");
        }

        public sealed class GenericNode(IServiceProvider services, string data)
            : ActionNode<GenericNode>(services) {
            public string Data { get; } = data;

            protected override Task Execute(Context context, CancellationToken ct) => Task.CompletedTask;
        }
    }
}
