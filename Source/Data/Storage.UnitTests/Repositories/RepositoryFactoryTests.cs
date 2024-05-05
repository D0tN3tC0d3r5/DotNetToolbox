namespace DotNetToolbox.Data.Repositories;

public class RepositoryFactoryTests {
    private readonly RepositoryFactory _factory = new();

    [Fact]
    public void CreateRepository_ReturnsRepository() {
        var result = _factory.CreateRepository<int>();

        result.Should().BeOfType<Repository<int>>();
    }

    [Fact]
    public void CreateRepository_WithName_ReturnsRepository() {
        var result = _factory.CreateRepository<int>("Name");

        result.Should().BeOfType<Repository<int>>();
        result.Name.Should().Be("Name");
    }

    [Fact]
    public void CreateRepository_WithStrategy_ReturnsRepository() {
        var result = _factory.CreateRepository<int, InMemoryRepositoryStrategy<int>>();

        result.Should().BeOfType<RepositoryBase<InMemoryRepositoryStrategy<int>, int>>();
    }

    [Fact]
    public void CreateRepository_WithNameAndStrategy_ReturnsRepository() {
        var result = _factory.CreateRepository<int, InMemoryRepositoryStrategy<int>>("Name");

        result.Should().BeOfType<RepositoryBase<InMemoryRepositoryStrategy<int>, int>>();
        result.Name.Should().Be("Name");
    }
}
