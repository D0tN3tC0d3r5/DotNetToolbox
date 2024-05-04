namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public async Task MaxByAsync_ForEmptySet_ReturnsNull() {
        var result = async () => await _emptyRepo.MaxByAsync(x => x.Name.Length);
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MaxByAsync_ReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repo.MaxByAsync(x => x.Name.Length);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxByAsync_WithTransformation_ReturnsMax() {
        var result = await _repo.MaxByAsync(x => x.Name.Length, x => $"{x!.Name}^");
        result.Should().Be("CCC^");
    }

    [Fact]
    public async Task MaxByAsync_ForSetWithNullableItem_IgnoreNullsAndReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repoWithNulls.MaxByAsync(x => x?.Name.Length);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxByAsync_ForSetWithNullableItems_WithTransformation_IgnoreNullsAndReturnsMax() {
        var result = await _repoWithNulls.MaxByAsync(x => x?.Name.Length, x => $"{x!.Name}^");
        result.Should().Be("CCC^");
    }

    [Fact]
    public async Task MaxByAsync_ForEmptySet_WithComparer_ReturnsNull() {
        var result = async () => await _emptyRepo.MaxByAsync(x => x, _comparer);
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MaxByAsync_WithComparer_ReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repo.MaxByAsync(x => x.Name.Length, Comparer<int>.Default);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxByAsync_WithComparerAndTransformation_ReturnsMax() {
        var result = await _repo.MaxByAsync(x => x.Name.Length, Comparer<int>.Default, x => $"{x!.Name}^");
        result.Should().Be("CCC^");
    }

    [Fact]
    public async Task MaxByAsync_ForSetWithNullableItems_WithComparer_IgnoreNullsAndReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repoWithNulls.MaxByAsync(x => x?.Name.Length, Comparer<int?>.Default);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxByAsync_ForSetWithNullableItems_WithComparerAndTransformation_IgnoreNullsAndReturnsMax() {
        var result = await _repoWithNulls.MaxByAsync(x => x?.Name.Length, Comparer<int?>.Default, x => $"{x!.Name}^");
        result.Should().Be("CCC^");
    }
}
