namespace DotNetToolbox.Data.Repositories;

public interface IAsyncRepository<TItem>
    : IAsyncQueryableRepository<TItem>, IAsyncReadOnlyRepository<TItem>, IAsyncUpdatableRepository<TItem>
    where TItem : class {
}
