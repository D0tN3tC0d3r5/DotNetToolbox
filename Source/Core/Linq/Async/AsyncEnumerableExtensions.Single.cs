// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        => FindSingle(source, _ => true, cancellationToken);

    public static ValueTask<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default)
        => FindSingle(source, predicate, cancellationToken);

    private static async ValueTask<TItem> FindSingle<TItem>(IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken) {
        IsNotNull(predicate);
        var result = default(TItem);
        var found  = false;
        await foreach(var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            if (!predicate(item)) continue;
            if (found) throw new InvalidOperationException("Collection contains more than one element satisfying the given criteria.");
            found = true;
            result = item;
        }
        return found
                   ? result!
                   : throw new InvalidOperationException("Collection does not contain any element satisfying the given criteria.");
    }
}
