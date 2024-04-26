// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        where TItem : struct, INumberBase<TItem>
        => AverageAsync(source, item => item, cancellationToken);

    public static async ValueTask<double> AverageAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> selector, CancellationToken cancellationToken = default)
        where TResult : struct, INumberBase<TResult> {
        IsNotNull(selector);
        var result = TResult.Zero;
        var count = 0L;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            result += selector(item);
            count++;
        }

        return count == 0
                   ? 0.0
                   : Convert.ToDouble(result) / count;
    }

    public static ValueTask<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem?> source, CancellationToken cancellationToken = default)
        where TItem : struct, INumberBase<TItem>
        => AverageAsync(source, item => item, cancellationToken);

    public static async ValueTask<double?> AverageAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken cancellationToken = default)
        where TResult : struct, INumberBase<TResult> {
        IsNotNull(selector);
        var result = default(TResult?);
        var count = 0L;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var value = selector(item);
            if (!value.HasValue)
                continue;
            result += value.Value;
            count++;
        }

        return count == 0
                   ? 0.0
                   : !result.HasValue
                       ? null
                       : Convert.ToDouble(result) / count;
    }
}
