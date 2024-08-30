namespace DotNetToolbox.Graph.Utilities;

public sealed class NodeSequencer
    : NumericSequencer<NodeSequencer>
    , INodeSequencer {
    public NodeSequencer()
        : base(1) {
    }
}
