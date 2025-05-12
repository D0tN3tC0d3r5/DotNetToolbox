namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task FindAsync_BaseStrategy_ShouldThrow() {
        var action = static async () => await _dummyDataSource.FindAsync(static _ => true);
        await action.Should().ThrowAsync<NotImplementedException>();
    }
    [Fact]
    public async Task FindByKeyAsync_BaseStrategy_ShouldThrow() {
        var action = static async () => await _dummyDataSource.FindByKeyAsync(0);
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public async Task FindAsync_FindsItem() {
        var result = await _updatableRepo.FindAsync(static x => x.Name == "BB");
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task FindAsync_WithFalsePredicate_FindsItem() {
        var result = await _updatableRepo.FindAsync(static x => x.Name == "Z");
        result.Should().BeNull();
    }

    [Fact]
    public async Task FindByKeyAsync_FindsItem() {
        var result = await _updatableRepo.FindByKeyAsync(2u);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task FindByKeyAsync_WithFalsePredicate_FindsItem() {
        var result = await _updatableRepo.FindByKeyAsync(5);
        result.Should().BeNull();
    }
}
