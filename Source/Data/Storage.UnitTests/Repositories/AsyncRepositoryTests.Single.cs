namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task SingleAsync_ForPopulatedSet_ReturnsSingleElement() {
        var expectedItem = new TestEntity("A");
        var result = await _singleRepo.SingleAsync();
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task SingleAsync_ForEmptySet_Throws() {
        var result = async () => await _emptyRepo.SingleAsync();
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SingleAsync_WithValidPredicate_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = await _repo.SingleAsync(x => x.Name == "BB");
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task SingleAsync_WithDuplicatedItem_Throws() {
        var result = async () => await _repoWithDuplicate.SingleAsync(x => x.Name == "CCC");
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task SingleAsync_WithInvalidPredicate_ReturnsElement() {
        var result = async () => await _emptyRepo.SingleAsync(x => x.Name == "K");
        await result.Should().ThrowAsync<InvalidOperationException>();
    }
}
