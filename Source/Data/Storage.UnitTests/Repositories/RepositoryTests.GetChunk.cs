namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void GetAsChunked_IfNotChunked_ReturnsNull() {
        var result = _dummyRepository.AsChunked();
        result.Should().BeNull();
    }

    [Fact]
    public void GetFirstChunk_BaseStrategy_ShouldThrow() {
        var action = () => _dummyChunkedRepository.AsChunked()!.GetFirstChunk();
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task GetFirstChunkAsync_BaseStrategy_ShouldThrow() {
        var action = async () => await _dummyChunkedRepository.AsChunked()!.GetFirstChunkAsync();
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void GetFirstChunk_GetsAChunk() {
        var result = _chunkedRepo.AsChunked()!.GetFirstChunk();
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(20);
    }

    [Fact]
    public async Task GetFirstChunkAsync_GetAChunk() {
        var firstItem = new TestEntity("0");
        var result = await _chunkedRepo.AsChunked()!.GetFirstChunkAsync();
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public void GetNextChunk_GetsAChunk() {
        var firstItem = new TestEntity("20");
        var result = _chunkedRepo.AsChunked()!.GetNextChunk(s => s.Name == "20");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetNextChunkAsync_GetAChunk() {
        var firstItem = new TestEntity("20");
        var result = await _chunkedRepo.AsChunked()!.GetNextChunkAsync(s => s.Name == "20");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public void GetLastChunk_GetsAChunk() {
        var firstItem = new TestEntity("80");
        var result = _chunkedRepo.AsChunked()!.GetNextChunk(s => s.Name == "80");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetLastChunkAsync_GetAChunk() {
        var firstItem = new TestEntity("80");
        var result = await _chunkedRepo.AsChunked()!.GetNextChunkAsync(s => s.Name == "80");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }
}
