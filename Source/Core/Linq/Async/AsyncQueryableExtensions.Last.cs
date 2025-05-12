// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => FindLast(source, static _ => true, ct);

    public static ValueTask<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindLast(source, predicate, ct);

    private static async ValueTask<TItem> FindLast<TItem>(IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct) {
        IsNotNull(predicate);
        var result = default(TItem);
        var found = false;
        await foreach (var item in IsNotNull(source).Where(predicate)) {
            ct.ThrowIfCancellationRequested();
            result = item;
            found = true;
        }
        return found
                   ? result!
                   : throw new InvalidOperationException("Collection contains no matching element.");
    }
}
