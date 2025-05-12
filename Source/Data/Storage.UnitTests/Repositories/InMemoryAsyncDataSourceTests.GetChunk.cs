namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task GetFirstChunkAsync_BaseStrategy_ShouldThrow() {
        var action = static async () => await _dummyDataSource.GetChunkAsync();
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task GetFirstChunkAsync_GetAChunk() {
        var firstItem = new TestEntity("0") { Id = 1 };
        var result = await _readOnlyRepo.GetChunkAsync();
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count.Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetNextChunkAsync_GetAChunk() {
        var firstItem = new TestEntity("20") { Id = 21 };
        var result = await _readOnlyRepo.GetChunkAsync(static s => s.Name == "20");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count.Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetLastChunkAsync_GetAChunk() {
        var firstItem = new TestEntity("80") { Id = 81 };
        var result = await _readOnlyRepo.GetChunkAsync(static s => s.Name == "80");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count.Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }
}
