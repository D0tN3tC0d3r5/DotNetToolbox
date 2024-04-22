namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryTests {
    private abstract record BaseTestEntity(string Name);
    private record TestEntity1(string Name) : BaseTestEntity(Name);
    private record TestEntity2(string Name, List<int> Numbers) : BaseTestEntity(Name);
    private readonly InMemoryRepository<BaseTestEntity> _emptySet = new();
    private readonly InMemoryRepository<TestEntity1> _set1 = [new("A"), new("B"), new("C")];
    private readonly InMemoryRepository<TestEntity1> _set2 = [new("X"), new("Y"), new("Z")];
    private readonly InMemoryRepository<TestEntity2> _set3 = [new("1", [1, 2, 3]), new("2", [4, 5, 6]), new("3", [3, 6, 9])];
    private readonly InMemoryRepository<BaseTestEntity> _mixedSet = [new TestEntity1("A"), new TestEntity1("Y"), new TestEntity2("3", [3, 6, 9])];

    [Fact]
    public void OfType_ForEmptySet_ReturnsFalse()
        => _emptySet.OfType<BaseTestEntity>().Any().Should().BeFalse();

    [Fact]
    public void OfType_ReturnsSubSet()
        => _mixedSet.OfType<TestEntity1>().Count().Should().Be(2);

    [Fact]
    public void Cast_ForEmptySet_ReturnsFalse()
        => _emptySet.Cast<BaseTestEntity>().Any().Should().BeFalse();

    [Fact]
    public void Cast_ReturnsSubSet()
        => _set1.Cast<TestEntity1>().Count().Should().Be(3);

    [Fact]
    public void Cast_ForInvalidCast_Throws() {
        var action = () => _set1.Cast<TestEntity2>().Any();
        action.Should().Throw<InvalidCastException>();
    }

    [Fact]
    public void Where_ReturnsFilteredSet() {
        var expectedSet = new TestEntity1[] { new("B"), new("C") };
        _set1.Where(x => x.Name != "A").ToArray().Should().BeEquivalentTo(expectedSet);
    }

    [Fact]
    public void Where_WithIndex_ReturnsFilteredSet() {
        var expectedSet = new TestEntity1[] { new("B"), new("C") };
        _set1.Where((x, i) => i >= 1).ToArray().Should().BeEquivalentTo(expectedSet);
    }

    [Fact]
    public void Select_ReturnsProjectedSet() {
        var expectedSet = new[] { "A*", "B*", "C*" };
        _set1.Select(x => $"{x.Name}*").ToArray().Should().BeEquivalentTo(expectedSet);
    }

    [Fact]
    public void Select_WithIndex_ReturnsFilteredSet() {
        var expectedSet = new[] { "0:A", "1:B", "2:C" };
        _set1.Select((x, i) => $"{i}:{x.Name}").ToArray().Should().BeEquivalentTo(expectedSet);
    }

    [Fact]
    public void SelectMany_ReturnsProjectedSet() {
        var expectedSet = new[] { 1, 2, 3, 4, 5, 6, 3, 6, 9 };
        _set3.SelectMony(x => x.Numbers).ToArray().Should().BeEquivalentTo(expectedSet);
    }

    [Fact]
    public void Any_ForEmptySet_ReturnsFalse()
        => _emptySet.Any().Should().BeFalse();

    [Fact]
    public void Any_ReturnsTrue()
        => _set1.Any().Should().BeTrue();

    [Fact]
    public void Count_ForEmptySet_ReturnsZero()
        => _emptySet.Count().Should().Be(0);

    [Fact]
    public void Count_ReturnsCount()
        => _set1.Count().Should().Be(3);

    [Fact]
    public void FirstOrDefault_ForEmptySet_ReturnsNull()
        => _emptySet.FirstOrDefault().Should().BeNull();

    [Fact]
    public void FirstOrDefault_ReturnsFirstElement() {
        var expectedItem = new TestEntity1("A");
        _set1.FirstOrDefault().Should().Be(expectedItem);
    }

    [Fact]
    public void Add_ReturnsNewCount() {
        _set1.Add(new("D"));
        _set1.Count().Should().Be(4);
    }

    [Fact]
    public void Update_ReturnsChangesItem() {
        _set1.Update(s => s.Name == "B", new("Z"));
        _set1.Count().Should().Be(3);
        _set1.FirstOrDefault(s => s.Name == "B").Should().BeNull();
        _set1.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public void Remove_ReturnsNewCount() {
        _set1.Remove(s => s.Name == "B");
        _set1.Count().Should().Be(2);
    }
}
