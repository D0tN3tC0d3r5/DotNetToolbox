namespace DotNetToolbox.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem?> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> source, int index, CancellationToken cancellationToken = default) {
        IsNotNegative(index);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (index == count++)
                return enumerator.Current;
        }
        throw new InvalidOperationException($"Collection does not contain an element at index {index}.");
    }

    public static async ValueTask<TItem?> ElementAtAsync<TItem>(this IAsyncQueryable<TItem> source, Index index, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (index.Value == count++) return enumerator.Current;
        }
        throw new InvalidOperationException($"Collection does not contain an element at index {index}.");
    }
}
