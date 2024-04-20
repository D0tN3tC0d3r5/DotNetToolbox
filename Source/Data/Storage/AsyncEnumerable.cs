namespace DotNetToolbox.Data;

public static class AsyncEnumerable {
    public static IAsyncEnumerable<T> Empty<T>() => AsyncEnumerable<T>.Empty;

    internal sealed class EmptyAsyncEnumerable<T> : IAsyncEnumerable<T> {
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ct = default)
            => AsyncEnumerator.Empty<T>();
    }
}

public class AsyncEnumerable<T>(IEnumerable<T>? data = null) : IAsyncEnumerable<T> {
    private readonly IEnumerable<T> _data = data ?? Enumerable.Empty<T>();

    public static IAsyncEnumerable<T> Empty { get; } = new AsyncEnumerable.EmptyAsyncEnumerable<T>();

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken ct = default) => new AsyncEnumerator<T>(_data.GetEnumerator(), ct);
}
