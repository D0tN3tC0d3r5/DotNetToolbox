namespace DotNetToolbox.AI.Jobs;

public interface IJob {
    string Id { get; }
    JobType Type { get; }
}

public interface IJob<in TInput, TOutput>
    : IJob {
    Task<Result<TOutput>> Execute(TInput input, CancellationToken ct);
}
