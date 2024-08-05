namespace DotNetToolbox.Graph.Nodes;

public class TerminalNode
    : Node<TerminalNode>,
      ITerminationNode {
    private const string _defaultLabel = "end";

    private TerminalNode(uint id, string label, IServiceProvider services)
        : base(id, label, services) {
    }

    private TerminalNode(uint id, IServiceProvider services)
        : base(id, services) { }

    internal static TerminalNode Create(uint id,
                                        string? label,
                                        IServiceProvider services)
        => new(id, label ?? _defaultLabel, services);

    protected sealed override INode? GetNext(Context context)
        => null;

    protected override void UpdateState(Context context) { }
}
