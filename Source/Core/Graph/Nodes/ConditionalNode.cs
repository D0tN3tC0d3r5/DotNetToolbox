namespace DotNetToolbox.Graph.Nodes;

public class ConditionalNode(uint id, string label, Func<Context, CancellationToken, Task<bool>> predicate, IServiceProvider services)
    : ConditionalNode<ConditionalNode>(id, label, services) {
    private readonly Func<Context, CancellationToken, Task<bool>> _predicate = IsNotNull(predicate);

    private const string _defaultLabel = "if";

    public ConditionalNode(uint id, Func<Context, CancellationToken, Task<bool>> predicate, IServiceProvider services)
        : this(id, _defaultLabel, predicate, services) {
    }
    public ConditionalNode(uint id, string label, Func<Context, bool> predicate, IServiceProvider services)
        : this(id, label, (ctx, ct) => Task.Run(() => predicate(ctx), ct), services) {
    }
    public ConditionalNode(uint id, Func<Context, bool> predicate, IServiceProvider services)
        : this(id, _defaultLabel, predicate, services) {
    }

    public static TNode Create<TNode>(uint id, string label, IServiceProvider services)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, services);

    public static TNode Create<TNode>(uint id, IServiceProvider services)
        where TNode : ConditionalNode<TNode>
        => InstanceFactory.Create<TNode>(id, services);

    protected override Task<bool> When(Context context, CancellationToken ct) => _predicate(context, ct);
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

    public INode? IsTrue { get; internal set; }
    public INode? IsFalse { get; internal set; }

    protected override Result IsValid(ISet<INode> visited) {
        var result = base.IsValid(visited);
        result += IsTrue?.Validate(visited) ?? Success();
        result += IsFalse?.Validate(visited) ?? Success();
        return result;
    }

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    protected sealed override async Task<INode?> GetNext(Context context, CancellationToken ct) {
        if (await When(context, ct)) {
            return await TryRunTrueNode(context, ct);
        }
        else {
            return await TryRunFalseNode(context, ct);
        }
    }

    protected abstract Task<bool> When(Context context, CancellationToken ct);

    private Task<INode?> TryRunTrueNode(Context context, CancellationToken ct)
        => IsTrue is not null ? IsTrue.Run(context, ct) : Task.FromResult<INode?>(null);

    private Task<INode?> TryRunFalseNode(Context context, CancellationToken ct)
        => IsFalse is not null ? IsFalse.Run(context, ct) : Task.FromResult<INode?>(null);
}
