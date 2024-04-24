// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections.Async.Generic;

public static class AsyncEnumerable {
    public static System.Collections.Async.Generic.IAsyncEnumerable<T> Empty<T>() => AsyncEnumerable<T>.Empty;

    internal sealed class EmptyAsyncEnumerable<T> : System.Collections.Async.Generic.IAsyncEnumerable<T> {
        IAsyncEnumerator IAsyncEnumerable.GetAsyncEnumerator(CancellationToken ct) => GetAsyncEnumerator(ct);
        public System.Collections.Async.Generic.IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ct = default)
            => AsyncEnumerator.Empty<T>();
    }
}

public class AsyncEnumerable<T>(IEnumerable<T>? data = null) : System.Collections.Async.Generic.IAsyncEnumerable<T> {
    private readonly IEnumerable<T> _data = data ?? [];
    public static System.Collections.Async.Generic.IAsyncEnumerable<T> Empty { get; } = new AsyncEnumerable.EmptyAsyncEnumerable<T>();

    IAsyncEnumerator IAsyncEnumerable.GetAsyncEnumerator(CancellationToken ct) => GetAsyncEnumerator(ct);
    public System.Collections.Async.Generic.IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ct = default) => new AsyncEnumerator<T>(_data.GetEnumerator(), ct);
}
