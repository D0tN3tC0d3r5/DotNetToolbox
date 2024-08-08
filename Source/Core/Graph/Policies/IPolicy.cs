namespace DotNetToolbox.Graph.Policies;

public interface IPolicy {
    Task Execute(Func<Context, CancellationToken, Task> action, Context ctx, CancellationToken ct = default);
}
