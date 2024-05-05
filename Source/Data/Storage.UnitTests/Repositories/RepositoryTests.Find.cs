namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void Find_BaseStrategy_ShouldThrow() {
        var action = () => _dummyRepository.Find(_ => true);
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task FindAsync_BaseStrategy_ShouldThrow() {
        var action = async () => await _dummyRepository.FindAsync(_ => true);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Find_FindsItem() {
        var result = _updatableRepo.Find(x => x.Name == "BB");
        result.Should().NotBeNull();
    }

    [Fact]
    public void Find_WithFalsePredicate_FindsItem() {
        var result = _updatableRepo.Find(x => x.Name == "Z");
        result.Should().BeNull();
    }

    [Fact]
    public async Task FindAsync_FindsItem() {
        var result = await _updatableRepo.FindAsync(x => x.Name == "BB");
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task FindAsync_WithFalsePredicate_FindsItem() {
        var result = await _updatableRepo.FindAsync(x => x.Name == "Z");
        result.Should().BeNull();
    }
}
