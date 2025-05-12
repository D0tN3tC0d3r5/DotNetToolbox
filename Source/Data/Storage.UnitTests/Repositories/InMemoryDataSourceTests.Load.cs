namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Load_ForEmptySet_ReturnsZero() {
        var action = _readOnlyRepo.Load;
        action.Should().NotThrow();
    }
}
