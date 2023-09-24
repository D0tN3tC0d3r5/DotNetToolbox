namespace DotNetToolbox.Extensions;

public class ConcurrentDictionaryExtensionsTests
{
    private readonly ConcurrentDictionary<int, object?> _concurrentDictionary;
    private readonly Func<int, string?> _getValue;

    public ConcurrentDictionaryExtensionsTests()
    {
        _concurrentDictionary = new();
        _getValue = Substitute.For<Func<int, string?>>();
    }

    [Fact]
    public void Get_ReturnsValueFromCache()
    {
        const int key = 100;
        const string expected = "Expected Value";

        _concurrentDictionary.TryAdd(key, expected);

        var actual = _concurrentDictionary.Get(key, _getValue);

        actual.Should().Be(expected);
    }

    [Fact]
    public void Get_WhenKeyIsNotInCache_CallsGetValue_()
    {
        const int key = 100;
        const string expected = "Expected Value";

        _getValue.Invoke(key).Returns(expected);

        var actual = _concurrentDictionary.Get(key, _getValue);

        actual.Should().Be(expected);
    }
}
