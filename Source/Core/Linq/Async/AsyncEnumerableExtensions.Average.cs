// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        where TItem : struct, INumberBase<TItem>
        => source.AverageAsync(x => x, cancellationToken);

    public static async ValueTask<double> AverageAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> selector, CancellationToken cancellationToken = default)
        where TResult : struct, INumberBase<TResult>
        => await GetAverage(source, x => (TResult?)selector(x), cancellationToken) ?? 0.0;

    public static ValueTask<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem?> source, CancellationToken cancellationToken = default)
        where TItem : struct, INumberBase<TItem>
        => source.AverageAsync(x => x, cancellationToken);

    public static ValueTask<double?> AverageAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken cancellationToken = default)
        where TResult : struct, INumberBase<TResult>
        => GetAverage(source, selector, cancellationToken);

    private static async ValueTask<double?> GetAverage<TItem, TResult>(IAsyncQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken cancellationToken)
        where TResult : struct, INumberBase<TResult> {
        IsNotNull(selector);
        var result = default(TResult?);
        var count = 0D;
        var isEmpty = true;
        await foreach (var item in source.AsConfigured(cancellationToken)) {
            isEmpty = false;
            var value = selector(item);
            if (!value.HasValue)
                continue;
            result ??= TResult.Zero;
            result += value.Value;
            count++;
        }
        return isEmpty
                ? throw new InvalidOperationException("Collection contains no elements.")
                : !result.HasValue
                    ? null
                    : Convert.ToDouble(result) / count;
    }
}
