namespace DotNetToolbox.Graph.Nodes;

public class BranchingNode
    : BranchingNode<BranchingNode> {
    private BranchingNode(Func<Context, string> selectPath,
                          IReadOnlyDictionary<string, INode?>? branches = null,
                          INodeFactory? factory = null)
        : base(factory) {
        SelectPath = IsNotNull(selectPath);
        if (branches is null) return;
        foreach (var branch in branches)
            Branches[branch.Key] = branch.Value;
    }

    protected sealed override Func<Context, string> SelectPath { get; }

    public static BranchingNode Create(Func<Context, string> selectPath,
                                       Action<BranchesBuilder> setPaths,
                                       INodeFactory? factory = null) {
        var node = new BranchingNode(selectPath, null, factory);
        var builder = new BranchesBuilder(node, factory);
        setPaths(builder);
        return node;
    }

    public static TNode Create<TNode>(INodeFactory? factory = null)
        where TNode : BranchingNode<TNode>
        => InstanceFactory.Create<TNode>(factory);
}

public abstract class BranchingNode<TNode>(INodeFactory? factory = null)
    : Node<string>(factory),
      IBranchingNode
    where TNode : BranchingNode<TNode> {
    protected abstract Func<Context, string> SelectPath { get; }

    protected override Result IsValid() {
        var result = Success();
        if (Branches.Count == 0)
            result += Invalid($"No path is registered for node '{Id}'.");

        return result;
    }

    protected override INode GetNext(Context context)
        => Branches.GetValueOrDefault(SelectPath(context))
        ?? throw new InvalidOperationException("The selected path was not found.");

    protected sealed override void UpdateState(Context context) { }
}
