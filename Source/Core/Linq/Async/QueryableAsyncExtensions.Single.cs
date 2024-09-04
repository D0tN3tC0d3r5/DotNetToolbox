// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<TItem> SingleAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => FindSingle(source, _ => true, ct);

    public static ValueTask<TItem> SingleAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindSingle(source, predicate, ct);

    private static async ValueTask<TItem> FindSingle<TItem>(IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct) {
        IsNotNull(predicate);
        var result = default(TItem);
        var found = false;
        var filteredSource = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        await foreach (var item in filteredSource) {
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
