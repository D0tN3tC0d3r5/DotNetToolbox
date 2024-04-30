namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void Remove_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.Remove(_ => true);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task RemoveAsync_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.RemoveAsync(_ => true);
        await action.Should().ThrowAsync<NotImplementedException>();
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

    [Fact]
    public async Task RemoveAsync_RemovesItem() {
        await _updatableRepo.RemoveAsync(s => s.Name == "BB");
        _updatableRepo.Count().Should().Be(2);
    }

    [Fact]
    public async Task RemoveAsync_WithNonExistingItem_DoesNothing() {
        await _updatableRepo.RemoveAsync(s => s.Name == "K");
        _updatableRepo.Count().Should().Be(3);
    }
}
