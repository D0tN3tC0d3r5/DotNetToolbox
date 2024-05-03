// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> LastAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => FindLast(source, _ => true, ct);

    public static ValueTask<TItem> LastAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindLast(source, predicate, ct);

    private static async ValueTask<TItem> FindLast<TItem>(IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct) {
        IsNotNull(predicate);
        var filteredSource = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        var result = default(TItem);
        var found = false;
        await foreach (var item in filteredSource) {
            result = item;
            found = true;
        }
        return found
                   ? result!
                   : throw new InvalidOperationException("Collection contains no matching element.");
    }
}
