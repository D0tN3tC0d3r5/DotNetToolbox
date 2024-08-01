namespace DotNetToolbox.AI.Jobs;

public abstract class Job<TRequest, TResponse>(JobType type, string id)
    : IJob<TRequest, TResponse> {
    protected Job(JobType type, IGuidProvider? guid = null)
        : this(type, (guid ?? GuidProvider.Default).AsSortable.Create().ToString()) {
    }

    public string Id { get; } = id;
    public JobType Type { get; } = type;

    public abstract Task<TResponse> Execute(TRequest request, CancellationToken ct);
}
