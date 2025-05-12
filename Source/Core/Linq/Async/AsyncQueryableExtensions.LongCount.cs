// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => source.LongCountAsync(static _ => true, ct);

    public static async ValueTask<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        IsNotNull(predicate);
        var count = 0L;
        await foreach (var _ in IsNotNull(source).Where(predicate)) {
            ct.ThrowIfCancellationRequested();
            count++;
        }
        return count;
    }
}
