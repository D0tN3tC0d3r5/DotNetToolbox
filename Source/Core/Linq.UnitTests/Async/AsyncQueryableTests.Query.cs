namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public void Query_ReturnsIQueryable() {
        var result = _repo.Where(x => x.Name == "BB");

        result.Should().BeOfType<EnumerableQuery<TestEntity>>();
    }
}
