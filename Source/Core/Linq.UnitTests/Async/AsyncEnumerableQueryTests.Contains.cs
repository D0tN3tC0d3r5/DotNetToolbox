namespace System.Linq.Async;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public void Contains_WithExistingItem_ReturnsTrue() {
        var item = new TestEntity("BB");
        var result = _repo.Contains(item);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ContainsAsync_WithExistingItem_ReturnsTrue() {
        var item = new TestEntity("BB");
        var result = await _repo.ContainsAsync(item);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ContainsAsync_WithInvalidItem_ReturnsFalse() {
        var item = new TestEntity("K");
        var result = await _repo.ContainsAsync(item);
        result.Should().BeFalse();
    }
}
