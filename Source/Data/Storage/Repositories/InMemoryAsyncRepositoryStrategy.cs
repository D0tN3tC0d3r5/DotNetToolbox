namespace DotNetToolbox.Data.Repositories;

internal sealed class InMemoryAsyncRepositoryStrategy<TRepository, TItem>(IEnumerable<TItem> source)
    : AsyncRepositoryStrategy<TItem>
    where TRepository : class, IAsyncRepository<TItem>
    where TItem : class {
    private readonly IQueryable<TItem> _source = source.AsQueryable();

    public override AsyncRepository<TResult> OfType<TResult>()
        where TResult : class {
        var result = _source.OfType<TResult>();
        return RepositoryFactory.CreateAsyncRepository<TRepository, TResult>(result);
    }

    public override AsyncRepository<TResult> Cast<TResult>()
        where TResult : class {
        var result = _source.Cast<TResult>();
        return RepositoryFactory.CreateAsyncRepository<TRepository, TResult>(result);
    }
}
