// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncQueryableExtensions {
    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, CancellationToken cancellationToken = default)
        => FindSingleOrDefault(source, _ => true, default, cancellationToken);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, Func<TItem, bool> predicate, CancellationToken cancellationToken = default)
        => FindSingleOrDefault(source, predicate, default, cancellationToken);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, TItem? defaultValue, CancellationToken cancellationToken = default)
        => FindSingleOrDefault(source, _ => true, defaultValue, cancellationToken);

    public static ValueTask<TItem?> SingleOrDefaultAsync<TItem>(this IQueryable<TItem> source, Func<TItem, bool> predicate, TItem? defaultValue, CancellationToken cancellationToken = default)
        => FindSingleOrDefault(source, predicate, defaultValue, cancellationToken);

    private static async ValueTask<TItem?> FindSingleOrDefault<TItem>(IQueryable<TItem> source, Func<TItem, bool> predicate, TItem? defaultValue, CancellationToken cancellationToken) {
        IsNotNull(predicate);
        await using var enumerator = IsNotNull(source).GetAsyncEnumerator(cancellationToken);
        var found = false;
        var result = defaultValue;
        while (await enumerator.MoveNextAsync().ConfigureAwait(false)) {
            if (!predicate(enumerator.Current))
                continue;
            if (found)
                throw new InvalidOperationException("Collection contains more than one matching element.");
            found = true;
            result = enumerator.Current;
        }
        return result;
    }
}
