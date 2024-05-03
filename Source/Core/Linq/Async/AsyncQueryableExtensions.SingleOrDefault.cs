// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => FindSingleOrDefault(source, _ => true, default, ct);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindSingleOrDefault(source, predicate, default, ct);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, TItem? defaultValue, CancellationToken ct = default)
        => FindSingleOrDefault(source, _ => true, defaultValue, ct);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct = default)
        => FindSingleOrDefault(source, predicate, defaultValue, ct);

    private static async ValueTask<TItem?> FindSingleOrDefault<TItem>(IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct) {
        IsNotNull(predicate);
        var result = defaultValue;
        var found = false;
        var filteredSource = IsNotNull(source).Where(predicate).AsAsyncQueryable().AsConfigured(ct);
        await foreach (var item in filteredSource) {
            if (found)
                throw new InvalidOperationException("Collection contains more than one matching element.");
            found = true;
            result = item;
        }
        return result;
    }
}
