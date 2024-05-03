// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<bool> AnyAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => source.AnyAsync(_ => true, ct);

    public static async ValueTask<bool> AnyAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        IsNotNull(predicate);
        var filteredSource = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        await foreach (var item in filteredSource)
            return true;
        return false;
    }
}
