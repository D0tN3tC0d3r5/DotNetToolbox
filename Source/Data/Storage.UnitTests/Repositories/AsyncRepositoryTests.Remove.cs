namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public void Remove_RemovesItem() {
        _set1.Remove(s => s.Name == "B");
        _set1.Count().Should().Be(2);
    }

    [Fact]
    public void Remove_NonExistingItem_DoesNothing() {
        _set1.Remove(s => s.Name == "K");
        _set1.Count().Should().Be(3);
    }

    [Fact]
    public async Task RemoveAsync_RemovesItem() {
        await _set1.RemoveAsync(s => s.Name == "B");
        _set1.Count().Should().Be(2);
    }

    [Fact]
    public async Task RemoveAsync_WithNonExistingItem_DoesNothing() {
        await _set1.RemoveAsync(s => s.Name == "K");
        _set1.Count().Should().Be(3);
    }
}
