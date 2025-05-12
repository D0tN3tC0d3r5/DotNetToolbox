// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => FindLastOrDefault(source, static _ => true, default, ct);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindLastOrDefault(source, predicate, default, ct);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IQueryable<TItem> source, TItem? defaultValue, CancellationToken ct = default)
        => FindLastOrDefault(source, static _ => true, defaultValue, ct);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct = default)
        => FindLastOrDefault(source, predicate, defaultValue, ct);

    private static async ValueTask<TItem?> FindLastOrDefault<TItem>(IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct) {
        IsNotNull(predicate);
        var result = defaultValue;
        var enumerable = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        await foreach (var item in enumerable) {
            ct.ThrowIfCancellationRequested();
            result = item;
        }
        return result;
    }
}
