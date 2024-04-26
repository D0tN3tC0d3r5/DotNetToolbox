// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<bool> SequenceEqualAsync<TItem>(this IAsyncQueryable<TItem> first, IEnumerable<TItem> second, CancellationToken cancellationToken = default) {
        await using var firstEnumerator = IsNotNull(first).GetAsyncEnumerator(cancellationToken);
        await using var secondEnumerator = IsNotNull(second).GetAsyncEnumerator(cancellationToken);

        while (await firstEnumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (!await secondEnumerator.MoveNextAsync().ConfigureAwait(false))
                // second has fewer elements
                return false;
            if (!Equals(firstEnumerator.Current, secondEnumerator.Current))
                return false;
        }

        // second has more elements
        return !await secondEnumerator.MoveNextAsync();
    }

    public static async ValueTask<bool> SequenceEqualAsync<TItem>(this IAsyncQueryable<TItem> first, IEnumerable<TItem> second, IEqualityComparer<TItem> comparer, CancellationToken cancellationToken = default) {
        await using var firstEnumerator = IsNotNull(first).GetAsyncEnumerator(cancellationToken);
        await using var secondEnumerator = IsNotNull(second).GetAsyncEnumerator(cancellationToken);

        while (await firstEnumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (!await secondEnumerator.MoveNextAsync().ConfigureAwait(false))
                // second has fewer elements
                return false;
            if (!comparer.Equals(firstEnumerator.Current, secondEnumerator.Current))
                return false;
        }

        // second has more elements
        return !await secondEnumerator.MoveNextAsync();
    }
}
