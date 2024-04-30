namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public async Task SingleOrDefaultAsync_ReturnsSingleElement() {
        var expectedItem = new TestEntity("A");
        var result = await _singleRepo.SingleOrDefaultAsync();
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ForEmptySet_ReturnsNull() {
        var result = await _emptyRepo.SingleOrDefaultAsync();
        result.Should().BeNull();
    }

    [Fact]
    public async Task SingleOrDefaultAsync_WithExistingItem_ReturnsSingleElement() {
        var expectedItem = new TestEntity("A");
        var result = await _repo.SingleOrDefaultAsync(x => x.Name == "A");
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_WithDuplicatedItem_ReturnsNull() {
        var result = async () => await _repoWithDuplicate.SingleOrDefaultAsync(x => x.Name == "CCC");
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SingleOrDefaultAsync_WithInvalidItem_ReturnsNull() {
        var result = await _repo.SingleOrDefaultAsync(x => x.Name == "K");
        result.Should().BeNull();
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ForPopulatedSet_WithDefaultValue_ReturnsSingleElement() {
        var expectedItem = new TestEntity("A");
        var defaultValue = new TestEntity("D");
        var result = await _singleRepo.SingleOrDefaultAsync(defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ForEmptySet_WithDefaultValue_ForEmptySet_ReturnsNull() {
        var defaultValue = new TestEntity("D");
        var result = await _emptyRepo.SingleOrDefaultAsync(defaultValue);
        result.Should().Be(defaultValue);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ForPopulatedSet_WithDefaultAndValidPredicate_ReturnsElement() {
        var expectedItem = new TestEntity("A");
        var defaultValue = new TestEntity("D");
        var result = await _repo.SingleOrDefaultAsync(x => x.Name == "A", defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task SingleOrDefaultAsync_ForPopulatedSet_WithDefaultAndInvalidPredicate_ReturnsDefaultValue() {
        var defaultValue = new TestEntity("D");
        var result = await _repo.SingleOrDefaultAsync(x => x.Name == "K", defaultValue);
        result.Should().Be(defaultValue);
    }
}
