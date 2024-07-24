namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNode(uint id, string label, Func<Context, bool> predicate)
    : ConditionalNode<ConditionalNode>(id, IsNotNull(label)) {
    private const string _defaultLabel = "if";

    protected override bool When(Context context) => IsNotNull(predicate)(context);

    internal static ConditionalNode Create(uint id,
                                           string? label,
                                           Func<Context, bool> predicate,
                                           WorkflowBuilder builder,
                                           Action<WorkflowBuilder> setTrueBranch,
                                           Action<WorkflowBuilder>? setFalseBranch = null) {
        var node = new ConditionalNode(id, label ?? _defaultLabel, predicate);
        builder.Nodes.Add(node);
        var trueBuilder = new WorkflowBuilder(builder.Id, builder.Nodes);
        setTrueBranch(trueBuilder);
        node.IsTrue = trueBuilder.Start;
        if (setFalseBranch == null)
            return node;

        var falseBuilder = new WorkflowBuilder(builder.Id, builder.Nodes);
        setFalseBranch(falseBuilder);
        node.IsFalse = falseBuilder.Start;

        return node;
    }

    public static TNode Create<TNode>(uint id, string? label = null)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(id, label);
}

public abstract class ConditionalNode<TNode>(uint id, string? label = null)
    : Node<TNode>(id, label),
      IConditionalNode
    where TNode : ConditionalNode<TNode> {
    protected abstract bool When(Context context);

    public INode? IsTrue { get; protected set; }
    public INode? IsFalse { get; protected set; }

    protected override Result IsValid(ISet<INode> visited) {
        var result = base.IsValid(visited);
        result += IsTrue?.Validate(visited) ?? Success();
        result += IsFalse?.Validate(visited) ?? Success();
        return result;
    }

    protected sealed override INode? GetNext(Context context)
        => When(context)
            ? IsTrue?.Run(context)
            : IsFalse?.Run(context);

    protected sealed override void UpdateState(Context context) { }
}
