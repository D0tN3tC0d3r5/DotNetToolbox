namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void Enumeration_ForIEnumerable_AllowsLoop() {
        IEnumerable enumerable = _repo;
        var enumerator = enumerable.GetEnumerator();
        enumerator.Should().NotBeNull();
        ((IDisposable)enumerator).Dispose();
    }

    [Fact]
    public void Enumeration_AllowsForEach() {
        var count = 0;
        var expectedNames = new[] { "A", "BB", "CCC" };
        foreach (var item in _repo) {
            expectedNames[count].Should().Be(item.Name);
            count++;
        }

        count.Should().Be(_repo.Count());
    }

    [Fact]
    public void Enumeration_ForChildRepo_AllowsForEach() {
        var count = 0;
        var expectedNames = new[] { "A", "BB", "CCC" };
        foreach (var item in _updatableRepo) {
            expectedNames[count].Should().Be(item.Name);
            count++;
        }

        count.Should().Be(_repo.Count());
    }
}
