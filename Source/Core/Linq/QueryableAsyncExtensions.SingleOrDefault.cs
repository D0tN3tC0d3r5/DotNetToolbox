// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => FindSingleOrDefault(source, static _ => true, default, ct);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindSingleOrDefault(source, predicate, default, ct);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, TItem? defaultValue, CancellationToken ct = default)
        => FindSingleOrDefault(source, static _ => true, defaultValue, ct);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct = default)
        => FindSingleOrDefault(source, predicate, defaultValue, ct);

    private static async ValueTask<TItem?> FindSingleOrDefault<TItem>(IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct) {
        IsNotNull(predicate);
        var result = defaultValue;
        var found = false;
        var enumerable = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        await foreach (var item in enumerable) {
            ct.ThrowIfCancellationRequested();
            if (found)
                throw new InvalidOperationException("Collection contains more than one matching element.");
            found = true;
            result = item;
        }
        return result;
    }
}
