namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task ElementAtAsync_WithInteger_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("B");
        var result = await _repo.ElementAtAsync(1);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtAsync_WithInteger_ForInvalidIndex_Throws() {
        var result = async () => await _emptyRepo.ElementAtAsync(5);
        await result.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task ElementAtAsync_WithIndex_ForIndexFromStart_ReturnsElement() {
        var expectedItem = new TestEntity("B");
        var result = await _repo.ElementAtAsync(new Index(1));
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtAsync_WithIndex_ForIndexFromEnd_ReturnsElement() {
        var expectedItem = new TestEntity("B");
        var result = await _repo.ElementAtAsync(^2);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtAsync_WithIndex_ForInvalidIndex_ReturnsNull() {
        var result = async () => await _emptyRepo.ElementAtAsync(^5);
        await result.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }
}
