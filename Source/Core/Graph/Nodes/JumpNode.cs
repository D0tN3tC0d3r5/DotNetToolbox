namespace DotNetToolbox.Graph.Nodes;

public sealed class JumpNode(uint id, IServiceProvider services, string targetTag, string? label = null)
    : JumpNode<JumpNode>(id, services, label) {
    protected override string DefaultLabel { get; } = "goto";

    public override string TargetTag { get; } = IsNotNull(targetTag);
}

public abstract class JumpNode<TNode>(uint id, IServiceProvider services, string? label = null)
    : Node<TNode>(id, services, null, label),
      IJumpNode
    where TNode : JumpNode<TNode> {
    protected override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult<INode?>(null);

    protected override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public override void ConnectTo(INode? next) {
        // Jump nodes are connected after the builder finishes.
    }

    public abstract string TargetTag { get; }
}
