using System.Collections.Async;

namespace DotNetToolbox.Linq.Async;

public interface IAsyncQueryable
    : IAsyncEnumerable {
    Type ElementType { get; }
    Expression Expression { get; }
    IAsyncQueryProvider Provider { get; }
}

public interface IAsyncQueryable<out TItem>
    : System.Collections.Async.Generic.IAsyncEnumerable<TItem>
    , IAsyncQueryable;
