namespace DotNetToolbox.Graph.Nodes;

public class NodeFactoryTests {
    private readonly NodeFactory _factory = new NodeFactory();

    [Fact]
    public void Start_ReturnsEntryNode() {
        var node = _factory.Start;
        node.Should().NotBeNull();
        node.Should().BeOfType<EntryNode>();
    }

    [Fact]
    public void If_ReturnsIfNode() {
        var node = _factory.If(_ => true, _ => { });
        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
    }

    [Fact]
    public void If_WithElse_ReturnsIfNode() {
        var node = _factory.If(_ => false, _ => { }, _ => { });
        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
    }

    [Fact]
    public void Select_ReturnsSelectNode() {
        var node = _factory.Select(_ => "1", b => {
            b.Case("1", _ => { });
            b.Case("2", _ => { });
        });
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<BranchingNode>();
    }

    [Fact]
    public void Do_ReturnsActionNode() {
        var node = _factory.Do(_ => { }, _ => { });
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<ActionNode>();
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
        var node1 = _factory.Do(_ => { });
        node1.Next = _factory.Do(_ => { }, _ => { });
        var result = node1.Validate();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Run_ReturnsResult() {
        var node = _factory.Do(c => c["Hello"] = "World!");
        var context = new Context();
        var result = node.Run(context);
        result.Should().BeNull();
        context["Hello"].Should().Be("World!");
    }
}
