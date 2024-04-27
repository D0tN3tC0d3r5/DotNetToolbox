namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Update_UpdatedItem() {
        _repo.Update(s => s.Name == "B", new("Z"));
        _repo.Count().Should().Be(3);
        _repo.FirstOrDefault(s => s.Name == "B").Should().BeNull();
        _repo.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_UpdatesItem() {
        await _repo.UpdateAsync(s => s.Name == "B", new("Z"));
        _repo.Count().Should().Be(3);
        _repo.FirstOrDefault(s => s.Name == "B").Should().BeNull();
        _repo.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }
}
