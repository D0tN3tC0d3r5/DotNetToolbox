namespace DotNetToolbox.Data.Strategies;

public partial class RepositoryStrategyProviderTests {
    [Fact]
    public void GetRequiredStrategyFor_ForRegisteredStrategy_ReturnsStrategy() {
        _provider.TryAdd<RepositoryStrategy>();

        var result = _provider.GetStrategyFor<int>();

        result.Should().BeOfType<RepositoryStrategy>();
    }

    [Fact]
    public void GetRequiredStrategyFor_ForNotRegisteredStrategy_ReturnsStrategy() {
        var action = () => _provider.GetStrategyFor<string>();

        action.Should().Throw<InvalidOperationException>();
    }
}
