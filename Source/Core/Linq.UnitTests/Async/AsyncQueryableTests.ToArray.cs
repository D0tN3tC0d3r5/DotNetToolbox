namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public void ToArray_ReturnsArray() {
        var expectedArray = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = _repo.ToArray();
        result.Should().BeEquivalentTo(expectedArray);
        result.Should().BeOfType<TestEntity[]>();
    }

    [Fact]
    public void ToArray_WithMapping_ReturnsMappedArray() {
        var expectedArray = new[] { "A*", "BB*", "CCC*" };
        var result = _repo.ToArray(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedArray);
        result.Should().BeOfType<string[]>();
    }

    [Fact]
    public async Task ToArrayAsync_ReturnsArray() {
        var expectedArray = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = await _repo.ToArrayAsync();
        result.Should().BeEquivalentTo(expectedArray);
        result.Should().BeOfType<TestEntity[]>();
    }

    [Fact]
    public async Task ToArrayAsync_WithMapping_ReturnsMappedArray() {
        var expectedArray = new[] { "A*", "BB*", "CCC*" };
        var result = await _repo.ToArrayAsync(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedArray);
        result.Should().BeOfType<string[]>();
    }

    [Fact]
    public async Task ToArrayAsync_WithMappingAndIndex_ReturnsMappedArray() {
        var expectedArray = new[] { "0:A", "1:BB", "2:CCC" };
        var result = await _repo.ToArrayAsync((x, i) => $"{i}:{x.Name}");
        result.Should().BeEquivalentTo(expectedArray);
        result.Should().BeOfType<string[]>();
    }

    [Fact]
    public async Task ToArrayAsync_ForBigRepo_ReturnsArray() {
        var expectedArray = Enumerable.Range(0, 1000).ToArray(x => new TestEntity($"{x}"));
        var result = await _bigRepo.ToArrayAsync();
        result.Should().BeEquivalentTo(expectedArray);
        result.Should().BeOfType<TestEntity[]>();
    }
}
