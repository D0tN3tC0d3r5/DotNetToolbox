// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> FirstAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => FindFirst(source, static _ => true, ct);

    public static ValueTask<TItem> FirstAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindFirst(source, predicate, ct);

    private static async ValueTask<TItem> FindFirst<TItem>(IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct) {
        IsNotNull(predicate);
        await foreach (var item in IsNotNull(source).Where(predicate)) {
            ct.ThrowIfCancellationRequested();
            return item;
        }
        throw new InvalidOperationException("Collection contains no matching element.");
    }
}
