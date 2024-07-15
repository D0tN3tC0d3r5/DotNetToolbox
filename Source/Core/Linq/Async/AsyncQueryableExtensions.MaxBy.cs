// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> MaxByAsync<TItem, TKey>(
            this IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            CancellationToken ct = default)
        => source.MaxByAsync(keySelector, Comparer<TKey>.Default, x => x, ct);

    public static ValueTask<TItem> MaxByAsync<TItem, TKey>(
            this IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            IComparer<TKey> keyComparer,
            CancellationToken ct = default)
        => source.MaxByAsync(keySelector, keyComparer, x => x, ct);

    public static ValueTask<TResult> MaxByAsync<TItem, TKey, TResult>(
            this IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            Func<TItem, TResult> valueSelector,
            CancellationToken ct = default)
        => source.MaxByAsync(keySelector, Comparer<TKey>.Default, valueSelector, ct);

    public static ValueTask<TResult> MaxByAsync<TItem, TKey, TResult>(
            this IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<TItem, TResult> valueSelector,
            CancellationToken ct = default)
        => GetMaxBy(source, keySelector, keyComparer, valueSelector, ct);

    private static async ValueTask<TResult> GetMaxBy<TItem, TKey, TResult>(
            IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<TItem, TResult> valueSelector,
            CancellationToken ct) {
        IsNotNull(keySelector);
        IsNotNull(valueSelector);
        object? key = null;
        var result = default(TResult);
        var isEmpty = true;
        await foreach (var item in IsNotNull(source).AsAsyncEnumerable(ct)) {
            isEmpty = false;
            var itemKey = keySelector(item);
            if (key is not null && keyComparer.Compare((TKey)key, itemKey) >= 1)
                continue;
            key = itemKey;
            result = valueSelector(item);
        }
        return isEmpty
            ? throw new InvalidOperationException("Collection contains no elements.")
            : result!;
    }
}
