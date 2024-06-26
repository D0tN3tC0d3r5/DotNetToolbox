// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections.Generic;

public class EnumerableExtensionsTests {
    [Fact]
    public void ToArray_FromNull_Throw() {
        // Arrange
        IEnumerable<int>? subject = default;

        // Act
        var result = () => subject!.ToArray(i => i + 2);

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToArray_GetsArray() {
        // Act
        var result = Enumerable.Range(0, 100).ToArray(i => i + 2);

        // Assert
        result.Should().BeOfType<int[]>();
        result.Should().HaveCount(100);
    }

    [Fact]
    public void ToArray_WithOutput_FromNull_GetsArray() {
        // Arrange
        IEnumerable<int>? subject = default;

        // Act
        var result = () => subject!.ToArray(i => $"{i + 2}");

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToArray_WithOutput_GetsArray() {
        // Act
        var result = Enumerable.Range(0, 100).ToArray(i => $"{i + 2}");

        // Assert
        result.Should().BeOfType<string[]>();
        result.Should().HaveCount(100);
    }

    [Fact]
    public void ToList_FromNull_Throw() {
        // Arrange
        IEnumerable<int>? subject = default;

        // Act
        var result = () => subject!.ToList(i => i + 2);

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToList_GetsArray() {
        // Act
        var result = Enumerable.Range(0, 100).ToList(i => i + 2);

        // Assert
        result.Should().BeOfType<List<int>>();
        result.Should().HaveCount(100);
    }

    [Fact]
    public void ToList_WithOutput_FromNull_GetsArray() {
        // Arrange
        IEnumerable<int>? subject = default;

        // Act
        var result = () => subject!.ToList(i => $"{i + 2}");

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToList_WithOutput_GetsArray() {
        // Act
        var result = Enumerable.Range(0, 100).ToList(i => $"{i + 2}");

        // Assert
        result.Should().BeOfType<List<string>>();
        result.Should().HaveCount(100);
    }

    [Fact]
    public void ToHashSet_FromNull_Throw() {
        // Arrange
        IEnumerable<int>? subject = default;

        // Act
        var result = () => subject!.ToHashSet(i => i + 2);

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToHashSet_GetsArray() {
        // Act
        var result = Enumerable.Range(0, 100).ToHashSet(i => i + 2);

        // Assert
        result.Should().BeOfType<HashSet<int>>();
        result.Should().HaveCount(100);
    }

    [Fact]
    public void ToHashSet_WithOutput_FromNull_GetsArray() {
        // Arrange
        IEnumerable<int>? subject = default;

        // Act
        var result = () => subject!.ToHashSet(i => $"{i + 2}");

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToHashSet_WithOutput_GetsArray() {
        // Act
        var result = Enumerable.Range(0, 100).ToHashSet(i => $"{i + 2}");

        // Assert
        result.Should().BeOfType<HashSet<string>>();
        result.Should().HaveCount(100);
    }

    [Fact]
    public void ToDictionary_WithTransformValue_GetsDictionary() {
        // Arrange
        var input = new Dictionary<string, int>() {
            ["One"] = 1,
            ["Two"] = 2,
        };

        // Act
        var result = input.ToDictionary<string, int>(i => i * 2);

        // Assert
        result.Should().BeOfType<Dictionary<string, int>>();
        result.Should().HaveCount(2);
        result["One"].Should().Be(2);
        result["Two"].Should().Be(4);
    }

    [Fact]
    public void ToDictionary_WithTransformElement_GetsDictionary() {
        // Arrange
        var input = new List<int>() { 1, 2 };

        // Act
        var result = input.ToDictionary(i => $"Key{i}", i => i * 2);

        // Assert
        result.Should().BeOfType<Dictionary<string, int>>();
        result.Should().HaveCount(2);
        result["Key1"].Should().Be(2);
        result["Key2"].Should().Be(4);
    }

    [Fact]
    public void ToHashSet_GetsHashSet() {
        // Act
        var result = Enumerable.Range(0, 100).ToHashSet(i => i + 2);

        // Assert
        result.Should().BeOfType<HashSet<int>>();
        result.Should().HaveCount(100);
    }

    [Fact]
    public void ToHashSet_WithOutput_GetsHashSet() {
        // Act
        var result = Enumerable.Range(0, 100).ToHashSet(i => $"{i + 2}");

        // Assert
        result.Should().BeOfType<HashSet<string>>();
        result.Should().HaveCount(100);
    }

    [Fact]
    public void ToIndexedItems_FromNull_Throw() {
        // Arrange
        IEnumerable<int>? subject = default;

        // Act
        var result = () => subject!.ToIndexedList();

        // Assert
        result.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void ToIndexedItems_GetsList() {
        // Act
        var result = Enumerable.Range(0, 100).ToIndexedList();

        // Assert
        result.Should().BeOfType<List<IndexedItem<int>>>();
        result.Should().HaveCount(100);
        result[0].Should().Be(new IndexedItem<int>(0, 0, false));
        result[99].Should().Be(new IndexedItem<int>(99, 99, true));
    }

    [Fact]
    public void ToIndexedItems_WithOutput_FromNull_GetsArray() {
        // Arrange
        IEnumerable<int>? subject = default;

        // Act
        var result = () => subject!.ToIndexedList(i => $"{i + 2}");

        // Assert
        result.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void ToIndexedItems_WithOutput_GetsList() {
        // Act
        var result = Enumerable.Range(0, 100).ToIndexedList(i => $"{i + 2}");

        // Assert
        result.Should().BeOfType<List<IndexedItem<string>>>();
        result.Should().HaveCount(100);
        result[0].Index.Should().Be(0);
        result[0].Value.Should().Be("2");
        result[0].IsFirst.Should().BeTrue();
        result[0].IsLast.Should().BeFalse();
        result[99].Index.Should().Be(99);
        result[99].Value.Should().Be("101");
        result[99].IsFirst.Should().BeFalse();
        result[99].IsLast.Should().BeTrue();
    }
}
