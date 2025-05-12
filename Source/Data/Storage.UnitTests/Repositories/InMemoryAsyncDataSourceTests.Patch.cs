namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task PatchAsync_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.PatchAsync(static _ => true, static (_, _) => Task.CompletedTask);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task PatchAsync_WithKey_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.PatchAsync(0, static (_, _) => Task.CompletedTask);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task PatchAsync_ChangesItem() {
        await _updatableRepo.PatchAsync(static s => s.Name == "BB", static (s, _) => {
            s.Name = "Z";
            return Task.CompletedTask;
        });
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public async Task PatchAsync_WithNonExistingItem_DoesNothing() {
        await _updatableRepo.PatchAsync(static s => s.Name == "K", static (s, _) => {
            s.Name = "Z";
            return Task.CompletedTask;
        });
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().NotBeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().BeNull();
    }

    [Fact]
    public async Task PatchAsync_WithKey_ChangesItem() {
        await _updatableRepo.PatchAsync(2, static (s, _) => {
            s.Name = "Z";
            return Task.CompletedTask;
        });
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public async Task PatchAsync_WithKey_WithNonExistingItem_DoesNothing() {
        await _updatableRepo.PatchAsync(99, static (s, _) => {
            s.Name = "Z";
            return Task.CompletedTask;
        });
        _updatableRepo.FirstOrDefault(static s => s.Name == "BB").Should().NotBeNull();
        _updatableRepo.FirstOrDefault(static s => s.Name == "Z").Should().BeNull();
    }
}
