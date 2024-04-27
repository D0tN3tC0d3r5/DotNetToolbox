// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, int index, CancellationToken cancellationToken = default)
        => source.ElementAtOrDefaultAsync(index, default, cancellationToken);

    public static ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Index index, CancellationToken cancellationToken = default)
        => source.ElementAtOrDefaultAsync(index, default, cancellationToken);

    public static async ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, int index, TItem? defaultValue, CancellationToken cancellationToken = default) {
        IsNotNegative(index);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (index != count++) continue;
            return enumerator.Current;
        }
        return defaultValue;
    }

    public static async ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Index index, TItem? defaultValue, CancellationToken cancellationToken = default) {
        if (!index.IsFromEnd)
            return await source.ElementAtOrDefaultAsync(index.Value, defaultValue, cancellationToken).ConfigureAwait(false);
        var list = await source.ToArrayAsync(cancellationToken).ConfigureAwait(false);
        return index.Value >= list.Length ? defaultValue : list[index];
    }
}
