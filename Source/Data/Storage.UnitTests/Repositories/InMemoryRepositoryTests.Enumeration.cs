namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryRepositoryTests {
    [Fact]
    public void Enumeration_ForIEnumerable_AllowsLoop() {
        IEnumerable enumerable = _readOnlyRepo;
        var enumerator = enumerable.GetEnumerator();
        enumerator.Should().NotBeNull();
        ((IDisposable)enumerator).Dispose();
    }

    [Fact]
    public void Enumeration_AllowsForEach() {
        var count = 0;
        var expectedNames = Enumerable.Range(0, 90).ToArray(i => $"{i}");
        foreach (var item in _readOnlyRepo) {
            expectedNames[count].Should().Be(item.Name);
            count++;
        }

        count.Should().Be(_readOnlyRepo.Count());
    }
}
