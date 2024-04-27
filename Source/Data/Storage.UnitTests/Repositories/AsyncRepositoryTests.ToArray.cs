namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task ToArrayAsync_ReturnsArray() {
        var expectedArray = new TestEntity[] { new("A"), new("B"), new("C") };
        var result = await _repo.ToArrayAsync();
        result.Should().BeEquivalentTo(expectedArray);
    }

    [Fact]
    public async Task ToArrayAsync_WithMapping_ReturnsMappedArray() {
        var expectedArray = new[] { "A*", "B*", "C*" };
        var result = await _repo.ToArrayAsync(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedArray);
    }

    [Fact]
    public async Task ToArrayAsync_WithMappingAndIndex_ReturnsMappedArray() {
        var expectedArray = new[] { "0:A", "1:B", "2:C" };
        var result = await _repo.ToArrayAsync((x, i) => $"{i}:{x.Name}");
        result.Should().BeEquivalentTo(expectedArray);
    }

    [Fact]
    public async Task ToArrayAsync_ForBigRepo_ReturnsArray() {
        var expectedArray = Enumerable.Range(0, 1000).ToArray(x => new TestEntity($"{x}"));
        var result = await _bigRepo.ToArrayAsync();
        result.Should().BeEquivalentTo(expectedArray);
    }
}
