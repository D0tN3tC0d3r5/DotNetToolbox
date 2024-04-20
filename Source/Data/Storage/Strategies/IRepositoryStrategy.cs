namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategy
    : IOrderedRepository;

// ReSharper disable once UnusedTypeParameter
public interface IRepositoryStrategy<TItem>
    : IRepositoryStrategy
    , IOrderedRepository<TItem>;
