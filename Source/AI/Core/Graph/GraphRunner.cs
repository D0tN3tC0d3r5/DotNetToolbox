namespace DotNetToolbox.AI.Graph;

public abstract class GraphRunner : IRunner {
    private readonly INode _startingNode;
    private readonly Map _initialState;

    protected GraphRunner(INode startingNode, Map? initialState = null) {
        _startingNode = IsNotNull(startingNode);
        _initialState = initialState ?? [];
        Reset();
    }

    public Map State { get; private set; } = [];
    public INode? CurrentNode { get; private set; }

    public virtual void Run() {
        while (CurrentNode is not null)
            CurrentNode = CurrentNode.Execute(State);
    }

    public void Reset() {
        State = _initialState;
        CurrentNode = _startingNode;
    }

    private bool _isValueDisposed;
    protected virtual void Dispose(bool disposing) {
        if (_isValueDisposed)
            return;
        if (disposing)
            State.Dispose();
        _isValueDisposed = true;
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
