namespace DotNetToolbox.Graph.Nodes;

public class EndNodeTests {
    private readonly NodeFactory _factory;

    public EndNodeTests() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();
        _factory = new(provider);
    }

    [Fact]
    public void CreateStop_WithoutLabel_ReturnsTerminalNodeWithDefaultLabel() {
        var node = _factory.CreateExit(1);

        node.Should().NotBeNull();
        node.Should().BeOfType<ExitNode>();
        node.Label.Should().Be("end");
    }

    [Fact]
    public void CreateStop_WithCustomLabel_ReturnsTerminalNodeWithCustomLabel() {
        const int customCode = -2;
        const string customTag = "CustomEnd";
        const string customLabel = "Custom Exit";
        var node = _factory.CreateExit(1, customCode, customTag, customLabel);

        node.Should().NotBeNull();
        node.Should().BeOfType<ExitNode>();
        node.ExitCode.Should().Be(customCode);
        node.Tag.Should().Be(customTag);
        node.Label.Should().Be(customLabel);
    }
}
