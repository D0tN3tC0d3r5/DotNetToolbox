using DotNetToolbox.Data.Strategies;

namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategyTests {

    private readonly InMemoryRepositoryStrategy<InMemoryRepository<int>, int> _strategy;

    public InMemoryRepositoryStrategyTests() {
        var data = new [] { 1, 2, 3 };
        _strategy = new(data);
    }

    [Fact]
    public void Any_ReturnsBoolean() {
        var result = _strategy.Any();
        result.Should().BeTrue();
    }

    [Fact]
    public void Count_ReturnsCount() {
        var result = _strategy.Count();
        result.Should().Be(3);
    }

    [Fact]
    public void ToArray_ReturnsArray() {
        var expectedResult = new[] { 1, 2, 3 };
        var result = _strategy.ToArray();
        result.Should().BeEquivalentTo(expectedResult);
    }
}
