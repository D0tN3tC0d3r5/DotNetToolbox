namespace System.Linq.Async;

public partial class AsyncEnumerableQueryTests {
    [Fact]
    public void ToHashSet_ReturnsHashSet() {
        var expectedHashSet = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = _repo.ToHashSet();
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public void ToHashSet_WithMapping_ReturnsMappedHashSet() {
        var expectedHashSet = new[] { "A*", "BB*", "CCC*" };
        var result = _repo.ToHashSet(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_ReturnsHashSet() {
        var expectedHashSet = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = await _repo.ToHashSetAsync();
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithMapping_ReturnsMappedHashSet() {
        var expectedHashSet = new[] { "A*", "BB*", "CCC*" };
        var result = await _repo.ToHashSetAsync(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithMappingAndIndex_ReturnsMappedHashSet() {
        var expectedHashSet = new[] { "0:A", "1:BB", "2:CCC" };
        var result = await _repo.ToHashSetAsync((x, i) => $"{i}:{x.Name}");
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_ForBigRepo_ReturnsHashSet() {
        var expectedHashSet = Enumerable.Range(0, 1000).ToHashSet(x => new TestEntity($"{x}"));
        var result = await _bigRepo.ToHashSetAsync();
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_ForDuplicatedValues_ReturnsHashSet() {
        var subject = new AsyncEnumerableQuery<TestEntity>([new("A"), new("A"), new("BB"), new("BB"), new("BB"), new("CCC")]);
        var expectedHashSet = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = await subject.ToHashSetAsync();
        result.Should().BeEquivalentTo(expectedHashSet);
    }
    [Fact]
    public async Task ToHashSetAsync_WithComparer_ReturnsHashSet() {
        var expectedHashSet = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = await _repo.ToHashSetAsync(_equalityComparer);
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithComparer_WithMapping_ReturnsMappedHashSet() {
        var expectedHashSet = new[] { "A*", "BB*", "CCC*" };
        var result = await _repo.ToHashSetAsync(x => $"{x.Name}*", EqualityComparer<string>.Default);
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithComparer_WithMappingAndIndex_ReturnsMappedHashSet() {
        var expectedHashSet = new[] { "0:A", "1:BB", "2:CCC" };
        var result = await _repo.ToHashSetAsync((x, i) => $"{i}:{x.Name}", EqualityComparer<string>.Default);
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithComparer_ForDuplicatedValues_ReturnsHashSet() {
        var subject = new AsyncEnumerableQuery<TestEntity>([new("A"), new("A"), new("BB"), new("BB"), new("BB"), new("CCC")]);
        var expectedHashSet = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = await subject.ToHashSetAsync(_equalityComparer);
        result.Should().BeEquivalentTo(expectedHashSet);
    }
}
