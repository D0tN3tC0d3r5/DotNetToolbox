namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategyTests {
    private readonly InMemoryRepositoryStrategy<string> _strategy;

    public InMemoryRepositoryStrategyTests() {
        var data = new[] { "A", "B", "C" };
        _strategy = new(data);
    }

    [Fact]
    public void ExecuteQuery_WithQuery_ShouldThrowNotImplementedException() {
        var act = _strategy.HaveAny;
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void ExecuteFunction_WithCommand_ShouldThrowNotImplementedException() {
        var act = _strategy.Count;
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void ExecuteAction_WithCommand_ShouldThrowNotImplementedException() {
        var act = _strategy.ToArray;
        act.Should().Throw<NotSupportedException>();
    }
}
