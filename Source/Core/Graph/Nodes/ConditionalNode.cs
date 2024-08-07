namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNode(uint id, string label, Func<Context, bool> predicate, IServiceProvider services)
    : ConditionalNode<ConditionalNode>(id, label, services) {
    public ConditionalNode(uint id, Func<Context, bool> predicate, IServiceProvider services)
        : this(id, _defaultLabel, predicate, services) {
    }

    private const string _defaultLabel = "if";

    protected override bool When(Context context) => IsNotNull(predicate)(context);

    public static TNode Create<TNode>(uint id, string label, IServiceProvider services)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, services);

    public static TNode Create<TNode>(uint id, IServiceProvider services)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(id, services);
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

    public INode? IsTrue { get; internal set; }
    public INode? IsFalse { get; internal set; }

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
