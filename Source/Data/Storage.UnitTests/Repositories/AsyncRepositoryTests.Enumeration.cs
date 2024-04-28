namespace DotNetToolbox.Data.Repositories;

public partial class AsyncRepositoryTests {
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
        var expectedNames = new[] { "X", "Y", "Z" };
        foreach (var item in _childRepo) {
            expectedNames[count].Should().Be(item.Name);
            count++;
        }

        count.Should().Be(_repo.Count());
    }
}
