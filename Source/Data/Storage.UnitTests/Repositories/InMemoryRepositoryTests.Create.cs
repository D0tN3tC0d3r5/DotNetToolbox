namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryRepositoryTests {
    [Fact]
    public void Create_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.Create(_ => { });
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task CreateAsync_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.CreateAsync((_, _) => Task.CompletedTask);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Create_CreatesItem() {
        var result = _updatableRepo.Create(s => s.Name = "Z");
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Z");
    }

    [Fact]
    public async Task CreateAsync_CreatesItem() {
        var result = await _updatableRepo.CreateAsync((s, _) => {
            s.Name = "Z";
            return Task.CompletedTask;
        });
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Z");
    }
}
