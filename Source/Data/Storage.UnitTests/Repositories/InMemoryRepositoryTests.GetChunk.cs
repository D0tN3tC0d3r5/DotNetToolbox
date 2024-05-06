namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryRepositoryTests {
    [Fact]
    public void GetFirstChunk_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.GetChunk();
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task GetFirstChunkAsync_BaseStrategy_ShouldThrow() {
        var action = async () => await _dummyRepository.GetChunkAsync();
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void GetFirstChunk_GetsAChunk() {
        var result = _readOnlyRepo.GetChunk();
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(20);
    }

    [Fact]
    public async Task GetFirstChunkAsync_GetAChunk() {
        var firstItem = new TestEntity("0") { Key = 1 };
        var result = await _readOnlyRepo.GetChunkAsync();
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public void GetNextChunk_GetsAChunk() {
        var firstItem = new TestEntity("20") { Key = 21 };
        var result = _readOnlyRepo.GetChunk(s => s.Name == "20");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetNextChunkAsync_GetAChunk() {
        var firstItem = new TestEntity("20") { Key = 21 };
        var result = await _readOnlyRepo.GetChunkAsync(s => s.Name == "20");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public void GetLastChunk_GetsAChunk() {
        var firstItem = new TestEntity("80") { Key = 81 };
        var result = _readOnlyRepo.GetChunk(s => s.Name == "80");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetLastChunkAsync_GetAChunk() {
        var firstItem = new TestEntity("80") { Key = 81 };
        var result = await _readOnlyRepo.GetChunkAsync(s => s.Name == "80");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count().Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }
}
