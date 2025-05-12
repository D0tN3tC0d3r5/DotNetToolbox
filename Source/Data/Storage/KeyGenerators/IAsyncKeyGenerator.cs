namespace DotNetToolbox.Data.KeyGenerators;

public interface IAsyncKeyGenerator<TKey> {
    Task<TKey> GenerateNextKeyAsync(string? storageKey = null, CancellationToken ct = default);
    Task<Result> InitializeAsync(string? storageKey = null, CancellationToken ct = default);
}
