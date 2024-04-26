namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task CountAsync_ForEmptySet_ReturnsZero() {
        var result = await _emptySet.CountAsync();
        result.Should().Be(0);
    }

    [Fact]
    public async Task Count_ReturnsCount() {
        var result = await _set1.CountAsync();
        result.Should().Be(3);
    }

    [Fact]
    public async Task Count_WithPredicate_ReturnsFilteredCount() {
        var result = await _set2.CountAsync(x => x.Name != "X");
        result.Should().Be(2);
    }
}
