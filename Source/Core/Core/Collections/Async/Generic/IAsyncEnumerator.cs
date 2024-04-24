// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections.Async.Generic;

public interface IAsyncEnumerator<out TItem> : IAsyncEnumerator {
    new TItem Current { get; }
}
