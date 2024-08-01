namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode(uint id, string label, Action<Context> execute, IPolicy? policy = null)
    : ActionNode<ActionNode>(id, label, policy) {
    private const string _defaultLabel = "action";

    protected override void Execute(Context context)
        => IsNotNull(execute)(context);

    internal static ActionNode Create(uint id,
                                      string? label,
                                      Action<Context> execute,
                                      IPolicy? policy = null)
        => new(id, label ?? _defaultLabel, execute, policy);

    public static TNode Create<TNode>(uint id,
                                      string? label = null,
                                      IPolicy? policy = null)
        where TNode : ActionNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, policy);
}

public abstract class ActionNode<TAction>(uint id, string? label, IPolicy? policy)
    : Node<TAction>(id, label),
      IActionNode
    where TAction : ActionNode<TAction> {
    private readonly IPolicy _policy = policy ?? Policy.Default;

    protected abstract void Execute(Context context);

    protected sealed override INode? GetNext(Context context)
        => Next;

    protected sealed override void UpdateState(Context context)
        => _policy.Execute(() => Execute(context));
}
