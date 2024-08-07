namespace DotNetToolbox.Graph.AI;

public class AnalysisActionNode(uint id, string label, string provider, IServiceProvider services)
    : ActionNode<AnalysisActionNode>(id, label, services) {
    private readonly IAgentFactory _agentFactory = services.GetRequiredService<IAgentFactory>();

    public AnalysisActionNode(uint id, string label, IServiceProvider services)
        : this(id, label, null!, services) {
    }

    protected override void Execute(Context context) {
        var jobContext = new JobContext(context) {
            Agent = _agentFactory.Create(provider),
        };
        var job = new AnalysisJob($"{Id}", jobContext);
        var input = context["AnalysisInput"] as string
                 ?? throw new InvalidOperationException("Analysis input not found in context");
        var result = job.Execute(input, CancellationToken.None).GetAwaiter().GetResult();
        context["AnalysisResult"] = result.Value;
    }
}
