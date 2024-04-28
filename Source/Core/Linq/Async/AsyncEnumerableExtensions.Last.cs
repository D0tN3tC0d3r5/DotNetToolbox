// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        => FindLast(source, _ => true, cancellationToken);

    public static ValueTask<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default)
        => FindLast(source, predicate, cancellationToken);

    private static async ValueTask<TItem> FindLast<TItem>(IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken) {
        IsNotNull(predicate);
        var result = default(TItem);
        var found  = false;
        await foreach(var item in source.AsConfigured(cancellationToken)) {
            if (!predicate(item)) continue;
            found  = true;
            result = item;
        }
        return found
                   ? result!
                   : throw new InvalidOperationException("Collection contains no matching element.");
    }
}
