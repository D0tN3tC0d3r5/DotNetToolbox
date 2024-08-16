namespace DotNetToolbox.Graph.Nodes;

public sealed class JumpNode(IServiceProvider services, uint id, string targetTag, string? tag = null, string? label = null)
    : JumpNode<JumpNode>(services, id, tag, label) {
    protected override string DefaultLabel { get; } = "goto";

    public override string TargetTag { get; } = IsNotNull(targetTag);
}

public abstract class JumpNode<TNode>(IServiceProvider services, uint id, string? tag = null, string? label = null)
    : Node<TNode>(services, id, tag, label),
      IJumpNode
    where TNode : JumpNode<TNode> {
    protected override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult(Next);

    protected override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public override Result ConnectTo(INode? next)
        => next is not null
        ? Success()
        : new ValidationError($"Jump target '{TargetTag}' not found.", Token?.ToSource());

    public abstract string TargetTag { get; }
}
