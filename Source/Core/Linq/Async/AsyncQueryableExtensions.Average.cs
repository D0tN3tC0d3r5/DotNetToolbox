// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<double> AverageAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => source.AverageAsync(static x => x, ct);

    public static ValueTask<double> AverageAsync<TItem, TResult>(this IAsyncQueryable<TItem> source,
                                                                 Func<TItem?, TResult?> selector,
                                                                 CancellationToken ct = default)
        => GetAverageAsync(source, selector, ct);

    private static async ValueTask<double> GetAverageAsync<TItem, TResult>(IAsyncQueryable<TItem> source, Func<TItem?, TResult?> selector, CancellationToken ct) {
        IsNotNull(selector);
        var sum = 0D;
        var count = 0D;
        var isEmpty = true;
        await foreach (var item in IsNotNull(source)) {
            ct.ThrowIfCancellationRequested();
            if (selector(item) is not { } value) continue;
            isEmpty = false;
            sum += Convert.ToDouble(value);
            count++;
        }
        return isEmpty ? 0D : sum / count;
    }
}
