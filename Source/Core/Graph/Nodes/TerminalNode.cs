namespace DotNetToolbox.Graph.Nodes;

public sealed class TerminalNode(uint id, IServiceProvider services, int exitCode = 0, string? tag = null, string? label = null)
    : TerminalNode<TerminalNode>(id, services, tag, label) {
    protected override string DefaultLabel { get; } = "end";

    public override int ExitCode { get; } = exitCode;
}

public abstract class TerminalNode<TNode>(uint id, IServiceProvider services, string? tag = null, string? label = null)
    : Node<TNode>(id, services, tag, label),
      ITerminationNode
    where TNode : TerminalNode<TNode> {
    protected override Task<INode?> GetNext(Context context, CancellationToken ct)
        => Task.FromResult<INode?>(null);

    protected override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public abstract int ExitCode { get; }
}
