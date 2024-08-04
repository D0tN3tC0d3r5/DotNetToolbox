namespace DotNetToolbox.AI.Jobs;

public abstract class Job<TInput, TOutput>(IJobStrategy<TInput, TOutput> strategy, string id, JobContext jobContext, IAgentFactory agentFactory)
    : IJob<TInput, TOutput> {

    protected Job(IJobStrategy<TInput, TOutput> strategy, JobContext jobContext, IAgentFactory agentFactory, IGuidProvider? guid = null)
        : this(strategy, (guid ?? GuidProvider.Default).AsSortable.Create().ToString(), jobContext, agentFactory) {
    }

    public string Id { get; } = id;
    public JobType Type { get; protected init; }

    public async Task<Result<TOutput>> Execute(TInput input, CancellationToken ct) {
        var agent = agentFactory.Create("provider");
        var chat = strategy.PrepareChat(jobContext, input);
        var result = await agent.SendRequest(this, chat, ct);

        if (result.HasException)
            throw new JobException($"An internal error occurred while executing the job.", result.Exception);
        if (result.HasErrors)
            return result.Errors;

        // Process the chat result into the output
        return strategy.GetResult(chat);
    }
}
