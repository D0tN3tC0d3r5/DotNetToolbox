// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem> SumAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        where TItem : struct, INumberBase<TItem> {
        var result = TItem.Zero;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            result += item;
        }
        return result;
    }

    public static async ValueTask<TResult> SumAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> selector, CancellationToken cancellationToken = default)
        where TResult : struct, INumberBase<TResult> {
        IsNotNull(selector);
        var result = TResult.Zero;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            result += selector(item);
        }
        return result;
    }

    public static async ValueTask<TItem?> SumAsync<TItem>(this IAsyncQueryable<TItem?> source, CancellationToken cancellationToken = default)
        where TItem : struct, INumberBase<TItem> {
        var result = default(TItem?);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            if (item.HasValue) result += item.Value;
        }
        return result;
    }

    public static async ValueTask<TResult?> SumAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken cancellationToken = default)
        where TResult : struct, INumberBase<TResult> {
        IsNotNull(selector);
        var result = default(TResult?);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var value = selector(item);
            if (value.HasValue) result += value.Value;
        }
        return result;
    }
}
