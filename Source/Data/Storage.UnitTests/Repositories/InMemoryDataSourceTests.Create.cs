namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Create_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.Create(static _ => { });
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Create_CreatesItem() {
        var result = _updatableRepo.Create(static s => s.Name = "Z");
        result.IsSuccessful.Should().BeTrue();
        result.Value.Name.Should().Be("Z");
    }
}
