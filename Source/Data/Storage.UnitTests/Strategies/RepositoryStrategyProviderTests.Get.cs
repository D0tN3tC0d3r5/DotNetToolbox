namespace DotNetToolbox.Data.Strategies;

public partial class RepositoryStrategyProviderTests {
    [Fact]
    public void GetRequiredStrategyFor_ForRegisteredStrategy_ReturnsStrategy() {
        _provider.TryAdd<ValueObjectRepositoryStrategy>();

        var result = _provider.GetStrategy<int>();

        result.Should().BeOfType<ValueObjectRepositoryStrategy>();
    }

    [Fact]
    public void GetRequiredStrategyFor_ForNotRegisteredStrategy_ReturnsStrategy() {
        var action = () => _provider.GetStrategy<string>();

        action.Should().Throw<InvalidOperationException>();
    }
}
