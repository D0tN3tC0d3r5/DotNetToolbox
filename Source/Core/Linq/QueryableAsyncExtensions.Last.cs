// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<TItem> LastAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => FindLast(source, static _ => true, ct);

    public static ValueTask<TItem> LastAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindLast(source, predicate, ct);

    private static async ValueTask<TItem> FindLast<TItem>(IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct) {
        IsNotNull(predicate);
        var result = default(TItem);
        var found = false;
        var enumerable = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        await foreach (var item in enumerable) {
            ct.ThrowIfCancellationRequested();
            result = item;
            found = true;
        }
        return found
                   ? result!
                   : throw new InvalidOperationException("Collection contains no matching element.");
    }
}
