namespace DotNetToolbox.Graph.Nodes;

public class NodeTests {
    [Fact]
    public void Void_ReturnsVoidNode() {
        var node = Node.Empty;
        node.Should().NotBeNull();
        node.Should().BeOfType<VoidNode>();
    }

    [Fact]
    public void If_ReturnsIfNode() {
        var node = Node.If(_ => true, Node.Empty);
        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
    }

    [Fact]
    public void If_WithId_ReturnsIfNode() {
        var node = Node.If("42", _ => true, Node.Empty);
        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
        node.Id.Should().Be("42");
    }

    [Fact]
    public void If_WithElse_ReturnsIfNode() {
        var node = Node.If(_ => false, Node.Empty, Node.Empty);
        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
    }

    [Fact]
    public void Select_ReturnsSelectNode() {
        var node = Node.Map(_ => 1, new Dictionary<int, INode?> {
            [1] = Node.Empty,
            [2] = null,
        });
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<BranchingNode<int>>();
    }

    [Fact]
    public void Select_WithId_ReturnsSelectNode() {
        var node = Node.Map("42", _ => "ThisOne", new Dictionary<string, INode?> {
            ["ThisOne"] = null,
        });
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<BranchingNode<string>>();
        node.Id.Should().Be("42");
    }

    [Fact]
    public void Select_WithoutKey_ReturnsSelectNode() {
        var node = Node.Map(_ => Node.Empty.Id, [
            Node.Do(_ => { }),
            Node.Empty,
            null,
        ]);
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<BranchingNode<string>>();
    }

    [Fact]
    public void Select_WithIdAndWithoutKey_ReturnsSelectNode() {
        var node = Node.Map("42", _ => Node.Empty.Id, [
            Node.Do(_ => { }),
            Node.Empty,
            null,
        ]);
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<BranchingNode<string>>();
        node.Id.Should().Be("42");
    }

    [Fact]
    public void Do_ReturnsActionNode() {
        var node = Node.Do(_ => { }, Node.Empty);
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<ActionNode>();
    }

    [Fact]
    public void Do_WithId_ReturnsActionNode() {
        var node = Node.Do("42", _ => { }, Node.Empty);
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<ActionNode>();
        node.Id.Should().Be("42");
    }

    private class ValidTestNode : Node {
        protected override void UpdateState(Context context) => throw new NotImplementedException();
        protected override INode GetNext(Context context) => throw new NotImplementedException();
    }

    private class InvalidTestNode : Node {
        protected override Result IsValid() => Result.Invalid("Not valid.");
        protected override void UpdateState(Context context) => throw new NotImplementedException();
        protected override INode GetNext(Context context) => throw new NotImplementedException();
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

    [Fact]
    public void Run_Chain_ReturnsResult() {
        var node1 = Node.Do(c => c["Hello"] = "World!")
                    .Then.Do;
        var node2 = Node.Do(c => c["Hello"] = "World!").Next = node1;
        var context = new Context();
        var result = node.Run(context);
        result.Should().BeNull();
        context["Hello"].Should().Be("World!");
    }
}
