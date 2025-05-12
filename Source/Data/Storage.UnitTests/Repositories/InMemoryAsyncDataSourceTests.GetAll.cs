namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task GetAllAsync_BaseStrategy_ShouldThrow() {
        var action = static async () => await _dummyDataSource.GetAllAsync();
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task GetAllAsync_GetAllItems() {
        var result = await _updatableRepo.GetAllAsync();
        result.Should().BeOfType<TestEntity[]>();
        result.Length.Should().Be(3);
    }
}
