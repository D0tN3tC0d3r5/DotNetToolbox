namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task AllAsync_WithExistingItem_ReturnsTrue() {
        var result = await _set1.AllAsync(x => x.Name.Length > 0);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AllAsync_WithInvalidItem_ReturnsFalse() {
        var result = await _set1.AllAsync(x => x.Name == "K");
        result.Should().BeFalse();
    }
}
