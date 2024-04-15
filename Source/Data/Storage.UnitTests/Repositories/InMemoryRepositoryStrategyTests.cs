namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategyTests {
    public class TestEntity {
        public string Name { get; set; } = default!;
    };

    private readonly InMemoryRepositoryStrategy<TestEntity> _strategy;

    public InMemoryRepositoryStrategyTests() {
        var data = new TestEntity[] { new(), new(), new() };
        _strategy = new(data);
    }

    [Fact]
    public void HaveAny_ShouldThrowNotImplementedException() {
        var result = _strategy.HaveAny();
        result.Should().BeTrue();
    }

    [Fact]
    public void Count_ShouldThrowNotImplementedException() {
        var result = _strategy.Count();
        result.Should().Be(3);
    }

    [Fact]
    public void ToArray_ShouldThrowNotImplementedException() {
        var expectedResult = new TestEntity[] { new(), new(), new() };
        var result = _strategy.ToArray();
        result.Should().BeEquivalentTo(expectedResult);
    }
}
