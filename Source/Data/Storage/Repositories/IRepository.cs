namespace DotNetToolbox.Data.Repositories;

public interface IRepository;

public interface IRepository<TItem>
    : IQueryableRepository<TItem>, IReadOnlyRepository<TItem>, IUpdatableRepository<TItem>
    where TItem : class;
