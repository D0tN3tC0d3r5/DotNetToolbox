// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IQueryable<TItem> source, int index, CancellationToken cancellationToken = default)
        => source.ElementAtOrDefaultAsync(index, default, cancellationToken);

    public static ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IQueryable<TItem> source, Index index, CancellationToken cancellationToken = default)
        => source.ElementAtOrDefaultAsync(index, default, cancellationToken);

    public static async ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IQueryable<TItem> source, int index, TItem? defaultValue, CancellationToken cancellationToken = default) {
        IsNotNegative(index);
        var count = 0;
        await foreach (var item in source.AsAsyncQueryable().AsConfigured(cancellationToken)) {
            if (index != count++)
                continue;
            return item;
        }
        return defaultValue;
    }

    public static async ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IQueryable<TItem> source, Index index, TItem? defaultValue, CancellationToken cancellationToken = default) {
        if (!index.IsFromEnd)
            return await source.ElementAtOrDefaultAsync(index.Value, defaultValue, cancellationToken).ConfigureAwait(false);
        var list = await source.ToArrayAsync(cancellationToken).ConfigureAwait(false);
        return index.Value >= list.Length ? defaultValue : list[index];
    }
}
