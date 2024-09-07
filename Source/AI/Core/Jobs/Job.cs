namespace DotNetToolbox.AI.Jobs;

public class Job<TInput, TOutput>(string id, JobContext context, Action<TInput> updateMessages, Func<TOutput> getResponse)
    : IJob<TInput, TOutput> {
    private readonly Action<TInput>? _updateMessages = updateMessages;
    private readonly Func<TOutput>? _getResponse = getResponse;

    public Job(IStringGuidProvider guid, JobContext context, Action<TInput> updateMessages, Func<TOutput> getResponse)
        : this(guid.CreateSortable(), context, updateMessages, getResponse) {
    }
    public Job(JobContext context, Action<TInput> updateMessages, Func<TOutput> getResponse)
        : this(StringGuidProvider.Default, context, updateMessages, getResponse) {
    }
    public Job(string id, JobContext context)
        : this(id, context, null!, null!) {
    }
    public Job(IStringGuidProvider guid, JobContext context)
        : this(guid.CreateSortable(), context, null!, null!) {
    }
    public Job(JobContext context)
        : this(StringGuidProvider.Default, context, null!, null!) {
    }

    public string Id { get; } = id;
    public JobType Type { get; protected init; } = JobType.Generic;

    protected JobContext Context { get; } = context;
    protected Messages Messages { get; } = new Messages(id);

    protected virtual void UpdateMessages(TInput input) {
        if (_updateMessages is null) throw new NotImplementedException();
        _updateMessages(input);
    }
    protected virtual TOutput GetResponse()
        => _getResponse is null
            ? throw new NotImplementedException()
            : _getResponse();

    public async Task<Result<TOutput>> Execute(TInput input, CancellationToken ct) {
        UpdateMessages(input);
        var result = await Context.Agent.SendRequest(Messages, Context, ct);
        return result.HasErrors
            ? result.Errors
            : GetResponse();
    }
}
