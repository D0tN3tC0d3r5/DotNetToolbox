namespace DotNetToolbox.Graph.PathBuilder;

public class PathBuilderTests {
    [Fact]
    public void If_WithPredicate_AddsIfNode() {
        var path = Start.If(_ => true);
        path.Should().NotBeNull();
        path.Should().BeOfType<IThenBuilder>();
    }

    [Fact]
    public void Then_WithAction_SetsTruePath() {
        var path = Start.If(_ => true).Then(_ => { });
        path.Should().NotBeNull();
        path.Should().BeAssignableTo<IElseBuilder>();
    }

    [Fact]
    public void Else_WithNode_SetsFalsePath() {
        var path = Start.If(_ => false).Then(_ => { }).Else(_ => { });
        path.Should().NotBeNull();
        path.Should().BeAssignableTo<IPathBuilder>();
    }
    [Fact]
    public void ElseIf_WithPredicate_AddsElseIfNode() {
        var path = Start.If(_ => false).Then(_ => { }).ElseIf(_ => true);
        path.Should().NotBeNull();
        path.Should().BeAssignableTo<IThenBuilder>();
    }
}
