namespace DotNetToolbox.Graph.PathBuilder;

public class NodeTests {
    [Fact]
    public void Void_ReturnsVoidNode() {
        var node = Node.Void;
        node.Should().NotBeNull();
        node.Should().BeOfType<VoidNode>();
    }

    [Fact]
    public void If_ReturnsIfNode() {
        var node = Node.If(_ => true, Node.Void);
        node.Should().NotBeNull();
        node.Should().BeOfType<IfNode>();
    }

    [Fact]
    public void If_WithId_ReturnsIfNode() {
        var node = Node.If("42", _ => true, Node.Void);
        node.Should().NotBeNull();
        node.Should().BeOfType<IfNode>();
        node.Id.Should().Be("42");
    }

    [Fact]
    public void If_WithElse_ReturnsIfNode() {
        var node = Node.If(_ => false, Node.Void, Node.Void);
        node.Should().NotBeNull();
        node.Should().BeOfType<IfNode>();
    }

    [Fact]
    public void Select_ReturnsSelectNode() {
        var node = Node.Select(_ => 1, new Dictionary<int, INode?> {
            [1] = Node.Void,
            [2] = null,
        });
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<SelectNode<int>>();
    }

    [Fact]
    public void Select_WithId_ReturnsSelectNode() {
        var node = Node.Select("42", _ => "ThisOne", new Dictionary<string, INode?> {
            ["ThisOne"] = null,
        });
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<SelectNode<string>>();
        node.Id.Should().Be("42");
    }

    [Fact]
    public void Select_WithoutKey_ReturnsSelectNode() {
        var node = Node.Select(_ => Node.Void.Id, [
            Node.Do(_ => { }),
            Node.Void,
            null,
        ]);
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<SelectNode<string>>();
    }

    [Fact]
    public void Select_WithIdAndWithoutKey_ReturnsSelectNode() {
        var node = Node.Select("42", _ => Node.Void.Id, [
            Node.Do(_ => { }),
            Node.Void,
            null,
        ]);
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<SelectNode<string>>();
        node.Id.Should().Be("42");
    }

    [Fact]
    public void Do_ReturnsActionNode() {
        var node = Node.Do(_ => { }, Node.Void);
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<ActionNode>();
    }

    [Fact]
    public void Do_WithId_ReturnsActionNode() {
        var node = Node.Do("42", _ => { }, Node.Void);
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<ActionNode>();
        node.Id.Should().Be("42");
    }

    private class ValidTestNode : Node {
        protected override void UpdateState(Context state) => throw new NotImplementedException();

        protected override INode? GetNext(Context state) => throw new NotImplementedException();
    }

    private class InvalidTestNode : Node {
        protected override Result IsValid() => Result.Invalid("Not valid.");
        protected override void UpdateState(Context state) => throw new NotImplementedException();

        protected override INode? GetNext(Context state) => throw new NotImplementedException();
    }

    [Fact]
    public void Validate_ForValidNode_ReturnsSuccess() {
        var node = new ValidTestNode();
        var result = node.Validate();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Validate_ForInvalidNode_ReturnsInvalid() {
        var node = new InvalidTestNode();
        var result = node.Validate();
        result.IsSuccess.Should().BeFalse();
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain("Not valid.");
    }

    [Fact]
    public void Validate_ForCircularPath_ReturnsResult() {
        var node1 = Node.Do(_ => { });
        node1.Next = Node.Do(_ => { }, node1);
        var result = node1.Validate();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Run_ReturnsResult() {
        var node = Node.Do(c => c["Hello"] = "World!");
        var context = new Context();
        var result = node.Run(context);
        result.Should().BeNull();
        context["Hello"].Should().Be("World!");
    }
}
