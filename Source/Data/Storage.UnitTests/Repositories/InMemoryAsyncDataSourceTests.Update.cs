namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task UpdateAsync_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.UpdateAsync(static _ => true, new(""));
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task UpdateAsync_WithEntity_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.UpdateAsync(new(""));
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesItem() {
        await _updatableRepo.UpdateAsync(static s => s.Name == "BB", new("Z"));
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_ForInvalidItem_UpdatesItem() {
        await _updatableRepo.UpdateAsync(static s => s.Name == "ZZ", new("K"));
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "K").Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_WithEntity_UpdatesItem() {
        await _updatableRepo.UpdateAsync(new("Z") { Id = 2 });
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_WithEntity_ForInvalidItem_UpdatesItem() {
        await _updatableRepo.UpdateAsync(new("K") { Id = 99 });
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "K").Should().BeNull();
    }
}
