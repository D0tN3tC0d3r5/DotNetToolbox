namespace System.Linq.Async;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public void ToList_ReturnsList() {
        var expectedList = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = _repo.ToList();
        result.Should().BeEquivalentTo(expectedList);
    }

    [Fact]
    public void ToList_WithMapping_ReturnsMappedList() {
        var expectedList = new[] { "A*", "BB*", "CCC*" };
        var result = _repo.ToList(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedList);
    }

    [Fact]
    public async Task ToListAsync_ReturnsList() {
        var expectedList = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = await _repo.ToListAsync();
        result.Should().BeEquivalentTo(expectedList);
    }

    [Fact]
    public async Task ToListAsync_WithMapping_ReturnsMappedList() {
        var expectedList = new[] { "A*", "BB*", "CCC*" };
        var result = await _repo.ToListAsync(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedList);
    }

    [Fact]
    public async Task ToList_WithMappingAndIndex_ReturnsMappedList() {
        var expectedList = new[] { "0:A", "1:BB", "2:CCC" };
        var result = await _repo.ToListAsync((x, i) => $"{i}:{x.Name}");
        result.Should().BeEquivalentTo(expectedList);
    }

    [Fact]
    public async Task ToList_ForBigRepo_ReturnsList() {
        var expectedList = Enumerable.Range(0, 1000).ToList(x => new TestEntity($"{x}"));
        var result = await _bigRepo.ToListAsync();
        result.Should().BeEquivalentTo(expectedList);
    }
}
