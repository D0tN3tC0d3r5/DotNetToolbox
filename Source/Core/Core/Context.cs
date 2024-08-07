namespace DotNetToolbox;

public abstract class Context
    : IContext {
    private bool _isDisposed;
    private readonly bool _isImported;
    private readonly ConcurrentDictionary<string, object> _data;

    protected Context(IDictionary<string, object>? source = null) {
        if (source is Context context) {
            _data = context._data;
            _isImported = true;
            return;
        }

        _data = source is not null ? new(source) : [];
    }

    private void Dispose(bool disposing) {
        if (_isDisposed || _isImported) return;
        if (disposing) {
            foreach (var disposable in _data.Values.OfType<IDisposable>()) disposable.Dispose();
        }

        _isDisposed = true;
    }

    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public TValue GetValueAs<TValue>(string key) {
        var value = this[key];
        return value switch {
            TValue result => result,
            not null when value.GetType().IsAssignableTo(typeof(TValue)) => (TValue)value,
            null when typeof(TValue).IsClass => default!,
            _ => throw new InvalidCastException($"The value for key '{key}' is not of type '{typeof(TValue)}'."),
        };
    }

    public bool TryGetValueAs<TValue>(string key, [MaybeNullWhen(false)] out TValue value) {
        value = default;
        if (!_data.TryGetValue(key, out var obj)) return false;
        switch (obj) {
            case TValue result:
                value = result;
                return true;
            case not null when obj.GetType().IsAssignableTo(typeof(TValue)):
                value = (TValue)obj;
                return true;
            default:
                return false;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_data).GetEnumerator();
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => _data.GetEnumerator();

    public bool IsReadOnly => false;
    public int Count => _data.Count;

    [AllowNull]
    public object this[string key] {
        get => _data[key];
        set {
            if (value is null) Remove(key);
            else _data[key] = value;
        }
    }

    public ICollection<string> Keys => _data.Keys;
    public ICollection<object> Values => _data.Values;
    public void Clear() => _data.Clear();
    public void Add(string key, object value) => _data.AddOrUpdate(key, k => this[k] = IsNotNull(value), (k, _) => this[k] = IsNotNull(value));
    public bool Remove(string key) => _data.TryRemove(key, out _);
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object value) => _data.TryGetValue(key, out value);
    public bool ContainsKey(string key) => _data.ContainsKey(key);

    void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item) => Add(item.Key, item.Value);
    bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item) => ContainsKey(item.Key) && this[item.Key].Equals(item.Value);
    bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item) => Remove(item.Key);

    IEnumerable<string> IReadOnlyDictionary<string, object>.Keys => _data.Keys;
    IEnumerable<object> IReadOnlyDictionary<string, object>.Values => _data.Values;

    public override string ToString() {
        var builder = new StringBuilder();
        ToText(builder);
        return builder.ToString();
    }

    private void ToText(StringBuilder builder, string? name = null, uint level = 0) {
        if (Keys.Count == 0) return;
        var indent = new string(' ', (int)level * 4);
        if (!string.IsNullOrWhiteSpace(name))
            builder.Append($"{name}:");
        if (Keys.Count == 0) builder.AppendLine(" [Empty]");
        builder.AppendLine();
        foreach (var key in Keys) {
            builder.Append($"{indent}- ");
            BuildItem(builder, key, this[key], level);
        }
    }

    private static void BuildItem(StringBuilder builder, string key, object value, uint level) {
        switch (value) {
            case null:
                builder.AppendLine($"{key}: null");
                return;
            case IContext context:
                ((Context)context).ToText(builder, key, level + 1);
                return;
            case string:
                builder.AppendLine($"{key}: {value}");
                return;
            case IEnumerable items:
                var counter = 1;
                builder.AppendLine($"{key}:");
                foreach (var item in items)
                    BuildItem(builder, $"{counter++} => ", item, level);
                return;
            case not null when value.GetType().IsClass:
                builder.AppendLine($"{key}: {JsonSerializer.Serialize(value)}");
                return;
            default:
                builder.AppendLine($"{key}: {value}");
                return;
        }
    }

    void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex) => throw new NotSupportedException();
}
