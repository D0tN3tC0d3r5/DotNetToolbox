namespace DotNetToolbox.Graph.Nodes;

public sealed class ExitNode(string id, int exitCode = 0, INodeSequence? sequence = null)
    : ExitNode<ExitNode>(id, sequence) {
    public ExitNode(int exitCode = 0, INodeSequence? sequence = null)
        : this(null!, exitCode, sequence) {
    }

    protected override string DefaultLabel { get; } = "end";

    public override int ExitCode { get; } = exitCode;
}

public abstract class ExitNode<TNode>(string? id, INodeSequence? sequence)
    : Node<TNode>(id, sequence),
      IExitNode
    where TNode : ExitNode<TNode> {
    protected override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult<INode?>(null);

    protected override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public override void ConnectTo(INode? next)
        => throw new ValidationException("An exit node cannot be conected to another node.", Token?.ToSource() ?? Id);

    public abstract int ExitCode { get; }
}
