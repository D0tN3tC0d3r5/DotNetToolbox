namespace DotNetToolbox.Data.Repositories;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public async Task MaxAsync_ForEmptySet_ReturnsZero() {
        var result = async () => await _emptyIntRepo.MaxAsync();
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MaxAsync_ReturnsMax() {
        var result = await _intRepo.MaxAsync();
        result.Should().Be(9);
    }

    [Fact]
    public async Task MaxAsync_WithTransformation_ReturnsMax() {
        var result = await _repo.MaxAsync(x => x.Name.Length);
        result.Should().Be(3);
    }

    [Fact]
    public async Task MaxAsync_WithNullableItem_IgnoreNullsAndReturnsMax() {
        var result = await _nullableIntRepo.MaxAsync();
        result.Should().Be(8);
    }

    [Fact]
    public async Task MaxAsync_WithNullableItemAndTransformation_IgnoreNullsAndReturnsMax() {
        var result = await _nullableIntRepo.MaxAsync(x => x * 3);
        result.Should().Be(24);
    }

    [Fact]
    public async Task MaxAsync_WithComparer_ReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repo.MaxAsync(_comparer!);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MaxAsync_WithComparerAndTransformation_ReturnsMax() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repoWithNulls!.MaxAsync(_comparer!);
        result.Should().Be(expectedItem);
    }
}
