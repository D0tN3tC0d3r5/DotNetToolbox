// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => source.AnyAsync(static _ => true, ct);

    public static async ValueTask<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        IsNotNull(predicate);
        await foreach (var _ in IsNotNull(source).Where(predicate)) {
            ct.ThrowIfCancellationRequested();
            return true;
        }
        return false;
    }
}
