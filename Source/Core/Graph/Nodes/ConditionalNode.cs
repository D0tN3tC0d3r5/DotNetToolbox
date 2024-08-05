namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNode
    : ConditionalNode<ConditionalNode> {
    private readonly Func<Context, bool> _predicate;

    public ConditionalNode(uint id, string label, Func<Context, bool> predicate, IServiceProvider services)
        : base(id, label, services) {
        _predicate = predicate;
    }

    public ConditionalNode(uint id, Func<Context, bool> predicate, IServiceProvider services)
        : base(id, _defaultLabel, services) {
        _predicate = predicate;
    }

    private const string _defaultLabel = "if";

    protected override bool When(Context context) => IsNotNull(_predicate)(context);

    internal static ConditionalNode Create(uint id,
                                           string? label,
                                           Func<Context, bool> predicate,
                                           WorkflowBuilder builder,
                                           Action<WorkflowBuilder> setTrueBranch,
                                           Action<WorkflowBuilder>? setFalseBranch,
                                           IServiceProvider services) {
        var node = new ConditionalNode(id, label ?? _defaultLabel, predicate, services);
        builder.Nodes.Add(node);
        var trueBuilder = new WorkflowBuilder(builder.Id, builder.Nodes, services);
        setTrueBranch(trueBuilder);
        node.IsTrue = trueBuilder.Start;
        if (setFalseBranch == null)
            return node;

        var falseBuilder = new WorkflowBuilder(builder.Id, builder.Nodes, services);
        setFalseBranch(falseBuilder);
        node.IsFalse = falseBuilder.Start;

        return node;
    }

    public static TNode Create<TNode>(uint id, string label, IServiceProvider services)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, services);

    public static TNode Create<TNode>(uint id, IServiceProvider services)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(id, _defaultLabel, services);
}

public abstract class ConditionalNode<TNode>
    : Node<TNode>,
      IConditionalNode
    where TNode : ConditionalNode<TNode> {
    protected ConditionalNode(uint id, string label, IServiceProvider services)
        : base(id, label, services) {
    }

    protected ConditionalNode(uint id, IServiceProvider services)
        : base(id, services) { }

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
