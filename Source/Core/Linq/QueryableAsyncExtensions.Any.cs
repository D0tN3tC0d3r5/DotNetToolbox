// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<bool> AnyAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => source.AnyAsync(static _ => true, ct);

    public static async ValueTask<bool> AnyAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        IsNotNull(predicate);
        var enumerable = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        await foreach (var _ in enumerable) {
            ct.ThrowIfCancellationRequested();
            return true;
        }
        return false;
    }
}
