namespace DotNetToolbox.Graph.Policies;

public interface IRetryPolicy {
    IReadOnlyList<TimeSpan> Delays { get; }
    byte MaxRetries { get; }
    Task Execute(Func<Map, CancellationToken, Task> action, Map ctx, CancellationToken ct = default);
}
