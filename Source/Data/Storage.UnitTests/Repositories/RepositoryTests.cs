namespace DotNetToolbox.Data.Repositories;

public class RepositoryTests {
    private abstract record BaseTestEntity(string Name);
    private sealed record TestEntity1(string Name) : BaseTestEntity(Name);
    private readonly Repository<BaseTestEntity> _emptySet = [];
    private readonly Repository<TestEntity1> _set1 = [new("A"), new("B"), new("C")];
    private readonly Repository<TestEntity1> _set2 = [new("X"), new("Y"), new("Z")];

    private sealed  class TestRepository : Repository<TestEntity1>;

    private sealed class DummyRepositoryStrategy : RepositoryStrategy<TestEntity1>;

    private readonly TestRepository _childRepo = [new("X"), new("Y"), new("Z")];

    private readonly RepositoryStrategyProvider _provider = new();

    public RepositoryTests() {
        _provider.TryAdd<DummyRepositoryStrategy>();
    }

    [Fact]
    public void Constructor_WithStrategyFactory_CreatesRepository() {
        var subject = new Repository<TestEntity1>(_provider);
        subject.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithSeed_CreatesRepository() {
        var subject = new Repository<TestEntity1>([new("A"), new("B"), new("C")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
    }

    [Fact]
    public void ElementType_ReturnsElementType() {
        var subject = new TestRepository();
        subject.ElementType.Should().Be(typeof(TestEntity1));
    }

    [Fact]
    public void Seed_PopulatesRepository() {
        var subject = new Repository<TestEntity1>();
        subject.Seed([new("A"), new("B"), new("C")]);
        subject.Count().Should().Be(3);
    }

    [Fact]
    public void Enumeration_FromInterface_AllowsForEach() {
        IEnumerable enumerable = _set2;
        var count = 0;
        foreach (var _ in enumerable) count++;
        count.Should().Be(_set2.Count());
    }

    [Fact]
    public void Enumeration_AllowsForEach() {
        var count = 0;
        var expectedNames = new[] { "X", "Y", "Z" };
        foreach (var item in _set2) {
            expectedNames[count].Should().Be(item.Name);
            count++;
        }

        count.Should().Be(_set2.Count());
    }

    [Fact]
    public void Enumeration_ForChildRepo_AllowsForEach() {
        var count = 0;
        var expectedNames = new[] { "X", "Y", "Z" };
        foreach (var item in _childRepo) {
            expectedNames[count].Should().Be(item.Name);
            count++;
        }

        count.Should().Be(_set2.Count());
    }

    [Fact]
    public void Add_AddsItem() {
        _set1.Add(new("D"));
        _set1.Count().Should().Be(4);
    }

    [Fact]
    public void Update_ChangesItem() {
        _set1.Update(s => s.Name == "B", new("Z"));
        _set1.Count().Should().Be(3);
        _set1.FirstOrDefault(s => s.Name == "B").Should().BeNull();
        _set1.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }

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
}
