namespace DotNetToolbox.Graph.Nodes;

public sealed class TerminalNode
    : Node<TerminalNode>,
      ITerminationNode {
    private const string _defaultLabel = "end";

    private TerminalNode(uint id, string label)
        : base(id, label) {
    }

    internal static TerminalNode Create(uint id,
                                      string? label)
        => new(id, label ?? _defaultLabel);

    protected override INode? GetNext(Context context)
        => null;

    protected override void UpdateState(Context context) { }
}
