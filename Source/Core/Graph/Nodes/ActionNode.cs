namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode
    : ActionNode<ActionNode> {
    private readonly Action<Context> _execute;

    private ActionNode(Action<Context>? execute,
                       INode? next = null,
                       IPolicy? policy = null,
                       IGuidProvider? guid = null)
        : base(guid) {
        Next = next;
        Policy = policy ?? Policy;
        _execute = IsNotNull(execute);
    }

    public static ActionNode Create(Action<Context> execute,
                                    INode? next = null,
                                    IPolicy? policy = null,
                                    IGuidProvider? guid = null)
        => new(execute, next, policy, guid);

    public static TNode Create<TNode>(IGuidProvider? guid = null)
        where TNode : ActionNode<TNode>
        => InstanceFactory.Create<TNode>(guid);

    protected override void Execute(Context context)
        => _execute(context);
}

public abstract class ActionNode<TAction>
    : Node,
      IActionNode
    where TAction : ActionNode<TAction> {
    protected ActionNode(IGuidProvider? guid = null)
        : base(guid) {
        Paths.Add(null);
    }

    protected IPolicy Policy { get; init; } = Policies.Policy.Default;

    public INode? Next {
        get => Paths[0];
        set => Paths[0] = value;
    }

    protected sealed override INode? GetNext(Context context)
        => Next;

    protected sealed override void UpdateState(Context context)
        => Policy.Execute(() => Execute(context));

    protected abstract void Execute(Context context);
}
