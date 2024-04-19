namespace DotNetToolbox.Data.Repositories;

public interface IAsyncOrderedRepository<TItem>
    : IAsyncOrderedQueryableRepository<TItem>, IAsyncReadOnlyRepository<TItem>, IAsyncUpdatableRepository<TItem>
    where TItem : class {
}