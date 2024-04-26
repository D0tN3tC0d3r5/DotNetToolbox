// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var found = false;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (found) throw new InvalidOperationException("Sequence contains more than one element.");
            found = true;
        }

        return found
            ? enumerator.Current
            : throw new InvalidOperationException("Collection contains no elements.");
    }

    public static async ValueTask<TItem> SingleAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var result = default(TItem);
        var found = false;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (found) throw new InvalidOperationException("Collection contains more than one element that satisfies the given predicate.");
            if (!predicate(enumerator.Current)) continue;
            found = true;
            result = enumerator.Current;
        }

        return found
            ? result!
            : throw new InvalidOperationException("Collection does not contain any element that satisfy the given predicate.");
    }
}
