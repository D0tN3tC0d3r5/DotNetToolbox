namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode(uint id, Func<Context, CancellationToken, Task> execute, string? tag = null, string? label = null, IPolicy? policy = null)
    : ActionNode<ActionNode>(id, tag, label, policy) {
    private readonly Func<Context, CancellationToken, Task> _execute = IsNotNull(execute);

    public ActionNode(uint id, Action<Context> execute, string? tag = null, string? label = null, IPolicy? policy = null)
        : this(id, (ctx, ct) => Task.Run(() => execute(ctx), ct), tag, label, policy) {
    }

    protected override string DefaultLabel { get; } = "action";

    protected override Task Execute(Context context, CancellationToken ct)
        => _execute(context, ct);
}

public abstract class ActionNode<TAction>(uint id, string? tag, string? label, IPolicy? policy = null)
    : Node<TAction>(id, tag, label),
      IActionNode
    where TAction : ActionNode<TAction> {
    private readonly IPolicy _policy = policy ?? Policy.Default;

    public sealed override Result ConnectTo(INode? next) {
        var result = Success();
        if (Next is null) Next = next;
        else result += Next.ConnectTo(next);
        return result;
    }

    protected sealed override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult(Next);

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => _policy.Execute(Execute, context, ct);

    protected abstract Task Execute(Context context, CancellationToken ct);
}
