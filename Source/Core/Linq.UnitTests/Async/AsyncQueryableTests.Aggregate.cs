namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public async Task Aggregate_ReturnsAggregate() {
        var result = await _repo.AggregateAsync(string.Empty, static (s, i) => new($"{s} {i.Name}"), static r => r.Trim());
        result.Should().Be("A BB CCC");
    }

    [Fact]
    public async Task AggregateAsync_ForEmptyRepo_Throws() {
        var result = static async () => await _emptyRepo.AggregateAsync(string.Empty, static (s, i) => $"{s}{i.Name}");
        await result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task AggregateAsync_WithSeed_ReturnsAggregate() {
        var result = await _repo.AggregateAsync(string.Empty, static (s, i) => $"{s}{i.Name} ");
        result.Should().Be("A BB CCC ");
    }

    [Fact]
    public async Task AggregateAsync_WithSeedAndResultSelector_ReturnsAggregate() {
        var result = await _repo.AggregateAsync(string.Empty, static (s, i) => $"{s}{i.Name} ", static s => s.Trim());
        result.Should().BeEquivalentTo("A BB CCC");
    }

    [Fact]
    public async Task AggregateAsync_ReturnsAggregate() {
        var expectedItem = new TestEntity("A BB CCC");
        var result = await _repo.AggregateAsync(static (s, i) => new($"{s.Name} {i.Name}"));
        result.Should().Be(expectedItem);
    }

    [Fact]
    public async Task AggregateAsync_WithResultSelector_ReturnsAggregate() {
        var result = await _repo.AggregateAsync(static (s, i) => new($"{s.Name} {i.Name}"), static s => s.Name);
        result.Should().Be("A BB CCC");
    }
}
