// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class QueryableAsyncExtensions {
    public static ValueTask<TItem[]> ToArrayAsync<TItem>(this IQueryable<TItem> source, CancellationToken ct = default)
        => IsNotNull(source).MakeArray(ct);

    public static ValueTask<TResult[]> ToArrayAsync<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, TResult>> mapping, CancellationToken ct = default)
        => IsNotNull(source).Select(mapping).MakeArray(ct);

    public static ValueTask<TResult[]> ToArrayAsync<TItem, TResult>(this IQueryable<TItem> source, Expression<Func<TItem, int, TResult>> mapping, CancellationToken ct = default)
        => IsNotNull(source).Select(mapping).MakeArray(ct);

    private static async ValueTask<TResult[]> MakeArray<TResult>(this IEnumerable<TResult> source, CancellationToken ct = default) {
        var capacity = 4;
        var result = new TResult[capacity];
        var index = 0;
        await using var enumerator = source.GetAsyncEnumerator(ct);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false) && index < Array.MaxLength) {
            result[index] = enumerator.Current;
            index++;
            if (index < capacity)
                continue;
            capacity <<= 1;
            if (capacity >= Array.MaxLength)
                capacity = Array.MaxLength;
            Array.Resize(ref result, capacity);
        }
        Array.Resize(ref result, index);
        return result;
    }
}
