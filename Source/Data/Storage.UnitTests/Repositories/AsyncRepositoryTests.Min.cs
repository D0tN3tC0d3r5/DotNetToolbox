namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task MinAsync_ForEmptySet_ReturnsZero() {
        var result = async () => await _emptyIntRepo.MinAsync();
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task MinAsync_ReturnsMin() {
        var result = await _intRepo.MinAsync();
        result.Should().Be(1);
    }

    [Fact]
    public async Task MinAsync_WithTransformation_ReturnsMin() {
        var result = await _repo.MinAsync(x => x.Name.Length);
        result.Should().Be(1);
    }

    [Fact]
    public async Task MinAsync_WithNullableItem_IgnoreNullsAndReturnsMin() {
        var result = await _nullableIntRepo.MinAsync();
        result.Should().Be(2);
    }

    [Fact]
    public async Task MinAsync_WithNullableItemAndTransformation_IgnoreNullsAndReturnsMin() {
        var result = await _nullableIntRepo.MinAsync(x => x * 3);
        result.Should().Be(6);
    }

    [Fact]
    public async Task MinAsync_WithComparer_ReturnsMin() {
        var expectedItem = new TestEntity("A");
        var result = await _repo.MinAsync(_comparer);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task MinAsync_WithComparerAndTransformation_ReturnsMin() {
        var expectedItem = new TestEntity("A");
        var result = await _repoWithNulls!.MinAsync(_comparer);
        result.Should().Be(expectedItem);
    }
}
