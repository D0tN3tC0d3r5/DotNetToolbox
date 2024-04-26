// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) { }
        return enumerator.Current
            ?? throw new InvalidOperationException("Collection contains no elements.");
    }

    public static async ValueTask<TItem> LastAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var result = default(TItem);
        var found = false;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (!predicate(enumerator.Current)) continue;
            found = true;
            result = enumerator.Current;
        }
        return found
            ? result!
            : throw new InvalidOperationException("Collection does not contain any element that satisfy the given predicate.");
        ;
    }
}
