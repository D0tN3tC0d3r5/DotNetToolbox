// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Async;

public static partial class AsyncEnumerableExtensions {
    public static async ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        var result = new Dictionary<TKey, TSource>();
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var key = keySelector(item);
            result.Add(key, item);
        }
        return result;
    }

    public static async ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        var result = new Dictionary<TKey, TSource>(comparer);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var key = keySelector(item);
            result.Add(key, item);
        }
        return result;
    }

    public static async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        var result = new Dictionary<TKey, TElement>();
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var key = keySelector(item);
            var value = elementSelector(item);
            result.Add(key, value);
        }
        return result;
    }

    public static async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        var result = new Dictionary<TKey, TElement>(comparer);
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var key = keySelector(item);
            var value = elementSelector(item);
            result.Add(key, value);
        }
        return result;
    }

    public static async ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TKey> keySelector, CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        var result = new Dictionary<TKey, TSource>();
        var index = 0;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var key = keySelector(item, index++);
            result.Add(key, item);
        }
        return result;
    }

    public static async ValueTask<Dictionary<TKey, TSource>> ToDictionaryAsync<TSource, TKey>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TKey> keySelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        var result = new Dictionary<TKey, TSource>(comparer);
        var index = 0;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var key = keySelector(item, index++);
            result.Add(key, item);
        }
        return result;
    }

    public static async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TKey> keySelector, Func<TSource, int, TElement> elementSelector, CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        var result = new Dictionary<TKey, TElement>();
        var index = 0;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var key = keySelector(item, index);
            var value = elementSelector(item, index);
            result.Add(key, value);
            index++;
        }
        return result;
    }

    public static async ValueTask<Dictionary<TKey, TElement>> ToDictionaryAsync<TSource, TKey, TElement>(this IAsyncEnumerable<TSource> source, Func<TSource, int, TKey> keySelector, Func<TSource, int, TElement> elementSelector, IEqualityComparer<TKey> comparer, CancellationToken cancellationToken = default)
        where TKey : notnull {
        IsNotNull(keySelector);
        IsNotNull(elementSelector);
        var result = new Dictionary<TKey, TElement>(comparer);
        var index = 0;
        await foreach (var item in IsNotNull(source).WithCancellation(cancellationToken).ConfigureAwait(false)) {
            var key = keySelector(item, index);
            var value = elementSelector(item, index);
            result.Add(key, value);
            index++;
        }
        return result;
    }
}
