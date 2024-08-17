namespace DotNetToolbox.Graph.Utilities;

public sealed class NodeSequence
    : CountableSequence<NodeSequence>
    , INodeSequence {
    public NodeSequence()
        : base(1) {
    }
}
