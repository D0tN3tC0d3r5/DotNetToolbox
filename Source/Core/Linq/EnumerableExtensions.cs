// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public static class EnumerableExtensions {
    public static IAsyncQueryable<TItem> AsAsyncQueryable<TItem>(this IEnumerable<TItem> source)
        => source as IAsyncQueryable<TItem> ?? new AsyncQueryable<TItem>(source);

    public static async IAsyncEnumerable<TItem> AsAsyncEnumerable<TItem>(this IEnumerable<TItem> source, [EnumeratorCancellation] CancellationToken ct = default) {
        await using var enumerable = source.GetAsyncEnumerator(ct);
        while (await enumerable.MoveNextAsync().ConfigureAwait(false))
            yield return enumerable.Current;
    }
}
