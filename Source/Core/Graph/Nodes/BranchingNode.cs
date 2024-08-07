namespace DotNetToolbox.Graph.Nodes;

public sealed class BranchingNode(uint id, string label, Func<Context, string> select, IServiceProvider services)
    : BranchingNode<BranchingNode>(id, label, services) {
    public BranchingNode(uint id, Func<Context, string> select, IServiceProvider services)
        : this(id, _defaultLabel, select, services) {
    }

    private const string _defaultLabel = "case";

    protected override string Select(Context context) => IsNotNull(select)(context);

    public static TNode Create<TNode>(uint id, string label, IServiceProvider services)
        where TNode : BranchingNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, services);

    public static TNode Create<TNode>(uint id, IServiceProvider services)
        where TNode : BranchingNode<TNode>
        => InstanceFactory.Create<TNode>(id, services);
}

public abstract class BranchingNode<TNode>
    : Node<TNode>,
      IBranchingNode
    where TNode : BranchingNode<TNode> {
    protected BranchingNode(uint id, string label, IServiceProvider services)
        : base(id, label, services) { }

    protected BranchingNode(uint id, IServiceProvider services)
        : base(id, services) {
    }

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
