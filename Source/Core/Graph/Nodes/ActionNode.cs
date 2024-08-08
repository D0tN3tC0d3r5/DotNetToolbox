using Microsoft.Extensions.DependencyInjection;

namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode(uint id, string label, Func<Context, CancellationToken, Task> execute, IServiceProvider services)
    : ActionNode<ActionNode>(id, label, services) {
    private const string _defaultLabel = "action";

    private readonly Func<Context, CancellationToken, Task> _execute = IsNotNull(execute);

    public ActionNode(uint id, Func<Context, CancellationToken, Task> execute, IServiceProvider services)
        : this(id, _defaultLabel, execute, services) {
    }
    public ActionNode(uint id, string label, Action<Context> execute, IServiceProvider services)
        : this(id, label, (ctx, ct) => Task.Run(() => execute(ctx), ct), services) {
    }
    public ActionNode(uint id, Action<Context> execute, IServiceProvider services)
        : this(id, _defaultLabel, execute, services) {
    }

    public static TNode Create<TNode>(uint id,
                                      string label,
                                      IServiceProvider services)
        where TNode : ActionNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, services);

    public static TNode Create<TNode>(uint id,
                                      IServiceProvider services)
        where TNode : ActionNode<TNode>
        => InstanceFactory.Create<TNode>(id, services);

    protected override Task Execute(Context context, CancellationToken ct)
        => _execute(context, ct);
}

public abstract class ActionNode<TAction>
    : Node<TAction>,
      IActionNode
    where TAction : ActionNode<TAction> {
    private readonly IPolicy _policy;

    protected ActionNode(uint id, string label, IServiceProvider services)
        : base(id, label, services) {
        _policy = Services.GetService<IPolicy>() ?? Policy.Default;
    }

    protected ActionNode(uint id, IServiceProvider services)
        : base(id, services) {
        _policy = Services.GetService<IPolicy>() ?? Policy.Default;
    }
    public INode? Next { get; set; }

    protected sealed override Task<INode?> GetNext(Context context, CancellationToken ct)
        => Task.FromResult(Next);

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => _policy.Execute(Execute, context, ct);

    protected abstract Task Execute(Context context, CancellationToken ct);
}
