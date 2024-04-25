namespace DotNetToolbox.Data.Repositories;

public class AsyncRepositoryTests {
    private abstract record BaseTestEntity(string Name);
    private sealed record TestEntity1(string Name) : BaseTestEntity(Name);
    private readonly AsyncRepository<BaseTestEntity> _emptySet = [];
    private readonly AsyncRepository<TestEntity1> _set1 = [new("A"), new("B"), new("C")];
    private readonly AsyncRepository<TestEntity1> _set2 = [new("X"), new("Y"), new("Z")];

    private class TestRepository : AsyncRepository<TestEntity1>;

    private class DummyAsyncRepositoryStrategy : AsyncRepositoryStrategy<TestEntity1>;

    private readonly TestRepository _childRepo = [new("X"), new("Y"), new("Z")];

    private readonly RepositoryStrategyProvider _provider = new();

    public AsyncRepositoryTests() {
        _provider.TryAdd<DummyAsyncRepositoryStrategy>();
    }

    [Fact]
    public void Constructor_WithStrategyFactory_CreatesRepository() {
        var subject = new AsyncRepository<TestEntity1>(_provider);
        subject.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithSeed_CreatesRepository() {
        var subject = new AsyncRepository<TestEntity1>([new("A"), new("B"), new("C")]);
        subject.Should().NotBeNull();
        subject.Count().Should().Be(3);
    }

    [Fact]
    public void Seed_PopulatesRepository() {
        var subject = new AsyncRepository<TestEntity1>();
        subject.Seed([new("A"), new("B"), new("C")]);
        subject.Count().Should().Be(3);
    }

    [Fact]
    public async Task SeedAsync_PopulatesRepository() {
        var subject = new AsyncRepository<TestEntity1>();
        var seed = new AsyncEnumerable<TestEntity1>([new("A"), new("B"), new("C")]);
        await subject.SeedAsync(seed);
        subject.Count().Should().Be(3);
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
    public async Task AsyncEnumeration_AllowsAsyncForEach() {
        var count = 0;
        var expectedNames = new[] { "X", "Y", "Z" };
        await foreach (var item in _childRepo) {
            expectedNames[count].Should().Be(item.Name);
            count++;
        }

        count.Should().Be(_set2.Count());
    }

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
    public async Task AnyAsync_WithExistingItem_ReturnsTrue() {
        var result = await _set1.AnyAsync(x => x.Name == "B");
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AnyAsync_WithInvalidItem_ReturnsFalse() {
        var result = await _set1.AnyAsync(x => x.Name == "K");
        result.Should().BeFalse();
    }

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

    [Fact]
    public async Task FirstOrDefaultAsync_ReturnsFirstElement() {
        var expectedItem = new TestEntity1("A");
        var result = await _set1.FirstOrDefaultAsync();
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_ForEmptySet_ReturnsNull() {
        var result = await _emptySet.FirstOrDefaultAsync();
        result.Should().BeNull();
    }

    [Fact]
    public async Task FirstOrDefaultAsync_WithExistingItem_ReturnsFirstElement() {
        var expectedItem = new TestEntity1("A");
        var result = await _set1.FirstOrDefaultAsync(x => x.Name == "A");
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task FirstOrDefaultAsync_WithInvalidItem_ReturnsNull() {
        var result = await _set1.FirstOrDefaultAsync(x => x.Name == "K");
        result.Should().BeNull();
    }

    [Fact]
    public void Add_AddsItem() {
        _set1.Add(new("D"));
        _set1.Count().Should().Be(4);
    }

    [Fact]
    public void Update_UpdatedItem() {
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

    [Fact]
    public async Task AddAsync_AddsItem() {
        await _set1.AddAsync(new("D"));
        _set1.Count().Should().Be(4);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesItem() {
        await _set1.UpdateAsync(s => s.Name == "B", new("Z"));
        _set1.Count().Should().Be(3);
        _set1.FirstOrDefault(s => s.Name == "B").Should().BeNull();
        _set1.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public async Task RemoveAsync_RemovesItem() {
        await _set1.RemoveAsync(s => s.Name == "B");
        _set1.Count().Should().Be(2);
    }

    [Fact]
    public async Task RemoveAsync_WithNonExistingItem_DoesNothing() {
        await _set1.RemoveAsync(s => s.Name == "K");
        _set1.Count().Should().Be(3);
    }
}
