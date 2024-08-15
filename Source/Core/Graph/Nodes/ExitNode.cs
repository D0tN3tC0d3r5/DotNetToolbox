namespace DotNetToolbox.Graph.Nodes;

public sealed class ExitNode(uint id, IServiceProvider services, int exitCode = 0, string? tag = null, string? label = null)
    : ExitNode<ExitNode>(id, services, tag, label) {
    protected override string DefaultLabel { get; } = "end";

    public override int ExitCode { get; } = exitCode;
}

public abstract class ExitNode<TNode>(uint id, IServiceProvider services, string? tag = null, string? label = null)
    : Node<TNode>(id, services, tag, label),
      IEndNode
    where TNode : ExitNode<TNode> {
    protected override Task<INode?> SelectPath(Context context, CancellationToken ct)
        => Task.FromResult<INode?>(null);

    protected override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;
    public override void ConnectTo(INode? next) {
        if (next is not null)
            throw new InvalidOperationException($"Cannot connect end node '{Tag}' to another node.");
    }

    public abstract int ExitCode { get; }
}
