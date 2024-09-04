// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<int> CountAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => source.CountAsync(_ => true, ct);

    public static async ValueTask<int> CountAsync<TItem>(this IQueryable<TItem> source, Expression<Func<TItem, bool>> predicate, CancellationToken ct = default) {
        IsNotNull(predicate);
        var filteredSource = IsNotNull(source).Where(predicate).AsAsyncEnumerable(ct);
        var count = 0;
        await foreach (var item in filteredSource)
            count++;
        return count;
    }
}
