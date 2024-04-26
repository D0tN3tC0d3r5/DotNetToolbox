// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, CancellationToken cancellationToken = default) {
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var found = false;
        var result = default(TItem);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (found) throw new InvalidOperationException("Sequence contains more than one element.");
            found = true;
            result = enumerator.Current;
        }
        return result;
    }

    public static async ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default) {
        IsNotNull(predicate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var found = false;
        var result = default(TItem);
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (found) throw new InvalidOperationException("Sequence contains more than one element.");
            if (!predicate(enumerator.Current)) continue;
            found = true;
            result = enumerator.Current;
        }
        return result;
    }

    public static async ValueTask<TItem> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, TItem defaultValue, CancellationToken cancellationToken = default) {
        IsNotNull(defaultValue);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var found = false;
        var result = defaultValue;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (found) throw new InvalidOperationException("Sequence contains more than one element.");
            found = true;
            result = enumerator.Current;
        }
        return result;
    }

    public static async ValueTask<TItem> SingleOrDefaultAsync<TItem>(this IAsyncQueryable<TItem> source, Func<TItem, bool> predicate, TItem defaultValue, CancellationToken cancellationToken = default) {
        IsNotNull(defaultValue);
        IsNotNull(predicate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var found = false;
        var result = defaultValue;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (found) throw new InvalidOperationException("Sequence contains more than one element.");
            if (!predicate(enumerator.Current)) continue;
            found = true;
            result = enumerator.Current;
        }
        return result;
    }
}
