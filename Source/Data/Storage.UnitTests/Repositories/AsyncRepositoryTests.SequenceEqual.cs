namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
    [Fact]
    public async Task SequenceEqualAsync_WithIdenticalSequence_ReturnsTrue() {
        var sequence = new TestEntity[] { new("A"), new("B"), new("C") };
        var result = await _set1.SequenceEqualAsync(sequence);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithWrongItem_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("K"), new("C") };
        var result = await _set1.SequenceEqualAsync(sequence);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithFewerItems_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("B") };
        var result = await _set1.SequenceEqualAsync(sequence);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithMoreItems_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("B"), new("C"), new("D") };
        var result = await _set1.SequenceEqualAsync(sequence);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithComparer_WithIdenticalSequence_ReturnsTrue() {
        var sequence = new TestEntity[] { new("A"), new("B"), new("C") };
        var result = await _set1.SequenceEqualAsync(sequence, new TestEntityEqualityComparer());
        result.Should().BeTrue();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithComparer_WithWrongItem_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("K"), new("C") };
        var result = await _set1.SequenceEqualAsync(sequence, new TestEntityEqualityComparer());
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithComparer_WithFewerItems_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("B") };
        var result = await _set1.SequenceEqualAsync(sequence, new TestEntityEqualityComparer());
        result.Should().BeFalse();
    }

    [Fact]
    public async Task SequenceEqualAsync_WithComparer_WithMoreItems_ReturnsFalse() {
        var sequence = new TestEntity[] { new("A"), new("B"), new("C"), new("D") };
        var result = await _set1.SequenceEqualAsync(sequence, new TestEntityEqualityComparer());
        result.Should().BeFalse();
    }
}
