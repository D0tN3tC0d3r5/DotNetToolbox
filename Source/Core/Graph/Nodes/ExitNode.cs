namespace DotNetToolbox.Graph.Nodes;

public sealed class ExitNode : ExitNode<ExitNode> {
    public ExitNode(int exitCode, IServiceProvider services)
        : base(services) {
        ExitCode = exitCode;
        Label = "end";
    }
    public ExitNode(IServiceProvider services)
        : this(0, services) {
    }
    public ExitNode(string tag, int exitCode, IServiceProvider services)
        : this(exitCode, services) {
        Tag = IsNotNullOrWhiteSpace(tag);
    }
    public ExitNode(string tag, IServiceProvider services)
        : this(tag, 0, services) {
    }

    public override int ExitCode { get; }
}

public abstract class ExitNode<TNode>(IServiceProvider services)
    : Node<TNode>(services),
      IExitNode
    where TNode : ExitNode<TNode> {
    protected override Task<INode?> SelectPath(Context context, CancellationToken ct = default)
        => Task.FromResult<INode?>(null);

    protected override Task UpdateState(Context context, CancellationToken ct = default)
        => Task.CompletedTask;

    public override void ConnectTo(INode? next)
        => throw new ValidationException("An exit node cannot be connected to another node.", Token?.ToSource() ?? Tag);

    public abstract int ExitCode { get; }
}
