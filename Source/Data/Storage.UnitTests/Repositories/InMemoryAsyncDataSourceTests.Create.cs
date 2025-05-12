namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task CreateAsync_WithValidationContext_ShouldThrow() {
        var action = static () => _dummyDataSource.CreateAsync(static (_, _) => Task.CompletedTask, Map.Empty());
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task CreateAsync_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.CreateAsync(static (_, _) => Task.CompletedTask);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task CreateAsync_CreatesItem() {
        var result = await _updatableRepo.CreateAsync(static (s, _) => {
            s.Name = "Z";
            return Task.CompletedTask;
        });
        result.Value.Should().BeOfType<TestEntity>();
    }

    [Fact]
    public async Task CreateAsync_WithValidationContext_CreatesItem() {
        var result = await _updatableRepo.CreateAsync(static (s, _) => {
            s.Name = "Z";
            return Task.CompletedTask;
        }, Map.Empty());
        result.IsSuccessful.Should().BeTrue();
        result.Value.Name.Should().Be("Z");
    }
}
