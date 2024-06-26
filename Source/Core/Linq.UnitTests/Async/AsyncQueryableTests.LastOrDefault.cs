namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public void LastOrDefault_WithInteger_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = _repo.LastOrDefault(x => x.Name == "BB");
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task LastOrDefaultAsync_ReturnsLastElement() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repo.LastOrDefaultAsync();
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task LastOrDefaultAsync_ForEmptySet_ReturnsNull() {
        var result = await _emptyRepo.LastOrDefaultAsync();
        result.Should().BeNull();
    }

    [Fact]
    public async Task LastOrDefaultAsync_WithExistingItem_ReturnsLastElement() {
        var expectedItem = new TestEntity("A");
        var result = await _repo.LastOrDefaultAsync(x => x.Name == "A");
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task LastOrDefaultAsync_WithInvalidItem_ReturnsNull() {
        var result = await _repo.LastOrDefaultAsync(x => x.Name == "K");
        result.Should().BeNull();
    }

    [Fact]
    public async Task LastOrDefaultAsync_ForPopulatedSet_WithDefaultValue_ReturnsLastElement() {
        var expectedItem = new TestEntity("CCC");
        var defaultValue = new TestEntity("D");
        var result = await _repo.LastOrDefaultAsync(defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task LastOrDefaultAsync_ForEmptySet_WithDefaultValue_ForEmptySet_ReturnsNull() {
        var defaultValue = new TestEntity("D");
        var result = await _emptyRepo.LastOrDefaultAsync(defaultValue);
        result.Should().Be(defaultValue);
    }

    [Fact]
    public async Task LastOrDefaultAsync_ForPopulatedSet_WithDefaultAndValidPredicate_ReturnsElement() {
        var expectedItem = new TestEntity("A");
        var defaultValue = new TestEntity("D");
        var result = await _repo.LastOrDefaultAsync(x => x.Name == "A", defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task LastOrDefaultAsync_ForPopulatedSet_WithDefaultAndInvalidPredicate_ReturnsDefaultValue() {
        var defaultValue = new TestEntity("D");
        var result = await _repo.LastOrDefaultAsync(x => x.Name == "K", defaultValue);
        result.Should().Be(defaultValue);
    }
}
