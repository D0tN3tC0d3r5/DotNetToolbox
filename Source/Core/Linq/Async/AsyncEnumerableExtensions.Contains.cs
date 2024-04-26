namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<bool> ContainsAsync<TItem>(this IAsyncQueryable<TItem> source, TItem item, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
            if (Equals(item, enumerator.Current)) return true;
        return false;
    }

    public static async ValueTask<bool> ContainsAsync<TItem>(this IAsyncQueryable<TItem> source, TItem item, IEqualityComparer<TItem> comparer, CancellationToken cancellationToken = default) {
        IsNotNull(comparer);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
            if (comparer.Equals(item, enumerator.Current)) return true;
        return false;
    }
}
