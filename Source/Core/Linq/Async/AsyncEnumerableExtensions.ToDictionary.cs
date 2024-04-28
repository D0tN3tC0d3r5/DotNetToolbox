// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default)
        where TKey : notnull
        => source.ToDictionaryAsync((x, _) => keySelector(x), (x, _) => x, null!, cancellationToken);

    public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        where TKey : notnull
        => source.ToDictionaryAsync((x, _) => keySelector(x), (x, _) => x, comparer, cancellationToken);

    public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default)
        where TKey : notnull
        => source.ToDictionaryAsync((x, _) => keySelector(x), (x, _) => elementSelector(x), null!, cancellationToken);

    public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        where TKey : notnull
        => source.ToDictionaryAsync((x, _) => keySelector(x), (x, _) => elementSelector(x), comparer, cancellationToken);

    public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Func<TSource, int, TKey> keySelector, CancellationToken cancellationToken = default)
        where TKey : notnull
        => source.ToDictionaryAsync(keySelector, (x, _) => x, null!, cancellationToken);

    public static ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncQueryable<TSource> source, Func<TSource, int, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        where TKey : notnull
        => source.ToDictionaryAsync(keySelector, (x, _) => x, comparer, cancellationToken);

    public static ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Func<TSource, int, TKey> keySelector, Func<TSource, int, TElement> elementSelector, CancellationToken cancellationToken = default)
        where TKey : notnull
        => source.ToDictionaryAsync(keySelector, elementSelector, null!, cancellationToken);

    public static async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncQueryable<TSource> source, Func<TSource, int, TKey> keySelector, Func<TSource, int, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        var result = new Dictionary<TKey, TElement>(comparer);
        var index = 0;
        await foreach (var item in source.AsConfigured(cancellationToken)) {
            var key = keySelector(item, index);
            var value = elementSelector(item, index);
            result.Add(key, value);
            index++;
        }
        return result;
    }
}
