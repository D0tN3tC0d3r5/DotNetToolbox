// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<int> CountAsync<TItem>(this IQueryable<TItem> source, CancellationToken cancellationToken = default)
        => source.CountAsync(_ => true, cancellationToken);

    public static async ValueTask<int> CountAsync<TItem>(this IQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        var count = 0;
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(cancellationToken)) {
            if (predicate(item))
                count++;
        }

        return count;
    }
}
