namespace DotNetToolbox.Graph.AI;

public class AnalysisActionNode(uint id, string label, string provider, IServiceProvider services)
    : ActionNode<AnalysisActionNode>(id, label, services) {
    public AnalysisActionNode(uint id, string label, IServiceProvider services)
        : this(id, label, null!, services) {
    }

    protected override async Task Execute(Context context, CancellationToken ct) {
        var input = context["AnalysisInput"] as string
                 ?? throw new InvalidOperationException("Analysis input not found in context");

        var factory = Services.GetRequiredService<IJobContextBuilderFactory>();
        using var jobContext = factory.CreateFrom(context)
                                      .WithAgentFrom(provider)
                                      .Build();
        var job = new AnalysisJob($"{Id}", jobContext);
        var result = await job.Execute(input, ct);

        context["AnalysisResult"] = result.Value;
    }
}
