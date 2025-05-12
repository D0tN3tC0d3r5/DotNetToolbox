// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => source.CountAsync(static _ => true, ct);

    public static async ValueTask<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        IsNotNull(predicate);
        var filteredSource = IsNotNull(source).Where(predicate);
        var count = 0;
        await foreach (var _ in filteredSource) {
            ct.ThrowIfCancellationRequested();
            count++;
        }
        return count;
    }
}
