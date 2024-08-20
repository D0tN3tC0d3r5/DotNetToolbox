namespace DotNetToolbox.Graph.Nodes;

public sealed class JumpNode : JumpNode<JumpNode> {
    internal JumpNode(string? id, INodeSequence? sequence, string targetTag)
        : base(id, sequence) {
        TargetTag = IsNotNull(targetTag);
    }

    public JumpNode(string id, string targetTag)
        : this(IsNotNullOrWhiteSpace(id), null, targetTag) {
    }
    public JumpNode(string targetTag, INodeSequence? sequence = null)
        : this(null, sequence, targetTag) {
    }

    protected override string DefaultLabel { get; } = "goto";

    public override string TargetTag { get; }
}

public abstract class JumpNode<TNode>(string? id, INodeSequence? sequence = null)
    : Node<TNode>(id, sequence),
      IJumpNode
    where TNode : JumpNode<TNode> {
    protected override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult(Next);

    protected override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public override void ConnectTo(INode? next)
        => Next = next;

    public abstract string TargetTag { get; }
}
