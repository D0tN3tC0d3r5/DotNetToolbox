// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections.Async.Generic;

public static class AsyncEnumerator {
    public static System.Collections.Async.Generic.IAsyncEnumerator<T> Empty<T>() => AsyncEnumerator<T>.Empty;

    internal sealed class EmptyAsyncEnumerator<T> : System.Collections.Async.Generic.IAsyncEnumerator<T> {
        object IAsyncEnumerator.Current => Current!;
        public T Current => throw new InvalidOperationException("The collection has no elements");
        public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(false);
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}

public class AsyncEnumerator<T>(IEnumerator<T> enumerator, CancellationToken ct = default)
    : System.Collections.Async.Generic.IAsyncEnumerator<T> {
    public static System.Collections.Async.Generic.IAsyncEnumerator<T> Empty { get; } = new AsyncEnumerator.EmptyAsyncEnumerator<T>();

    object IAsyncEnumerator.Current => Current!;
    public T Current => enumerator.Current;

    public ValueTask<bool> MoveNextAsync()
        => ct.IsCancellationRequested
               ? ValueTask.FromCanceled<bool>(ct)
               : ValueTask.FromResult(enumerator.MoveNext());

    protected virtual ValueTask DisposeAsyncCore() {
        enumerator.Dispose();
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync() {
        await DisposeAsyncCore();
        GC.SuppressFinalize(this);
    }
}
