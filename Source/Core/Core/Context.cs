namespace DotNetToolbox;

public class Context(IServiceProvider services, IDictionary<string, object>? source = null)
    : Map(source),
      IContext {
    private bool _isDisposed;

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool isDisposing) {
        if (_isDisposed) return;
        if (isDisposing)
            foreach (var key in MyKeys) Remove(key);
        _isDisposed = true;
    }

    public IServiceProvider Services { get; } = services;
}
