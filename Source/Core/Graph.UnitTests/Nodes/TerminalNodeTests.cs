namespace DotNetToolbox.Graph.Nodes;

public class TerminalNodeTests {
    private readonly NodeFactory _factory = new();

    [Fact]
    public void CreateStop_WithoutLabel_ReturnsTerminalNodeWithDefaultLabel() {
        var node = _factory.CreateStop(1);

        node.Should().NotBeNull();
        node.Should().BeOfType<TerminalNode>();
        node.Label.Should().Be("end");
    }

    [Fact]
    public void CreateStop_WithCustomLabel_ReturnsTerminalNodeWithCustomLabel() {
        const string customLabel = "CustomEnd";
        var node = _factory.CreateStop(1, customLabel);

        node.Should().NotBeNull();
        node.Should().BeOfType<TerminalNode>();
        node.Label.Should().Be(customLabel);
    }
}