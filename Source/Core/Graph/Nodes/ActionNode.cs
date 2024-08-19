﻿namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode(string id, Func<Context, CancellationToken, Task> execute, INodeSequence? sequence = null, IPolicy? policy = null)
    : ActionNode<ActionNode>(id, sequence, policy) {
    public ActionNode(string id, Action<Context> execute, INodeSequence? sequence = null, IPolicy? policy = null)
        : this(id, (ctx, ct) => Task.Run(() => execute(ctx), ct), sequence, policy) {
    }
    public ActionNode(Func<Context, CancellationToken, Task> execute, INodeSequence? sequence = null, IPolicy? policy = null)
        : this(null!, execute, sequence, policy) {
    }
    public ActionNode(Action<Context> execute, INodeSequence? sequence = null, IPolicy? policy = null)
        : this(null!, (ctx, ct) => Task.Run(() => execute(ctx), ct), sequence, policy) {
    }

    private readonly Func<Context, CancellationToken, Task> _execute = IsNotNull(execute);

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
