// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static IEnumerable<TSource> ToEnumerable<TSource>(this IAsyncQueryable<TSource> source)
    {
        var enumerable = source.GetAsyncEnumerator();
        try {
            while (enumerable.MoveNextAsync().AsTask().GetAwaiter().GetResult()) {
                yield return enumerable.Current;
            }
        }
        finally {
            enumerable.DisposeAsync().Wait();
        }
    }

    public static async IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(this IQueryable<TSource> source, CancellationToken ct = default) {
        await using var enumerable = source.GetAsyncEnumerator(ct);
        while (enumerable.MoveNextAsync().AsTask().GetAwaiter().GetResult())
            yield return enumerable.Current;
    }
}
