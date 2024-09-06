namespace DotNetToolbox;

public interface IMap : IMap<object> {
    TValue? GetValueOrDefaultAs<TValue>(string key);
    bool TryGetValueAs<TValue>(string key, [MaybeNullWhen(false)] out TValue value);
}

[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Does not apply")]
public interface IMap<TValue>
    : IDictionary<string, TValue> {
    TValue? GetValueOrDefault(string key);
}
