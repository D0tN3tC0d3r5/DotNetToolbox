// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static ValueTask<TItem[]> ToArrayAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default)
        => ToArrayAsync(source, (item, _) => item, ct);

    public static ValueTask<TResult[]> ToArrayAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, CancellationToken ct = default)
        => ToArrayAsync(source, (item, _) => mapping(item), ct);

    public static async ValueTask<TResult[]> ToArrayAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, int, TResult> mapping, CancellationToken ct = default) {
        var capacity = 4;
        var result = new TResult[capacity];
        var length = 0;
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(ct);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false) && length < Array.MaxLength) {
            result[length++] = mapping(enumerator.Current, length);
            if (length < capacity)
                continue;
            capacity <<= 1;
            if (capacity >= Array.MaxLength)
                capacity = Array.MaxLength;
            Array.Resize(ref result, capacity);
        }
        Array.Resize(ref result, length);
        return result;
    }
}
