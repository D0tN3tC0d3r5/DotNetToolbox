using DotNetToolbox.Data.Strategies;

namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategyTests {
    public record TestEntity(string Name);
    private readonly InMemoryRepositoryStrategy<InMemoryRepository<TestEntity>, TestEntity> _strategy;

    public InMemoryRepositoryStrategyTests() {
        var data = new TestEntity[] { new("One"), new("Two"), new("Three") };
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
        var expectedResult = new TestEntity[] { new("One"), new("Two"), new("Three") };
        var result = _strategy.ToArray();
        result.Should().BeEquivalentTo(expectedResult);
    }
}
