// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => FindSingle(source, static _ => true, ct);

    public static ValueTask<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindSingle(source, predicate, ct);

    private static async ValueTask<TItem> FindSingle<TItem>(IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct) {
        IsNotNull(predicate);
        var result = default(TItem);
        var found = false;
        await foreach (var item in IsNotNull(source).Where(predicate)) {
            ct.ThrowIfCancellationRequested();
            if (found)
                throw new InvalidOperationException("Collection contains more than one matching element.");
            found = true;
            result = item;
        }
        return found
                   ? result!
                   : throw new InvalidOperationException("Collection contains no matching element.");
    }
}
