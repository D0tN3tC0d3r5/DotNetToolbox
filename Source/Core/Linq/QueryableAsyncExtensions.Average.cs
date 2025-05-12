// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<double> AverageAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        where TItem : INumberBase<TItem>
        => source.AverageAsync(static x => x, ct);

    public static ValueTask<double> AverageAsync<TItem, TResult>(this IQueryable<TItem> source,
                                                                 Func<TItem, TResult?> selector,
                                                                 CancellationToken ct = default)
        where TResult : INumberBase<TResult>
        => GetAverageAsync(source, selector, ct);

    private static async ValueTask<double> GetAverageAsync<TItem, TResult>(IQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken ct)
        where TResult : INumberBase<TResult> {
        IsNotNull(selector);
        var result = TResult.Zero;
        var count = 0D;
        var isEmpty = true;
        var enumerable = IsNotNull(source).AsAsyncEnumerable(ct);
        await foreach (var item in enumerable) {
            ct.ThrowIfCancellationRequested();
            isEmpty = false;
            if (selector(item) is not { } value) continue;
            result += value;
            count++;
        }
        return isEmpty
                ? 0D
                : Convert.ToDouble(result) / count;
    }
}
