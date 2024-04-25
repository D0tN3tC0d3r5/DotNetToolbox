// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections.Async.Generic;

public static class AsyncEnumerableExtensions {

    public static IAsyncEnumerable<TItem> WithCancellation<TItem>(this AsyncEnumerable<TItem> source, CancellationToken cancellationToken)
        => new AsyncEnumerable<TItem>(source, cancellationToken);
}
