namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode(uint id, IServiceProvider services, Func<Context, CancellationToken, Task> execute, string? tag = null, string? label = null)
    : ActionNode<ActionNode>(id, services, tag, label) {
    private readonly Func<Context, CancellationToken, Task> _execute = IsNotNull(execute);

    public ActionNode(uint id, IServiceProvider services, Action<Context> execute, string? tag = null, string? label = null)
        : this(id, services, (ctx, ct) => Task.Run(() => execute(ctx), ct), tag, label) {
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

    protected ActionNode(uint id, IServiceProvider services, string? tag = null, string? label = null)
        : base(id, services, tag, label) {
        _policy = Services.GetService<IPolicy>() ?? Policy.Default;
    }

    public sealed override void ConnectTo(INode? next) {
        if (Next is null) Next = next;
        else Next?.ConnectTo(next);
    }

    protected sealed override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult(Next);

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => _policy.Execute(Execute, context, ct);

    protected abstract Task Execute(Context context, CancellationToken ct);
}
