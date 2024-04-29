namespace DotNetToolbox.Data.Repositories;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public async Task MinByAsync_ForEmptySet_ReturnsNull() {
        var result = async () => await _emptyRepo.MinByAsync(x => x.Name.Length);
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MinByAsync_ReturnsMin() {
        var expectedItem = new TestEntity("A");
        var result = await _repo.MinByAsync(x => x.Name.Length);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MinByAsync_WithTransformation_ReturnsMin() {
        var result = await _repo.MinByAsync(x => x.Name.Length, x => $"{x!.Name}^");
        result.Should().Be("A^");
    }

    [Fact]
    public async Task MinByAsync_ForSetWithNullableItem_IgnoreNullsAndReturnsMin() {
        var expectedItem = new TestEntity("A");
        var result = await _repoWithNulls.MinByAsync(x => x?.Name.Length);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MinByAsync_ForSetWithNullableItems_WithTransformation_IgnoreNullsAndReturnsMin() {
        var result = await _repoWithNulls.MinByAsync(x => x?.Name.Length, x => $"{x!.Name}^");
        result.Should().Be("A^");
    }

    [Fact]
    public async Task MinByAsync_ForEmptySet_WithComparer_ReturnsNull() {
        var result = async () => await _emptyRepo.MinByAsync(x => x, _comparer);
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MinByAsync_WithComparer_ReturnsMin() {
        var expectedItem = new TestEntity("A");
        var result = await _repo.MinByAsync(x => x.Name.Length, Comparer<int>.Default);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MinByAsync_WithComparerAndTransformation_ReturnsMin() {
        var result = await _repo.MinByAsync(x => x.Name.Length, Comparer<int>.Default, x => $"{x!.Name}^");
        result.Should().Be("A^");
    }

    [Fact]
    public async Task MinByAsync_ForSetWithNullableItems_WithComparer_IgnoreNullsAndReturnsMin() {
        var expectedItem = new TestEntity("A");
        var result = await _repoWithNulls.MinByAsync(x => x?.Name.Length, Comparer<int?>.Default);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MinByAsync_ForSetWithNullableItems_WithComparerAndTransformation_IgnoreNullsAndReturnsMin() {
        var result = await _repoWithNulls.MinByAsync(x => x?.Name.Length, Comparer<int?>.Default, x => $"{x!.Name}^");
        result.Should().Be("A^");
    }
}
