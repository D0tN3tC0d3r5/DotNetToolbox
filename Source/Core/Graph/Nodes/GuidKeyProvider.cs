namespace DotNetToolbox.Graph.Nodes;

public class GuidKeyProvider
    : IKeyProvider<Guid> {
    private readonly IGuidProvider _guid;
    private Guid _next;

    public GuidKeyProvider(IGuidProvider? guid = null) {
        _guid = guid ?? new GuidProvider();
        Reset();
    }

    public void Reset()
        => _next = _guid.AsSortable.Create();

    public Guid Peek()
        => _next;

    public Guid GetNext() {
        var result = Peek();
        Reset();
        return result;
    }
}
