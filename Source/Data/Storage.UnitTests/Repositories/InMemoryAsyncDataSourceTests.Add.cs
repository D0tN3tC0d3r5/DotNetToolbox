namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryAsyncDataSourceTests {
    [Fact]
    public async Task AddAsync_AddsItem() {
        await _updatableRepo.AddAsync(new("D"));
        _updatableRepo.Count().Should().Be(4);
    }
}
