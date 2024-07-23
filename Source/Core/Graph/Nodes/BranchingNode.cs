namespace DotNetToolbox.Graph.Nodes;

public sealed class BranchingNode(string label, Func<Context, string> select)
    : BranchingNode<BranchingNode>(IsNotNull(label)) {
    private const string _defaultLabel = "case";

    protected override string Select(Context context) => IsNotNull(select)(context);

    public BranchingNode(Func<Context, string> select)
        : this(_defaultLabel, select) {
    }

    internal static BranchingNode Create(string? label,
                                         Func<Context, string> selectPath,
                                         Action<BranchesBuilder> setPaths,
                                         HashSet<INode?>? nodes = null) {
        BranchingNode node = label is null
            ? new(selectPath)
            : new(label, selectPath);
        nodes?.Add(node);
        var builder = new BranchesBuilder(nodes, node);
        setPaths(builder);
        return node;
    }

    public static TNode Create<TNode>(string? label = null)
        where TNode : BranchingNode<TNode>
        => InstanceFactory.Create<TNode>(label);
}

public abstract class BranchingNode<TNode>(string? label = null)
    : Node<TNode>(label),
      IBranchingNode
    where TNode : BranchingNode<TNode> {
    protected abstract string Select(Context context);

    public Dictionary<string, INode?> Choices { get; } = [];

    protected override Result IsValid(ISet<INode> visited) {
        var result = base.IsValid(visited);
        return Choices.Values.Where(node => node is not null).Distinct()
                    .Aggregate(result, (current, node) => current + node!.Validate(visited));
    }

    protected override INode GetNext(Context context)
        => Choices.GetValueOrDefault(Select(context))
        ?? throw new InvalidOperationException("The selected path was not found.");

    protected sealed override void UpdateState(Context context) { }
}
