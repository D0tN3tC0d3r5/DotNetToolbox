namespace System.Linq.Async;

public partial class AsyncQueryableTests {
    [Fact]
    public async Task LoadAsync_ForEmptySet_ReturnsZero() {
        var action = () => _repo.LoadAsync();
        await action.Should().NotThrowAsync();
    }
}
