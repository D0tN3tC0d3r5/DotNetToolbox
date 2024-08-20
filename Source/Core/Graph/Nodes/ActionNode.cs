namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode : ActionNode<ActionNode> {
    internal ActionNode(string? id, INodeSequence? sequence, IPolicy? policy, Func<Context, CancellationToken, Task> execute)
        : base(id, sequence, policy) {
        _execute = IsNotNull(execute);
    }

    public ActionNode(string id, Func<Context, CancellationToken, Task> execute, IPolicy? policy = null)
        : this(IsNotNullOrWhiteSpace(id), null, policy, execute) {
    }
    public ActionNode(string id, Action<Context> execute, IPolicy? policy = null)
        : this(IsNotNullOrWhiteSpace(id), null, policy, (ctx, ct) => Task.Run(() => execute(ctx), ct)) {
    }
    public ActionNode(Func<Context, CancellationToken, Task> execute, IPolicy? policy = null, INodeSequence? sequence = null)
        : this(null, sequence, policy, execute) {
    }
    public ActionNode(Action<Context> execute, IPolicy? policy = null, INodeSequence? sequence = null)
        : this(null, sequence, policy, (ctx, ct) => Task.Run(() => execute(ctx), ct)) {
    }

    private readonly Func<Context, CancellationToken, Task> _execute;

    protected override string DefaultLabel { get; } = "action";

    protected override Task Execute(Context context, CancellationToken ct)
        => _execute(context, ct);

    public static TNode Create<TNode>(IServiceProvider services, string? id = null)
        where TNode : ActionNode<TNode>
        => Node.Create<TNode>(services, id);
}

public abstract class ActionNode<TAction>(string? id, INodeSequence? sequence = null, IPolicy? policy = null)
    : Node<TAction>(id, sequence),
      IActionNode
    where TAction : ActionNode<TAction> {
    private readonly IPolicy _policy = policy ?? Policy.Default;

    public sealed override void ConnectTo(INode? next) {
        if (Next is null) Next = next;
        else Next.ConnectTo(next);
    }

    protected sealed override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult(Next);

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => _policy.Execute(Execute, context, ct);

    protected abstract Task Execute(Context context, CancellationToken ct);
}
