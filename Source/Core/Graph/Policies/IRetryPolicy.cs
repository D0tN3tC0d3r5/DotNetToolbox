namespace DotNetToolbox.Graph.Policies;

public interface IRetryPolicy
    : IPolicy {
    IReadOnlyList<TimeSpan> Delays { get; }
    byte MaxRetries { get; }
}
