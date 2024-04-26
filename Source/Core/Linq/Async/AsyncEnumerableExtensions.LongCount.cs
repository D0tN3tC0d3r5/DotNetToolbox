// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0L;
        while (await enumerator.MoveNextAsync()) count++;
        return count;
    }

    public static async ValueTask<long> LongCountAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0L;
        while (await enumerator.MoveNextAsync()) if (predicate(enumerator.Current)) count++;
        return count;
    }
}
