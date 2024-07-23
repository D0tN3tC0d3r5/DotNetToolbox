namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNode
    : ConditionalNode<ConditionalNode> {
    private ConditionalNode(Func<Context, bool> predicate,
                            INode? trueNode = null,
                            INode? falseNode = null,
                            INodeFactory? factory = null)
        : base(factory) {
        Predicate = IsNotNull(predicate);
        True = trueNode;
        False = falseNode;
    }

    protected sealed override Func<Context, bool> Predicate { get; }

    public static ConditionalNode Create(Func<Context, bool> predicate,
                                         Action<WorkflowBuilder> setTrueBranch,
                                         Action<WorkflowBuilder>? setFalseBranch = null,
                                         INodeFactory? factory = null) {
        var node = new ConditionalNode(predicate, null, null, factory);
        var trueBuilder = new WorkflowBuilder(factory);
        setTrueBranch(trueBuilder);
        node.True = trueBuilder.Start;
        if (setFalseBranch == null)
            return node;

        var falseBuilder = new WorkflowBuilder(factory);
        setFalseBranch(falseBuilder);
        node.False = falseBuilder.Start;

        return node;
    }

    public static TNode Create<TNode>(IGuidProvider? guid = null)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(guid);
}

public abstract class ConditionalNode<TNode>
    : Node<bool>,
      IConditionalNode
    where TNode : ConditionalNode<TNode> {
    protected ConditionalNode(INodeFactory? factory = null)
        : base(factory) {
        True = null;
        False = null;
    }

    protected abstract Func<Context, bool> Predicate { get; }

    public INode? True {
        get => Branches[true];
        set => Branches[true] = value;
    }

    public INode? False {
        get => Branches[false];
        set => Branches[false] = value;
    }

    protected sealed override INode? GetNext(Context context)
        => Predicate(context)
            ? True
            : False;

    protected sealed override void UpdateState(Context context) { }
}
