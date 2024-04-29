namespace DotNetToolbox.Data.Repositories;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public async Task AnyAsync_ForEmptySet_ReturnsFalse() {
        var result = await _emptyRepo.AnyAsync();
        result.Should().BeFalse();
    }

    [Fact]
    public async Task AnyAsync_ReturnsTrue() {
        var result = await _repo.AnyAsync();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AnyAsync_WithExistingItem_ReturnsTrue() {
        var result = await _repo.AnyAsync(x => x.Name == "BB");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AnyAsync_WithInvalidItem_ReturnsFalse() {
        var result = await _repo.AnyAsync(x => x.Name == "K");
        result.Should().BeFalse();
    }
}
