namespace DotNetToolbox.Data.Repositories;

public partial class ValueObjectRepositoryTests {
    [Fact]
    public async Task SequenceEqualAsync_WithIdenticalSequence_ReturnsTrue() {
        var sequence = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = await _repo.SequenceEqualAsync(sequence);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithWrongItem_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("K"), new("CCC") };
        var result = await _repo.SequenceEqualAsync(sequence);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithFewerItems_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("BB") };
        var result = await _repo.SequenceEqualAsync(sequence);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithMoreItems_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("BB"), new("CCC"), new("D") };
        var result = await _repo.SequenceEqualAsync(sequence);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithComparer_WithIdenticalSequence_ReturnsTrue() {
        var sequence = new TestEntity[] { new("A"), new("BB"), new("CCC") };
        var result = await _repo.SequenceEqualAsync(sequence, _equalityComparer);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithComparer_WithWrongItem_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("K"), new("CCC") };
        var result = await _repo.SequenceEqualAsync(sequence, _equalityComparer);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithComparer_WithFewerItems_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("BB") };
        var result = await _repo.SequenceEqualAsync(sequence, _equalityComparer);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithComparer_WithMoreItems_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("BB"), new("CCC"), new("D") };
        var result = await _repo.SequenceEqualAsync(sequence, _equalityComparer);
        result.Should().BeFalse();
    }
}
