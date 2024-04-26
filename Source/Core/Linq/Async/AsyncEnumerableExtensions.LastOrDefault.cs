// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) { }
        return enumerator.Current
            ?? default;
    }

    public static async ValueTask<TItem?> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var result = default(TItem);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
            result = predicate(enumerator.Current) ? enumerator.Current : result;
        return result;
    }

    public static async ValueTask<TItem> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem defaultValue, CancellationToken cancellationToken = default) {
        IsNotNull(defaultValue);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) { }
        return enumerator.Current
            ?? defaultValue;
    }

    public static async ValueTask<TItem> LastOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, TItem defaultValue, CancellationToken cancellationToken = default) {
        IsNotNull(defaultValue);
        IsNotNull(predicate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var result = defaultValue;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
            result = predicate(enumerator.Current) ? enumerator.Current : result;
        return result;
    }
}
