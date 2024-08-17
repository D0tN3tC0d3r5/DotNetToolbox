namespace DotNetToolbox.Graph.Utilities;

public sealed class NodeIdGenerator
    : IdGenerator<NodeIdGenerator, uint> {
    private uint _first;
    private uint _next;

    public NodeIdGenerator()
        : this(1) {
    }

    public NodeIdGenerator(uint first) {
        Reset(first);
    }

    public override uint First => _first;
    public override uint Peek() => _next;
    public override uint Consume() => _next++;
    public override void Set(uint next)
        => _next = next >= _first ? next : throw new ArgumentOutOfRangeException(nameof(next), "The next value must be greater than or equal to the first value.");
    public override void Reset(uint first = 1) {
        _first = IsNotZero(first);
        _next = _first;
    }
}
