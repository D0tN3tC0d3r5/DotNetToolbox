namespace DotNetToolbox.Data.Repositories;

public class RepositoryTests {
    private readonly Repository<int?> _emptySet = new();
    private readonly Repository<int> _set = new([1, 2, 3]);

    [Fact]
    public void Count_ForEmptySet_ReturnsCount() {
        var result = _emptySet.Count();

        result.Should().Be(0);
    }

    [Fact]
    public void HaveAny_ForEmptySet_Returns() {
        var result = _emptySet.HaveAny();

        result.Should().BeFalse();
    }

    [Fact]
    public void GetFirst_ForEmptySet_Returns() {
        var result = _emptySet.GetFirst();

        result.Should().BeNull();
    }

    [Fact]
    public void GetList_ForEmptySet_Returns() {
        var result = _emptySet.ToArray();

        result.Should().BeEmpty();
    }

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
    public void GetFirst_ForInMemory_Returns() {
        var result = _set.GetFirst();

        result.Should().Be(1);
    }

    [Fact]
    public void GetList_ForInMemory_Returns() {
        var result = _set.ToArray();

        result.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Fact]
    public void Add_ForInMemory_ReturnsCount() {
        _set.Add(4);
        _set.Count().Should().Be(4);
    }
}
