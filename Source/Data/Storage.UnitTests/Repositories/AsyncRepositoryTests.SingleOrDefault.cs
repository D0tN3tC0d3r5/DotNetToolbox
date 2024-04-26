//namespace DotNetToolbox.Data.Repositories;

//public partial class AsyncRepositoryTests {
//    [Fact]
//    public async Task SingleOrDefaultAsync_ReturnsSingleElement() {
//        var expectedItem = new TestEntity("A");
//        var result = await _set1.SingleOrDefaultAsync();
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task SingleOrDefaultAsync_ForEmptySet_ReturnsNull() {
//        var result = await _emptySet.SingleOrDefaultAsync();
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task SingleOrDefaultAsync_WithExistingItem_ReturnsSingleElement() {
//        var expectedItem = new TestEntity("A");
//        var result = await _set1.SingleOrDefaultAsync(x => x.Name == "A");
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task SingleOrDefaultAsync_WithInvalidItem_ReturnsNull() {
//        var result = await _set1.SingleOrDefaultAsync(x => x.Name == "K");
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task SingleOrDefaultAsync_WithDefaultValue_ReturnsSingleElement() {
//        var expectedItem = new TestEntity("A");
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.SingleOrDefaultAsync(defaultValue);
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task SingleOrDefaultAsync_WithDefaultValue_ForEmptySet_ReturnsNull() {
//        var defaultValue = new TestEntity("D");
//        var result = await _emptySet.SingleOrDefaultAsync(defaultValue);
//        result.Should().Be(defaultValue);
//    }

//    [Fact]
//    public async Task SingleOrDefaultAsync_WithDefaultValue_WithExistingItem_ReturnsSingleElement() {
//        var expectedItem = new TestEntity("A");
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.SingleOrDefaultAsync(x => x.Name == "A", defaultValue);
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task SingleOrDefaultAsync_WithDefaultValue_WithInvalidItem_ReturnsNull() {
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.SingleOrDefaultAsync(x => x.Name == "K", defaultValue);
//        result.Should().Be(defaultValue);
//    }
//}
