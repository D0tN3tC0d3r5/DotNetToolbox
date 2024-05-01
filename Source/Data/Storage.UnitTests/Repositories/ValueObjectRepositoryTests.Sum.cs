namespace DotNetToolbox.Data.Repositories;

public partial class ValueObjectRepositoryTests {
    [Fact]
    public async Task SumAsync_ForEmptySet_ReturnsZero() {
        var result = await _emptyIntRepo.SumAsync();
        result.Should().Be(0);
    }

    [Fact]
    public async Task SumAsync_ReturnsSum() {
        var result = await _intRepo.SumAsync();
        result.Should().Be(45);
    }

    [Fact]
    public async Task SumAsync_WithTransformation_ReturnsSum() {
        var result = await _repo.SumAsync(x => x.Name.Length);
        result.Should().Be(6);
    }

    [Fact]
    public async Task SumAsync_ForEmptyNullableInt_ReturnsZero() {
        var result = await _emptyNullableIntRepo.SumAsync();
        result.Should().Be(0);
    }

    [Fact]
    public async Task SumAsync_WithNullableItem_IgnoreNullsAndReturnsSum() {
        var result = await _nullableIntRepo.SumAsync();
        result.Should().Be(20);
    }

    [Fact]
    public async Task SumAsync_WithNullableItemAndTransformation_IgnoreNullsAndReturnsSum() {
        var result = await _nullableIntRepo.SumAsync(x => x * 3);
        result.Should().Be(60);
    }
}
