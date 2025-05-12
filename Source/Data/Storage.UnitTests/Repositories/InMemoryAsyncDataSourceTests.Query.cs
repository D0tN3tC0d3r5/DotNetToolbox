namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public void Query_ReturnsIAsyncQueryable() {
        var result = _readOnlyRepo.Where(static x => x.Name == "BB");

        result.Should().BeOfType<EnumerableQuery<TestEntity>>();
    }
}
