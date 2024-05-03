// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<double> AverageAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        where TItem : struct, INumberBase<TItem>
        => source.AverageAsync(x => x, ct);

    public static async ValueTask<double> AverageAsync<TItem, TResult>(this IQueryable<TItem> source, Func<TItem, TResult> selector, CancellationToken ct = default)
        where TResult : struct, INumberBase<TResult>
        => await GetAverage(source, x => (TResult?)selector(x), ct) ?? 0.0;

    public static ValueTask<double?> AverageAsync<TItem>(this IAsyncQueryable<TItem?> source, CancellationToken ct = default)
        where TItem : struct, INumberBase<TItem>
        => source.AverageAsync(x => x, ct);

    public static ValueTask<double?> AverageAsync<TItem, TResult>(this IQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken ct = default)
        where TResult : struct, INumberBase<TResult>
        => GetAverage(source, selector, ct);

    private static async ValueTask<double?> GetAverage<TItem, TResult>(IQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken ct)
        where TResult : struct, INumberBase<TResult> {
        IsNotNull(selector);
        var result = default(TResult?);
        var count = 0D;
        var isEmpty = true;
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(ct)) {
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
