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
    public void CreateStop_WithoutId_ReturnsTerminalNodeWithDefaultLabel() {
        var node = _factory.CreateExit();

        node.Should().NotBeNull();
        node.Should().BeOfType<ExitNode>();
        node.ExitCode.Should().Be(0);
        node.Id.Should().Be("1");
        node.Label.Should().Be("end");
    }

    [Fact]
    public void CreateStop_WithCustomId_ReturnsTerminalNodeWithCustomLabel() {
        const int customCode = -2;
        const string customId = "CustomEnd";
        var node = _factory.CreateExit(customId, customCode);

        node.Should().NotBeNull();
        var exitNode = node.Should().BeOfType<ExitNode>().Subject;
        exitNode.ExitCode.Should().Be(customCode);
        exitNode.Id.Should().Be(customId);
        node.Label.Should().Be(customId);
    }
}
