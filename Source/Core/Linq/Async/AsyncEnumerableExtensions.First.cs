namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem> FirstAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        return await enumerator.MoveNextAsync().ConfigureAwait(false)
                   ? enumerator.Current
                   : throw new InvalidOperationException("Collection contains no elements.");
    }

    public static async ValueTask<TItem> FirstAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            if (!predicate(item)) continue;
            return item;
        }
        throw new InvalidOperationException("Collection does not contain any element that satisfy the given predicate.");
    }
}
