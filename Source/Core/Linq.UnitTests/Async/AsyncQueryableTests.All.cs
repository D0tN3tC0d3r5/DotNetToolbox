namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public void All_WithExistingItem_ReturnsTrue() {
        var result = _repo.All(x => x.Name.Length > 0);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AllAsync_WithExistingItem_ReturnsTrue() {
        var result = await _repo.AllAsync(x => x.Name.Length > 0);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AllAsync_WithInvalidItem_ReturnsFalse() {
        var result = await _repo.AllAsync(x => x.Name == "K");
        result.Should().BeFalse();
    }
}
