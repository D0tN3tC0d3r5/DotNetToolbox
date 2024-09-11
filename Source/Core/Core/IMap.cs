namespace DotNetToolbox;

public interface IMap : IMap<object> {
    TValue GetRequiredValueAs<TValue>(string key);
    TValue GetValue<TValue>(string key, TValue defaultValue)
        where TValue : notnull;
    TValue? GetValueAs<TValue>(string key)
        where TValue : class;
    bool TryGetValueAs<TValue>(string key, [MaybeNullWhen(false)] out TValue value);
    List<TValue> GetRequiredList<TValue>(string key);
    List<TValue> GetList<TValue>(string key);
    bool TryGetList<TValue>(string key, out List<TValue> value);
}

[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Does not apply")]
public interface IMap<TValue>
    : IDictionary<string, TValue>,
      IDisposable {
    bool Remove(string key, bool disposeValue);
    TValue? GetValueOrDefault(string key, TValue? defaultValue = default);
}
