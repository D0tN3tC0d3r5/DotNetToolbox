namespace DotNetToolbox.Graph.Policies;

public static class Policy {
    public static RetryPolicy Default => new();
}

public abstract class Policy<TPolicy>
    : IPolicy,
      IHasDefault<TPolicy>
    where TPolicy : Policy<TPolicy>, new() {
    public abstract Task Execute(Func<Context, CancellationToken, Task> action, Context ctx, CancellationToken ct = default);
    public static TPolicy Default => new();
}
