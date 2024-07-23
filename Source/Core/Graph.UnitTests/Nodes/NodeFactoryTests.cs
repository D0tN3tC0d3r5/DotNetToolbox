namespace DotNetToolbox.Graph.Nodes;

public class NodeFactoryTests {
    private readonly NodeFactory _factory = new();

    [Fact]
    public void CreateStart_ReturnsEntryNode() {
        var node = _factory.CreateStart();
        node.Should().NotBeNull();
        node.Should().BeOfType<StartingNode>();
    }

    [Fact]
    public void CreateIf_ReturnsIfNode() {
        var node = _factory.CreateFork(_ => true, _ => { });
        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
    }

    [Fact]
    public void CreateIf_WithElse_ReturnsIfNode() {
        var node = _factory.CreateFork(_ => false, _ => { }, _ => { });
        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
    }

    [Fact]
    public void CreateSelect_ReturnsSelectNode() {
        var node = _factory.CreateChoice(_ => "1", b => {
            b.Case("1", _ => { });
            b.Case("2", _ => { });
        });
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<BranchingNode>();
    }

    [Fact]
    public void CreateAction_ReturnsActionNode() {
        var node = _factory.CreateAction(_ => { });
        node.Should().NotBeNull();
        node.Should().BeAssignableTo<ActionNode>();
    }

    private class ValidTestNode : Node<ValidTestNode> {
        protected override void UpdateState(Context context) => throw new NotImplementedException();
        protected override INode GetNext(Context context) => throw new NotImplementedException();
    }

    private class InvalidTestNode : Node<InvalidTestNode> {
        protected override Result IsValid(ISet<INode> visited) => Result.Invalid("Not valid.");
        protected override void UpdateState(Context context) => throw new NotImplementedException();
        protected override INode GetNext(Context context) => throw new NotImplementedException();
    }

    [Fact]
    public void CreateValidate_ForValidNode_ReturnsSuccess() {
        var node = new ValidTestNode();
        var result = node.Validate();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void CreateValidate_ForInvalidNode_ReturnsInvalid() {
        var node = new InvalidTestNode();
        var result = node.Validate();
        result.IsSuccess.Should().BeFalse();
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain("Not valid.");
    }

    [Fact]
    public void CreateValidate_ForCircularPath_ReturnsResult() {
        var node1 = _factory.CreateAction(_ => { });
        node1.Next = _factory.CreateAction(_ => { });
        var result = node1.Validate();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void CreateRun_ReturnsResult() {
        var node = _factory.CreateAction(c => c["Hello"] = "World!");
        var context = new Context();
        var result = node.Run(context);
        result.Should().BeNull();
        context["Hello"].Should().Be("World!");
    }
}
