namespace DotNetToolbox.Data.Strategies;

public partial class RepositoryStrategyProviderTests {
    // ReSharper disable ClassNeverInstantiated.Local
    private sealed class RepositoryStrategy : RepositoryStrategy<int>;
    private sealed class NewRepositoryStrategy : RepositoryStrategy<string>;
    private sealed class DuplicatedRepositoryStrategy : RepositoryStrategy<int>;
    // ReSharper restore ClassNeverInstantiated.Local

    private readonly RepositoryStrategyProvider _provider = new();
}
