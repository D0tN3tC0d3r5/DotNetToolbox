// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<Dictionary<TKey, TItem>> ToDictionaryAsync<TItem, TKey>(this IQueryable<TItem> source, Func<TItem, TKey> keySelector, CancellationToken ct = default)
        where TKey : notnull
        => source.ToDictionaryAsync((x, _) => keySelector(x), (x, _) => x, null!, ct);

    public static ValueTask<Dictionary<TKey, TItem>> ToDictionaryAsync<TItem, TKey>(this IQueryable<TItem> source, Func<TItem, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken ct = default)
        where TKey : notnull
        => source.ToDictionaryAsync((x, _) => keySelector(x), (x, _) => x, comparer, ct);

    public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TItem, TKey, TElement>(this IQueryable<TItem> source, Func<TItem, TKey> keySelector, Func<TItem, TElement> elementSelector, CancellationToken ct = default)
        where TKey : notnull
        => source.ToDictionaryAsync((x, _) => keySelector(x), (x, _) => elementSelector(x), null!, ct);

    public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TItem, TKey, TElement>(this IQueryable<TItem> source, Func<TItem, TKey> keySelector, Func<TItem, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken ct = default)
        where TKey : notnull
        => source.ToDictionaryAsync((x, _) => keySelector(x), (x, _) => elementSelector(x), comparer, ct);

    public static ValueTask<Dictionary<TKey, TItem>> ToDictionaryAsync<TItem, TKey>(this IQueryable<TItem> source, Func<TItem, int, TKey> keySelector, CancellationToken ct = default)
        where TKey : notnull
        => source.ToDictionaryAsync(keySelector, (x, _) => x, null!, ct);

    public static ValueTask<Dictionary<TKey, TItem>> ToDictionaryAsync<TItem, TKey>(this IQueryable<TItem> source, Func<TItem, int, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken ct = default)
        where TKey : notnull
        => source.ToDictionaryAsync(keySelector, (x, _) => x, comparer, ct);

    public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TItem, TKey, TElement>(this IQueryable<TItem> source, Func<TItem, int, TKey> keySelector, Func<TItem, int, TElement> elementSelector, CancellationToken ct = default)
        where TKey : notnull
        => source.ToDictionaryAsync(keySelector, elementSelector, null!, ct);

    public static async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TItem, TKey, TElement>(this IQueryable<TItem> source, Func<TItem, int, TKey> keySelector, Func<TItem, int, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken ct = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        var result = new Dictionary<TKey, TElement>(comparer);
        var index = 0;
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(ct)) {
            var key = keySelector(item, index);
            var value = elementSelector(item, index);
            result.Add(key, value);
            index++;
        }
        return result;
    }
}
