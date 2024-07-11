namespace DotNetToolbox.Graph;

public class Map
    : Dictionary<string, object>, IDisposable {
    private bool _isDisposed;

    protected virtual void Dispose(bool disposing) {
        if (_isDisposed)
            return;
        if (disposing) {
            foreach (var disposable in Values.OfType<IDisposable>())
                disposable.Dispose();
        }
        _isDisposed = true;
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
