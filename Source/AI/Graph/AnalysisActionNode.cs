namespace DotNetToolbox.Graph.AI;

public class AnalysisActionNode(string tag, string provider, IServiceProvider services)
    : ActionNode<AnalysisActionNode>(tag, services) {
    public AnalysisActionNode(string tag, IServiceProvider services)
        : this(tag, null!, services) {
    }

    protected override async System.Threading.Tasks.Task Execute(Context context, CancellationToken ct) {
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
