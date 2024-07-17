namespace DotNetToolbox.Graph;

// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - This class should allow the user to extend it.
public class Context
    : Dictionary<string, object>
    , IDisposable {
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
