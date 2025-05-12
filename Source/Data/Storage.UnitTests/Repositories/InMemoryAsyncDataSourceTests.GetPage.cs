namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task GetPageAsync_BaseStrategy_ShouldThrow() {
        var action = static async () => await _dummyDataSource.GetPageAsync();
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task GetPageAsync_GetAPage() {
        var firstItem = new TestEntity("0") { Id = 1 };
        var result = await _readOnlyRepo.GetPageAsync();
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count.Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetPageAsync_WithIndex_GetAPage() {
        var firstItem = new TestEntity("20") { Id = 21 };
        var result = await _readOnlyRepo.GetPageAsync(1);
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count.Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetLastPageAsync_GetAPage() {
        var firstItem = new TestEntity("80") { Id = 81 };
        var result = await _readOnlyRepo.GetPageAsync(4);
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count.Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }
}
