namespace DotNetToolbox.Graph.Policies;

public interface IRetryPolicy {
    IReadOnlyList<TimeSpan> Delays { get; }
    byte MaxRetries { get; }
    Task Execute(Func<Context, CancellationToken, Task> action, Context ctx, CancellationToken ct = default);
}
