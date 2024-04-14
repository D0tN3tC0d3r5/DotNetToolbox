namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategyTests {
    private readonly InMemoryRepositoryStrategy<string> _strategy;

    public InMemoryRepositoryStrategyTests() {
        var data = new[] { "A", "B", "C" };
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
        var result = _strategy.ToArray();
        result.Should().BeEquivalentTo(["A", "B", "C"]);
    }
}
