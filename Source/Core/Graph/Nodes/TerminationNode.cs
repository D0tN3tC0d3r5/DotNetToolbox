namespace DotNetToolbox.Graph.Nodes;

public sealed class TerminationNode(string? label = null)
    : TerminationNode<TerminationNode>(label ?? _defaultLabel) {
    private const string _defaultLabel = "end";

    internal static TerminationNode Create(string? label)
        => new(label);

    public static TNode Create<TNode>(string? label = null)
        where TNode : TerminationNode<TNode>
        => InstanceFactory.Create<TNode>(label);
}

public class TerminationNode<TNode>(string? label = null)
    : Node<TNode>(label),
      ITerminationNode
    where TNode : TerminationNode<TNode> {
    protected override INode? GetNext(Context context)
        => null;

    protected override void UpdateState(Context context) { }
}
