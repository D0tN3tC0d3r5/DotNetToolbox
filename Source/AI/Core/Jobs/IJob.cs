namespace DotNetToolbox.AI.Jobs;

public interface IJob {
    string Id { get; }
    Task<Result> Execute(CancellationToken ct);
}
