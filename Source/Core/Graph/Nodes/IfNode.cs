namespace DotNetToolbox.Graph.Nodes;

public class IfNode
    : IfNode<IfNode> {
    private IfNode(string? id,
                   Func<Context, bool> predicate,
                   INode trueNode,
                   INode? falseNode,
                   IGuidProvider? guid = null)
        : base(id, guid) {
        Predicate = IsNotNull(predicate);
        True = trueNode;
        False = falseNode;
    }

    protected sealed override Func<Context, bool> Predicate { get; }

    public static IfNode Create(string id,
                                Func<Context, bool> predicate,
                                INode truePath,
                                INode? falsePath = null)
        => new(IsNotNullOrWhiteSpace(id), predicate, truePath, falsePath);

    public static IfNode Create(Func<Context, bool> predicate,
                                INode truePath,
                                INode? falsePath = null,
                                IGuidProvider? guid = null)
        => new(id: null, predicate, truePath, falsePath, guid);

    public static TNode Create<TNode>(string id)
        where TNode : IfNode<TNode>
        => InstanceFactory.Create<TNode>(id);

    public static TNode Create<TNode>(IGuidProvider? guid = null)
        where TNode : IfNode<TNode>
        => InstanceFactory.Create<TNode>(guid);
}

public abstract class IfNode<TNode>
    : Node,
      IIfNode
    where TNode : IfNode<TNode> {
    protected IfNode(string? id, IGuidProvider? guid = null)
        : base(id, guid) {
        Paths.Add(Void);
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
