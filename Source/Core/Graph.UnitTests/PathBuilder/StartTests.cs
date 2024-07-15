namespace DotNetToolbox.Graph.PathBuilder;

public class StartTests {
    [Fact]
    public void Void_ReturnsVoidNode() {
        var path = Start.Void;
        path.Should().NotBeNull();
        path.Should().BeOfType<VoidNode>();
    }

    [Fact]
    public void If_ReturnsIfNode() {
        var path = Start.If(_ => true, Start.Void);
        path.Should().NotBeNull();
        path.Should().BeOfType<IfNode>();
    }

    [Fact]
    public void If_WithElse_ReturnsIfNode() {
        var path = Start.If(_ => false, Start.Void, Start.Void);
        path.Should().NotBeNull();
        path.Should().BeOfType<IfNode>();
    }

    [Fact]
    public void Select_ReturnsSelectNode() {
        var path = Start.Select(_ => "First", new Dictionary<string, INode?> {
            ["First"] = Start.Void,
            ["Second"] = null,
        });
        path.Should().NotBeNull();
        path.Should().BeAssignableTo<SelectNode>();
    }

    [Fact]
    public void Select_WithoutKey_ReturnsSelectNode() {
        var path = Start.Select(_ => nameof(Start.Void), [
            Start.Void,
            null,
        ]);
        path.Should().NotBeNull();
        path.Should().BeAssignableTo<SelectNode>();
    }

    [Fact]
    public void Do_ReturnsActionNode() {
        var path = Start.Do(_ => { }, Start.Void);
        path.Should().NotBeNull();
        path.Should().BeAssignableTo<ActionNode>();
    }
}
