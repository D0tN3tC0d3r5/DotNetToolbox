// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<TItem> SumAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        where TItem : INumberBase<TItem>
        => source.SumAsync(static x => x, ct);

    public static ValueTask<TResult> SumAsync<TItem, TResult>(this IQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken ct = default)
        where TResult : INumberBase<TResult>
        => GetSumAsync(source, selector, ct);

    private static async ValueTask<TResult> GetSumAsync<TItem, TResult>(IQueryable<TItem> source, Func<TItem, TResult?> selector, CancellationToken ct)
        where TResult : INumberBase<TResult> {
        IsNotNull(selector);
        var result = TResult.Zero;
        var enumerable = IsNotNull(source).AsAsyncEnumerable(ct);
        await foreach (var item in enumerable) {
            ct.ThrowIfCancellationRequested();
            if (selector(item) is not { } value) continue;
            result += value;
        }

        return result;
    }
}
