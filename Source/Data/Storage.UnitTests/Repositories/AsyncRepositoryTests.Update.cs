namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Update_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.Update(_ => true, new(""));
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task UpdateAsync_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.UpdateAsync(_ => true, new(""));
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Update_UpdatedItem() {
        _updatableRepo.Update(s => s.Name == "BB", new("Z"));
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesItem() {
        await _updatableRepo.UpdateAsync(s => s.Name == "BB", new("Z"));
        _updatableRepo.Count().Should().Be(3);
        _updatableRepo.FirstOrDefault(s => s.Name == "BB").Should().BeNull();
        _updatableRepo.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }
}
