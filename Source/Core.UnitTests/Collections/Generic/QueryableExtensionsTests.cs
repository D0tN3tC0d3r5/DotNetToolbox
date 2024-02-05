namespace DotNetToolbox.Collections.Generic;

public class QueryableExtensionsTests {
    [Fact]
    public void ToArray_WithProject_ReturnsProjectedArray() {
        // Arrange
        var source = new List<int> { 1, 2, 3 }.AsQueryable();
        Expression<Func<int, int>> project = x => x * 2;

        // Act
        var result = source.ToArray<int>(project);

        // Assert
        result.Should().Equal([2, 4, 6]);
    }

    [Fact]
    public void ToDictionary_WithProjectAndSelectors_ReturnsProjectedDictionary() {
        // Arrange
        var source = new List<string> { "one", "two", "three" }.AsQueryable();
        Expression<Func<string, string>> project = x => x.ToUpper();

        // Act
        var result = source.ToDictionary<string, string, int>(project, x => x, x => x.Length);

        // Assert
        result.Should().Equal(new Dictionary<string, int> { { "ONE", 3 }, { "TWO", 3 }, { "THREE", 5 } });
    }

    [Fact]
    public void ToHashSet_WithProject_ReturnsProjectedHashSet() {
        // Arrange
        var source = new List<int> { 1, 2, 3 }.AsQueryable();
        Expression<Func<int, int>> project = x => x * 2;

        // Act
        var result = source.ToHashSet<int>(project);

        // Assert
        result.Should().Equal(new HashSet<int> { 2, 4, 6 });
    }
}
