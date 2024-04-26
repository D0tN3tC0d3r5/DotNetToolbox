// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) count++;
        return count;
    }

    public static async ValueTask<int> CountAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
            if (predicate(enumerator.Current)) count++;
        return count;
    }
}
