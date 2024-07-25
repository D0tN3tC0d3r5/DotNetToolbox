namespace DotNetToolbox.Graph.Nodes;

public class BranchingNodeTests {
    private readonly NodeFactory _factory = new();

    [Fact]
    public void CreateChoice_WithoutLabel_ReturnsBranchingNodeWithDefaultLabel() {
        var builder = new WorkflowBuilder();
        var node = _factory.CreateChoice(1, _ => "default", builder, _ => { });

        node.Should().NotBeNull();
        node.Should().BeOfType<BranchingNode>();
        node.Label.Should().Be("case");
    }

    [Fact]
    public void CreateChoice_WithCustomLabel_ReturnsBranchingNodeWithCustomLabel() {
        const string customLabel = "CustomChoice";
        var builder = new WorkflowBuilder();
        var node = _factory.CreateChoice(1, customLabel, _ => "default", builder, _ => { });

        node.Should().NotBeNull();
        node.Should().BeOfType<BranchingNode>();
        node.Label.Should().Be(customLabel);
    }

    [Fact]
    public void CreateChoice_WithGenericType_ReturnsCustomBranchingNode() {
        var node = _factory.CreateChoice<CustomBranchingNode>(1);

        node.Should().NotBeNull();
        node.Should().BeOfType<CustomBranchingNode>();
    }

    [Fact]
    public void CreateChoice_WithMultipleBranches_SetsAllBranches() {
        var builder = new WorkflowBuilder();
        var node = _factory.CreateChoice(1, _ => "key", builder, b => b
                                                                     .Is("key1", _ => { })
                                                                     .Is("key2", _ => { })
                                                                     .Is("key3", _ => { }));

        node.Should().BeOfType<BranchingNode>();
        var branchingNode = (BranchingNode)node;
        branchingNode.Choices.Should().HaveCount(3);
        branchingNode.Choices.Should().ContainKeys("key1", "key2", "key3");
    }

    [Fact]
    public void CreateChoice_RunMethodWithExistingKey_ExecutesCorrectBranch() {
        using var context = new Context();
        var builder = new WorkflowBuilder();
        var node = _factory.CreateChoice(1, _ => "key2", builder, b => b
                                                                      .Is("key1", br => br.Do(ctx => ctx["branch"] = "1"))
                                                                      .Is("key2", br => br.Do(ctx => ctx["branch"] = "2"))
                                                                      .Is("key3", br => br.Do(ctx => ctx["branch"] = "3")));

        node.Run(context);

        context["branch"].Should().Be("2");
    }

    [Fact]
    public void CreateChoice_RunMethodWithNonExistingKey_ThrowsInvalidOperationException() {
        var builder = new WorkflowBuilder();
        var node = _factory.CreateChoice(1, _ => "nonexistent", builder, b => b
                                                                             .Is("key1", _ => { })
                                                                             .Is("key2", _ => { }));

        var action = () => {
            using var context = new Context();
            return node.Run(context);
        };

        action.Should().Throw<InvalidOperationException>()
              .WithMessage("The path 'nonexistent' was not found.");
    }

    [Fact]
    public void CreateChoice_ValidateMethod_ValidatesAllBranches() {
        var builder = new WorkflowBuilder();
        var node = _factory.CreateChoice(1, _ => "key", builder, b => b
                                                                     .Is("key1", br => br.Do(_ => { }))
                                                                     .Is("key2", br => br.Do(_ => { }))
                                                                     .Is("key3", _ => { }));

        var result = node.Validate();

        result.IsSuccess.Should().BeTrue();
    }

    private class CustomBranchingNode(uint id, string? label = null)
        : BranchingNode<CustomBranchingNode>(id, label) {
        protected override string Select(Context context) => "default";
    }
}