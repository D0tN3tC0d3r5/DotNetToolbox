namespace DotNetToolbox.Data.Repositories;

public partial class ValueObjectRepositoryTests {
    [Fact]
    public async Task ElementAtOrDefaultAsync_WithInteger_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = await _repo.ElementAtOrDefaultAsync(1);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithInteger_ForInvalidIndex_ReturnsDefaultValue() {
        var result = await _emptyRepo.ElementAtOrDefaultAsync(5);
        result.Should().BeNull();
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndex_ForIndexFromStart_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = await _repo.ElementAtOrDefaultAsync(new Index(1));
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndex_ForIndexFromEnd_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var result = await _repo.ElementAtOrDefaultAsync(^2);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndex_ForInvalidIndex_ReturnsDefaultValue() {
        var result = await _repo.ElementAtOrDefaultAsync(^5);
        result.Should().BeNull();
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIntegerAndDefaultValue_ForValidIndex_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var defaultValue = new TestEntity("D");
        var result = await _repo.ElementAtOrDefaultAsync(1, defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIntegerAndDefaultValue_ForInvalidIndex_ReturnsDefaultValue() {
        var defaultValue = new TestEntity("D");
        var result = await _emptyRepo.ElementAtOrDefaultAsync(5, defaultValue);
        result.Should().Be(defaultValue);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndexAndDefaultValue_ForIndexFromStart_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var defaultValue = new TestEntity("D");
        var result = await _repo.ElementAtOrDefaultAsync(new Index(1), defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndexAndDefaultValue_ForIndexFromEnd_ReturnsElement() {
        var expectedItem = new TestEntity("BB");
        var defaultValue = new TestEntity("D");
        var result = await _repo.ElementAtOrDefaultAsync(^2, defaultValue);
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task ElementAtOrDefaultAsync_WithIndexAndDefaultValue_ForInvalidIndex_ReturnsDefaultValue() {
        var defaultValue = new TestEntity("D");
        var result = await _repo.ElementAtOrDefaultAsync(^5, defaultValue);
        result.Should().Be(defaultValue);
    }
}
