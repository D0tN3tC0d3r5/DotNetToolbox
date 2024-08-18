namespace DotNetToolbox.Graph.Nodes;

public class ExitNodeTests {
    private readonly INodeFactory _factory;

    public ExitNodeTests() {
        var services = new ServiceCollection();
        services.AddTransient<INodeFactory>(p => new NodeFactory(p));
        var provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<INodeFactory>();
    }

    [Fact]
    public void CreateStop_WithoutLabel_ReturnsTerminalNodeWithDefaultLabel() {
        var node = _factory.CreateExit("1");

        node.Should().NotBeNull();
        node.Should().BeOfType<ExitNode>();
        node.Label.Should().Be("end");
    }

    [Fact]
    public void CreateStop_WithCustomLabel_ReturnsTerminalNodeWithCustomLabel() {
        const int customCode = -2;
        const string customTag = "CustomEnd";
        var node = _factory.CreateExit(customTag, customCode);

        node.Should().NotBeNull();
        var exitNode = node.Should().BeOfType<ExitNode>().Subject;
        exitNode.ExitCode.Should().Be(customCode);
        exitNode.Id.Should().Be(customTag);
    }
}
