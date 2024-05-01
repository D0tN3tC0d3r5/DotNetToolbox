using DotNetToolbox.Data.Strategies.ValueObject;

namespace DotNetToolbox.Data.Strategies;

public partial class RepositoryStrategyProviderTests {
    // ReSharper disable ClassNeverInstantiated.Local
    private sealed class ValueObjectRepositoryStrategy : ValueObjectRepositoryStrategy<int>;
    private sealed class NewValueObjectRepositoryStrategy : ValueObjectRepositoryStrategy<string>;
    private sealed class DuplicatedValueObjectRepositoryStrategy : ValueObjectRepositoryStrategy<int>;
    // ReSharper restore ClassNeverInstantiated.Local

    private readonly RepositoryStrategyProvider _provider = new();
}
