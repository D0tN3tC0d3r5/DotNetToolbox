// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<bool> ContainsAsync<TItem>(this IQueryable<TItem> source, TItem searchedItem, CancellationToken ct = default)
        => source.ContainsAsync(searchedItem, EqualityComparer<TItem>.Default, ct);

    public static async ValueTask<bool> ContainsAsync<TItem>(this IQueryable<TItem> source, TItem searchedItem, IEqualityComparer<TItem> comparer, CancellationToken ct = default) {
        IsNotNull(comparer);
        await foreach (var item in IsNotNull(source).AsAsyncEnumerable(ct)) {
            if (!comparer.Equals(searchedItem, item))
                continue;
            return true;
        }
        return false;
    }
}
