namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task ElementAtAsync_WithInteger_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("B");
        var result = await _set1.ElementAtAsync(1);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtAsync_WithInteger_ForInvalidIndex_Throws() {
        var result = async () => await _emptySet.ElementAtAsync(5);
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ElementAtAsync_WithIndex_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("B");
        var result = await _set1.ElementAtAsync(^2);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtAsync_WithIndex_ForInvalidIndex_ReturnsNull() {
        var result = async () => await _emptySet.ElementAtAsync(^5);
        await result.Should().ThrowAsync<InvalidOperationException>();
    }
}
