namespace DotNetToolbox.Graph.Nodes;

public class IfNode(string id, Func<Context, CancellationToken, Task<bool>> predicate, INodeSequence? sequence = null)
    : IfNode<IfNode>(id, sequence) {
    public IfNode(string id, Func<Context, bool> predicate, INodeSequence? sequence = null)
        : this(id, (ctx, ct) => Task.Run(() => predicate(ctx), ct), sequence) {
    }
    public IfNode(Func<Context, CancellationToken, Task<bool>> predicate, INodeSequence? sequence = null)
        : this(null!, predicate, sequence) {
    }
    public IfNode(Func<Context, bool> predicate, INodeSequence? sequence = null)
        : this(null!, (ctx, ct) => Task.Run(() => predicate(ctx), ct), sequence) {
    }

    private readonly Func<Context, CancellationToken, Task<bool>> _predicate = IsNotNull(predicate);

    protected override string DefaultLabel { get; } = "if";

    protected override Task<bool> When(Context context, CancellationToken ct) => _predicate(context, ct);

    public static TNode Create<TNode>(uint id, string label, IServiceProvider services)
        where TNode : IfNode<TNode>
        => InstanceFactory.Create<TNode>(id, label, services);
    public static TNode Create<TNode>(uint id, IServiceProvider services)
        where TNode : IfNode<TNode>
        => InstanceFactory.Create<TNode>(id, services);
}

public abstract class IfNode<TNode>(string id, INodeSequence? sequence = null)
    : Node<TNode>(id, sequence),
      IIfNode
    where TNode : IfNode<TNode> {
    public INode? IsTrue { get; set; }
    public INode? IsFalse { get; set; }

    protected override Result IsValid(ISet<INode> visited) {
        var result = base.IsValid(visited);
        result += IsTrue?.Validate(visited) ?? new ValidationError("The true node is not set.", Token?.ToSource());
        result += IsFalse?.Validate(visited) ?? Success();
        return result;
    }

    protected sealed override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    protected sealed override async Task<INode?> SelectPath(Context context, CancellationToken ct)
        => await When(context, ct)
               ? await TryRunTrueNode(context, ct)
               : await TryRunFalseNode(context, ct);

    protected abstract Task<bool> When(Context context, CancellationToken ct);

    private Task<INode?> TryRunTrueNode(Context context, CancellationToken ct)
        => IsTrue is not null ? IsTrue.Run(context, ct) : Task.FromResult<INode?>(null);

    private Task<INode?> TryRunFalseNode(Context context, CancellationToken ct)
        => IsFalse is not null ? IsFalse.Run(context, ct) : Task.FromResult<INode?>(null);

    public sealed override Result ConnectTo(INode? next) {
        var result = Success();
        var token = next?.Token;
        if (IsTrue is null) IsTrue = next;
        else IsTrue.ConnectTo(next);
        if (IsFalse is null) IsFalse = next;
        else IsFalse.ConnectTo(next);
        return result;
    }
}
