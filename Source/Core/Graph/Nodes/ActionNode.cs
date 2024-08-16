namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode(IServiceProvider services, uint id, Func<Context, CancellationToken, Task> execute, string? tag = null, string? label = null)
    : ActionNode<ActionNode>(services, id, tag, label) {
    private readonly Func<Context, CancellationToken, Task> _execute = IsNotNull(execute);

    public ActionNode(IServiceProvider services, uint id, Action<Context> execute, string? tag = null, string? label = null)
        : this(services, id, (ctx, ct) => Task.Run(() => execute(ctx), ct), tag, label) {
    }

    protected override string DefaultLabel { get; } = "action";

    protected override Task Execute(Context context, CancellationToken ct)
        => _execute(context, ct);
}

public abstract class ActionNode<TAction>
    : Node<TAction>,
      IActionNode
    where TAction : ActionNode<TAction> {
    private readonly IPolicy _policy;

    protected ActionNode(IServiceProvider services, uint id, string? tag = null, string? label = null)
        : base(services, id, tag, label) {
        _policy = Services.GetService<IPolicy>() ?? Policy.Default;
    }

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
