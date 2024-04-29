namespace System.Linq.Async;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public void ElementAt_WithInteger_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = _repo.ElementAt(1);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public void ElementAt_WithIndex_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = _repo.ElementAt(^2);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtAsync_WithInteger_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
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
        var expectedItem = new TestEntity("BB");
        var result = await _repo.ElementAtAsync(new Index(1));
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtAsync_WithIndex_ForIndexFromEnd_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = await _repo.ElementAtAsync(^2);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtAsync_WithIndex_ForInvalidIndex_ReturnsNull() {
        var result = async () => await _emptyRepo.ElementAtAsync(^5);
        await result.Should().ThrowAsync<ArgumentOutOfRangeException>();
    }
}
