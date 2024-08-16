namespace DotNetToolbox.Graph.Nodes;

public class CaseNodeTests {
    private readonly ServiceProvider _provider;
    private readonly NodeFactory _factory;

    public CaseNodeTests() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        services.AddTransient<INodeFactory>(p => new NodeFactory(p));
        _provider = services.BuildServiceProvider();
        _factory = new NodeFactory(_provider);
    }

    [Fact]
    public void CreateChoice_WithoutLabel_ReturnsBranchingNodeWithDefaultLabel() {
        var node = _factory.CreateChoice(1, _ => "default", _ => { });

        node.Should().NotBeNull();
        node.Should().BeOfType<CaseNode>();
        node.Label.Should().Be("case");
    }

    [Fact]
    public void CreateChoice_WithCustomLabel_ReturnsBranchingNodeWithCustomLabel() {
        const string customLabel = "Custom Choice";
        const string customTag = "Action1";
        var node = _factory.CreateChoice(1, _ => "default", _ => { }, customTag, customLabel);

        node.Should().NotBeNull();
        node.Should().BeOfType<CaseNode>();
        node.Label.Should().Be(customLabel);
    }

    [Fact]
    public void CreateChoice_WithMultipleBranches_SetsAllBranches() {
        var node = _factory.CreateChoice(1,
                                         _ => "key",
                                         b => b.Is("key1", _ => { })
                                               .Is("key2", _ => { })
                                               .Is("key3", _ => { }));

        node.Should().BeOfType<CaseNode>();
        var branchingNode = (CaseNode)node;
        branchingNode.Choices.Should().HaveCount(3);
        branchingNode.Choices.Should().ContainKeys("key1", "key2", "key3");
    }

    [Fact]
    public async Task Run_MethodWithExistingKey_ExecutesCorrectBranch() {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        using var context = new Context(provider);
        var node = _factory.CreateChoice(1,
                                         _ => "key2",
                                         b => b.Is("key1", br => br.Do(ctx => ctx["branch"] = "1"))
                                               .Is("key2", br => br.Do(ctx => ctx["branch"] = "2"))
                                               .Is("key3", br => br.Do(ctx => ctx["branch"] = "3")));

        await node.Run(context);

        context["branch"].Should().Be("2");
    }

    [Fact]
    public async Task Run_MethodWithNonExistingKey_ThrowsInvalidOperationException() {
        var node = _factory.CreateChoice(1,
                                         _ => "nonexistent",
                                         b => b.Is("key1", _ => { })
                                               .Is("key2", _ => { }));

        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();

        var action = async () => {
            using var context = new Context(provider);
            return await node.Run(context);
        };

        await action.Should().ThrowAsync<InvalidOperationException>()
              .WithMessage("The path 'nonexistent' was not found.");
    }

    [Fact]
    public void CreateChoice_ValidateMethod_ValidatesAllBranches() {
        var node = _factory.CreateChoice(1,
                                         _ => "key",
                                         b => b.Is("key1", c => c.Do(_ => { }))
                                               .Is("key2", c => c.Do(_ => { }))
                                               .Is("key3", _ => { }));

        var result = node.Validate();

        result.IsSuccess.Should().BeTrue();
    }
}
