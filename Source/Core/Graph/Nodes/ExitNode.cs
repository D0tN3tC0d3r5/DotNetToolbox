namespace DotNetToolbox.Graph.Nodes;

public sealed class ExitNode : ExitNode<ExitNode> {
    internal ExitNode(string? id, INodeSequence? sequence, int exitCode)
        : base(id, sequence) {
        ExitCode = exitCode;
    }

    public ExitNode(string id, int exitCode = 0)
        : this(IsNotNullOrWhiteSpace(id), null, exitCode) {
    }
    public ExitNode(int exitCode = 0, INodeSequence? sequence = null)
        : this(null, sequence, exitCode) {
    }

    protected override string DefaultLabel { get; } = "end";

    public override int ExitCode { get; }
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
