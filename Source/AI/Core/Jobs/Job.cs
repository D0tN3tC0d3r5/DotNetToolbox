namespace DotNetToolbox.AI.Jobs;

public abstract class Job<TInput, TOutput>(IJobStrategy<TInput, TOutput> strategy, string id, JobContext context)
    : IJob<TInput, TOutput> {
    protected Job(IJobStrategy<TInput, TOutput> strategy, JobContext context, IGuidProvider? guid = null)
        : this(strategy, (guid ?? GuidProvider.Default).AsSortable.Create().ToString(), context) {
    }

    public string Id { get; } = id;
    public JobType Type { get; protected init; }
    protected JobContext Context => context;

    public async Task<Result<TOutput>> Execute(TInput input, CancellationToken ct) {
        Context.Job = this;
        var chat = new Chat(Id, Context);
        strategy.AddPrompt(chat, input);
        var result = await Context.Agent.SendRequest(this, chat, ct);

        if (result.HasException)
            throw new JobException("An internal error occurred while executing the job.", result.Exception);
        if (result.HasErrors)
            return result.Errors;

        // Process the chat result into the output
        return strategy.GetResult(chat);
    }
}
