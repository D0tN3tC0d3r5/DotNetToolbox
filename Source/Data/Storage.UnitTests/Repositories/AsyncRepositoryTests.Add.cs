namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Add_AddsItem() {
        _set1.Add(new("D"));
        _set1.Count().Should().Be(4);
    }

    [Fact]
    public async Task AddAsync_AddsItem() {
        await _set1.AddAsync(new("D"));
        _set1.Count().Should().Be(4);
    }
}
