// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => FindFirstOrDefault(source, static _ => true, default, ct);

    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default)
        => FindFirstOrDefault(source, predicate, default, ct);

    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem? defaultValue, CancellationToken ct = default)
        => FindFirstOrDefault(source, static _ => true, defaultValue, ct);

    public static ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct = default)
        => FindFirstOrDefault(source, predicate, defaultValue, ct);

    private static async ValueTask<TItem?> FindFirstOrDefault<TItem>(IAsyncQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, TItem? defaultValue, CancellationToken ct) {
        IsNotNull(predicate);
        await using var filteredSource = IsNotNull(source).Where(predicate).GetAsyncEnumerator(ct);
        return await filteredSource.MoveNextAsync().ConfigureAwait(false)
                   ? filteredSource.Current
                   : defaultValue;
    }
}
