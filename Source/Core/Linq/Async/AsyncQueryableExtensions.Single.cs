// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem> SingleAsync<TItem>(this IQueryable<TItem> source, CancellationToken cancellationToken = default)
        => FindSingle(source, _ => true, cancellationToken);

    public static ValueTask<TItem> SingleAsync<TItem>(this IQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default)
        => FindSingle(source, predicate, cancellationToken);

    private static async ValueTask<TItem> FindSingle<TItem>(IQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken) {
        IsNotNull(predicate);
        var result = default(TItem);
        var found = false;
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(cancellationToken)) {
            if (!predicate(item))
                continue;
            if (found)
                throw new InvalidOperationException("Collection contains more than one matching element.");
            found = true;
            result = item;
        }
        return found
                   ? result!
                   : throw new InvalidOperationException("Collection contains no matching element.");
    }
}
