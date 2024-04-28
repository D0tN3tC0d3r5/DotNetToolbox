// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq;

public class QueryableExtensionsTests {
    [Fact]
    public void ToArray_WithProject_ReturnsProjectedArray() {
        // Arrange
        var source = new List<int> { 1, 2, 3 }.AsQueryable();

        // Act
        var result = source.ToArray<int>(Project);

        // Assert
        result.Should().Equal([2, 4, 6]);
        return;

        static int Project(int x) => x * 2;
    }

    [Fact]
    public void ToList_WithProject_ReturnsProjectedArray() {
        // Arrange
        var source = new List<int> { 1, 2, 3 }.AsQueryable();

        // Act
        var result = source.ToList<int>(Project);

        // Assert
        result.Should().Equal([2, 4, 6]);
        return;

        static int Project(int x) => x * 2;
    }

    [Fact]
    public void ToDictionary_WithProjectAndSelectors_ReturnsProjectedDictionary() {
        // Arrange
        var source = new List<string> { "one", "two", "three" }.AsQueryable();

        // Act
        var result = source.ToDictionary(x => x.ToUpper(CultureInfo.InvariantCulture), x => x.Length);

        // Assert
        result.Should().Equal(new Dictionary<string, int> { { "ONE", 3 }, { "TWO", 3 }, { "THREE", 5 } });
    }

    [Fact]
    public void ToHashSet_WithProject_ReturnsProjectedHashSet() {
        // Arrange
        var source = new List<int> { 1, 2, 3 }.AsQueryable();

        // Act
        var result = source.ToHashSet<int>(Project);

        // Assert
        result.Should().Equal(new HashSet<int> { 2, 4, 6 });
        return;

        static int Project(int x) => x * 2;
    }
}
