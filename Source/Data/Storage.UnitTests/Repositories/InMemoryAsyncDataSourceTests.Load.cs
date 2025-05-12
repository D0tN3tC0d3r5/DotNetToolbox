namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task LoadAsync_ForEmptySet_ReturnsZero() {
        var action = static () => _readOnlyRepo.LoadAsync();
        await action.Should().NotThrowAsync();
    }
}
