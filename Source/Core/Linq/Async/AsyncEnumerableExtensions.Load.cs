// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async Task LoadAsync<TSource>(this IAsyncQueryable<TSource> source, CancellationToken cancellationToken = default) {
        await using var enumerator = source.GetAsyncEnumerator(cancellationToken);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) { }
    }
}
