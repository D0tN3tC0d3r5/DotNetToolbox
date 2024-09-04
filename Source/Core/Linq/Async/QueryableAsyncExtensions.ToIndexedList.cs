// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static Task<List<IndexedItem<TItem>>> ToIndexedListAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => IsNotNull(source).MakeIndexedListAsync(ct);

    public static Task<List<IndexedItem<TResult>>> ToIndexedListAsync<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, TResult>> transform, CancellationToken ct = default)
        => IsNotNull(source).Select(transform).MakeIndexedListAsync(ct);
    private static async Task<List<IndexedItem<TResult>>> MakeIndexedListAsync<TResult>(this IQueryable<TResult> source, CancellationToken ct = default) {
        await using var enumerator = IsNotNull(source).Select((x, i) => KeyValuePair.Create(i, x)).GetAsyncEnumerator(ct);
        var list = new List<IndexedItem<TResult>>();
        var hasNext = await enumerator.MoveNextAsync().ConfigureAwait(false);
        while (hasNext) {
            var item = enumerator.Current;
            hasNext = await enumerator.MoveNextAsync().ConfigureAwait(false);
            list.Add(new(item.Key, item.Value, !hasNext));
        }
        return list;
    }
}
