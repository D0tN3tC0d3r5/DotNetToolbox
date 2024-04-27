namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task ToHashSetAsync_ReturnsHashSet() {
        var expectedHashSet = new TestEntity[] { new("A"), new("B"), new("C") };
        var result = await _repo.ToHashSetAsync();
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithMapping_ReturnsMappedHashSet() {
        var expectedHashSet = new[] { "A*", "B*", "C*" };
        var result = await _repo.ToHashSetAsync(x => $"{x.Name}*");
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithMappingAndIndex_ReturnsMappedHashSet() {
        var expectedHashSet = new[] { "0:A", "1:B", "2:C" };
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
        var subject = new AsyncRepository<TestEntity>([new("A"), new("A"), new("B"), new("B"), new("B"), new("C")]);
        var expectedHashSet = new TestEntity[] { new("A"), new("B"), new("C") };
        var result = await subject.ToHashSetAsync();
        result.Should().BeEquivalentTo(expectedHashSet);
    }
    [Fact]
    public async Task ToHashSetAsync_WithComparer_ReturnsHashSet() {
        var expectedHashSet = new TestEntity[] { new("A"), new("B"), new("C") };
        var result = await _repo.ToHashSetAsync(_equalityComparer);
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithComparer_WithMapping_ReturnsMappedHashSet() {
        var expectedHashSet = new[] { "A*", "B*", "C*" };
        var result = await _repo.ToHashSetAsync(x => $"{x.Name}*", EqualityComparer<string>.Default);
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithComparer_WithMappingAndIndex_ReturnsMappedHashSet() {
        var expectedHashSet = new[] { "0:A", "1:B", "2:C" };
        var result = await _repo.ToHashSetAsync((x, i) => $"{i}:{x.Name}", EqualityComparer<string>.Default);
        result.Should().BeEquivalentTo(expectedHashSet);
    }

    [Fact]
    public async Task ToHashSetAsync_WithComparer_ForDuplicatedValues_ReturnsHashSet() {
        var subject = new AsyncRepository<TestEntity>([new("A"), new("A"), new("B"), new("B"), new("B"), new("C")]);
        var expectedHashSet = new TestEntity[] { new("A"), new("B"), new("C") };
        var result = await subject.ToHashSetAsync(_equalityComparer);
        result.Should().BeEquivalentTo(expectedHashSet);
    }
}
