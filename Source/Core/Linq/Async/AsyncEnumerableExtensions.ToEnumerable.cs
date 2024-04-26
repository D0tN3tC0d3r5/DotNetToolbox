namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static IEnumerable<TSource> ToEnumerable<TSource>(this IAsyncQueryable<TSource> source) {
        var enumerable = source.GetAsyncEnumerator();
        try {
            while (!enumerable.MoveNextAsync().GetResult()) {
                yield return enumerable.Current;
            }
        }
        finally {
            enumerable.DisposeAsync().Wait();
        }
    }
}
