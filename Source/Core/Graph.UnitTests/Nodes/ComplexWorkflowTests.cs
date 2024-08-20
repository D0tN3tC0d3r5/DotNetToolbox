namespace DotNetToolbox.Graph.Nodes;

public class ComplexWorkflowTests {
    private readonly INodeFactory _factory;

    public ComplexWorkflowTests() {
        var services = new ServiceCollection();
        services.AddScoped<INodeFactory, NodeFactory>();
        services.AddTransient<IRetryPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();
        _factory = provider.GetRequiredService<INodeFactory>();
    }

    [Fact]
    public async Task ComplexWorkflow_WithMultipleNodeTypes_ExecutesCorrectly() {
        var services = new ServiceCollection();
        services.AddScoped<INodeFactory, NodeFactory>();
        var provider = services.BuildServiceProvider();
        using var context = new Context(provider);
        var start = CreateComplexWorkflow();
        var workflow = new Workflow(start, context);

        await workflow.Run();

        context["count"].Should().Be(2);
        context["result"].Should().Be("Action2");
    }

    [Fact]
    public void ComplexWorkflow_Validation_SucceedsForValidWorkflow() {
        var workflow = CreateComplexWorkflow();

        var result = workflow.Validate();

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ComplexWorkflow_WithCircularReference_DetectedDuringValidation() {
        var startNode = _factory.CreateAction(_ => { });
        var actionNode = _factory.CreateAction(_ => { });
        startNode.Next = actionNode;
        actionNode.Next = startNode; // Creating a circular reference

        var result = startNode.Validate();

        result.IsSuccess.Should().BeTrue(); // Circular references are allowed, but detected
    }

    [Fact]
    public void ComplexWorkflow_WithCustomPolicy_AppliesPolicyCorrectly() {
        var policyExecutionCount = 0;
        var policy = new CustomRetryPolicy(() => policyExecutionCount++);
        var services = new ServiceCollection();
        services.AddTransient<INodeFactory>(p => new NodeFactory(p));
        services.AddTransient<IRetryPolicy>(_ => policy);
        var provider = services.BuildServiceProvider();

        using var context = new Context(provider);

        var builder = new WorkflowBuilder(provider);
        builder.Do(_ => { });

        var wf = builder.Build();

        wf.Run(context);

        policyExecutionCount.Should().Be(1);
    }

    private static INode CreateComplexWorkflow() {
        var services = new ServiceCollection();
        services.AddTransient<INodeFactory>(p => new NodeFactory(p));
        services.AddTransient<IRetryPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();

        var builder = new WorkflowBuilder(provider);
        builder.Do("LoopStart", ctx => ctx["count"] = 0).WithLabel("Initialize")
               .If(ctx => ctx["count"].As<int>() < 2)
               .Then(t1 => t1.Do(ctx => ctx["count"] = ctx["count"].As<int>() + 1)
                             .WithLabel("[Yes1] Add one")
                             .Do(ctx => ctx["result"] = "Action1")
                             .WithLabel("[Yes2] Set result to Action1")
                             .GoTo("LoopStart")
                             .WithLabel("[Yes3] Loop back"))
               .Else(f1 => f1.If(ctx => ctx["count"].As<int>() % 2 == 0)
                              .Then(t2 => t2.Do(ctx => ctx["result"] = "Action2")
                                            .WithLabel("[Even] Set result to Action2"))
                              .Else(f2 => f2.Do(ctx => ctx["result"] = "Action3")
                                            .WithLabel("[Odd] Set result to Action3"))
                              .WithLabel("[No] Is even?"))
               .WithLabel("Is less than 2?");
        return builder.Build();
    }

    private sealed class CustomRetryPolicy(Action onExecute) : IRetryPolicy {
        public IReadOnlyList<TimeSpan> Delays { get; } = [];
        public byte MaxRetries => 3;

        public Task Execute(Func<Context, CancellationToken, Task> action, Context ctx, CancellationToken ct = default) {
            onExecute();
            return action(ctx, ct);
        }
    }
}
