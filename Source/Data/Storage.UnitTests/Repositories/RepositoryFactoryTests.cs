namespace DotNetToolbox.Data.Repositories;

public class RepositoryFactoryTests {
    private readonly RepositoryFactory _factory = new();

    [Fact]
    public void CreateRepository_ReturnsRepository() {
        var result = _factory.CreateRepository<int>();

        result.Should().BeOfType<InMemoryRepository<int>>();
    }

    [Fact]
    public void CreateRepository_WithData_ReturnsSeededRepository() {
        var result = _factory.CreateRepository([1, 2, 3]);

        result.Should().BeOfType<InMemoryRepository<int>>();
        result.Count().Should().Be(3);
    }

    [Fact]
    public void CreateRepository_WithStrategy_ReturnsRepository() {
        var result = _factory.CreateRepository<int, InMemoryRepositoryStrategy<int>>();

        result.Should().BeOfType<Repository<InMemoryRepositoryStrategy<int>, int>>();
    }

    [Fact]
    public void CreateRepository_WithStrategyAndData_ReturnsSeededRepository() {
        var result = _factory.CreateRepository<int, InMemoryRepositoryStrategy<int>>([1, 2, 3]);

        result.Should().BeOfType<Repository<InMemoryRepositoryStrategy<int>, int>>();
        result.Count().Should().Be(3);
    }
}
