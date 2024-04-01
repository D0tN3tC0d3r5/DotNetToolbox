namespace DotNetToolbox.Collections.Generic;

public interface IAsyncQueryProvider : IQueryProvider {
    TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);
}