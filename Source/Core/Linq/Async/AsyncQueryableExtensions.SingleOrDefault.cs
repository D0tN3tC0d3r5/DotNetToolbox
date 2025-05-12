// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => FindSingleOrDefault(source, static _ => true, default, ct);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindSingleOrDefault(source, predicate, default, ct);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem? defaultValue, CancellationToken ct = default)
        => FindSingleOrDefault(source, static _ => true, defaultValue, ct);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct = default)
        => FindSingleOrDefault(source, predicate, defaultValue, ct);

    private static async ValueTask<TItem?> FindSingleOrDefault<TItem>(IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct) {
        IsNotNull(predicate);
        var result = defaultValue;
        var found = false;
        await foreach (var item in IsNotNull(source).Where(predicate)) {
            ct.ThrowIfCancellationRequested();
            if (found)
                throw new InvalidOperationException("Collection contains more than one matching element.");
            found = true;
            result = item;
        }
        return result;
    }
}
