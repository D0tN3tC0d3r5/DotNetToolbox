namespace DotNetToolbox.Collections.Generic;

public class EnumerableExtensionsTests {
    [Fact]
    public void ToArray_FromNull_Throw() {
        // Arrange
        IEnumerable<int>? subject = default;

        // Act
        var result = () => subject!.ToArray<int>(i => i + 2);

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void ToArray_GetsArray() {
        // Act
        var result = Enumerable.Range(0, 100).ToArray<int>(i => i + 2);

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
        var result = input.ToDictionary<int, string, int>(i => i, i => $"Key{i}", i => i * 2);

        // Assert
        result.Should().BeOfType<Dictionary<string, int>>();
        result.Should().HaveCount(2);
        result["Key1"].Should().Be(2);
        result["Key2"].Should().Be(4);
    }

    [Fact]
    public void ToHashSet_GetsHashSet() {
        // Act
        var result = Enumerable.Range(0, 100).ToHashSet<int>(i => i + 2);

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
}
