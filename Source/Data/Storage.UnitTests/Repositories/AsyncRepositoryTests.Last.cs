//namespace DotNetToolbox.Data.Repositories;

//public partial class AsyncRepositoryTests {
//    [Fact]
//    public async Task LastAsync_ReturnsLastElement() {
//        var expectedItem = new TestEntity("A");
//        var result = await _set1.LastAsync();
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task LastAsync_ForEmptySet_ReturnsNull() {
//        var result = await _emptySet.LastAsync();
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task LastAsync_WithExistingItem_ReturnsLastElement() {
//        var expectedItem = new TestEntity("A");
//        var result = await _set1.LastAsync(x => x.Name == "A");
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task LastAsync_WithInvalidItem_ReturnsNull() {
//        var result = await _set1.LastAsync(x => x.Name == "K");
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task LastAsync_WithDefaultValue_ReturnsLastElement() {
//        var expectedItem = new TestEntity("A");
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.LastAsync(defaultValue);
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task LastAsync_WithDefaultValue_ForEmptySet_ReturnsNull() {
//        var defaultValue = new TestEntity("D");
//        var result = await _emptySet.LastAsync(defaultValue);
//        result.Should().Be(defaultValue);
//    }

//    [Fact]
//    public async Task LastAsync_WithDefaultValue_WithExistingItem_ReturnsLastElement() {
//        var expectedItem = new TestEntity("A");
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.LastAsync(x => x.Name == "A", defaultValue);
//        result.Should().Be(expectedItem);
//    }

//    [Fact]
//    public async Task LastAsync_WithDefaultValue_WithInvalidItem_ReturnsNull() {
//        var defaultValue = new TestEntity("D");
//        var result = await _set1.LastAsync(x => x.Name == "K", defaultValue);
//        result.Should().Be(defaultValue);
//    }
//}
