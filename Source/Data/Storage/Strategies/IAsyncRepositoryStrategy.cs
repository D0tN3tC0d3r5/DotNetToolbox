namespace DotNetToolbox.Data.Strategies;

public interface IAsyncRepositoryStrategy
    : IAsyncOrderedRepository;

// ReSharper disable once UnusedTypeParameter
public interface IAsyncRepositoryStrategy<TItem>
    : IAsyncRepositoryStrategy
    , IAsyncOrderedRepository<TItem>;
