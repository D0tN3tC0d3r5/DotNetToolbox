namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
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
