namespace DotNetToolbox.Graph.PathBuilder;

public class NodeTests {
    [Fact]
    public void Void_ReturnsVoidNode() {
        var path = Node.Void;
        path.Should().NotBeNull();
        path.Should().BeOfType<VoidNode>();
    }

    [Fact]
    public void If_ReturnsIfNode() {
        var path = Node.If(_ => true, Node.Void);
        path.Should().NotBeNull();
        path.Should().BeOfType<IfNode>();
    }

    [Fact]
    public void If_WithElse_ReturnsIfNode() {
        var path = Node.If(_ => false, Node.Void, Node.Void);
        path.Should().NotBeNull();
        path.Should().BeOfType<IfNode>();
    }

    [Fact]
    public void Select_ReturnsSelectNode() {
        var path = Node.Select(_ => 1, new Dictionary<int, INode?> {
            [1] = Node.Void,
            [2] = null,
        });
        path.Should().NotBeNull();
        path.Should().BeAssignableTo<SelectNode<int>>();
    }

    [Fact]
    public void Select_WithoutKey_ReturnsSelectNode() {
        var path = Node.Select(_ => Node.Void.Id, [
            Node.Do(_ => { }),
            Node.Void,
            null,
        ]);
        path.Should().NotBeNull();
        path.Should().BeAssignableTo<SelectNode<string>>();
    }

    [Fact]
    public void Do_ReturnsActionNode() {
        var path = Node.Do(_ => { }, Node.Void);
        path.Should().NotBeNull();
        path.Should().BeAssignableTo<ActionNode>();
    }
}
