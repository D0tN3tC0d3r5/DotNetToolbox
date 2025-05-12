namespace DotNetToolbox.Data.KeyGenerators;

public class InMemoryAsyncKeyGenerator<TKeyValue>
    : IAsyncKeyGenerator<TKeyValue> {
    public static InMemoryAsyncKeyGenerator<TKeyValue> Instance { get; } = new();

    private readonly ConcurrentDictionary<string, TKeyValue> _keyStorage = new();

    private InMemoryAsyncKeyGenerator() { }

    public virtual Task<TKeyValue> GenerateNextKeyAsync(string? storageKey = null, CancellationToken ct = default) {
        var key = _keyStorage.AddOrUpdate(storageKey ?? string.Empty, AddValueFactory, UpdateValueFactory);
        return Task.FromResult(key);
    }

    public virtual Task<Result> InitializeAsync(string? storageKey = null, CancellationToken ct = default)
        => Task.FromResult(Result.Success());

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
