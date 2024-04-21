namespace DotNetToolbox.Data.Strategies;

public interface IAsyncUnitOfWorkRepositoryStrategy
    : IAsyncRepositoryStrategy
    , IAsyncUnitOfWorkRepository;

public interface IAsyncUnitOfWorkRepositoryStrategy<TItem>
    : IAsyncRepositoryStrategy<TItem>,
    IAsyncUnitOfWorkRepositoryStrategy
    where TItem : class;
