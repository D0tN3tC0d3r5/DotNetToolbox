namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode : ActionNode<ActionNode> {
    private ActionNode(string? tag, string? name, Func<Context, CancellationToken, Task>? execute, IServiceProvider services)
        : base(tag, services) {
        Name = name ?? Name;
        Label = name ?? "action";
        _execute = execute!;
    }
    public ActionNode(string? name, IServiceProvider services)
        : this(null, name, null, services) {
    }
    public ActionNode(string? tag, string? name, IServiceProvider services)
        : this(tag, name, null, services) {
    }
    public ActionNode(Func<Context, CancellationToken, Task> execute, IServiceProvider services)
        : this(null, null, execute, services) {
    }
    public ActionNode(string? tag, Func<Context, CancellationToken, Task> execute, IServiceProvider services)
        : this(tag, null, execute, services) {
        Tag = IsNotNullOrWhiteSpace(tag);
    }
    public ActionNode(Action<Context> execute, IServiceProvider services)
        : this(null, null, (ctx, ct) => Task.Run(() => execute(ctx), ct), services) {
    }
    public ActionNode(string? tag, Action<Context> execute, IServiceProvider services)
        : this(tag, null, (ctx, ct) => Task.Run(() => execute(ctx), ct), services) {
    }

    private readonly Func<Context, CancellationToken, Task> _execute;

    protected override Task Execute(Context context, CancellationToken ct)
        => _execute(context, ct);

    public static TNode Create<TNode>(IServiceProvider services, params object?[] args)
        where TNode : ActionNode<TNode>
        => Node.Create<TNode>(services, args);
}

public abstract class ActionNode<TNode>(string? tag, IServiceProvider services)
    : Node<TNode>(tag, services),
      IActionNode
    where TNode : ActionNode<TNode> {
    public sealed override void ConnectTo(INode? next) {
        if (Next is null) Next = next;
        else Next.ConnectTo(next);
    }

    public string Name { get; set; } = typeof(TNode).Name;
    public IRetryPolicy Retry { get; set; } = services.GetService<IRetryPolicy>()
                                           ?? RetryPolicy.Default;

    protected sealed override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult(Next);

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => Retry.Execute(Execute, context, ct);

    protected abstract Task Execute(Context context, CancellationToken ct);
}
