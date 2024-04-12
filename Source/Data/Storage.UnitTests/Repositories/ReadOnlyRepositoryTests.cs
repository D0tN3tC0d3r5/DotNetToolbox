namespace DotNetToolbox.Data.Repositories;

public class ReadOnlyRepositoryTests {
    private readonly ReadOnlyRepository<int?> _emptySet = new();
    private readonly ReadOnlyRepository<int> _set = new([1, 2, 3]);

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
    public void FindFirst_ForEmptySet_Returns() {
        var result = _emptySet.FindFirst();

        result.Should().BeNull();
    }

    [Fact]
    public void GetList_ForEmptySet_Returns() {
        var result = _emptySet.GetList();

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
