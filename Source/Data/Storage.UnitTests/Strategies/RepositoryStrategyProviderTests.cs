namespace DotNetToolbox.Data.Strategies;

public partial class RepositoryStrategyProviderTests {
    // ReSharper disable ClassNeverInstantiated.Local
    private sealed class RepositoryStrategy : RepositoryStrategy<int>;
    private sealed class NewRepositoryStrategy : RepositoryStrategy<string>;
    private sealed class DuplicatedRepositoryStrategy : RepositoryStrategy<int>;
    private sealed class AsyncRepositoryStrategy : AsyncRepositoryStrategy<int>;
    private sealed class UnitOfWorkRepositoryStrategy : UnitOfWorkRepositoryStrategy<int>;
    private sealed class AsyncUnitOfWorkRepositoryStrategy : AsyncUnitOfWorkRepositoryStrategy<int>;
    // ReSharper restore ClassNeverInstantiated.Local

    private readonly RepositoryStrategyProvider _provider = new();
}
