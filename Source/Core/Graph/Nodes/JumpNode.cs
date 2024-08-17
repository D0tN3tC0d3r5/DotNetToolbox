namespace DotNetToolbox.Graph.Nodes;

public sealed class JumpNode(string id, string targetTag, INodeSequence? sequence = null)
    : JumpNode<JumpNode>(id, sequence) {
    public JumpNode(string targetTag, INodeSequence? sequence = null)
        : this(null!, targetTag, sequence) {
    }

    protected override string DefaultLabel { get; } = "goto";

    public override string TargetTag { get; } = IsNotNull(targetTag);
}

public abstract class JumpNode<TNode>(string id, INodeSequence? sequence = null)
    : Node<TNode>(id, sequence),
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
