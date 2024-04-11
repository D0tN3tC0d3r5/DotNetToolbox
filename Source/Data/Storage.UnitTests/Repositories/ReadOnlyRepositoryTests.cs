namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyRepositoryTests {
    private readonly ReadOnlyRepository<int> _set = new([1, 2, 3]);

    [Fact]
    public async Task Count_ForInMemory_ReturnsCount() {
        var result = await _set.Count();

        result.Should().Be(3);
    }

    [Fact]
    public async Task HaveAny_ForInMemory_Returns() {
        var result = await _set.HaveAny();

        result.Should().BeTrue();
    }

    [Fact]
    public async Task FindFirst_ForInMemory_Returns() {
        var result = await _set.FindFirst();

        result.Should().Be(1);
    }

    [Fact]
    public async Task GetList_ForInMemory_Returns() {
        var result = await _set.GetList();

        result.Should().BeEquivalentTo([1, 2, 3]);
    }
}
