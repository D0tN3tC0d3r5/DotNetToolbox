namespace DotNetToolbox.Data.KeyGenerators;

public class InMemoryKeyGenerator<TKeyValue>
    : IKeyGenerator<TKeyValue> {
    public static InMemoryKeyGenerator<TKeyValue> Instance { get; } = new();

    private readonly ConcurrentDictionary<string, TKeyValue> _keyStorage = new();

    private InMemoryKeyGenerator() { }

    public virtual TKeyValue GenerateNextKey(string? storageKey = null)
        => _keyStorage.AddOrUpdate(storageKey ?? string.Empty, AddValueFactory, UpdateValueFactory);

    public virtual Result Initialize(string? storageKey = null)
        => Result.Success();

    private static TKeyValue UpdateValueFactory(string key, TKeyValue value)
        => value switch {
               uint uintValue => (TKeyValue)(object)(uintValue + 1),
               int intValue => (TKeyValue)(object)(intValue + 1),
               Guid => (TKeyValue)(object)Guid.CreateVersion7(),
               _ => throw new NotSupportedException($"Type {typeof(TKeyValue)} is not supported for key generation.")
           };

    private static TKeyValue AddValueFactory(string key)
        => typeof(TKeyValue) switch {
               { } t when t == typeof(uint) => (TKeyValue)(object)1u,
               { } t when t == typeof(int) => (TKeyValue)(object)1,
               { } t when t == typeof(Guid) => (TKeyValue)(object)Guid.Empty,
               _ => throw new NotSupportedException($"Type {typeof(TKeyValue)} is not supported for key generation.")
           };
}
