namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task ToIndexedListAsync_ReturnsIndexedList() {
        var expectedIndexedList = new IndexedItem<TestEntity>[] {
            new(0, new("A"), false),
            new(1, new("B"), false),
            new(2, new("C"), true),
        };
        var result = await _repo.ToIndexedListAsync();
        result.Should().BeEquivalentTo(expectedIndexedList);
    }

    [Fact]
    public async Task ToIndexedList_WithMapping_ReturnsMappedIndexedList() {
        var expectedIndexedList = new IndexedItem<string>[] {
            new(0, "A*", false),
            new(1, "B*", false),
            new(2, "C*", true),
        };
        var result = await _repo.ToIndexedListAsync(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedIndexedList);
    }
}
