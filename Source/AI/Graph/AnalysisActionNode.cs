namespace DotNetToolbox.Graph.AI;

public class AnalysisActionNode
    : ActionNode<AnalysisActionNode> {
    private readonly IAgent _agent;

    public AnalysisActionNode(uint id, string label, IServiceProvider services)
        : this(id, label, null!, services) {
    }

    public AnalysisActionNode(uint id, string label, string provider, IServiceProvider services)
        : base(id, label, services) {
        var agentFactory = services.GetRequiredService<IAgentFactory>();
        _agent = agentFactory.Create(provider);
    }

    protected override void Execute(Context context) {
        var job = new AnalysisJob($"{Id}", context, _agent);
        var input = context["AnalysisInput"] as string ?? throw new InvalidOperationException("Analysis input not found in context");
        var result = job.Execute(input, CancellationToken.None).GetAwaiter().GetResult();
        context["AnalysisResult"] = result.Value;
    }
}
