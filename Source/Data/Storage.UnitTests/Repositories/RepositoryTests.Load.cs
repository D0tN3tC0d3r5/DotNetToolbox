namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void Load_ForEmptySet_ReturnsZero() {
        var action = () => _repo.Load();
        action.Should().NotThrow();
    }

    [Fact]
    public async Task LoadAsync_ForEmptySet_ReturnsZero() {
        var action = () => _repo.LoadAsync();
        await action.Should().NotThrowAsync();
    }
}
