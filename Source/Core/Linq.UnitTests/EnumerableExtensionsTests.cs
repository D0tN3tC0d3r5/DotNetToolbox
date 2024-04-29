namespace System.Linq;

public class EnumerableExtensionsTests {
    [Fact]
    public void ToAsyncQueryable_ReturnsIQueryable() {
        var list = new[] { 1, 2, 3 };

        var query = list.ToAsyncQueryable();

        query.Should().BeOfType<AsyncEnumerableQuery<int>>();
    }
}
