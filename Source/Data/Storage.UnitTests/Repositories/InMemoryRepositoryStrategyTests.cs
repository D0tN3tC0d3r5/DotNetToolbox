namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryStrategyTests
{
    private readonly InMemoryRepositoryStrategy<string> _strategy = new();

    //[Fact]
    //public void ExecuteAsync_WithQuery_ShouldThrowNotImplementedException()
    //{
    //    Expression query = default!; // Replace with actual query
    //    var act = () => _strategy.ExecuteAsync<int>(query, CancellationToken.None);
    //    act.Should().Throw<NotImplementedException>();
    //}

    //[Fact]
    //public async Task ExecuteAsync_WithCommand_ShouldThrowNotImplementedException()
    //{
    //    string command = default!; // Replace with actual command
    //    var act = () => _strategy.ExecuteAsync<int>(command, CancellationToken.None);
    //    await act.Should().ThrowAsync<NotImplementedException>();
    //}

    //[Fact]
    //public async Task ExecuteAsync_WithCommand_AndQuery_ShouldThrowNotImplementedException() {
    //    string command = default!; // Replace with actual command
    //    Expression query = default!; // Replace with actual query
    //    var act = () => _strategy.ExecuteAsync<int>(command, query, CancellationToken.None);
    //    await act.Should().ThrowAsync<NotImplementedException>();
    //}

    //[Fact]
    //public async Task ExecuteAsync_WithCommand_AndInput_ShouldThrowNotImplementedException() {
    //    string command = default!; // Replace with actual command
    //    var input = 0; // Replace with actual input
    //    var act = () => _strategy.ExecuteAsync<int, int>(command, input, CancellationToken.None);
    //    await act.Should().ThrowAsync<NotImplementedException>();
    //}
}
