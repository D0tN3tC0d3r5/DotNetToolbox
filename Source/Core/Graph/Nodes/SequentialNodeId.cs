namespace DotNetToolbox.Graph.Nodes;

public class SequentialNodeId(uint first = 1)
    : NodeId<uint> {
    private uint _next = first;
    private readonly uint _first = first;

    public override void Reset() => _next = _first;

    public override uint Next => _next++;
}
