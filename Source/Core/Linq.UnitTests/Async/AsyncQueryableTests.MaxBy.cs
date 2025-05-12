namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public void MaxBy_ReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = _repo.MaxBy(static x => x.Name.Length);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxByAsync_ForEmptySet_ReturnsNull() {
        var result = static async () => await _emptyRepo.MaxByAsync(static x => x.Name.Length);
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MaxByAsync_ReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repo.MaxByAsync(static x => x.Name.Length);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxByAsync_WithTransformation_ReturnsMax() {
        var result = await _repo.MaxByAsync(static x => x.Name.Length, static x => $"{x.Name}^");
        result.Should().Be("CCC^");
    }

    [Fact]
    public async Task MaxByAsync_ForSetWithNullableItem_IgnoreNullsAndReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repoWithNulls.MaxByAsync(static x => x?.Name.Length);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxByAsync_ForSetWithNullableItems_WithTransformation_IgnoreNullsAndReturnsMax() {
        var result = await _repoWithNulls.MaxByAsync(static x => x?.Name.Length, static x => $"{x!.Name}^");
        result.Should().Be("CCC^");
    }

    [Fact]
    public async Task MaxByAsync_ForEmptySet_WithComparer_ReturnsNull() {
        var result = static async () => await _emptyRepo.MaxByAsync(static x => x, _comparer);
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MaxByAsync_WithComparer_ReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repo.MaxByAsync(static x => x.Name.Length, Comparer<int>.Default);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxByAsync_WithComparerAndTransformation_ReturnsMax() {
        var result = await _repo.MaxByAsync(static x => x.Name.Length, Comparer<int>.Default, static x => $"{x.Name}^");
        result.Should().Be("CCC^");
    }

    [Fact]
    public async Task MaxByAsync_ForSetWithNullableItems_WithComparer_IgnoreNullsAndReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repoWithNulls.MaxByAsync(static x => x?.Name.Length, Comparer<int?>.Default);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxByAsync_ForSetWithNullableItems_WithComparerAndTransformation_IgnoreNullsAndReturnsMax() {
        var result = await _repoWithNulls.MaxByAsync(static x => x?.Name.Length, Comparer<int?>.Default, static x => $"{x!.Name}^");
        result.Should().Be("CCC^");
    }
}
