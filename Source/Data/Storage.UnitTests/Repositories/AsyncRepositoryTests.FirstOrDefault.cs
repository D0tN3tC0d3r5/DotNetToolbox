namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task FirstOrDefaultAsync_ReturnsFirstElement() {
        var expectedItem = new TestEntity("A");
        var result = await _set1.FirstOrDefaultAsync();
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ForEmptySet_ReturnsNull() {
        var result = await _emptySet.FirstOrDefaultAsync();
        result.Should().BeNull();
    }

    [Fact]
    public async Task FirstOrDefaultAsync_WithExistingItem_ReturnsFirstElement() {
        var expectedItem = new TestEntity("A");
        var result = await _set1.FirstOrDefaultAsync(x => x.Name == "A");
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_WithInvalidItem_ReturnsNull() {
        var result = await _set1.FirstOrDefaultAsync(x => x.Name == "K");
        result.Should().BeNull();
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ForPopulatedSet_WithDefaultValue_ReturnsFirstElement() {
        var expectedItem = new TestEntity("A");
        var defaultValue = new TestEntity("D");
        var result = await _set1.FirstOrDefaultAsync(defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ForEmptySet_WithDefaultValue_ForEmptySet_ReturnsNull() {
        var defaultValue = new TestEntity("D");
        var result = await _emptySet.FirstOrDefaultAsync(defaultValue);
        result.Should().Be(defaultValue);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ForPopulatedSet_WithDefaultAndValidPredicate_ReturnsElement() {
        var expectedItem = new TestEntity("A");
        var defaultValue = new TestEntity("D");
        var result = await _set1.FirstOrDefaultAsync(x => x.Name == "A", defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ForPopulatedSet_WithDefaultAndInvalidPredicate_ReturnsDefaultValue() {
        var defaultValue = new TestEntity("D");
        var result = await _set1.FirstOrDefaultAsync(x => x.Name == "K", defaultValue);
        result.Should().Be(defaultValue);
    }
}
