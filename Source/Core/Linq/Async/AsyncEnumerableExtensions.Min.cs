// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem> MinAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        where TItem : struct, IMinMaxValue<TItem>, INumber<TItem> {
        var result = TItem.MaxValue;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            result = TItem.Min(result, item);
        }
        return result;
    }

    public static async ValueTask<TResult> MinAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> selector, CancellationToken cancellationToken = default)
        where TResult : struct, IMinMaxValue<TResult>, INumber<TResult> {
        IsNotNull(selector);
        var result = TResult.MaxValue;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            result = TResult.Min(result, selector(item));
        }
        return result;
    }

    public static async ValueTask<TItem?> MinAsync<TItem>(this IAsyncQueryable<TItem?> source, CancellationToken cancellationToken = default)
        where TItem : struct, IMinMaxValue<TItem>, INumber<TItem> {
        var result = default(TItem?);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            if (!item.HasValue)
                continue;
            result = !result.HasValue
                         ? item
                         : TItem.Min(result.Value, item.Value);
        }
        return result;
    }

    public static async ValueTask<TResult?> MinAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken cancellationToken = default)
        where TResult : struct, IMinMaxValue<TResult>, INumber<TResult> {
        IsNotNull(selector);
        var result = default(TResult?);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var value = selector(item);
            if (!value.HasValue)
                continue;
            result = !result.HasValue
                         ? value
                         : TResult.Min(result.Value, value.Value);
        }
        return result;
    }
}
