namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Query_ReturnsIQueryable() {
        var result = _readOnlyRepo.Where(x => x.Name == "BB");

        result.Should().BeOfType<EnumerableQuery<TestEntity>>();
    }
}
