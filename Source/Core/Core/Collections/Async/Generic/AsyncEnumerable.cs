// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections.Async.Generic;

public static class AsyncEnumerable {
    public static IAsyncEnumerable<TItem> Empty<TItem>()
        =>  new AsyncEnumerable<TItem>();
}

public class AsyncEnumerable<TItem>(IEnumerable<TItem>? data = null)
    : IAsyncEnumerable<TItem> {
    private readonly IEnumerable<TItem> _data = data ?? [];
    private readonly CancellationToken _cancellationToken;

    internal AsyncEnumerable(AsyncEnumerable<TItem> source, CancellationToken cancellationToken)
        : this(source._data) {
        _cancellationToken = cancellationToken;
    }

    //IAsyncEnumerator IAsyncEnumerable.GetAsyncEnumerator(CancellationToken cancellationToken)
    //    => GetAsyncEnumerator(cancellationToken);

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken cancellationToken = default) {
        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _cancellationToken);
        return new AsyncEnumerator<TItem>(_data.GetEnumerator(), cts.Token);
    }
}
