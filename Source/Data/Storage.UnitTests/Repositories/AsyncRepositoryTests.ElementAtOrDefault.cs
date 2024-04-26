namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task ElementAtOrDefaultAsync_WithInteger_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("B");
        var result = await _set1.ElementAtOrDefaultAsync(1);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithInteger_ForInvalidIndex_ReturnsDefaultValue() {
        var result = await _emptySet.ElementAtOrDefaultAsync(5);
        result.Should().BeNull();
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndex_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("B");
        var result = await _set1.ElementAtOrDefaultAsync(^2);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndex_ForInvalidIndex_ReturnsDefaultValue() {
        var result = await _set1.ElementAtOrDefaultAsync(^5);
        result.Should().BeNull();
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIntegerAndDefaultValue_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("B");
        var defaultValue = new TestEntity("D");
        var result = await _set1.ElementAtOrDefaultAsync(1, defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIntegerAndDefaultValue_ForInvalidIndex_ReturnsDefaultValue() {
        var defaultValue = new TestEntity("D");
        var result = await _emptySet.ElementAtOrDefaultAsync(5, defaultValue);
        result.Should().Be(defaultValue);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndexAndDefaultValue_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("B");
        var defaultValue = new TestEntity("D");
        var result = await _set1.ElementAtOrDefaultAsync(^2, defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndexAndDefaultValue_ForInvalidIndex_ReturnsDefaultValue() {
        var defaultValue = new TestEntity("D");
        var result = await _set1.ElementAtOrDefaultAsync(^5, defaultValue);
        result.Should().Be(defaultValue);
    }
}
