namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public async Task ToIndexedListAsync_ReturnsIndexedList() {
        var expectedIndexedList = new IndexedItem<TestEntity>[] {
            new(0, new("A"), false),
            new(1, new("BB"), false),
            new(2, new("CCC"), true),
        };
        var result = await _repo.ToIndexedListAsync();
        result.Should().BeEquivalentTo(expectedIndexedList);
    }

    [Fact]
    public async Task ToIndexedList_WithMapping_ReturnsMappedIndexedList() {
        var expectedIndexedList = new IndexedItem<string>[] {
            new(0, "A*", false),
            new(1, "BB*", false),
            new(2, "CCC*", true),
        };
        var result = await _repo.ToIndexedListAsync(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedIndexedList);
    }
}
