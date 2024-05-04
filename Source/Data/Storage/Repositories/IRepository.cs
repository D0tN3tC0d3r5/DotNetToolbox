namespace DotNetToolbox.Data.Repositories;

public interface IRepository
    : IAsyncQueryable
    , IQueryableRepository
    , IUpdatableRepository {

    string Name { get; }
}

public interface IRepository<TItem>
    : IRepository
    , IAsyncQueryable<TItem>
    , IQueryableRepository<TItem>
    , IUpdatableRepository<TItem> {

    IPagedQueryableRepository<TItem>? AsPaged();
    IChunkedQueryableRepository<TItem>? AsChunked();
}
