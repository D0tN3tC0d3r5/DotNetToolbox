namespace DotNetToolbox.Graph.Nodes;

public sealed class BranchingNode(uint id, string label, Func<Context, string> select)
    : BranchingNode<BranchingNode>(id, IsNotNull(label)) {
    private const string _defaultLabel = "case";

    protected override string Select(Context context) => IsNotNull(select)(context);

    internal static BranchingNode Create(uint id,
                                         string? label,
                                         Func<Context, string> selectPath,
                                         WorkflowBuilder builder,
                                         Action<BranchesBuilder> setPaths) {
        var node = new BranchingNode(id, label ?? _defaultLabel, selectPath);
        builder.Nodes.Add(node);
        var branchesBuilder = new BranchesBuilder(builder, node);
        setPaths(branchesBuilder);
        return node;
    }

    public static TNode Create<TNode>(uint id, string? label = null)
        where TNode : BranchingNode<TNode>
        => InstanceFactory.Create<TNode>(id, label);
}

public abstract class BranchingNode<TNode>(uint id, string? label = null)
    : Node<TNode>(id, label),
      IBranchingNode
    where TNode : BranchingNode<TNode> {
    protected abstract string Select(Context context);

    public Dictionary<string, INode?> Choices { get; } = [];

    protected override Result IsValid(ISet<INode> visited) {
        var result = base.IsValid(visited);
        var choices = Choices.Values
                             .Where(c => c is not null)
                             .Cast<INode>()
                             .Distinct();
        return choices.Aggregate(result, ValidateChoice);

        Result ValidateChoice(Result current, INode choice)
            => current + choice.Validate(visited);
    }

    protected override INode? GetNext(Context context) {
        var key = Select(context);
        var choice = Choices.GetValueOrDefault(key)
            ?? throw new InvalidOperationException($"The path '{key}' was not found.");
        return choice.Run(context);
    }

    protected sealed override void UpdateState(Context context) { }
}
