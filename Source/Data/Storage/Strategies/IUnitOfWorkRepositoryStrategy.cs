namespace DotNetToolbox.Data.Strategies;

public interface IUnitOfWorkRepositoryStrategy<TItem>
    : IRepositoryStrategy<TItem>
    , IUnitOfWorkRepository<TItem>;
