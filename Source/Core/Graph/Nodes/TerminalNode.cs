namespace DotNetToolbox.Graph.Nodes;

public sealed class TerminalNode
    : Node<TerminalNode>,
      ITerminationNode {
    private const string _defaultLabel = "end";

    private TerminalNode(uint id, string label, IServiceProvider services)
        : base(id, label, services) {
    }

    protected sealed override INode? GetNext(Context context)
        => null;

    protected override void UpdateState(Context context) { }

    public static TerminalNode Create(uint id,
                                      string label,
                                      IServiceProvider services)
        => new(id, IsNotNull(label), services);

    public static TerminalNode Create(uint id,
                                      IServiceProvider services)
        => new(id, _defaultLabel, services);
}
