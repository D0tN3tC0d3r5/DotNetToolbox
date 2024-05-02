// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> MinByAsync<TItem, TKey>(
            this IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            CancellationToken cancellationToken = default)
        => source.MinByAsync(keySelector, Comparer<TKey>.Default, x => x, cancellationToken);

    public static ValueTask<TItem> MinByAsync<TItem, TKey>(
            this IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            IComparer<TKey> keyComparer,
            CancellationToken cancellationToken = default)
        => source.MinByAsync(keySelector, keyComparer, x => x, cancellationToken);

    public static ValueTask<TResult> MinByAsync<TItem, TKey, TResult>(
            this IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            Func<TItem, TResult> valueSelector,
            CancellationToken cancellationToken = default)
        => source.MinByAsync(keySelector, Comparer<TKey>.Default, valueSelector, cancellationToken);

    public static ValueTask<TResult> MinByAsync<TItem, TKey, TResult>(
            this IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<TItem, TResult> valueSelector,
            CancellationToken cancellationToken = default)
        => GetMinBy(source, keySelector, keyComparer, valueSelector, cancellationToken);

    private static async ValueTask<TResult> GetMinBy<TItem, TKey, TResult>(
            IQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<TItem, TResult> valueSelector,
            CancellationToken cancellationToken) {
        IsNotNull(keySelector);
        IsNotNull(valueSelector);
        object? key = null;
        var result = default(TResult);
        var isEmpty = true;
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(cancellationToken)) {
            isEmpty = false;
            var itemKey = keySelector(item);
            if (key is not null && keyComparer.Compare((TKey)key, itemKey) <= 1)
                continue;
            key = itemKey;
            result = valueSelector(item);
        }
        return isEmpty
            ? throw new InvalidOperationException("Collection contains no elements.")
            : result!;
    }
}
