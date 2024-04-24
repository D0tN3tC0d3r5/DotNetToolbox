namespace DotNetToolbox.Data.Strategies;

public interface IRepositoryStrategy
    : IRepository;

// ReSharper disable once UnusedTypeParameter
public interface IRepositoryStrategy<TItem>
    : IRepositoryStrategy
    , IRepository<TItem> {
}
