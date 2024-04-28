// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<TItem> MaxByAsync<TItem, TKey>(
            this IAsyncQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            CancellationToken cancellationToken = default)
        => source.MaxByAsync(keySelector, Comparer<TKey>.Default, x => x, cancellationToken);

    public static ValueTask<TItem> MaxByAsync<TItem, TKey>(
            this IAsyncQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            IComparer<TKey> keyComparer,
            CancellationToken cancellationToken = default)
        => source.MaxByAsync(keySelector, keyComparer, x => x, cancellationToken);

    public static ValueTask<TResult> MaxByAsync<TItem, TKey, TResult>(
            this IAsyncQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            Func<TItem, TResult> valueSelector,
            CancellationToken cancellationToken = default)
        => source.MaxByAsync(keySelector, Comparer<TKey>.Default, valueSelector, cancellationToken);

    public static ValueTask<TResult> MaxByAsync<TItem, TKey, TResult>(
            this IAsyncQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<TItem, TResult> valueSelector,
            CancellationToken cancellationToken = default)
        => GetMaxBy(source, keySelector, keyComparer, valueSelector, cancellationToken);

    private static async ValueTask<TResult> GetMaxBy<TItem, TKey, TResult>(
            IAsyncQueryable<TItem> source,
            Func<TItem, TKey> keySelector,
            IComparer<TKey> keyComparer,
            Func<TItem, TResult> valueSelector,
            CancellationToken cancellationToken) {

        IsNotNull(keySelector);
        IsNotNull(valueSelector);
        object? key = null;
        var result = default(TResult);
        var isEmpty = true;
        await foreach (var item in source.AsConfigured(cancellationToken)) {
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
