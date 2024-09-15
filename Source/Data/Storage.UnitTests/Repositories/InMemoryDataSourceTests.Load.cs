namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Load_BaseStrategy_ShouldThrow() {
        var action = _dummyDataSource.Load;
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task LoadAsync_BaseStrategy_ShouldThrow() {
        var action = () => _dummyDataSource.LoadAsync();
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Load_ForEmptySet_ReturnsZero() {
        var action = _readOnlyRepo.Load;
        action.Should().NotThrow();
    }

    [Fact]
    public async Task LoadAsync_ForEmptySet_ReturnsZero() {
        var action = () => _readOnlyRepo.LoadAsync();
        await action.Should().NotThrowAsync();
    }
}
