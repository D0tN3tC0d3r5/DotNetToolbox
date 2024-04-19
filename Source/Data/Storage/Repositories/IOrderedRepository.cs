namespace DotNetToolbox.Data.Repositories;

public interface IOrderedRepository<TItem>
    : IOrderedQueryableRepository<TItem>, IReadOnlyRepository<TItem>, IUpdatableRepository<TItem>
    where TItem : class {
}
