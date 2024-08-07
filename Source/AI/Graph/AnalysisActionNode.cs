namespace DotNetToolbox.Graph.AI;

public class AnalysisActionNode(uint id, string label, string provider, IServiceProvider services)
    : ActionNode<AnalysisActionNode>(id, label, services) {

    public AnalysisActionNode(uint id, string label, IServiceProvider services)
        : this(id, label, null!, services) {
    }

    protected override void Execute(Context context) {
        var input = context["AnalysisInput"] as string
                 ?? throw new InvalidOperationException("Analysis input not found in context");

        var agentFactory = Services.GetRequiredService<IAgentFactory>();
        var contextBulder = Services.GetRequiredService<IJobContextBuilder>();
        var jobContext = new JobContext(Services, context) {
            Agent = agentFactory.Create(provider),
        };
        var job = new AnalysisJob($"{Id}", jobContext);
        var result = job.Execute(input, CancellationToken.None).GetAwaiter().GetResult();

        context["AnalysisResult"] = result.Value;
    }
}
