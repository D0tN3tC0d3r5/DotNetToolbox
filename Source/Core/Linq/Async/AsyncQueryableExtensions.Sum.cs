// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> SumAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        where TItem : struct, INumberBase<TItem>
        => source.SumAsync(x => x, ct);

    public static async ValueTask<TResult> SumAsync<TItem, TResult>(this IQueryable<TItem> source, Func<TItem, TResult> selector, CancellationToken ct = default)
        where TResult : struct, INumberBase<TResult>
        => await GetSum(source, x => (TResult?)selector(x!), ct)
        ?? TResult.Zero;

    public static ValueTask<TItem?> SumAsync<TItem>(this IAsyncQueryable<TItem?> source, CancellationToken ct = default)
        where TItem : struct, INumberBase<TItem>
        => source.SumAsync(x => x, ct);

    public static ValueTask<TResult?> SumAsync<TItem, TResult>(this IQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken ct = default)
        where TResult : struct, INumberBase<TResult>
        => GetSum(source, selector!, ct);

    private static async ValueTask<TResult?> GetSum<TItem, TResult>(IQueryable<TItem> source, Func<TItem?, TResult?> selector, CancellationToken ct)
        where TResult : struct, INumberBase<TResult> {
        IsNotNull(selector);
        var result = TResult.Zero;
        await foreach (var item in IsNotNull(source).AsAsyncEnumerable(ct)) {
            var value = selector(item);
            if (!value.HasValue)
                continue;
            result += value.Value;
        }
        return result;
    }
}
