namespace DotNetToolbox.AI.Jobs;

public interface IJob {
    JobType Type { get; }
    string Id { get; }
}

public interface IJob<in TInput, TOutput>
    : IJob {
    Task<Result<TOutput>> Execute(TInput input, CancellationToken ct);
}
