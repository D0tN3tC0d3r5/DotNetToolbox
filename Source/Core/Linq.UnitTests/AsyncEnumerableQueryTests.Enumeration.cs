namespace DotNetToolbox.Data.Repositories;

public partial class AsyncEnumerableQueryTests {
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
}
