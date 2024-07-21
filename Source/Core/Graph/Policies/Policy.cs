namespace DotNetToolbox.Graph.Policies;

public abstract class Policy
    : IPolicy,
      IHasDefault<Policy> {
    public abstract void Execute(Action action);

    public static Policy Default => new NoRetryPolicy();
}
