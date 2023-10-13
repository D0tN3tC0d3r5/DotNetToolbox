namespace System.Collections.Generic;

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
    public void ToDictionary_GetsDictionary() {
        // Arrange
        var input = new Dictionary<string, int>() {
            ["One"] = 1,
            ["Two"] = 2,
        };

        // Act
        var result = input.ToDictionary();

        // Assert
        result.Should().BeOfType<Dictionary<string, int>>();
        result.Should().HaveCount(2);
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
}
