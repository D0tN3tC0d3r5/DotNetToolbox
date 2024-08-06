namespace DotNetToolbox.Graph.Policies;

public static class Policy {
    public static RetryPolicy Default => new();

}

public abstract class Policy<TPolicy>
    : IPolicy,
      IHasDefault<TPolicy>
    where TPolicy : Policy<TPolicy>, new() {
    public abstract void Execute(Action action);
    public static TPolicy Default => new();
}
