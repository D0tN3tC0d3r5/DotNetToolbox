namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategyTests {
    private readonly InMemoryRepositoryStrategy<string> _strategy;

    public InMemoryRepositoryStrategyTests() {
        var repository = new ItemSet<string>();
        _strategy = new(repository);
    }

    [Fact]
    public void ExecuteQuery_WithQuery_ShouldThrowNotImplementedException() {
        Expression query = default!; // Replace with actual query
        var act = () => _strategy.ExecuteQuery<string[]>(query, CancellationToken.None);
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void ExecuteFunction_WithCommand_ShouldThrowNotImplementedException() {
        string command = default!; // Replace with actual command
        var act = () => _strategy.ExecuteFunction<int>(command, CancellationToken.None);
        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void ExecuteAction_WithCommand_ShouldThrowNotImplementedException() {
        string command = default!; // Replace with actual command
        var act = () => _strategy.ExecuteAction(command, CancellationToken.None);
        act.Should().Throw<NotSupportedException>();
    }
}
