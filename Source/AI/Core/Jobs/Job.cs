namespace DotNetToolbox.AI.Jobs;

public abstract class Job<TInput, TOutput>(string id, IAgentFactory? agentFactory = null)
    : IJob<TInput, TOutput> {
    private readonly IAgentFactory _agentFactory = agentFactory ?? default!;

    protected Job(IGuidProvider? guid = null, IAgentFactory? agentFactory = null)
        : this((guid ?? GuidProvider.Default).AsSortable.Create().ToString(), agentFactory) {
    }

    public string Id { get; } = id;
    public JobType Type { get; protected init; }
    public abstract string Instructions { get; protected init; }

    public async Task<Result<TOutput>> Execute(TInput input, CancellationToken ct) {
        var agent = _agentFactory.Create("provider");
        var chat = PrepareChat(input);
        var result = await agent.SendRequest(this, chat, ct);

        if (result.HasException)
            throw new JobException($"An internal error occurred while executing the job.", result.Exception);
        if (result.HasErrors)
            return result.Errors;

        // Process the chat result into the output
        return GetFinalResult(chat);
    }

    protected abstract IChat PrepareChat(TInput input);
    protected abstract TOutput GetFinalResult(IChat chat);
}
