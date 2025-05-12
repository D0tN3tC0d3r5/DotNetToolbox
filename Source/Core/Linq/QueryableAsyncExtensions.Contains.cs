// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<bool> ContainsAsync<TItem>(this IQueryable<TItem> source, TItem searchedItem, CancellationToken ct = default)
        => source.ContainsAsync(searchedItem, EqualityComparer<TItem>.Default, ct);

    public static async ValueTask<bool> ContainsAsync<TItem>(this IQueryable<TItem> source, TItem searchedItem, IEqualityComparer<TItem> comparer, CancellationToken ct = default) {
        IsNotNull(comparer);
        var enumerable = IsNotNull(source).AsAsyncEnumerable(ct);
        await foreach (var item in enumerable) {
            ct.ThrowIfCancellationRequested();
            if (!comparer.Equals(searchedItem, item))
                continue;
            return true;
        }
        return false;
    }
}
