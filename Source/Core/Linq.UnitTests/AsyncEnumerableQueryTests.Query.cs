namespace DotNetToolbox.Data.Repositories;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public void Query_ReturnsIQueryable() {
        var result = _repo.Where(x => x.Name == "BB");

        result.Should().BeOfType<EnumerableQuery<TestEntity>>();
    }
}
