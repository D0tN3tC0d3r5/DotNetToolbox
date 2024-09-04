namespace System.Linq;

public static partial class QueryableExtensions {
    public static async IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IQueryable<TSource> source, [EnumeratorCancellation] CancellationToken ct = default) {
        await using var enumerable = source.GetAsyncEnumerator(ct);
        while (await enumerable.MoveNextAsync().ConfigureAwait(false))
            yield return enumerable.Current;
    }
}
