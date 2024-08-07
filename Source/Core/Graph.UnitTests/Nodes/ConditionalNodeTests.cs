namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNodeTests {
    private readonly NodeFactory _factory;
    private readonly WorkflowBuilder _builder;

    public ConditionalNodeTests() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();
        _factory = new(provider);
        _builder = new(provider);
    }

    [Fact]
    public void CreateFork_WithoutLabel_ReturnsConditionalNodeWithDefaultLabel() {
        var node = _factory.CreateFork(1, _ => true, _builder, _ => { });

        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
        node.Label.Should().Be("if");
    }

    [Fact]
    public void CreateFork_WithCustomLabel_ReturnsConditionalNodeWithCustomLabel() {
        const string customLabel = "CustomFork";
        var node = _factory.CreateFork(1, customLabel, _ => true, _builder, _ => { });

        node.Should().NotBeNull();
        node.Should().BeOfType<ConditionalNode>();
        node.Label.Should().Be(customLabel);
    }

    [Fact]
    public void CreateFork_WithGenericType_ReturnsCustomConditionalNode() {
        var node = _factory.CreateFork<CustomConditionalNode>(1);

        node.Should().NotBeNull();
        node.Should().BeOfType<CustomConditionalNode>();
    }

    [Fact]
    public void CreateFork_WithTrueBranchOnly_SetsOnlyTrueBranch() {
        var node = _factory.CreateFork(1, _ => true, _builder, t => t.Do(_ => { }));

        node.Should().BeOfType<ConditionalNode>();
        node.IsTrue.Should().NotBeNull();
        node.IsFalse.Should().BeNull();
    }

    [Fact]
    public void CreateFork_WithBothBranches_SetsBothBranches() {
        var node = _factory.CreateFork(1, _ => true, _builder,
                                       t => t.Do(_ => { }),
                                       f => f.Do(_ => { }));

        node.Should().BeOfType<ConditionalNode>();
        node.IsTrue.Should().NotBeNull();
        node.IsFalse.Should().NotBeNull();
    }

    [Fact]
    public void CreateFork_RunMethodWithTrueCondition_ExecutesTrueBranch() {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        using var context = new Context(provider);
        var node = _factory.CreateFork(1, _ => true,
                                       _builder,
                                       t => t.Do(ctx => ctx["branch"] = "true"),
                                       f => f.Do(ctx => ctx["branch"] = "false"));

        node.Run(context);

        context["branch"].Should().Be("true");
    }

    private sealed class CustomContext(IServiceProvider provider)
        : Context(provider);

    [Fact]
    public void CreateFork_RunMethodWithFalseCondition_ExecutesFalseBranch() {
        var services = new ServiceCollection();
        var provider = services.BuildServiceProvider();
        var context = new CustomContext(provider) {
            ["Disposable"] = new CustomContext(provider),
        };
        var node = _factory.CreateFork(1, _ => false,
                                       _builder,
                                       t => t.Do(ctx => ctx["branch"] = "true"),
                                       f => f.Do(ctx => ctx["branch"] = "false"));

        node.Run(context);

        context["branch"].Should().Be("false");
        context.Dispose();
        context.Dispose();
    }

    [Fact]
    public void CreateFork_ValidateMethod_ValidatesBothBranches() {
        var node = _factory.CreateFork(1, _ => true,
                                       _builder,
                                       t => t.Do(_ => { }),
                                       f => f.Do(_ => { }));

        var result = node.Validate();

        result.IsSuccess.Should().BeTrue();
    }

    // ReSharper disable once ClassNeverInstantiated.Local - Test class
    private sealed class CustomConditionalNode
        : ConditionalNode<CustomConditionalNode> {
        public CustomConditionalNode(uint id, string label, IServiceProvider services)
            : base(id, label, services) {
        }

        public CustomConditionalNode(uint id, IServiceProvider services)
            : base(id, services) { }

        protected override bool When(Context context) => true;
    }
}
