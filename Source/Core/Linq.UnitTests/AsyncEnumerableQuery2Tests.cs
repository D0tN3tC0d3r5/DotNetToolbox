namespace DotNetToolbox.Data.Repositories;

public class AsyncEnumerableQuery2Tests {
    [Fact]
    public void Query_ReturnsIQueryable() {
        var list = new[] { 1, 2, 3 };

        var query = list.ToAsyncQueryable();

        var subject = query.Should().BeOfType<AsyncEnumerableQuery<int>>().Subject;
    }
}
