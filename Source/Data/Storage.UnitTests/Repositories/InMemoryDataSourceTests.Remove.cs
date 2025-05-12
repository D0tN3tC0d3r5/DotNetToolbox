namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Remove_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.Remove(static _ => true);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Remove_WithKey_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.Remove(0);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Remove_RemovesItem() {
        _updatableRepo.Remove(static s => s.Name == "BB");
        _updatableRepo.Count().Should().Be(2);
    }

    [Fact]
    public void Remove_NonExistingItem_DoesNothing() {
        _updatableRepo.Remove(static s => s.Name == "K");
        _updatableRepo.Count().Should().Be(3);
    }

    [Fact]
    public void Remove_WithKey_RemovesItem() {
        _updatableRepo.Remove(2);
        _updatableRepo.Count().Should().Be(2);
    }

    [Fact]
    public void Remove_WithKey_NonExistingItem_DoesNothing() {
        _updatableRepo.Remove(99);
        _updatableRepo.Count().Should().Be(3);
    }
}
