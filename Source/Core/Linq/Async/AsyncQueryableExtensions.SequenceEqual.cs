// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<bool> SequenceEqualAsync<TItem>(this IQueryable<TItem> first, IEnumerable<TItem> second, CancellationToken ct = default)
        => first.SequenceEqualAsync(second, EqualityComparer<TItem>.Default, ct);

    public static async ValueTask<bool> SequenceEqualAsync<TItem>(this IQueryable<TItem> first, IEnumerable<TItem> second, IEqualityComparer<TItem> comparer, CancellationToken ct = default) {
        await using var firstEnumerator = IsNotNull(first).GetAsyncEnumerator(ct);
        await using var secondEnumerator = IsNotNull(second).GetAsyncEnumerator(ct);

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
