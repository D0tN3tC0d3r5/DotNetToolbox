namespace System.Linq.Async;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public void Count_ReturnsCount() {
        var result = _repo.Count();
        result.Should().Be(3);
    }

    [Fact]
    public async Task CountAsync_ForEmptySet_ReturnsZero() {
        var result = await _emptyRepo.CountAsync();
        result.Should().Be(0);
    }

    [Fact]
    public async Task CountAsync_ReturnsCount() {
        var result = await _repo.CountAsync();
        result.Should().Be(3);
    }

    [Fact]
    public async Task CountAsync_WithPredicate_ReturnsFilteredCount() {
        var result = await _repo.CountAsync(x => x.Name != "A");
        result.Should().Be(2);
    }
}
