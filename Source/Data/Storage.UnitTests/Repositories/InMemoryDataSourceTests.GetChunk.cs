namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void GetFirstChunk_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.GetChunk();
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void GetFirstChunk_GetsAChunk() {
        var result = _readOnlyRepo.GetChunk();
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count.Should().Be(20);
    }

    [Fact]
    public void GetNextChunk_GetsAChunk() {
        var firstItem = new TestEntity("20") { Id = 21 };
        var result = _readOnlyRepo.GetChunk(static s => s.Name == "20");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count.Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public void GetLastChunk_GetsAChunk() {
        var firstItem = new TestEntity("80") { Id = 81 };
        var result = _readOnlyRepo.GetChunk(static s => s.Name == "80");
        result.Should().BeOfType<Chunk<TestEntity>>();
        result.Items.Count.Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }
}
