namespace DotNetToolbox.Data.Strategies;

public interface IAsyncUnitOfWorkRepositoryStrategy<TItem>
    : IAsyncRepositoryStrategy<TItem>
    , IAsyncUnitOfWorkRepository<TItem>;
