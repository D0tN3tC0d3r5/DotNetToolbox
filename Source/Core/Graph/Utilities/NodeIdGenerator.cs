namespace DotNetToolbox.Graph.Utilities;

public sealed class NodeIdGenerator
    : IdGenerator<NodeIdGenerator, uint> {
    private static readonly ConcurrentDictionary<string, NodeIdGenerator> _providers = [];
    private uint _first;
    private uint _next;

    internal NodeIdGenerator() {
        Reset();
    }

    public override uint First => _first;
    public override uint PeekNext() => _next;
    public override uint ConsumeNext() => _next++;
    public override void SetNext(uint next)
        => _next = IsNotZero(next) - 1;
    public override void Reset(uint first = 1) {
        _first = IsNotZero(first) - 1;
        _next = _first;
    }
}
