// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => FindLastOrDefault(source, _ => true, default, ct);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindLastOrDefault(source, predicate, default, ct);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IQueryable<TItem> source, TItem? defaultValue, CancellationToken ct = default)
        => FindLastOrDefault(source, _ => true, defaultValue, ct);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct = default)
        => FindLastOrDefault(source, predicate, defaultValue, ct);

    private static async ValueTask<TItem?> FindLastOrDefault<TItem>(IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct) {
        IsNotNull(predicate);
        var filteredSource = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        var result = defaultValue;
        await foreach (var item in filteredSource)
            result = item;
        return result;
    }
}
