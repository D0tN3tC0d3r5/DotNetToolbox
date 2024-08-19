namespace DotNetToolbox.Graph.Nodes;

public class CaseNodeTests {
    private readonly INodeFactory _factory;

    public CaseNodeTests() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        services.AddScoped<INodeFactory, NodeFactory>();
        var provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<INodeFactory>();
    }

    [Fact]
    public void CreateCase_WithoutId_ReturnsBranchingNodeWithDefaultLabel() {
        var node = _factory.CreateCase(_ => "default", []);

        node.Should().NotBeNull();
        node.Should().BeOfType<CaseNode>();
        node.Id.Should().Be("1");
        node.Label.Should().Be("case");
    }

    [Fact]
    public void CreateCase_WithCustomId_ReturnsBranchingNodeWithCustomLabel() {
        const string customId = "Action1";
        var node = _factory.CreateCase(customId, _ => "default", []);

        node.Should().NotBeNull();
        node.Should().BeOfType<CaseNode>();
        node.Id.Should().Be(customId);
        node.Label.Should().Be(customId);
    }

    [Fact]
    public void CreateCase_WithMultipleBranches_SetsAllBranches() {
        var node = _factory.CreateCase("1",
                                         _ => "key",
                                         new() {
                                             ["key1"] = null,
                                             ["key2"] = null,
                                             ["key3"] = null,
                                         },
                                         _factory.CreateAction("o", ctx => { }));

        node.Should().BeOfType<CaseNode>();
        var branchingNode = (CaseNode)node;
        branchingNode.Choices.Should().HaveCount(4);
        branchingNode.Choices.Should().ContainKeys("key1", "key2", "key3", "");
    }

    [Fact]
    public async Task Run_MethodWithExistingKey_ExecutesCorrectBranch() {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        using var context = new Context(provider);
        var node = _factory.CreateCase("1",
                                         _ => "key2",
                                         new() {
                                             ["key1"] = _factory.CreateAction("k1", ctx => ctx["branch"] = "1"),
                                             ["key2"] = _factory.CreateAction("k2", ctx => ctx["branch"] = "2"),
                                             ["key3"] = _factory.CreateAction("k3", ctx => ctx["branch"] = "3"),
                                         });

        await node.Run(context);

        context["branch"].Should().Be("2");
    }

    [Fact]
    public async Task Run_MethodWithNonExistingKey_ThrowsInvalidOperationException() {
        var node = _factory.CreateCase("1",
                                         _ => "nonexistent",
                                         new() {
                                             ["key1"] = _factory.CreateAction("k1", ctx => ctx["branch"] = "1"),
                                             ["key2"] = _factory.CreateAction("k2", ctx => ctx["branch"] = "2"),
                                         });

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
    public async Task Run_MethodWithNonExistingKeyAndWithOtherwise_ExecutesOtherwise() {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        using var context = new Context(provider);
        var node = _factory.CreateCase("1",
                                         _ => "nonexistent",
                                         new() {
                                             ["key1"] = _factory.CreateAction("k1", ctx => ctx["branch"] = "1"),
                                             ["key2"] = _factory.CreateAction("k2", ctx => ctx["branch"] = "2"),
                                         },
                                         _factory.CreateAction("o", ctx => ctx["branch"] = "9"));

        await node.Run(context);

        context["branch"].Should().Be("9");
    }

    [Fact]
    public void CreateCase_ValidateMethod_ValidatesAllBranches() {
        var node = _factory.CreateCase("1",
                                         _ => "key",
                                         new() {
                                             ["key1"] = null,
                                             ["key2"] = null,
                                             ["key3"] = null,
                                         });

        var result = node.Validate();

        result.IsSuccess.Should().BeTrue();
    }
}
