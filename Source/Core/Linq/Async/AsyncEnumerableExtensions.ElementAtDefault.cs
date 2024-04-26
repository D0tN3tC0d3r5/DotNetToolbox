// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, int index, CancellationToken cancellationToken = default) {
        IsNotNegative(index);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (index == count++) return enumerator.Current;
        }
        return default;
    }

    public static async ValueTask<TItem?> ElementAtOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Index index, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (index.Value == count++) return enumerator.Current;
        }
        return default;
    }

    public static async ValueTask<TItem> ElementAtOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, int index, TItem defaultValue, CancellationToken cancellationToken = default) {
        IsNotNull(defaultValue);
        IsNotNegative(index);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (index == count++)
                return enumerator.Current;
        }
        return defaultValue;
    }

    public static async ValueTask<TItem> ElementAtOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Index index, TItem defaultValue, CancellationToken cancellationToken = default) {
        IsNotNull(defaultValue);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var count = 0;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (index.Value == count++)
                return enumerator.Current;
        }
        return defaultValue;
    }
}
