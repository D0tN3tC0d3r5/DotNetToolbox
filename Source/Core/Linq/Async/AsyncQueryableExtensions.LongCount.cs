// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<long> LongCountAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => source.LongCountAsync(_ => true, ct);

    public static async ValueTask<long> LongCountAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        IsNotNull(predicate);
        var count = 0L;
        var filteredSource = IsNotNull(source).Where(predicate).AsAsyncQueryable().AsConfigured(ct);
        await foreach (var item in filteredSource) count++;
        return count;
    }
}
