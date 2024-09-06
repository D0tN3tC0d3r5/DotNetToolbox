namespace DotNetToolbox.AI.Jobs;

public abstract class Job<TInput, TOutput>(IJobStrategy<TInput, TOutput> strategy, string id, JobContext context)
    : IJob<TInput, TOutput> {
    protected Job(IJobStrategy<TInput, TOutput> strategy, IStringGuidProvider guid, JobContext context)
        : this(strategy, guid.CreateSortable(), context) {
    }
    protected Job(IJobStrategy<TInput, TOutput> strategy, JobContext context)
        : this(strategy, StringGuidProvider.Default, context) {
    }

    public string Id { get; } = id;
    public JobType Type { get; protected init; }
    public JobContext Context => context;

    public async Task<Result<TOutput>> Execute(TInput input, CancellationToken ct) {
        var chat = new Chat(Id);
        strategy.AddPrompt(chat, input, Context);
        var result = await Context.Connection.SendRequest(this, chat, ct);

        if (result.HasException)
            throw new JobException("An internal error occurred while executing the job.", result.Exception);
        if (result.HasErrors)
            return result.Errors;

        // Process the chat result into the output
        return strategy.GetResult(chat, Context);
    }
}
