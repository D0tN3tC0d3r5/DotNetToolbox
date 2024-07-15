// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections.Async.Generic;

public static class AsyncEnumerable {
    public static IAsyncEnumerable<TItem> Empty<TItem>()
        => new AsyncEnumerable<TItem>();
}

public class AsyncEnumerable<TItem>(IEnumerable<TItem>? data = null)
    : IAsyncEnumerable<TItem> {
    private readonly IEnumerable<TItem> _data = data ?? [];

    public IAsyncEnumerator<TItem> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new AsyncEnumerator<TItem>(_data.GetEnumerator(), cancellationToken);
}
