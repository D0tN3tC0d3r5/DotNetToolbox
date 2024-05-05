namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void Patch_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.Patch(_ => true, _ => { });
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task PatchAsync_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.PatchAsync(_ => true, (_, _) => Task.CompletedTask);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Patch_ChangesItem() {
        _updatableRepo.Patch(s => s.Name == "BB", s => s.Name = "Z");
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public void Patch_NonExistingItem_DoesNothing() {
        _updatableRepo.Patch(s => s.Name == "K", s => s.Name = "Z");
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(s => s.Name == "Z").Should().BeNull();
    }

    [Fact]
    public async Task PatchAsync_ChangesItem() {
        await _updatableRepo.PatchAsync(s => s.Name == "BB", (s, _) => {
            s.Name = "Z";
            return Task.CompletedTask;
        });
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public async Task PatchAsync_WithNonExistingItem_DoesNothing() {
        await _updatableRepo.PatchAsync(s => s.Name == "K", (s, _) => {
            s.Name = "Z";
            return Task.CompletedTask;
        });
        _updatableRepo.FirstOrDefault(s => s.Name == "BB").Should().NotBeNull();
        _updatableRepo.FirstOrDefault(s => s.Name == "Z").Should().BeNull();
    }
}
