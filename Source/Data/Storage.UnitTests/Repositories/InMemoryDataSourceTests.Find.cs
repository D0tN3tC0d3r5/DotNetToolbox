namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Find_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.Find(static _ => true);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void FindByKey_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.FindByKey(0);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Find_FindsItem() {
        var result = _updatableRepo.Find(static x => x.Name == "BB");
        result.Should().NotBeNull();
    }

    [Fact]
    public void Find_WithFalsePredicate_FindsItem() {
        var result = _updatableRepo.Find(static x => x.Name == "Z");
        result.Should().BeNull();
    }

    [Fact]
    public void FindByKey_FindsItem() {
        var result = _updatableRepo.FindByKey(2);
        result.Should().NotBeNull();
    }

    [Fact]
    public void FindByKey_WithFalsePredicate_FindsItem() {
        var result = _updatableRepo.FindByKey(99);
        result.Should().BeNull();
    }
}
