namespace DotNetToolbox.Graph.Nodes;

public sealed class JumpNode : JumpNode<JumpNode> {
    public JumpNode(string targetTag, IServiceProvider services)
        : base(services) {
        TargetTag = IsNotNull(targetTag);
        Label = "goto";
    }

    public JumpNode(string tag, string targetTag, IServiceProvider services)
        : this(targetTag, services) {
        Tag = IsNotNullOrWhiteSpace(tag);
    }

    public override string TargetTag { get; }
}

public abstract class JumpNode<TNode>(IServiceProvider services)
    : Node<TNode>(services),
      IJumpNode
    where TNode : JumpNode<TNode> {
    protected override Task<INode?> SelectPath(Context context, CancellationToken ct = default)
        => Task.FromResult(Next);

    protected override Task UpdateState(Context context, CancellationToken ct = default)
        => Task.CompletedTask;

    public override void ConnectTo(INode? next)
        => Next = next;

    public abstract string TargetTag { get; }
}
