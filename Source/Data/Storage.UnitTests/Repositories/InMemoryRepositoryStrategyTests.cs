namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategyTests {

    public class TestEntity(string name) {
        public string Name { get; set; } = name;
    };

    private readonly InMemoryRepositoryStrategy<Repository<TestEntity>, TestEntity> _strategy;

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
