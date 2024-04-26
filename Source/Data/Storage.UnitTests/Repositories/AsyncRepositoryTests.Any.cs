namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task AnyAsync_ForEmptySet_ReturnsFalse() {
        var result = await _emptySet.AnyAsync();
        result.Should().BeFalse();
    }

    [Fact]
    public async Task AnyAsync_ReturnsTrue() {
        var result = await _set1.AnyAsync();
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AnyAsync_WithExistingItem_ReturnsTrue() {
        var result = await _set1.AnyAsync(x => x.Name == "B");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AnyAsync_WithInvalidItem_ReturnsFalse() {
        var result = await _set1.AnyAsync(x => x.Name == "K");
        result.Should().BeFalse();
    }
}
