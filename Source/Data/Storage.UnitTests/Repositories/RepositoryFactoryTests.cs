namespace DotNetToolbox.Data.Repositories;

public class RepositoryFactoryTests {
    private readonly RepositoryFactory _factory = new();

    [Fact]
    public void CreateRepository_ReturnsRepository() {
        var result = _factory.CreateRepository<Repository<int>, int>();

        result.Should().BeOfType<Repository<int>>();
    }

    [Fact]
    public void CreateRepository_WithStrategy_ReturnsRepository() {
        var result = _factory.CreateRepository<Repository<int>, int>([]);

        result.Should().BeOfType<Repository<int>>();
    }

    [Fact]
    public void CreateAsyncRepository_ReturnsRepository() {
        var result = _factory.CreateAsyncRepository<AsyncRepository<int>, int>();

        result.Should().BeOfType<AsyncRepository<int>>();
    }

    [Fact]
    public void CreateAsyncRepository_WithStrategy_ReturnsRepository() {
        var result = _factory.CreateAsyncRepository<AsyncRepository<int>, int>([]);

        result.Should().BeOfType<AsyncRepository<int>>();
    }
}
