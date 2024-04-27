namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Add_AddsItem() {
        _repo.Add(new("D"));
        _repo.Count().Should().Be(4);
    }

    [Fact]
    public async Task AddAsync_AddsItem() {
        await _repo.AddAsync(new("D"));
        _repo.Count().Should().Be(4);
    }
}
