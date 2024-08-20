namespace DotNetToolbox.Graph.Nodes;

public partial class NodeTests {
    public class ExitNodeTests : NodeTests {
        [Fact]
        public void CreateStop_WithoutTag_ReturnsTerminalNodeWithDefaultLabel() {
            var node = CreateFactory().CreateExit();

            node.Should().NotBeNull();
            node.Should().BeOfType<ExitNode>();
            node.ExitCode.Should().Be(0);
            node.Id.Should().Be(1);
            node.Tag.Should().BeNull();
            node.Label.Should().Be("exit");
        }

        [Fact]
        public void CreateStop_WithCustomTag_ReturnsTerminalNodeWithCustomLabel() {
            const int customCode = -2;
            const string customId = "CustomEnd";
            var node = CreateFactory().CreateExit(customId, customCode);

            node.Should().NotBeNull();
            var exitNode = node.Should().BeOfType<ExitNode>().Subject;
            exitNode.ExitCode.Should().Be(customCode);
            exitNode.Tag.Should().Be(customId);
            node.Label.Should().Be("exit");
        }
    }
}
