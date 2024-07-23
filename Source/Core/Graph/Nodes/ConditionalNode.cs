namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNode(string label, Func<Context, bool> predicate)
    : ConditionalNode<ConditionalNode>(IsNotNull(label)) {
    private const string _defaultLabel = "if";

    public ConditionalNode(Func<Context, bool> predicate)
        : this(_defaultLabel, predicate) {
    }

    protected override bool Check(Context context) => IsNotNull(predicate)(context);

    internal static ConditionalNode Create(string? label,
                                           Func<Context, bool> predicate,
                                           Action<WorkflowBuilder> setTrueBranch,
                                           Action<WorkflowBuilder>? setFalseBranch = null,
                                           HashSet<INode?>? nodes = null) {
        ConditionalNode node = label is null
            ? new(predicate)
            : new(label, predicate);
        nodes?.Add(node);
        var trueBuilder = new WorkflowBuilder(nodes);
        setTrueBranch(trueBuilder);
        node.True = trueBuilder.Start;
        if (setFalseBranch == null)
            return node;

        var falseBuilder = new WorkflowBuilder(nodes);
        setFalseBranch(falseBuilder);
        node.False = falseBuilder.Start;

        return node;
    }

    public static TNode Create<TNode>(string? label = null)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(label);
}

public abstract class ConditionalNode<TNode>(string? label = null)
    : Node<TNode>(label),
      IConditionalNode
    where TNode : ConditionalNode<TNode> {
    protected abstract bool Check(Context context);

    public INode? True { get; set; }
    public INode? False { get; set; }

    protected override Result IsValid(ISet<INode> visited) {
        var result = base.IsValid(visited);
        result += True?.Validate(visited) ?? Success();
        result += False?.Validate(visited) ?? Success();
        return result;
    }

    protected sealed override INode? GetNext(Context context)
        => Check(context)
            ? True
            : False;

    protected sealed override void UpdateState(Context context) { }
}
