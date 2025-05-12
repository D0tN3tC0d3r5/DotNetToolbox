namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void GetAll_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.GetAll();
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void GetAll_GetAllItems() {
        var result = _updatableRepo.GetAll();
        result.Should().BeOfType<TestEntity[]>();
        result.Length.Should().Be(3);
    }
}
