namespace DotNetToolbox;

public class ContextTests {
    [Fact]
    public void Constructor_WithNullSource_CreatesEmptyContext() {
        // Act
        var context = new Map();

        // Assert
        context.Should().NotBeNull();
        context.Count.Should().Be(0);
    }

    [Fact]
    public void Constructor_WithNonNullSource_CreatesContextWithSourceItems() {
        // Arrange
        var source = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 2 } };

        // Act
        var context = new Map(source);

        // Assert
        context.Should().NotBeNull();
        context.Count.Should().Be(2);
        context["key1"].Should().Be("value1");
        context["key2"].Should().Be(2);
    }

    [Fact]
    public void Dispose_DisposesAllKeys() {
        // Arrange
        var source = new Dictionary<string, object> { { "key1", "value1" }, { "key2", 2 } };
        var context = new Map(source);

        // Act
        context.Dispose();

        // Assert
        context.Count.Should().Be(0);
    }

    [Fact]
    public void Dispose_SetsIsDisposedToTrue() {
        // Arrange
        var context = new Map();

        // Act
        context.Dispose();

        // Assert
        var isDisposedField = typeof(Map).GetField("_isDisposed", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var isDisposed = (bool)isDisposedField?.GetValue(context)!;
        isDisposed.Should().BeTrue();
    }
}
