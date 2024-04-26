namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        return await enumerator.MoveNextAsync();
    }

    public static async ValueTask<bool> AnyAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken)) {
            if (predicate(item))
                return true;
        }

        return false;
    }
}
