namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task AggregateAsync_ForEmptyRepo_Throws() {
        var result = async () => await _emptyRepo.AggregateAsync(string.Empty, (s, i) => $"{s}{i.Name}");
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AggregateAsync_WithSeed_ReturnsAggregate() {
        var result = await _repo.AggregateAsync(string.Empty, (s, i) => $"{s}{i.Name} ");
        result.Should().Be("A BB CCC ");
    }

    [Fact]
    public async Task AggregateAsync_WithSeedAndResultSelector_ReturnsAggregate() {
        var result = await _repo.AggregateAsync(string.Empty, (s, i) => $"{s}{i.Name} ", s => s.Trim());
        result.Should().BeEquivalentTo("A BB CCC");
    }

    [Fact]
    public async Task AggregateAsync_ReturnsAggregate() {
        var expectedItem = new TestEntity("A BB CCC");
        var result = await _repo.AggregateAsync((s, i) => new($"{s.Name} {i.Name}"));
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task AggregateAsync_WithResultSelector_ReturnsAggregate() {
        var result = await _repo.AggregateAsync((s, i) => new($"{s.Name} {i.Name}"), s => s.Name);
        result.Should().Be("A BB CCC");
    }
}
