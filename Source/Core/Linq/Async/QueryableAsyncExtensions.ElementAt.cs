// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static async ValueTask<TItem?> ElementAtAsync<TItem>(this IQueryable<TItem> source, int index, CancellationToken ct = default) {
        IsNotNegative(index);
        var count = 0;
        await foreach (var item in IsNotNull(source).AsAsyncEnumerable(ct)) {
            if (index != count++)
                continue;
            return item;
        }
        throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range. Must be non-negative and less than the size of the collection.");
    }

    public static async ValueTask<TItem?> ElementAtAsync<TItem>(this IQueryable<TItem> source, Index index, CancellationToken ct = default) {
        if (!index.IsFromEnd)
            return await source.ElementAtAsync(index.Value, ct).ConfigureAwait(false);
        var list = await source.ToArrayAsync(ct).ConfigureAwait(false);
        return index.Value >= list.Length
            ? throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range. Must be non-negative and less than the size of the collection.")
            : list[index];
    }
}
