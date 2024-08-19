namespace DotNetToolbox.Graph.Nodes;

public class IfNodeTests {
    private readonly INodeFactory _factory;

    public IfNodeTests() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        services.AddScoped<INodeFactory, NodeFactory>();
        var provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<INodeFactory>();
    }

    [Fact]
    public void CreateIf_WithoutId_ReturnsConditionalNodeWithDefaultLabel() {
        var node = _factory.CreateIf(_ => true,
                                     _factory.CreateAction(_ => { }));

        node.Should().NotBeNull();
        node.Should().BeOfType<IfNode>();
        node.Id.Should().Be("2");
        node.Label.Should().Be("if");
    }

    [Fact]
    public void CreateIf_WithCustomId_ReturnsConditionalNodeWithCustomLabel() {
        const string customTag = "Action1";
        var node = _factory.CreateIf(customTag,
                                       _ => true,
                                       _factory.CreateAction(_ => { }));

        node.Should().NotBeNull();
        node.Should().BeOfType<IfNode>();
        node.Id.Should().Be(customTag);
        node.Label.Should().Be(customTag);
    }

    [Fact]
    public void CreateIf_WithTrueBranchOnly_SetsOnlyTrueBranch() {
        var node = _factory.CreateIf("1",
                                       _ => true,
                                       _factory.CreateAction(_ => { }));

        var ifNode = node.Should().BeOfType<IfNode>().Subject;
        ifNode.Then.Should().NotBeNull();
        ifNode.Else.Should().BeNull();
    }

    [Fact]
    public void CreateIf_WithBothBranches_SetsBothBranches() {
        var node = _factory.CreateIf("1",
                                     _ => true,
                                     _factory.CreateAction(_ => { }),
                                     _factory.CreateAction(_ => { }));

        var ifNode = node.Should().BeOfType<IfNode>().Subject;
        ifNode.Then.Should().NotBeNull();
        ifNode.Else.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateIf_RunMethodWithTrueCondition_ExecutesTrueBranch() {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        using var context = new Context(provider);
        var node = _factory.CreateIf("1",
                                     _ => true,
                                     _factory.CreateAction(ctx => ctx["branch"] = "true"),
                                     _factory.CreateAction(ctx => ctx["branch"] = "false"));

        await node.Run(context);

        context["branch"].Should().Be("true");
    }

    private sealed class CustomContext(IServiceProvider provider)
        : Context(provider);

    [Fact]
    public async Task CreateIf_RunMethodWithFalseCondition_ExecutesFalseBranch() {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var context = new CustomContext(provider) {
            ["Disposable"] = new CustomContext(provider),
        };
        var node = _factory.CreateIf("1", _ => false,
                                     _factory.CreateAction(ctx => ctx["branch"] = "true"),
                                     _factory.CreateAction(ctx => ctx["branch"] = "false"));

        await node.Run(context);

        context["branch"].Should().Be("false");
        context.Dispose();
        context.Dispose();
    }

    [Fact]
    public void CreateIf_ValidateMethod_ValidatesBothBranches() {
        var node = _factory.CreateIf("1", _ => true,
                                     _factory.CreateAction(ctx => ctx["branch"] = "true"),
                                     _factory.CreateAction(ctx => ctx["branch"] = "false"));

        var result = node.Validate();

        result.IsSuccess.Should().BeTrue();
    }
}
