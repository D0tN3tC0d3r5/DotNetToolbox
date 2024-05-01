namespace DotNetToolbox.Data.Repositories;

public partial class ValueObjectRepositoryTests {
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
