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
        services.AddTransient<INodeFactory>(p => new NodeFactory(p));
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
        var policy = new CustomPolicy(() => policyExecutionCount++);
        var services = new ServiceCollection();
        services.AddTransient<INodeFactory>(p => new NodeFactory(p));
        services.AddTransient<IPolicy>(_ => policy);
        var provider = services.BuildServiceProvider();

        using var context = new Context(provider);

        var builder = new WorkflowBuilder(provider);
        builder.Do(_ => { });

        var wf = builder.Build().Value!;

        wf.Run(context);

        policyExecutionCount.Should().Be(1);
    }

    private static INode CreateComplexWorkflow() {
        var services = new ServiceCollection();
        services.AddTransient<INodeFactory>(p => new NodeFactory(p));
        services.AddTransient<IPolicy, RetryPolicy>();
        var provider = services.BuildServiceProvider();

        var builder = new WorkflowBuilder(provider);
        builder.Do(ctx => ctx["count"] = 0, "Initialize")
               .If("LoopStart",
                   ctx => ctx["count"].As<int>() < 2,
                   if1 => if1.IsTrue(t1 => t1
                            .Do(ctx => ctx["count"] = ctx["count"].As<int>() + 1, "[Yes1] Add one")
                            .Do(ctx => ctx["result"] = "Action1", "[Yes2] Set result to Action1")
                            .JumpTo("LoopStart", "[Yes3] Loop back"))
                          .IsFalse(f1 => f1
                            .If(ctx => ctx["count"].As<int>() % 2 == 0,
                                if2 => if2
                                .IsTrue(t2 => t2
                                    .Do(ctx => ctx["result"] = "Action2", "[Even] Set result to Action2"))
                                .IsFalse(f2 => f2
                                    .Do(ctx => ctx["result"] = "Action3", "[Odd] Set result to Action3")), "[No] Is even?")), "Is less than 2?");
        return builder.Build().Value!;
    }

    private sealed class CustomPolicy(Action onExecute) : IPolicy {
        public Task Execute(Func<Context, CancellationToken, Task> action, Context ctx, CancellationToken ct = default) {
            onExecute();
            return action(ctx, ct);
        }
    }
}
