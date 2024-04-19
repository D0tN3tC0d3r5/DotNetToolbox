namespace DotNetToolbox.Data.Repositories;

public interface IRepositoryStrategy;

// ReSharper disable once UnusedTypeParameter
public interface IRepositoryStrategy<TItem>
    : IRepositoryStrategy
    where TItem : class;
