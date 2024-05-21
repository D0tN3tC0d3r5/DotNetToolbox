namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public void ToEnumerable_AllowsLoop() {
        var count = 0;
        var expectedNames = new[] { "A", "BB", "CCC" };
        foreach (var item in _repo.ToEnumerable()) {
            expectedNames[count].Should().Be(item.Name);
            count++;
        }

        count.Should().Be(_repo.Count());
    }
}
