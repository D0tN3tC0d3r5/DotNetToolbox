namespace DotNetToolbox.Graph;

public sealed class Graph(INode startingNode) : IGraph {
    private readonly INode _startingNode = IsNotNull(startingNode);

    public Context State { get; private set; } = [];
    public INode? CurrentNode { get; private set; }

    public void Run(Context? initialState = null) {
        CurrentNode = _startingNode;
        State = initialState ?? [];
        while (CurrentNode is not null)
            CurrentNode = CurrentNode.Run(State);
    }

    private bool _isValueDisposed;
    public void Dispose() {
        if (_isValueDisposed)
            return;
        State.Dispose();
        _isValueDisposed = true;
    }
}
