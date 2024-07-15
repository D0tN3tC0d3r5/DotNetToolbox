namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public void ToDictionary_ReturnsDictionary() {
        var expectedDictionary = new Dictionary<int, string> {
            [1] = "A",
            [2] = "BB",
            [3] = "CCC",
        };
        var result = _repo.ToDictionary(k => k.Name.Length, v => $"{v.Name}");
        result.Should().BeEquivalentTo(expectedDictionary);
    }

    [Fact]
    public async Task ToDictionaryAsync_WithKeySelector_ReturnsDictionary() {
        var expectedDictionary = new Dictionary<string, TestEntity> {
            ["A"] = new("A"),
            ["BB"] = new("BB"),
            ["CCC"] = new("CCC"),
        };
        var result = await _repo.ToDictionaryAsync(x => x.Name);
        result.Should().BeEquivalentTo(expectedDictionary);
    }

    [Fact]
    public async Task ToDictionaryAsync_WithKeyAndValueSelector_ReturnsDictionary() {
        var expectedDictionary = new Dictionary<string, string> {
            ["A"] = "A*",
            ["BB"] = "BB*",
            ["CCC"] = "CCC*",
        };
        var result = await _repo.ToDictionaryAsync(k => k.Name, v => $"{v.Name}*");
        result.Should().BeEquivalentTo(expectedDictionary);
    }

    [Fact]
    public async Task ToDictionaryAsync_WithIndexAndKeySelector_ReturnsDictionary() {
        var expectedDictionary = new Dictionary<int, TestEntity> {
            [0] = new("A"),
            [1] = new("BB"),
            [2] = new("CCC"),
        };
        var result = await _repo.ToDictionaryAsync((_, i) => i);
        result.Should().BeEquivalentTo(expectedDictionary);
    }

    [Fact]
    public async Task ToDictionaryAsync_WithIndexAndKeyAndValueSelector_ReturnsDictionary() {
        var expectedDictionary = new Dictionary<int, string> {
            [0] = "0:A",
            [1] = "1:BB",
            [2] = "2:CCC",
        };
        var result = await _repo.ToDictionaryAsync((_, i) => i, (v, i) => $"{i}:{v.Name}");
        result.Should().BeEquivalentTo(expectedDictionary);
    }

    [Fact]
    public async Task ToDictionaryAsync_WithKeySelectorAndComparer_ReturnsDictionary() {
        var expectedDictionary = new Dictionary<string, TestEntity> {
            ["A"] = new("A"),
            ["BB"] = new("BB"),
            ["CCC"] = new("CCC"),
        };
        var result = await _repo.ToDictionaryAsync(x => x.Name, EqualityComparer<string>.Default);
        result.Should().BeEquivalentTo(expectedDictionary);
    }

    [Fact]
    public async Task ToDictionaryAsync_WithKeyAndValueSelectorAndComparer_ReturnsDictionary() {
        var expectedDictionary = new Dictionary<string, string> {
            ["A"] = "A*",
            ["BB"] = "BB*",
            ["CCC"] = "CCC*",
        };
        var result = await _repo.ToDictionaryAsync(k => k.Name, v => $"{v.Name}*", EqualityComparer<string>.Default);
        result.Should().BeEquivalentTo(expectedDictionary);
    }
    [Fact]
    public async Task ToDictionaryAsync_WithIndexAndKeySelectorAndComparer_ReturnsDictionary() {
        var expectedDictionary = new Dictionary<int, TestEntity> {
            [0] = new("A"),
            [1] = new("BB"),
            [2] = new("CCC"),
        };
        var result = await _repo.ToDictionaryAsync((_, i) => i, EqualityComparer<int>.Default);
        result.Should().BeEquivalentTo(expectedDictionary);
    }

    [Fact]
    public async Task ToDictionaryAsync_WithIndexAndKeyAndValueSelectorAndComparer_ReturnsDictionary() {
        var expectedDictionary = new Dictionary<int, string> {
            [0] = "0:A",
            [1] = "1:BB",
            [2] = "2:CCC",
        };
        var result = await _repo.ToDictionaryAsync((_, i) => i, (v, i) => $"{i}:{v.Name}", EqualityComparer<int>.Default);
        result.Should().BeEquivalentTo(expectedDictionary);
    }
}
