namespace DotNetToolbox;

public class Context(IEnumerable<KeyValuePair<string, object>>? source = null)
    : Context<object>(source),
      IContext {
    public TValue? GetValueOrDefaultAs<TValue>(string key)
       => TryGetValueAs<TValue>(key, out var value)
              ? value
              : default;

    public bool TryGetValueAs<TValue>(string key, [MaybeNullWhen(false)] out TValue value) {
        value = default;
        if (!TryGetValue(key, out var obj) || obj is not TValue result) return false;
        value = result;
        return true;
    }
}

public class Context<TValue>(IEnumerable<KeyValuePair<string, TValue>>? source = null)
    : Map<TValue>(source),
      IContext<TValue> {
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
}
