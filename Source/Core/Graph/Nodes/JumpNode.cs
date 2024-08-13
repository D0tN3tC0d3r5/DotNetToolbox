namespace DotNetToolbox.Graph.Nodes;

public abstract class JumpNode<TNode>(uint id, IServiceProvider services, string? tag = null, string? label = null)
    : Node<TerminalNode>(id, services, tag, label),
      ITerminationNode
    where TNode : TerminalNode<TNode> {
    protected override Task<INode?> GetNext(Context context, CancellationToken ct)
        => Task.FromResult<INode?>(null);

    protected override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public abstract int ExitCode { get; }
}
