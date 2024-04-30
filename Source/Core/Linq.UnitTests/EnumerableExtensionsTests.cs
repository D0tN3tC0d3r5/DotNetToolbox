namespace System.Linq;

public class EnumerableExtensionsTests {
    [Fact]
    public void ToAsyncQueryable_ReturnsIQueryable() {
        var list = new[] { 1, 2, 3 };

        var query = list.AsAsyncQueryable();

        query.Should().BeOfType<AsyncQueryable<int>>();
    }
}
