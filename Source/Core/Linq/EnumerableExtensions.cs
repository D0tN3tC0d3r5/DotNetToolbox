namespace System.Linq;

public static class EnumerableExtensions {
    public static IAsyncQueryable<TItem> AsAsyncQueryable<TItem>(this IEnumerable<TItem> source)
        => new AsyncQueryable<TItem>(source);

    public static IAsyncOrderedQueryable<TItem> AsAsyncOrderedQueryable<TItem>(this IEnumerable<TItem> source)
        => new AsyncOrderedQueryable<TItem>(source);

    public static async IAsyncEnumerable<TItem> AsAsyncEnumerable<TItem>(this IEnumerable<TItem> source, [EnumeratorCancellation] CancellationToken ct = default) {
        await using var enumerable = source.GetAsyncEnumerator(ct);
        while (await enumerable.MoveNextAsync().ConfigureAwait(false))
            yield return enumerable.Current;
    }

    public static IAsyncOrderedQueryable<TItem> AsAsyncOrderedQueryable<TItem>(this IQueryable<TItem> source) => new AsyncOrderedQueryable<TItem>(source);
    public static IAsyncOrderedQueryable<TItem> AsAsyncOrderedQueryable<TItem>(this IOrderedQueryable<TItem> source) => new AsyncOrderedQueryable<TItem>(source);
}
