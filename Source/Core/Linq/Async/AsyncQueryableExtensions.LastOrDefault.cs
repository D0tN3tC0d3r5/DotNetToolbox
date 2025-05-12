// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => FindLastOrDefault(source, static _ => true, default, ct);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindLastOrDefault(source, predicate, default, ct);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem? defaultValue, CancellationToken ct = default)
        => FindLastOrDefault(source, static _ => true, defaultValue, ct);

    public static ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct = default)
        => FindLastOrDefault(source, predicate, defaultValue, ct);

    private static async ValueTask<TItem?> FindLastOrDefault<TItem>(IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct) {
        IsNotNull(predicate);
        var filteredSource = IsNotNull(source).Where(predicate);
        var result = defaultValue;
        await foreach (var item in filteredSource) {
            ct.ThrowIfCancellationRequested();
            result = item;
        }
        return result;
    }
}
