namespace DotNetToolbox.AI.Jobs;

public abstract class Job<TRequest, TResponse>(JobType type, string id, JobContext? context = null)
    : IJob<TRequest, TResponse> {
    private bool _isInProgress;

    protected Job(JobType type, JobContext? context = null, IGuidProvider? guid = null)
        : this(type, (guid ?? GuidProvider.Default).AsSortable.Create().ToString(), context) {
    }

    public string Id { get; } = id;
    public JobType Type { get; } = type;
    public JobContext Context { get; init; } = context ?? new JobContext();

    public async Task<TResponse> Execute(TRequest request, CancellationToken ct) {
        if (_isInProgress)
            throw new JobException($"The job {Id} is already in progress.");
        try {
            _isInProgress = true;
            return await Process(request, ct);
        }
        catch (Exception ex) {
            throw new JobException($"An error occurred while processing job {Id}!", ex);
        }
        finally {
            _isInProgress = false;
        }
    }

    protected abstract Task<TResponse> Process(TRequest request, CancellationToken ct);

    public void Deconstruct(out string id, out JobContext input) {
        id = Id;
        input = Context;
    }
}
