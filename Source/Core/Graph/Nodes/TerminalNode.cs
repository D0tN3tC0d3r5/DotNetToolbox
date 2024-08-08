namespace DotNetToolbox.Graph.Nodes;

public sealed class TerminalNode
    : Node<TerminalNode>,
      ITerminationNode {
    private const string _defaultLabel = "end";

    private TerminalNode(uint id, string label, IServiceProvider services)
        : base(id, label, services) {
    }

    protected override Task<INode?> GetNext(Context context, CancellationToken ct)
        => Task.FromResult<INode?>(null);

    protected override Task UpdateState(Context context, CancellationToken ct)
        => Task.CompletedTask;

    public static TerminalNode Create(uint id,
                                      string label,
                                      IServiceProvider services)
        => new(id, IsNotNull(label), services);

    public static TerminalNode Create(uint id,
                                      IServiceProvider services)
        => new(id, _defaultLabel, services);
}
