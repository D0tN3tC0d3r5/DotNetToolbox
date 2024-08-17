namespace DotNetToolbox.Graph.Nodes;

public sealed class ExitNode(uint id, int exitCode = 0, string? tag = null, string? label = null)
    : ExitNode<ExitNode>(id, tag, label) {
    protected override string DefaultLabel { get; } = "end";

    public override int ExitCode { get; } = exitCode;
}

public abstract class ExitNode<TNode>(uint id, string? tag, string? label)
    : Node<TNode>(id, tag, label),
      IExitNode
    where TNode : ExitNode<TNode> {
    protected override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult<INode?>(null);

    protected override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public override Result ConnectTo(INode? next)
        => new ValidationError("Cannot connect to an exit node.", Token?.ToSource());

    public abstract int ExitCode { get; }
}
