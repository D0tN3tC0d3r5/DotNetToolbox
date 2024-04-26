//namespace DotNetToolbox.Data.Repositories;

//public partial class AsyncRepositoryTests {
//    [Fact]
//    public async Task SingleAsync_ReturnsSingleElement() {
//        var expectedItem = new TestEntity("A");
//        var result = await _set1.SingleAsync();
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task SingleAsync_ForEmptySet_ReturnsNull() {
//        var result = await _emptySet.SingleAsync();
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task SingleAsync_WithExistingItem_ReturnsSingleElement() {
//        var expectedItem = new TestEntity("A");
//        var result = await _set1.SingleAsync(x => x.Name == "A");
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task SingleAsync_WithInvalidItem_ReturnsNull() {
//        var result = await _set1.SingleAsync(x => x.Name == "K");
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task SingleAsync_WithDefaultValue_ReturnsSingleElement() {
//        var expectedItem = new TestEntity("A");
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.SingleAsync(defaultValue);
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task SingleAsync_WithDefaultValue_ForEmptySet_ReturnsNull() {
//        var defaultValue = new TestEntity("D");
//        var result = await _emptySet.SingleAsync(defaultValue);
//        result.Should().Be(defaultValue);
//    }

//    [Fact]
//    public async Task SingleAsync_WithDefaultValue_WithExistingItem_ReturnsSingleElement() {
//        var expectedItem = new TestEntity("A");
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.SingleAsync(x => x.Name == "A", defaultValue);
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task SingleAsync_WithDefaultValue_WithInvalidItem_ReturnsNull() {
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.SingleAsync(x => x.Name == "K", defaultValue);
//        result.Should().Be(defaultValue);
//    }
//}
