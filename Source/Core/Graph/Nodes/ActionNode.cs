namespace DotNetToolbox.Graph.Nodes;

public sealed class ActionNode
    : ActionNode<ActionNode> {
    private readonly Action<Context> _execute;

    private ActionNode(Action<Context>? execute,
                       INode? next = null,
                       IPolicy? policy = null,
                       INodeFactory? factory = null)
        : base(factory) {
        Next = next;
        Policy = policy ?? Policy;
        _execute = IsNotNull(execute);
    }

    internal static ActionNode Create(Action<Context> execute,
                                    Action<WorkflowBuilder>? setTrueBranch = null,
                                    IPolicy? policy = null,
                                    INodeFactory? factory = null) {
        var node = new ActionNode(execute, null, policy, factory);
        if (setTrueBranch == null)
            return node;

        var builder = new WorkflowBuilder(factory);
        setTrueBranch(builder);
        node.Next = builder.Start;
        return node;
    }

    public static TNode Create<TNode>(INodeFactory? factory = null)
        where TNode : ActionNode<TNode>
        => InstanceFactory.Create<TNode>(factory);

    protected override void Execute(Context context)
        => _execute(context);
}

public abstract class ActionNode<TAction>
    : Node,
      IActionNode
    where TAction : ActionNode<TAction> {
    protected ActionNode(INodeFactory? factory = null)
        : base(factory) {
        Next = null;
    }

    protected IPolicy Policy { get; init; } = Policies.Policy.Default;

    public INode? Next {
        get => Branches[0];
        set => Branches[0] = value;
    }

    protected sealed override INode? GetNext(Context context)
        => Next;

    protected sealed override void UpdateState(Context context)
        => Policy.Execute(() => Execute(context));

    protected abstract void Execute(Context context);
}
