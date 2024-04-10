namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategyTests
{
    private readonly InMemoryRepositoryStrategy _strategy = new InMemoryRepositoryStrategy();

    [Fact]
    public void Execute_WithQuery_ShouldThrowNotImplementedException()
    {
        Expression query = null; // Replace with actual query
        Action act = () => _strategy.Execute<int>(query);
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void ExecuteAsync_WithQuery_ShouldThrowNotImplementedException()
    {
        Expression query = null; // Replace with actual query
        var act = () => _strategy.ExecuteAsync<int>(query, CancellationToken.None);
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Execute_WithCommand_ShouldThrowNotImplementedException()
    {
        string command = null; // Replace with actual command
        Action act = () => _strategy.Execute<int>(command);
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task ExecuteAsync_WithCommand_ShouldThrowNotImplementedException()
    {
        string command = null; // Replace with actual command
        var act = () => _strategy.ExecuteAsync<int>(command, CancellationToken.None);
        await act.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Execute_WithCommand_AndQuery_ShouldThrowNotImplementedException() {
        string command = null; // Replace with actual command
        Expression query = null; // Replace with actual query
        Action act = () => _strategy.Execute<int>(command, query);
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task ExecuteAsync_WithCommand_AndQuery_ShouldThrowNotImplementedException() {
        string command = null; // Replace with actual command
        Expression query = null; // Replace with actual query
        var act = () => _strategy.ExecuteAsync<int>(command, query, CancellationToken.None);
        await act.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Execute_WithCommand_AndInput_ShouldThrowNotImplementedException() {
        string command = null; // Replace with actual command
        var input = 0; // Replace with actual input
        Action act = () => _strategy.Execute<int, int>(command, input);
        act.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task ExecuteAsync_WithCommand_AndInput_ShouldThrowNotImplementedException() {
        string command = null; // Replace with actual command
        var input = 0; // Replace with actual input
        var act = () => _strategy.ExecuteAsync<int, int>(command, input, CancellationToken.None);
        await act.Should().ThrowAsync<NotImplementedException>();
    }
}
