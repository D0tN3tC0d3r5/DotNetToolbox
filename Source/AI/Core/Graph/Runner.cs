namespace DotNetToolbox.AI.Graph;

public abstract class Runner {
    private bool _isValueDisposed;
    private readonly INode _startingNode;

    protected Runner(INode startingNode, IDisposable state) {
        _startingNode = IsNotNull(startingNode);
        CurrentNode = _startingNode;
        CurrentState = state;
    }

    public IDisposable CurrentState { get; }

    public INode CurrentNode { get; private set; }

    public object? Run(object? input) {
    }

    public TOutput Run<TInput, TOutput>(TInput input) {
        var result = CurrentNode.Execute();
        while (result.Next is not TerminalNode<TOutput>) {
            result = CurrentNode.Execute();
        }
        return result.Value;
    }

    public void Reset() => CurrentNode = _startingNode;

    protected virtual void Dispose(bool disposing) {
        if (_isValueDisposed)
            return;
        if (disposing) {
            CurrentState.Dispose();
        }
        _isValueDisposed = true;
    }

    public void Dispose() {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
