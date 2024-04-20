namespace DotNetToolbox.Data.Strategies;

public abstract partial class AsyncRepositoryStrategy
    : IAsyncRepositoryStrategy;

public abstract partial class AsyncRepositoryStrategy<TItem>
    : AsyncRepositoryStrategy
    , IAsyncRepositoryStrategy<TItem>;
