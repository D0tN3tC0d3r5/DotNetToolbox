namespace DotNetToolbox.Graph.Policies;

public interface IRetryPolicy {
    IReadOnlyList<TimeSpan> Delays { get; }
    byte MaxRetries { get; }

    void Execute(Action action);
}
