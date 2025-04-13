namespace WebApi.Services;

internal sealed class CacheService(IDistributedCache cache, ILogger<CacheService> logger) : ICacheService {
    private readonly JsonSerializerOptions _serializerOptions = new();

    public async Task<T?> GetAsync<T>(string key, CancellationToken ct = default) {
        try {
            var bytes = await cache.GetAsync(key, ct);
            return bytes is null || bytes.Length == 0
                       ? default
                       : JsonSerializer.Deserialize<T>(bytes, _serializerOptions);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to get or deserialize cache entry for key '{CacheKey}'.", key);
            return default; // Or handle/rethrow as appropriate
        }
    }

    public Task<string?> GetStringAsync(string key, CancellationToken ct = default) => cache.GetStringAsync(key, ct);

    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken ct = default) {
        try {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(value, _serializerOptions);
            await cache.SetAsync(key, bytes, options ?? new DistributedCacheEntryOptions(), ct);
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to serialize or set cache entry for key '{CacheKey}'.", key);
        }
    }

    public Task SetStringAsync(string key, string value, DistributedCacheEntryOptions? options = null, CancellationToken ct = default) => cache.SetStringAsync(key, value, options ?? new DistributedCacheEntryOptions(), ct);

    public Task RemoveAsync(string key, CancellationToken ct = default) => cache.RemoveAsync(key, ct);

    public Task RefreshAsync(string key, CancellationToken ct = default) => cache.RefreshAsync(key, ct);
}