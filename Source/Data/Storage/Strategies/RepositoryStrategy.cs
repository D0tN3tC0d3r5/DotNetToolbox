namespace DotNetToolbox.Data.Strategies;

public abstract partial class RepositoryStrategy
    : IRepositoryStrategy;

public abstract partial class RepositoryStrategy<TItem>
    : RepositoryStrategy,
      IRepositoryStrategy<TItem>
    where TItem : class;
