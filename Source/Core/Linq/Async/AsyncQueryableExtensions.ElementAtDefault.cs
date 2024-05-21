// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IQueryable<TItem> source, int index, CancellationToken ct = default)
        => source.ElementAtOrDefaultAsync(index, default, ct);

    public static ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IQueryable<TItem> source, Index index, CancellationToken ct = default)
        => source.ElementAtOrDefaultAsync(index, default, ct);

    public static async ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IQueryable<TItem> source, int index, TItem? defaultValue, CancellationToken ct = default) {
        IsNotNegative(index);
        var count = 0;
        await foreach (var item in IsNotNull(source).AsAsyncEnumerable(ct)) {
            if (index != count++)
                continue;
            return item;
        }
        return defaultValue;
    }

    public static async ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IQueryable<TItem> source, Index index, TItem? defaultValue, CancellationToken ct = default) {
        if (!index.IsFromEnd)
            return await source.ElementAtOrDefaultAsync(index.Value, defaultValue, ct).ConfigureAwait(false);
        var list = await source.ToArrayAsync(ct).ConfigureAwait(false);
        return index.Value >= list.Length ? defaultValue : list[index];
    }
}
