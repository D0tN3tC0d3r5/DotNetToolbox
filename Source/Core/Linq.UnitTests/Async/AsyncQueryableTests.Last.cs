namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public void Last_WithInteger_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = _repo.Last(static x => x.Name == "BB");
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task LastAsync_ForPopulatedSet_ReturnsLastElement() {
        var expectedItem = new TestEntity("CCC");
        var result = await _repo.LastAsync();
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task LastAsync_ForEmptySet_Throws() {
        var result = static async () => await _emptyRepo.LastAsync();
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task LastAsync_WithValidPredicate_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = await _repo.LastAsync(static x => x.Name == "BB");
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task LastAsync_WithInvalidPredicate_ReturnsElement() {
        var result = static async () => await _emptyRepo.LastAsync(static x => x.Name == "K");
        await result.Should().ThrowAsync<InvalidOperationException>();
    }
}
