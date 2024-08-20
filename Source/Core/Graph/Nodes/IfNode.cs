namespace DotNetToolbox.Graph.Nodes;

public class IfNode : IfNode<IfNode> {
    internal IfNode(string? id, INodeSequence? sequence, Func<Context, CancellationToken, Task<bool>> predicate)
        : base(id, sequence) {
        _predicate = IsNotNull(predicate);
    }

    public IfNode(string id, Func<Context, CancellationToken, Task<bool>> predicate)
        : this(IsNotNullOrWhiteSpace(id), null, predicate) {
    }
    public IfNode(Func<Context, CancellationToken, Task<bool>> predicate, INodeSequence? sequence = null)
        : this(null, sequence, predicate) {
    }
    public IfNode(string id, Func<Context, bool> predicate)
        : this(IsNotNullOrWhiteSpace(id), null, (ctx, ct) => Task.Run(() => predicate(ctx), ct)) {
    }
    public IfNode(Func<Context, bool> predicate, INodeSequence? sequence = null)
        : this(null, sequence, (ctx, ct) => Task.Run(() => predicate(ctx), ct)) {
    }

    private readonly Func<Context, CancellationToken, Task<bool>> _predicate;

    protected override string DefaultLabel { get; } = "if";

    protected override Task<bool> When(Context context, CancellationToken ct) => _predicate(context, ct);
}

public abstract class IfNode<TNode>(string? id, INodeSequence? sequence)
    : Node<TNode>(id, sequence),
      IIfNode
    where TNode : IfNode<TNode> {
    public INode? Then { get; set; }
    public INode? Else { get; set; }

    protected override Result IsValid(ISet<INode> visited) {
        var result = base.IsValid(visited);
        result += Then?.Validate(visited) ?? new ValidationError("The true node is not set.", Token?.ToSource());
        result += Else?.Validate(visited) ?? Success();
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
        => Then is not null ? Then.Run(context, ct) : Task.FromResult<INode?>(null);

    private Task<INode?> TryRunFalseNode(Context context, CancellationToken ct)
        => Else is not null ? Else.Run(context, ct) : Task.FromResult<INode?>(null);

    public sealed override void ConnectTo(INode? next) {
        if (Then is null) Then = next;
        else Then.ConnectTo(next);
        if (Else is null) Else = next;
        else Else.ConnectTo(next);
    }
}
