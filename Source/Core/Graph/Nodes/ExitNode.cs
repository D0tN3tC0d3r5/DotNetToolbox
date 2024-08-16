namespace DotNetToolbox.Graph.Nodes;

public sealed class ExitNode(IServiceProvider services, uint id, int exitCode = 0, string? tag = null, string? label = null)
    : ExitNode<ExitNode>(services, id, tag, label) {
    protected override string DefaultLabel { get; } = "end";

    public override int ExitCode { get; } = exitCode;
}

public abstract class ExitNode<TNode>(IServiceProvider services, uint id, string? tag = null, string? label = null)
    : Node<TNode>(services, id, tag, label),
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
