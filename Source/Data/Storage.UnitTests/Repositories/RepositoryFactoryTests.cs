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
        var result = _factory.CreateRepository<Repository<int>, int>(new InMemoryRepositoryStrategy<int>());

        result.Should().BeOfType<Repository<int>>();
    }
}
