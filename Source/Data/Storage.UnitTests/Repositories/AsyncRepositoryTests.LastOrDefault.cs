//namespace DotNetToolbox.Data.Repositories;

//public partial class AsyncRepositoryTests {
//    [Fact]
//    public async Task LastOrDefaultAsync_ReturnsLastElement() {
//        var expectedItem = new TestEntity("A");
//        var result = await _set1.LastOrDefaultAsync();
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task LastOrDefaultAsync_ForEmptySet_ReturnsNull() {
//        var result = await _emptySet.LastOrDefaultAsync();
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task LastOrDefaultAsync_WithExistingItem_ReturnsLastElement() {
//        var expectedItem = new TestEntity("A");
//        var result = await _set1.LastOrDefaultAsync(x => x.Name == "A");
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task LastOrDefaultAsync_WithInvalidItem_ReturnsNull() {
//        var result = await _set1.LastOrDefaultAsync(x => x.Name == "K");
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task LastOrDefaultAsync_WithDefaultValue_ReturnsLastElement() {
//        var expectedItem = new TestEntity("A");
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.LastOrDefaultAsync(defaultValue);
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task LastOrDefaultAsync_WithDefaultValue_ForEmptySet_ReturnsNull() {
//        var defaultValue = new TestEntity("D");
//        var result = await _emptySet.LastOrDefaultAsync(defaultValue);
//        result.Should().Be(defaultValue);
//    }

//    [Fact]
//    public async Task LastOrDefaultAsync_WithDefaultValue_WithExistingItem_ReturnsLastElement() {
//        var expectedItem = new TestEntity("A");
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.LastOrDefaultAsync(x => x.Name == "A", defaultValue);
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task LastOrDefaultAsync_WithDefaultValue_WithInvalidItem_ReturnsNull() {
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.LastOrDefaultAsync(x => x.Name == "K", defaultValue);
//        result.Should().Be(defaultValue);
//    }
//}
