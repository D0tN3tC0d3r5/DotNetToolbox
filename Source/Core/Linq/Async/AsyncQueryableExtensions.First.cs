// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> FirstAsync<TItem>(this IQueryable<TItem> source, CancellationToken cancellationToken = default)
        => FindFirst(source, _ => true, cancellationToken);

    public static ValueTask<TItem> FirstAsync<TItem>(this IQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default)
        => FindFirst(source, predicate, cancellationToken);

    private static async ValueTask<TItem> FindFirst<TItem>(IQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken) {
        IsNotNull(predicate);
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(cancellationToken)) {
            if (!predicate(item))
                continue;
            return item;
        }
        throw new InvalidOperationException("Collection contains no matching element.");
    }
}
