// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections.Async;

public interface IAsyncEnumerable {
    IAsyncEnumerator GetAsyncEnumerator(CancellationToken ct = default);
}
