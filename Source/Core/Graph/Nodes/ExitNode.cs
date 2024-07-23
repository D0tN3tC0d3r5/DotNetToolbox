namespace DotNetToolbox.Graph.Nodes;

public sealed class ExitNode
    : Node {
    private ExitNode(INodeFactory? factory = null)
        : base(factory) {
    }

    public static ExitNode Create(INodeFactory? factory = null)
        => new(factory);

    protected override INode? GetNext(Context context)
        => null;

    protected override void UpdateState(Context context) { }
}
