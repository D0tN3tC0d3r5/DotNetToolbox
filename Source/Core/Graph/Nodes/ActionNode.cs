namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode : ActionNode<ActionNode> {
    public ActionNode(Func<Context, CancellationToken, Task> execute, IServiceProvider services)
        : base(services) {
        _execute = IsNotNull(execute);
        Label = "action";
    }
    public ActionNode(string tag, Func<Context, CancellationToken, Task> execute, IServiceProvider services)
        : this(execute, services) {
        Tag = IsNotNullOrWhiteSpace(tag);
    }
    public ActionNode(Action<Context> execute, IServiceProvider services)
        : this((ctx, ct) => Task.Run(() => execute(ctx), ct), services) {
    }
    public ActionNode(string tag, Action<Context> execute, IServiceProvider services)
        : this(tag, (ctx, ct) => Task.Run(() => execute(ctx), ct), services) {
    }

    private readonly Func<Context, CancellationToken, Task> _execute;

    protected override Task Execute(Context context, CancellationToken ct)
        => _execute(context, ct);

    public static TNode Create<TNode>(IServiceProvider services, params object?[] args)
        where TNode : ActionNode<TNode>
        => Node.Create<TNode>(services, args);
}

public abstract class ActionNode<TNode>(IServiceProvider services)
    : Node<TNode>(services),
      IActionNode
    where TNode : ActionNode<TNode> {
    public sealed override void ConnectTo(INode? next) {
        if (Next is null) Next = next;
        else Next.ConnectTo(next);
    }

    public IRetryPolicy Retry { get; set; } = services.GetService<IRetryPolicy>()
                                           ?? RetryPolicy.Default;

    protected sealed override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult(Next);

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => Retry.Execute(Execute, context, ct);

    protected abstract Task Execute(Context context, CancellationToken ct);
}
