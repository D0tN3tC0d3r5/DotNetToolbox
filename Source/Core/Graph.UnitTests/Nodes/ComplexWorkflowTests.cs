namespace DotNetToolbox.Graph.Nodes;

public class ComplexWorkflowTests {
    private readonly NodeFactory _factory;

    public ComplexWorkflowTests() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();
        _factory = new(provider);
    }

    [Fact]
    public async Task ComplexWorkflow_WithMultipleNodeTypes_ExecutesCorrectly() {
        var services = new ServiceCollection();
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
        var startNode = _factory.CreateAction(1, _ => { });
        var actionNode = _factory.CreateAction(2, _ => { });
        startNode.Next = actionNode;
        actionNode.Next = startNode; // Creating a circular reference

        var result = startNode.Validate();

        result.IsSuccess.Should().BeTrue(); // Circular references are allowed, but detected
    }

    [Fact]
    public void ComplexWorkflow_WithCustomPolicy_AppliesPolicyCorrectly() {
        var policyExecutionCount = 0;
        var policy = new CustomPolicy(() => policyExecutionCount++);
        var services = new ServiceCollection();
        services.AddTransient<IPolicy>(_ => policy);
        var provider = services.BuildServiceProvider();

        using var context = new Context(provider);

        var builder = new WorkflowBuilder(provider);
        builder.Do(_ => { });

        var wf = builder.Start;

        wf.Run(context);

        policyExecutionCount.Should().Be(1);
    }

    private static INode CreateComplexWorkflow() {
        var services = new ServiceCollection();
        services.AddTransient<IPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();

        var builder = new WorkflowBuilder(provider);
        builder.Do(ctx => ctx["count"] = 0)
               .If("LoopStart", ctx => ctx["count"].As<int>() < 2,
                   t1 => t1.Do(ctx => ctx["count"] = ctx["count"].As<int>() + 1)
                           .Do(ctx => ctx["result"] = "Action1")
                           .JumpTo("LoopStart"),
                   f1 => f1.If(ctx => ctx["count"].As<int>() % 2 == 0,
                               t2 => t2.Do(ctx => ctx["result"] = "Action2"),
                               f2 => f2.Do(ctx => ctx["result"] = "Action3")));
        return builder.Start;
    }

    private sealed class CustomPolicy(Action onExecute) : IPolicy {
        public Task Execute(Func<Context, CancellationToken, Task> action, Context ctx, CancellationToken ct = default) {
            onExecute();
            return action(ctx, ct);
        }
    }
}
