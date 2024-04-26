// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem?> MaxByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Func<TItem, TKey> keySelector, CancellationToken cancellationToken = default)
        where TKey : struct, IMinMaxValue<TKey>, INumber<TKey> {
        IsNotNull(keySelector);
        var key = TKey.MinValue;
        var result = default(TItem);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var itemKey = keySelector(item);
            if (itemKey <= key)
                continue;
            key = itemKey;
            result = item;
        }
        return result;
    }

    public static async ValueTask<TItem?> MaxByAsync<TItem, TKey>(this IAsyncQueryable<TItem> source, Func<TItem, TKey> keySelector, IComparer<TKey> comparer, CancellationToken cancellationToken = default)
        where TKey : struct, IMinMaxValue<TKey>, INumber<TKey> {
        IsNotNull(keySelector);
        var key = TKey.MinValue;
        var result = default(TItem);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var itemKey = keySelector(item);
            if (comparer.Compare(itemKey, key) < 1)
                continue;
            key = itemKey;
            result = item;
        }
        return result;
    }

    public static async ValueTask<TResult?> MaxByAsync<TItem, TKey, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TKey> keySelector, Func<TItem, TResult?> valueSelector, CancellationToken cancellationToken = default)
        where TKey : struct, IMinMaxValue<TKey>, INumber<TKey> {
        IsNotNull(keySelector);
        IsNotNull(valueSelector);
        var key = TKey.MinValue;
        var result = default(TResult);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var itemKey = keySelector(item);
            if (itemKey <= key)
                continue;
            key = itemKey;
            result = valueSelector(item);
        }
        return result;
    }

    public static async ValueTask<TResult?> MaxByAsync<TItem, TKey, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TKey> keySelector, IComparer<TKey> comparer, Func<TItem, TResult?> valueSelector, CancellationToken cancellationToken = default)
        where TKey : struct, IMinMaxValue<TKey>, INumber<TKey> {
        IsNotNull(keySelector);
        IsNotNull(valueSelector);
        var key = TKey.MinValue;
        var result = default(TResult);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var itemKey = keySelector(item);
            if (comparer.Compare(itemKey, key) < 1)
                continue;
            key = itemKey;
            result = valueSelector(item);
        }
        return result;
    }
}
