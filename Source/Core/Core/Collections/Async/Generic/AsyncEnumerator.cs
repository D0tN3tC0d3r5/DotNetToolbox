// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections.Async.Generic;

public static class AsyncEnumerator {
    public static IAsyncEnumerator<TItem> Empty<TItem>()
        => new AsyncEnumerator<TItem>(Enumerable.Empty<TItem>().GetEnumerator());
}

public sealed class AsyncEnumerator<TItem>(IEnumerator<TItem> enumerator, CancellationToken cancellationToken = default)
    : IAsyncEnumerator<TItem> {
    //object IAsyncEnumerator.Current => Current!;
    public TItem Current => enumerator.Current;

    public ValueTask<bool> MoveNextAsync()
        => ValueTask.FromResult(!cancellationToken.IsCancellationRequested && enumerator.MoveNext());

    public ValueTask DisposeAsync() {
        enumerator.Dispose();
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    ~AsyncEnumerator()
        => enumerator.Dispose();
}
