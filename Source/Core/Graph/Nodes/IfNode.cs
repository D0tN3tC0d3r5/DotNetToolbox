namespace DotNetToolbox.Graph.Nodes;

public class IfNode : IfNode<IfNode> {
    private readonly Func<Context, CancellationToken, Task<bool>> _predicate;

    public IfNode(Func<Context, CancellationToken, Task<bool>> predicate, IServiceProvider services)
        : base(services) {
        _predicate = IsNotNull(predicate);
        Label = "if";
    }
    public IfNode(string tag, Func<Context, CancellationToken, Task<bool>> predicate, IServiceProvider services)
        : this(predicate, services) {
        Tag = IsNotNullOrWhiteSpace(tag);
    }
    public IfNode(Func<Context, bool> predicate, IServiceProvider services)
        : this((ctx, ct) => Task.Run(() => predicate(ctx), ct), services) {
    }
    public IfNode(string tag, Func<Context, bool> predicate, IServiceProvider services)
        : this(tag, (ctx, ct) => Task.Run(() => predicate(ctx), ct), services) {
    }

    protected override Task<bool> If(Context context, CancellationToken ct = default) => _predicate(context, ct);
}

public abstract class IfNode<TNode>(IServiceProvider services)
    : Node<TNode>(services),
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

    protected sealed override Task UpdateState(Context context, CancellationToken ct = default)
        => Task.CompletedTask;

    protected sealed override async Task<INode?> SelectPath(Context context, CancellationToken ct = default)
        => await If(context, ct)
               ? await ThenDo(context, ct)
               : await ElseDo(context, ct);

    protected abstract Task<bool> If(Context context, CancellationToken ct = default);

    private Task<INode?> ThenDo(Context context, CancellationToken ct)
        => Then is not null ? Then.Run(context, ct) : Task.FromResult<INode?>(null);

    private Task<INode?> ElseDo(Context context, CancellationToken ct)
        => Else is not null ? Else.Run(context, ct) : Task.FromResult<INode?>(null);

    public sealed override void ConnectTo(INode? next) {
        if (Then is null) Then = next;
        else Then.ConnectTo(next);
        if (Else is null) Else = next;
        else Else.ConnectTo(next);
    }
}
