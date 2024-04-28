// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem?> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> source, int index, CancellationToken cancellationToken = default) {
        IsNotNegative(index);
        var count = 0;
        await foreach (var item in source.AsConfigured(cancellationToken)) {
            if (index != count++) continue;
            return item;
        }
        throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range. Must be non-negative and less than the size of the collection.");
    }

    public static async ValueTask<TItem?> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> source, Index index, CancellationToken cancellationToken = default) {
        if (!index.IsFromEnd)
            return await source.ElementAtAsync(index.Value, cancellationToken).ConfigureAwait(false);
        var list = await source.ToArrayAsync(cancellationToken).ConfigureAwait(false);
        return index.Value >= list.Length
            ? throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range. Must be non-negative and less than the size of the collection.")
            : list[index];
    }
}
