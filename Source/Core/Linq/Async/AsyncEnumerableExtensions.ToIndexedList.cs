// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static Task<List<IndexedItem<TItem>>> ToIndexedListAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default)
        => source.ToIndexedListAsync(i => i, cancellationToken);

    public static async Task<List<IndexedItem<TResult>>> ToIndexedListAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> transform, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var list = new List<IndexedItem<TResult>>();
        var index = 0;
        var hasNext = await enumerator.MoveNextAsync().ConfigureAwait(false);
        while (hasNext) {
            var value = transform(enumerator.Current);
            hasNext = await enumerator.MoveNextAsync().ConfigureAwait(false);
            list.Add(new(index++, value, !hasNext));
        }
        return list;
    }
}
