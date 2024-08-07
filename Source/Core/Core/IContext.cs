namespace DotNetToolbox;

[SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Does not apply")]
// ReSharper disable once PossibleInterfaceMemberAmbiguity - Does not apply
public interface IContext
    : IDictionary<string, object>,
      IReadOnlyDictionary<string, object>,
      IDisposable {
    TValue GetValueAs<TValue>(string key);
    bool TryGetValueAs<TValue>(string key, [MaybeNullWhen(false)] out TValue value);
}
