// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> FirstAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => FindFirst(source, _ => true, ct);

    public static ValueTask<TItem> FirstAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindFirst(source, predicate, ct);

    private static async ValueTask<TItem> FindFirst<TItem>(IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct) {
        IsNotNull(predicate);
        var filteredSource = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        await foreach (var item in filteredSource)
            return item;
        throw new InvalidOperationException("Collection contains no matching element.");
    }
}
