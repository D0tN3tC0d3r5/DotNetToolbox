// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<bool> ContainsAsync<TItem>(this IQueryable<TItem> source, TItem searchedItem, CancellationToken cancellationToken = default)
        => source.ContainsAsync(searchedItem, EqualityComparer<TItem>.Default, cancellationToken);

    public static async ValueTask<bool> ContainsAsync<TItem>(this IQueryable<TItem> source, TItem searchedItem, IEqualityComparer<TItem> comparer, CancellationToken cancellationToken = default) {
        IsNotNull(comparer);
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(cancellationToken)) {
            if (!comparer.Equals(searchedItem, item))
                continue;
            return true;
        }
        return false;
    }
}
