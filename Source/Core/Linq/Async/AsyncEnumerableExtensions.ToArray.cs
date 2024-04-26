namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem[]> ToArrayAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken ct = default) {
        var capacity = 4;
        var result = new TItem[capacity];
        var length = 0;
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(ct);
        while (await enumerator.MoveNextAsync() && length < Array.MaxLength) {
            result[length++] = enumerator.Current;
            if (length < capacity) continue;
            capacity <<= 1;
            if (capacity >= Array.MaxLength) capacity = Array.MaxLength;
            Array.Resize(ref result, capacity);
        }
        return result;
    }

    public static async ValueTask<TResult[]> ToArrayAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, TResult> mapping, CancellationToken ct = default) {
        var capacity = 4;
        var result = new TResult[capacity];
        var length = 0;
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(ct);
        while (await enumerator.MoveNextAsync() && length < Array.MaxLength) {
            result[length++] = mapping(enumerator.Current);
            if (length < capacity)
                continue;
            capacity <<= 1;
            if (capacity >= Array.MaxLength)
                capacity = Array.MaxLength;
            Array.Resize(ref result, capacity);
        }
        return result;
    }

    public static async ValueTask<TResult[]> ToArrayAsync<TItem, TResult>(this IAsyncQueryable<TItem> source, Func<TItem, int, TResult> mapping, CancellationToken ct = default) {
        var capacity = 4;
        var result = new TResult[capacity];
        var length = 0;
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(ct);
        while (await enumerator.MoveNextAsync() && length < Array.MaxLength) {
            result[length++] = mapping(enumerator.Current, length);
            if (length < capacity) continue;
            capacity <<= 1;
            if (capacity >= Array.MaxLength)
                capacity = Array.MaxLength;
            Array.Resize(ref result, capacity);
        }
        return result;
    }
}
