namespace DotNetToolbox.Data;

public static class AsyncEnumerator {
    public static IAsyncEnumerator<T> Empty<T>() => AsyncEnumerator<T>.Empty;

    internal sealed class EmptyAsyncEnumerator<T> : IAsyncEnumerator<T> {
        public T Current => throw new InvalidOperationException("The collection has no elements");
        public ValueTask<bool> MoveNextAsync() => new(false);
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}

public class AsyncEnumerator<T>(IEnumerator<T> enumerator, CancellationToken ct = default)
    : IAsyncEnumerator<T> {
    public static IAsyncEnumerator<T> Empty { get; } = new AsyncEnumerator.EmptyAsyncEnumerator<T>();

    public T Current => enumerator.Current;

    public ValueTask<bool> MoveNextAsync()
        => ct.IsCancellationRequested
               ? new(Task.FromCanceled<bool>(ct))
               : new ValueTask<bool>(enumerator.MoveNext());

    public ValueTask DisposeAsync() {
        enumerator.Dispose();
        return default;
    }
}
