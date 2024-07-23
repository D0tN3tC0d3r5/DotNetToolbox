namespace DotNetToolbox.Graph.Nodes;

public sealed class StartingNode(string? label = null)
    : StartingNode<StartingNode>(label ?? _defaultLabel) {
    private const string _defaultLabel = "start";

    internal static StartingNode Create(string? label)
        => new(label);

    public static TNode Create<TNode>(string? label = null)
        where TNode : StartingNode<TNode>
        => InstanceFactory.Create<TNode>(label);
}

public class StartingNode<TNode>(string? label = null)
    : Node<TNode>(label),
      IStartingNode
    where TNode : StartingNode<TNode> {
    protected override INode? GetNext(Context context)
        => Next;

    protected override void UpdateState(Context context) { }
}
