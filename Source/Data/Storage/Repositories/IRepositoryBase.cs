namespace DotNetToolbox.Data.Repositories;

public interface IRepositoryBase
    : IAsyncQueryable
    , IQueryableRepository
    , IUpdatableRepository {

    string Name { get; }
}

public interface IRepositoryBase<TItem>
    : IRepositoryBase, IAsyncQueryable<TItem> {
    IPagedRepository<TItem>? AsPaged();
    IChunkedRepository<TItem>? AsChunked();
}
