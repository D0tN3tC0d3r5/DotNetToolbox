namespace DotNetToolbox.Data.Repositories;

public class InMemoryAsyncRepositoryTests {
    private abstract record BaseTestEntity(string Name);
    private sealed record TestEntity1(string Name) : BaseTestEntity(Name);
    private record TestEntity2(string Name, List<int> Numbers) : BaseTestEntity(Name);
    private readonly InMemoryAsyncRepository<BaseTestEntity> _emptySet = [];
    private readonly InMemoryAsyncRepository<TestEntity1> _set1 = [new("A"), new("B"), new("C")];
    private readonly InMemoryAsyncRepository<TestEntity1> _set2 = [new("X"), new("Y"), new("Z")];
    private readonly InMemoryAsyncRepository<TestEntity2> _set3 = [new("One", [1, 2, 3]), new("Two", [4, 5, 6]), new("Three", [3, 6, 9])];
    private readonly InMemoryAsyncRepository<BaseTestEntity> _mixedSet = [new TestEntity1("A"), new TestEntity1("Y"), new TestEntity2("3", [3, 6, 9])];

    [Fact]
    public void Enumeration_AllowsLoop() {
        var count = 0;
        var expectedNames = new[] { "X", "Y", "Z" };
        foreach (var item in _set2) {
            expectedNames[count].Should().Be(item.Name);
            count++;
        }

        count.Should().Be(_set2.Count());
    }

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
        _set3.SelectMany(x => x.Numbers).ToArray().Should().BeEquivalentTo(expectedSet);
    }

    [Fact]
    public void SelectMany_WithIndex_ReturnsProjectedSet() {
        var expectedSet = new[] { "0=>1", "0=>2", "0=>3", "1=>4", "1=>5", "1=>6", "2=>3", "2=>6", "2=>9" };
        _set3.SelectMany((x, i) => x.Numbers.Select(n => $"{i}=>{n}")).ToArray().Should().BeEquivalentTo(expectedSet);
    }

    [Fact]
    public void SelectMany_WithParent_ReturnsProjectedSet() {
        var expectedSet = new[] { "One:1", "One:2", "One:3", "Two:4", "Two:5", "Two:6", "Three:3", "Three:6", "Three:9" };
        _set3.SelectMany(x => x.Numbers, (p, n) => $"{p.Name}:{n}").ToArray().Should().BeEquivalentTo(expectedSet);
    }

    [Fact]
    public void SelectMany_WithParentAndIndex_ReturnsProjectedSet() {
        var expectedSet = new[] { "One:0=>1", "One:0=>2", "One:0=>3", "Two:1=>4", "Two:1=>5", "Two:1=>6", "Three:2=>3", "Three:2=>6", "Three:2=>9" };
        _set3.SelectMany((x, i) => x.Numbers.Select(n => $"{i}=>{n}"), (p, n) => $"{p.Name}:{n}").ToArray().Should().BeEquivalentTo(expectedSet);
    }

    [Fact]
    public void Any_ForEmptySet_ReturnsFalse()
        => _emptySet.Any().Should().BeFalse();

    [Fact]
    public void Any_ReturnsTrue()
        => _set1.Any().Should().BeTrue();

    [Fact]
    public async Task AnyAsync_ForEmptySet_ReturnsFalse() {
        var result = await _emptySet.AnyAsync();
        result.Should().BeFalse();
    }

    [Fact]
    public async Task AnyAsync_ReturnsTrue() {
        var result = await _set1.AnyAsync();
        result.Should().BeTrue();
    }

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
