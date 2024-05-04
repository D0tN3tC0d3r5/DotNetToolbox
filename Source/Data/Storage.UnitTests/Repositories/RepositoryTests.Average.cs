namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public async Task AverageAsync_ForEmptySet_Throws() {
        var result = async () => await _emptyIntRepo.AverageAsync();
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AverageAsync_ReturnsAverage() {
        var result = await _intRepo.AverageAsync();
        result.Should().Be(5);
    }

    [Fact]
    public async Task AverageAsync_WithTransformation_ReturnsAverage() {
        var result = await _repo.AverageAsync(x => x.Name.Length);
        result.Should().Be(2);
    }

    [Fact]
    public async Task AverageAsync_ForEmptyNullableSet_ReturnsNull() {
        var result = async () => await _emptyNullableIntRepo.AverageAsync();
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AverageAsync_WithNullableItem_IgnoreNullsAndReturnsAverage() {
        var result = await _nullableIntRepo.AverageAsync();
        result.Should().Be(5);
    }

    [Fact]
    public async Task AverageAsync_WhenAllItemsAreNull_ReturnsNull() {
        var result = await _allNullIntRepo.AverageAsync();
        result.Should().BeNull();
    }

    [Fact]
    public async Task AverageAsync_WithNullableItemAndTransformation_IgnoreNullsAndReturnsAverage() {
        var result = await _nullableIntRepo.AverageAsync(x => x * 3);
        result.Should().Be(15);
    }
}
