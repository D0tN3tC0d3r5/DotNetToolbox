namespace DotNetToolbox.Data.Strategies;

public partial class RepositoryStrategyProviderTests {
    [Fact]
    public void GetRequiredStrategyFor_ForRegisteredStrategy_ReturnsStrategy() {
        _provider.TryAdd<RepositoryStrategy>();

        var result = _provider.GetStrategyFor<int>();

        result.Should().BeOfType<RepositoryStrategy>();
    }

    [Fact]
    public void GetRequiredUnitOfWorkStrategyFor_ForRegisteredStrategy_ReturnsStrategy() {
        _provider.TryAdd<UnitOfWorkRepositoryStrategy>();

        var result = _provider.GetStrategyFor<int>();

        result.Should().BeOfType<UnitOfWorkRepositoryStrategy>();
    }

    [Fact]
    public void GetRequiredAsyncStrategyFor_ForRegisteredStrategy_ReturnsStrategy() {
        _provider.TryAdd<AsyncRepositoryStrategy>();

        var result = _provider.GetStrategyFor<int>();

        result.Should().BeOfType<AsyncRepositoryStrategy>();
    }

    [Fact]
    public void GetRequiredAsyncUnitOfWorkStrategyFor_ForRegisteredStrategy_ReturnsStrategy() {
        _provider.TryAdd<AsyncUnitOfWorkRepositoryStrategy>();

        var result = _provider.GetStrategyFor<int>();

        result.Should().BeOfType<AsyncUnitOfWorkRepositoryStrategy>();
    }
}
