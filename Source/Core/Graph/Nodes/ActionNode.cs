namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode(string label, Action<Context> execute, IPolicy? policy = null)
    : ActionNode<ActionNode>(label, policy) {
    private const string _defaultLabel = "action";

    public ActionNode(Action<Context> execute, IPolicy? policy = null)
        : this(_defaultLabel, execute, policy) {
    }

    protected override void Execute(Context context)
        => IsNotNull(execute)(context);

    internal static ActionNode Create(string? label,
                                      Action<Context> execute,
                                      IPolicy? policy = null)
        => label is null
            ? new(execute, policy)
            : new(label, execute, policy);

    public static TNode Create<TNode>(string? label = null,
                                      IPolicy? policy = null)
        where TNode : ActionNode<TNode>
        => InstanceFactory.Create<TNode>(label, policy);
}

public abstract class ActionNode<TAction>(string? label, IPolicy? policy)
    : Node<TAction>(label),
      IActionNode
    where TAction : ActionNode<TAction> {

    private readonly IPolicy _policy = policy ?? Policy.Default;

    protected abstract void Execute(Context context);

    protected sealed override INode? GetNext(Context context)
        => Next;

    protected sealed override void UpdateState(Context context)
        => _policy.Execute(() => Execute(context));
}
