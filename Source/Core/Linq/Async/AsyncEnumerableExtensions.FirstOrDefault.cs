namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        return await enumerator.MoveNextAsync().ConfigureAwait(false)
                   ? enumerator.Current
                   : default;
    }

    public static async ValueTask<TItem?> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            if (predicate(item)) return item;
        }

        return default;
    }

    public static async ValueTask<TItem> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem defaultValue, CancellationToken cancellationToken = default) {
        IsNotNull(defaultValue);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        return await enumerator.MoveNextAsync().ConfigureAwait(false)
                   ? enumerator.Current
                   : defaultValue;
    }

    public static async ValueTask<TItem> FirstOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, TItem defaultValue, CancellationToken cancellationToken = default) {
        IsNotNull(defaultValue);
        IsNotNull(predicate);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            if (predicate(item))
                return item;
        }

        return defaultValue;
    }
}
