namespace DotNetToolbox.Data.Strategies;

public interface IAsyncRepositoryStrategy
    : IAsyncRepository;

// ReSharper disable once UnusedTypeParameter
public interface IAsyncRepositoryStrategy<TItem>
    : IAsyncRepositoryStrategy
    , IAsyncRepository<TItem>
    , IRepositoryStrategy<TItem> {
}
