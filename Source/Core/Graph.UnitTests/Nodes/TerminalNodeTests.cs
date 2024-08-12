namespace DotNetToolbox.Graph.Nodes;

public class TerminalNodeTests {
    private readonly NodeFactory _factory;

    public TerminalNodeTests() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();
        _factory = new(provider);
    }

    [Fact]
    public void CreateStop_WithoutLabel_ReturnsTerminalNodeWithDefaultLabel() {
        var node = _factory.CreateStop(1);

        node.Should().NotBeNull();
        node.Should().BeOfType<TerminalNode>();
        node.Label.Should().Be("end");
    }

    [Fact]
    public void CreateStop_WithCustomLabel_ReturnsTerminalNodeWithCustomLabel() {
        const int customCode = -2;
        const string customTag = "CustomEnd";
        const string customLabel = "Custom End";
        var node = _factory.CreateStop(1, customCode, customTag, customLabel);

        node.Should().NotBeNull();
        node.Should().BeOfType<TerminalNode>();
        node.ExitCode.Should().Be(customCode);
        node.Tag.Should().Be(customTag);
        node.Label.Should().Be(customLabel);
    }
}
