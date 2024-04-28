namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void Remove_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.Remove(_ => true);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void Remove_RemovesItem() {
        _updatableRepo.Remove(s => s.Name == "BB");
        _updatableRepo.Count().Should().Be(2);
    }

    [Fact]
    public void Remove_NonExistingItem_DoesNothing() {
        _updatableRepo.Remove(s => s.Name == "K");
        _updatableRepo.Count().Should().Be(3);
    }
}
