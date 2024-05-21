namespace System.Linq;

public class EnumerableExtensionsTests {
    [Fact]
    public void AsAsyncQueryable_ReturnsAsyncQueryable() {
        var list = new[] { 1, 2, 3 };

        var query = list.AsAsyncQueryable();

        query.Should().BeOfType<AsyncQueryable<int>>();
    }

    [Fact]
    public async Task AsAsyncEnumerable_ShouldIterateAsynchronously() {
        var list = new[] { 1, 2, 3 };

        var count = 0;
        var query = list.AsAsyncEnumerable();
        await foreach (var item in query) {
            item.Should().Be(list[count]);
            count++;
        }
    }
}
