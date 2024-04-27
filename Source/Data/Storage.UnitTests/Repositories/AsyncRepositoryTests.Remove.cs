namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Remove_RemovesItem() {
        _repo.Remove(s => s.Name == "B");
        _repo.Count().Should().Be(2);
    }

    [Fact]
    public void Remove_NonExistingItem_DoesNothing() {
        _repo.Remove(s => s.Name == "K");
        _repo.Count().Should().Be(3);
    }

    [Fact]
    public async Task RemoveAsync_RemovesItem() {
        await _repo.RemoveAsync(s => s.Name == "B");
        _repo.Count().Should().Be(2);
    }

    [Fact]
    public async Task RemoveAsync_WithNonExistingItem_DoesNothing() {
        await _repo.RemoveAsync(s => s.Name == "K");
        _repo.Count().Should().Be(3);
    }
}
