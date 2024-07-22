namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNode
    : ConditionalNode<ConditionalNode> {
    private ConditionalNode(Func<Context, bool> predicate,
                            INode trueNode,
                            INode? falseNode,
                            IGuidProvider? guid = null)
        : base(guid) {
        Predicate = IsNotNull(predicate);
        True = trueNode;
        False = falseNode;
    }

    protected sealed override Func<Context, bool> Predicate { get; }

    public static ConditionalNode Create(Func<Context, bool> predicate,
                                         INode truePath,
                                         INode? falsePath = null,
                                         IGuidProvider? guid = null)
        => new(predicate, truePath, falsePath, guid);

    public static TNode Create<TNode>(IGuidProvider? guid = null)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(guid);
}

public abstract class ConditionalNode<TNode>
    : Node,
      IConditionalNode
    where TNode : ConditionalNode<TNode> {
    protected ConditionalNode(IGuidProvider? guid = null)
        : base(guid) {
        Paths.Add(Factory.Void);
        Paths.Add(null);
    }

    protected abstract Func<Context, bool> Predicate { get; }

    public INode? True {
        get => Paths[0];
        set => Paths[0] = value;
    }

    public INode? False {
        get => Paths[1];
        set => Paths[1] = value;
    }

    protected sealed override INode? GetNext(Context context)
        => Predicate(context)
            ? True
            : False;

    protected sealed override void UpdateState(Context context) { }
}
