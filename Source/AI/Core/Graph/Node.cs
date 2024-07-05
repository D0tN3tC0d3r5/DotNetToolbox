namespace DotNetToolbox.AI.Graph;

public abstract class Runner(Node entry, IDisposable state)
    : IEnumerator<Node> {
    private bool _isValueDisposed;

    public IDisposable State { get; } = state;

    public Node Current { get; } = entry;
    object IEnumerator.Current => Current;

    public bool MoveNext() => throw new NotImplementedException();
    public void Reset() => throw new NotImplementedException();

    protected virtual void Dispose(bool disposing) {
        if (_isValueDisposed)
            return;
        if (disposing) {
            State.Dispose();
        }
        _isValueDisposed = true;
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}

public abstract record End(object? Value = null) : NodeResult(Value) {
}

public abstract record NodeResult(object? Value = null, Node? Next = null) {
}

public abstract class Node(string id, string name) {
    public string Id { get; set; } = id;
    public string Name { get; set; } = name;
    public List<Node> Edges { get; set; } = [];

    public abstract Task<NodeResult> Run(object input);

    public override string ToString() => Name;
}
