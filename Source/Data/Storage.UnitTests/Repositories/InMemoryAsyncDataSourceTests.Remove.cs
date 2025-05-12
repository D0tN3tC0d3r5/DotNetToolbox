namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task RemoveAsync_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.RemoveAsync(static _ => true);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task RemoveAsync_WithKey_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.RemoveAsync(0);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task RemoveAsync_RemovesItem() {
        await _updatableRepo.RemoveAsync(static s => s.Name == "BB");
        _updatableRepo.Count().Should().Be(2);
    }

    [Fact]
    public async Task RemoveAsync_WithNonExistingItem_DoesNothing() {
        await _updatableRepo.RemoveAsync(static s => s.Name == "K");
        _updatableRepo.Count().Should().Be(3);
    }

    [Fact]
    public async Task RemoveAsync_WithKey_RemovesItem() {
        await _updatableRepo.RemoveAsync(2);
        _updatableRepo.Count().Should().Be(2);
    }

    [Fact]
    public async Task RemoveAsync_WithKey_WithNonExistingItem_DoesNothing() {
        await _updatableRepo.RemoveAsync(99);
        _updatableRepo.Count().Should().Be(3);
    }
}
