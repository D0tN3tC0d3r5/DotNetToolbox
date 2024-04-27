// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem?> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> source, int index, CancellationToken cancellationToken = default) {
        IsNotNegative(index);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (index != count++) continue;
            return enumerator.Current;
        }
        throw new ArgumentOutOfRangeException(nameof(index), index, $"Collection contains only {count} elements.");
    }

    public static async ValueTask<TItem?> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> source, Index index, CancellationToken cancellationToken = default) {
        if (!index.IsFromEnd)
            return await source.ElementAtAsync(index.Value, cancellationToken).ConfigureAwait(false);
        var list = await source.ToArrayAsync(cancellationToken).ConfigureAwait(false);
        return index.Value >= list.Length
            ? throw new ArgumentOutOfRangeException(nameof(index), index, $"Collection contains only {list.Length} elements.")
            : list[index];
    }
}
