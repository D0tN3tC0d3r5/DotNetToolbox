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
    public void FindByKey_FindsItem() {
        var result = _updatableRepo.FindByKey(2);
        result.Should().NotBeNull();
    }

    [Fact]
    public void FindByKey_WithFalsePredicate_FindsItem() {
        var result = _updatableRepo.FindByKey(99);
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

    [Fact]
    public async Task FindByKeyAsync_FindsItem() {
        var result = await _updatableRepo.FindByKeyAsync(2);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task FindByKeyAsync_WithFalsePredicate_FindsItem() {
        var result = await _updatableRepo.FindByKeyAsync(5);
        result.Should().BeNull();
    }
}
