namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyRepositoryTests {
    private readonly ReadOnlyRepository<int> _set = new([1, 2, 3]);

    [Fact]
    public void Count_ForInMemory_ReturnsCount() {
        var result = _set.Count();

        result.Should().Be(3);
    }

    [Fact]
    public void HaveAny_ForInMemory_Returns() {
        var result = _set.HaveAny();

        result.Should().BeTrue();
    }

    [Fact]
    public void FindFirst_ForInMemory_Returns() {
        var result = _set.FindFirst();

        result.Should().Be(1);
    }

    [Fact]
    public void GetList_ForInMemory_Returns() {
        var result = _set.GetList();

        result.Should().BeEquivalentTo([1, 2, 3]);
    }
}
