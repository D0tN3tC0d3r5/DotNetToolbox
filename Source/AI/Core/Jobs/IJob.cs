namespace DotNetToolbox.AI.Jobs;

public interface IJob {
    JobType Type { get; }
    string Id { get; }
}

public interface IJob<in TRequest, TResponse>
    : IJob {
    Task<TResponse> Execute(TRequest request, CancellationToken ct);
}
