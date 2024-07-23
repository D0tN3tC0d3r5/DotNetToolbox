namespace DotNetToolbox.Graph.Nodes;

public class SequentialKeyProvider
    : IKeyProvider<uint> {
    private readonly uint _start;

    public SequentialKeyProvider(uint start = 0) {
        _start = start;
        Reset();
    }

    private static uint _current = 0;
    public uint GetNext() => ++_current;
    public void Reset() => _current = _start;
    public uint Peek() => _current;
}
