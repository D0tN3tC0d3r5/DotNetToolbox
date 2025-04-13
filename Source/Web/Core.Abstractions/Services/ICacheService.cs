using Microsoft.Extensions.Caching.Distributed;

namespace WebApi.Services;

/// <summary>
/// Provides an abstraction for interacting with a distributed cache.
/// </summary>
public interface ICacheService {
    /// <summary>
    /// Gets a value from the cache asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the value to get.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="ct">Optional cancellation token.</param>
    /// <returns>The cached value, or default(T) if not found or deserialization fails.</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken ct = default);

    /// <summary>
    /// Gets a string value from the cache asynchronously.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="ct">Optional cancellation token.</param>
    /// <returns>The cached string, or null if not found.</returns>
    Task<string?> GetStringAsync(string key, CancellationToken ct = default);

    /// <summary>
    /// Sets a value in the cache asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the value to set.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="options">Optional cache entry options.</param>
    /// <param name="ct">Optional cancellation token.</param>
    Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Sets a string value in the cache asynchronously.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The string value to cache.</param>
    /// <param name="options">Optional cache entry options.</param>
    /// <param name="ct">Optional cancellation token.</param>
    Task SetStringAsync(string key, string value, DistributedCacheEntryOptions? options = null, CancellationToken ct = default);

    /// <summary>
    /// Removes a value from the cache asynchronously.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="ct">Optional cancellation token.</param>
    Task RemoveAsync(string key, CancellationToken ct = default);

    /// <summary>
    /// Refreshes a value in the cache asynchronously, resetting its sliding expiration timeout.
    /// </summary>
    /// <param name="key">The cache key.</param>
    /// <param name="ct">Optional cancellation token.</param>
    Task RefreshAsync(string key, CancellationToken ct = default);
}
