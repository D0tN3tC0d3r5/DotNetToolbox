namespace DotNetToolbox.Graph.AI;

public class AnalysisActionNode(string tag, IServiceProvider services)
    : ActionNode<AnalysisActionNode>(tag, services) {
    protected override async System.Threading.Tasks.Task Execute(Context context, CancellationToken ct) {
        var input = context["AnalysisInput"] as string
                 ?? throw new InvalidOperationException("Analysis input not found in context");

        var factory = Services.GetRequiredService<IJobContextBuilderFactory>();
        if (!context.TryGetValueAs<Model>("Model", out var model)) throw new InvalidOperationException("Model not found in context");
        using var jobContext = factory.CreateFrom(context)
                                      .WithModel(model)
                                      .Build();
        var job = new AnalysisJob($"{Id}", jobContext);
        var result = await job.Execute(input, ct);

        context["AnalysisResult"] = result.Value;
    }
}
