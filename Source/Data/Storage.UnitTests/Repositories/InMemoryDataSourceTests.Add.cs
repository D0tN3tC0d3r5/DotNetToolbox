namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void Add_BaseStrategy_ShouldThrow() {
        var action = () => _dummyDataSource.Add(new("D"));
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task AddAsync_BaseStrategy_ShouldThrow() {
        var action = () => _dummyDataSource.AddAsync(new("D"));
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void Add_AddsItem() {
        _updatableRepo.Add(new("D"));
        _updatableRepo.Count().Should().Be(4);
    }

    [Fact]
    public async Task AddAsync_AddsItem() {
        await _updatableRepo.AddAsync(new("D"));
        _updatableRepo.Count().Should().Be(4);
    }
}
